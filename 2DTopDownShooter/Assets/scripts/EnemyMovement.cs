﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private GameObject player;
	private Transform target;
    public float speed = 7.5f;
    public float waitTime = 0;
    public float startWaitTime = 0;
    //States representing what type of movement Enemy should be executing right now
    public enum State {chase, attack, die, still};
    public State currentAction = State.chase;
    //Physics stuff relevant to movement
    private Rigidbody2D rb;
    private float originalMass;

    [Header("Attack")]
    public Sprite defaultSprite;
    public Sprite attackSprite;
    private SpriteRenderer sr;
    private EnemyAttackSegC attackData;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMass = rb.mass;
        attackData = GetComponentInChildren<EnemyAttackSegC>();
        sr = GetComponentInChildren<SpriteRenderer>();

        waitTime = startWaitTime;
		
        //Looks for the first GameObject tagged as "Player" and sets that GO's transform as the player attribute.
		player = GameObject.FindGameObjectsWithTag("PlayerBall")[0];
		//At start, assume nothing is obstructing enemy from player so set player's transform as target
		target = player.transform;
    }

    void Update()
    {
        if(player.GetComponent<PlayerHealth>().hp <= 0)
        {
            sr.sprite = defaultSprite;
            currentAction = State.still;
        }
        else if(attackData.getCurrentAttackState() == EnemyAttackSegC.AttackState.Attacking)
        {
            sr.sprite = attackSprite;
            currentAction = State.attack;
        }
        else if(attackData.getCurrentAttackState() == EnemyAttackSegC.AttackState.Cooldown || 
            attackData.getCurrentAttackState() == EnemyAttackSegC.AttackState.Windup)
        {
            sr.sprite = defaultSprite;
            currentAction = State.attack;
        }
        
        switch(currentAction)
        {
            case State.chase:
            {
                Move();
                break;
            }
            case State.attack:
            {        
                ExecuteAttack();
                break;
            }
            case State.die:
            {
                break;
            }
            case State.still:
            {
                rb.velocity = Vector2.zero;
                break;
            }
            default:
                break;
        }
    }

    private void Move()
    {
        //Face the target node/player
        Look(target);
        //Move rigidbody and enemy position towards the target, smoothing any collisions with other objects.
        rb.MovePosition(Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime));
        
    }

    private void Look(Transform toLook)
    {
        /*This was given by the Scaffold.
        */
        Vector3 dir = toLook.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        sr.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void ExecuteAttack()
    {
        /* Zero Out Movement
        For some reason, the enemy will move oddly without these
        such as spiraling away from the player.
        This also makes sure that if another enemy is trying to shove its
        way to the player, the enemies that are being shoved won't spiral away
        from the player.*/
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.mass = 100000f;
        
        //Look(target);
        
        //Track how long the enemy has been attacking.
        //If it's longer than or equal the time it should
        //take to attack, stop and go back to chasing the player.
        if(attackData.getCurrentAttackState() == EnemyAttackSegC.AttackState.Normal)
        {
            sr.sprite = defaultSprite;
            currentAction = State.chase;
            rb.mass = originalMass;
        }
    }
}
