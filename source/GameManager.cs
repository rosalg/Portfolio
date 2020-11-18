using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{
    [HideInInspector]
    public bool m_WaveInProgress = false;
    [HideInInspector]
    public Wave_t m_currentWave;
    public Coroutine m_spawningCoroutine;
    private bool serverStart = false;
    private Queue<Wave_t> m_Waves = new Queue<Wave_t>(); 

    private int m_NumWaves;
    public int EnemySpawnDelay;
    public Vector3[] enemySpawnLocations;
    public GameObject enemy;
    public bool SetTesting;
    public int SetWaves;
    public int SetEnemyMoveSpeedMultiplier;
    public static int EnemyMoveSpeedMultiplier;
    public static bool Testing;
    [Header("Starting Positions")]
    public Transform startingPos1;
    public Transform startingPos2;

    public static int counter = 0;

    void Awake()
    {
        EnemyMoveSpeedMultiplier = SetEnemyMoveSpeedMultiplier;
        Testing = SetTesting;
        m_NumWaves = SetWaves;
    }

    public class Wave_t
    {
        public int NumEnemies;
        public int ActiveEnemies;
        public int SpawnedEnemies;
        private int WaveNumber;
        private Vector3[] SpawnLocations;
        private int DelayTime;
        private GameObject Enemy;

        public Wave_t( int numEnemies, Vector3[] spawnLocations, int delayTime, GameObject enemy, int waveNumber )
        {
            NumEnemies = numEnemies;
            ActiveEnemies = 0;
            SpawnedEnemies = 0;
            SpawnLocations = spawnLocations;
            DelayTime = delayTime;
            Enemy = enemy;
            WaveNumber = waveNumber;
        }


        public IEnumerator SpawnEnemies()
        {
            GameObject player0 = OurNetworkManager.players[0];
            GameObject player1 = OurNetworkManager.players[1];
            

            for (int i = 0; i < NumEnemies; i++)
            {
                Vector3 spawnLocation = SpawnLocations[Mathf.FloorToInt(UnityEngine.Random.Range(0, SpawnLocations.Length - 1))];

                //for now alternate between the two players
                GameObject enemy = Instantiate(Enemy, spawnLocation, Quaternion.identity);
                enemy.GetComponent<Enemy>().moveForce = EnemyMoveSpeedMultiplier * WaveNumber;
                NetworkServer.Spawn(enemy);
                if (Testing)
                {
                    enemy.GetComponent<Enemy>().TargetPlayerNumber = 0;
                    enemy.GetComponent<Enemy>().player = player0.transform;
                    NetworkConnection conn = OurNetworkManager.playerNetworkConnections[player0];
                    enemy.GetComponent<Enemy>().TargetRenderColor(conn);
                }
                else if (i % 2 == 0)
                {
                    enemy.GetComponent<Enemy>().TargetPlayerNumber = 0;
                    enemy.GetComponent<Enemy>().player = player0.transform;
                    NetworkConnection conn = OurNetworkManager.playerNetworkConnections[player1];
                    enemy.GetComponent<Enemy>().TargetRenderColor(conn);
                }
                else
                {
                    enemy.GetComponent<Enemy>().TargetPlayerNumber = 1;
                    enemy.GetComponent<Enemy>().player = player1.transform;
                    NetworkConnection conn = OurNetworkManager.playerNetworkConnections[player0];
                    enemy.GetComponent<Enemy>().TargetRenderColor(conn);
                }
                SpawnedEnemies++;
                ActiveEnemies++;
                yield return new WaitForSeconds( DelayTime );
                //Debug.Log("Just returned from helper call wait function.");
            }
            yield return new WaitForSeconds( 0 );
        }

        private IEnumerator HelperWaitForSeconds( int delayTime )
        {
            yield return new WaitForSeconds( delayTime );
        }
    }

    [ServerCallback]
    public override void OnStartServer()
    {
        CreateWaves();
    }

    public void CreateWaves()
    {
        // Reset state in case previous game was interrupted.
        m_Waves.Clear();
        m_WaveInProgress = false;
        m_NumWaves = SetWaves;

        for (int i = 0; i < m_NumWaves; i++)
        {
            AddWave((i + 1) * 2, enemySpawnLocations, EnemySpawnDelay, enemy, i + 1);
        }
        serverStart = true;
    }

    [ServerCallback]
    void Update()
    {
        if ( serverStart && OurNetworkManager.readyForWaves )
            StartNextWave(); // Why does this need to be called on every server udpate? A wave should only start once, right?
        if (m_currentWave != null && m_currentWave.NumEnemies == m_currentWave.SpawnedEnemies && m_currentWave.ActiveEnemies == 0)
            m_WaveInProgress = false;
    }

    public void AddWave( int numEnems, Vector3[] spawnLocs, int delayTime, GameObject enemy, int WaveNumber )
    {
        m_Waves.Enqueue( new Wave_t( numEnems, spawnLocs, delayTime, enemy, WaveNumber ) );
    }

    public void StartNextWave()
    {

        if ( m_Waves.Count > 0 && !m_WaveInProgress )
        {
            counter++;
            m_currentWave = m_Waves.Dequeue();
            m_spawningCoroutine = StartCoroutine(m_currentWave.SpawnEnemies());
            m_WaveInProgress = true;
        }
    }
    /* TODO: Write an EndGame function that can be called from an Enemy script that ends the game
     * 
     */
}
