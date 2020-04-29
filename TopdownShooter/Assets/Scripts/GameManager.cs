using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public List<string> enemyTags;
        public float timeBetweenSpawns;
    }

    public Transform projectileParent;
    public Transform enemyParent;
    private List<float> playAreaBounds;
    public List<float> PlayAreaBounds { get { return playAreaBounds; } private set {; } }
    private Camera cam;
    public GameObject playerPrefab;
    public GameObject playerObj;

    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber;
    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    public int level = 1;

    private void Awake()
    {
        cam = Camera.main;
        playerObj = Instantiate(playerPrefab, new Vector3(3.0f, 0, -12f), Quaternion.identity);
        PopulatePlayAreaBounds();
    }

    void Start()
    {
        NextWave();
    }

    private void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            GameObject enemyObj = ObjectPooler.Instance.SpawnFromPool(currentWave.enemyTags[Random.Range(0, currentWave.enemyTags.Count)], new Vector3(0, 0, playAreaBounds[0] + 2.0f),
                                                                    Quaternion.Euler(0, 180f, 0), enemyParent);
            Enemy newEnemy = enemyObj.GetComponent<Enemy>();
            float msBetweenShots = 0, muzzleVel = 0, damage = 0, delayBetweenMultipleshots = 0;
            int numOfMultishot = 0;
            if (newEnemy.tag == "enemy1")
            {
                msBetweenShots = 1500 - (currentWaveNumber * 50);
                muzzleVel = 3.0f * level * currentWaveNumber;
                if (muzzleVel > 20f)
                    muzzleVel = 20f;
                if (muzzleVel < 5f)
                    muzzleVel = 5f;
                damage = 2.0f * currentWaveNumber;
            }
            else if(newEnemy.tag == "enemy2")
            {
                msBetweenShots = 1500 + (currentWaveNumber * 50);
                muzzleVel = 3.0f * level * currentWaveNumber;
                if (muzzleVel > 20f)
                    muzzleVel = 20f;
                if (muzzleVel < 5f)
                    muzzleVel = 5f;
                damage = 3.0f * level * (currentWaveNumber / 2);
                numOfMultishot = (2 * level) + currentWaveNumber;
                delayBetweenMultipleshots = 1.0f / (level + currentWaveNumber);
            }
            else if (newEnemy.tag == "enemy4")
            {
                msBetweenShots = 1500 - (currentWaveNumber * 50);
                muzzleVel = 3.0f * level * currentWaveNumber;
                if (muzzleVel > 20f)
                    muzzleVel = 20f;
                if (muzzleVel < 5f)
                    muzzleVel = 5f;
                damage = 3.0f * level * (currentWaveNumber / 2);
                numOfMultishot = (2 * level) + currentWaveNumber;
                delayBetweenMultipleshots = 1.0f / (level + currentWaveNumber);
            }
            Debug.Log("msBetweenShots-> " + msBetweenShots);
            Debug.Log("muzzleVe-> " + muzzleVel);
            Debug.Log("damage-> " + damage);
            newEnemy.Init(msBetweenShots, muzzleVel, damage, numOfMultishot, delayBetweenMultipleshots);
            newEnemy.InitBezier(new Vector3(Random.Range(PlayAreaBounds[2], PlayAreaBounds[3]), 0,
                                    Random.Range(PlayAreaBounds[0], PlayAreaBounds[1])));
            newEnemy.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
        print("Enemy died");
    }

    private void NextWave()
    {
        currentWaveNumber++;
        //level++;
        print("wave: " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    private void SpawnEnemies()
    {
        GameObject enemyObj = ObjectPooler.Instance.SpawnFromPool("enemy1", new Vector3(0, 0, playAreaBounds[0] + 2.0f), Quaternion.Euler(0, 180f, 0), enemyParent);
        Enemy newEnemy = enemyObj.GetComponent<Enemy>();
        newEnemy.InitBezier(new Vector3(Random.Range(PlayAreaBounds[2], PlayAreaBounds[3]), 0,
                                    Random.Range(PlayAreaBounds[0], PlayAreaBounds[1])));
        //enemyObj = ObjectPooler.Instance.SpawnFromPool("enemy4", new Vector3(0, 0, playAreaBounds[0] + 2.0f), Quaternion.Euler(0, 180f, 0), enemyParent);
        //newEnemy = enemyObj.GetComponent<Enemy>();
        //newEnemy.InitBezier(new Vector3(Random.Range(PlayAreaBounds[2], PlayAreaBounds[3]), 0,
        //                    Random.Range(PlayAreaBounds[0], PlayAreaBounds[1])));
    }

    /// <summary>
    /// playarea bounds in worldspace.
    /// </summary>
    private void PopulatePlayAreaBounds()
    {
        playAreaBounds = new List<float>();
        playAreaBounds.Add(Camera.main.orthographicSize);
        playAreaBounds.Add(-Camera.main.orthographicSize);
        float offsetX = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        playAreaBounds.Add(-(Camera.main.transform.position.x + offsetX));
        playAreaBounds.Add(Camera.main.transform.position.x + offsetX);
    }
}
