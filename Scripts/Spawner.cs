using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemaining;
    int enemiesRemaniningToSpawn;
    float nextSpawnTime;

    void Start()
    {
        NextWave();
    }
    
    void Update()
    {
        if(enemiesRemaniningToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemaniningToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy newEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            newEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()
    {
        enemiesRemaining--;

        if(enemiesRemaining == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;

        if(currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemaniningToSpawn = currentWave.enemyCount;
            enemiesRemaining = enemiesRemaniningToSpawn;
        }   
    }

    [System.Serializable]
	public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
