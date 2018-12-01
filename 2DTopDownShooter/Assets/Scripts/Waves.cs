﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waves : MonoBehaviour {
    private int Wave = 0;
    public int WaveOneEnemies;
    public int WaveTwoEnemies;
    public int numberOfEnemies;
    public int enemyHealthOrigin;
    public int enemyDamageOrigin;
    private int otherNumEnemies;
    public GameObject playerChar;
    public GameObject enemyType;
    public GameObject enemyDamageType;
    public GameObject SpawnOne;
    public GameObject SpawnTwo;
    public GameObject SpawnThree;
    public Transform SpawnPoint;
    public GameObject[] NumEnemies;
    public GameObject[] OtherEnemies;
    private Waves FirstSpawn;
    private Waves SecondSpawn;
    private Waves ThirdSpawn;
    private PlayerHealth health;
    private EnemyAttackSegC enemyDamage;
    private enemyBehavior enemyHealth;
    private float playerHP;
    private bool EnemiesPresent;
    // Use this for initialization
    void Start()
    {
        EnemiesPresent = false;
        health = playerChar.GetComponent<PlayerHealth>();
        enemyDamage = enemyDamageType.GetComponent<EnemyAttackSegC>();
        enemyHealth = enemyType.GetComponent<enemyBehavior>();
        playerHP = health.maxHp;
        enemyHealth.startingHealth = enemyHealthOrigin;
        enemyDamage.damage = enemyDamageOrigin;
        NumEnemies = new GameObject[numberOfEnemies];
        FirstSpawn = SpawnOne.GetComponent<Waves>();
        SecondSpawn = SpawnTwo.GetComponent<Waves>();
        ThirdSpawn = SpawnThree.GetComponent<Waves>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Wave > 2)
        {
            Debug.Log("YOU WIN");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (NumEnemies[i] != null)
            {
                EnemiesPresent = true;
                break;
            }
            else
            {
                EnemiesPresent = false;
            }
        }
        bool firstspawnen = !FirstSpawn.EnemiesPresent;
        bool secondspawnen = !SecondSpawn.EnemiesPresent;
        bool thirdspawnen = !ThirdSpawn.EnemiesPresent;
        bool nextwave = !EnemiesPresent && !FirstSpawn.EnemiesPresent && !SecondSpawn.EnemiesPresent && !ThirdSpawn.EnemiesPresent;
        Debug.Log("This: " + !EnemiesPresent + " One: " + firstspawnen + " Two: " + secondspawnen + " Three: " + thirdspawnen + " Move one to next wave? " + nextwave + " For Object: " + gameObject.name);
        if (nextwave)
        {
            Wave++;
            SpawnEnemies();
        }
    }
    public void SpawnEnemies()
    {
        if (Wave == 1)
        {
            for (int i = 0; i < WaveOneEnemies; i++)
            {
                NumEnemies[i] = Instantiate(enemyType, SpawnPoint.position, SpawnPoint.rotation);
            }
            Debug.Log("Spawning 1");
        }
        if (Wave == 2)
        {
            for (int i = 0; i < WaveTwoEnemies; i++)
            {
                NumEnemies[i] = Instantiate(enemyType, SpawnPoint.position, SpawnPoint.rotation);
                enemyHealth.startingHealth = 2 * enemyHealthOrigin;
                enemyDamage.damage = 2 * enemyDamageOrigin;
            }
            Debug.Log("Spawning 2");
        }
    }
}
