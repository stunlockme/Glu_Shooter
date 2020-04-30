using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Transform powerupParent;
    private List<float> playAreaBounds;
    public List<float> PlayAreaBounds { get { return playAreaBounds; } private set {; } }
    private Camera cam;
    public GameObject playerPrefab;
    public Text scoreText;
    public Text waveNumberText;
    public Text levelNumberText;
    public int score;
    public int highScore;
    public float nextPowerupTime;
    public string[] powerupTags;
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
        level = PlayerPrefs.GetInt("LevelNumber", 1);
        Debug.Log("levelNumber-> " + level);
        scoreText.text = "Score-> " + score.ToString();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        Debug.Log("highScore-> " + highScore);
        NextWave();
    }

    private void Update()
    {
        SpawnEnemy();
        SpawnPowerup();
    }

    private void SpawnPowerup()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextPowerupTime)
        {
            nextPowerupTime = Time.time + nextPowerupTime;
            string powerupTag = powerupTags[Random.Range(0, powerupTags.Length)];
            GameObject powerupObj = ObjectPooler.Instance.SpawnFromPool(powerupTag, new Vector3(Random.Range(playAreaBounds[2], playAreaBounds[3]), 0, playAreaBounds[0] + 2.0f),
                                                                    Quaternion.Euler(0, 180f, 0), powerupParent);
            Debug.Log("spawning powerup-> " + powerupObj);
        }
    }

    private void SpawnEnemy()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            GameObject enemyObj = ObjectPooler.Instance.SpawnFromPool(currentWave.enemyTags[Random.Range(0, currentWave.enemyTags.Count)], new Vector3(Random.Range(playAreaBounds[2], playAreaBounds[3]), 0, playAreaBounds[0] + 2.0f),
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
                damage = 3.0f + level + currentWaveNumber; ;
                numOfMultishot = (2 * level) + currentWaveNumber;
                delayBetweenMultipleshots = 1.0f / (level + currentWaveNumber);
            }
            else if (newEnemy.tag == "enemy3")
            {
                msBetweenShots = 1500 + (currentWaveNumber * 50);
                muzzleVel = 3.0f * level * currentWaveNumber;
                if (muzzleVel > 20f)
                    muzzleVel = 20f;
                if (muzzleVel < 5f)
                    muzzleVel = 5f;
                damage = 4.0f + level + currentWaveNumber;
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
                damage = 5.0f + level + currentWaveNumber;
                numOfMultishot = (2 * level) + currentWaveNumber;
                delayBetweenMultipleshots = 1.0f / (level + currentWaveNumber);
            }
            else if (newEnemy.tag == "enemy5")
            {
                msBetweenShots = 1500 + (currentWaveNumber * 50);
                muzzleVel = 3.0f * level * currentWaveNumber;
                if (muzzleVel > 20f)
                    muzzleVel = 20f;
                if (muzzleVel < 5f)
                    muzzleVel = 5f;
                damage = 4.0f + level + currentWaveNumber;
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
        waveNumberText.text = "Wave-> " + currentWaveNumber.ToString();
        //level++;
        print("wave: " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
        else
        {
            currentWaveNumber = 0;
            NextWave();
            level++;
            Player player = playerObj.GetComponent<Player>();
            player.healthBar.CurrentValue = player.healthBar.MaxValue;
        }
        levelNumberText.text = "Level-> " + level.ToString();
        if(level == 2)
        {
            PlayerPrefs.SetInt("Level2", 1);
            Debug.Log("setting level2");
        }
        else if(level == 3)
        {
            PlayerPrefs.SetInt("Level3", 1);
        }
        else if (level == 4)
        {
            PlayerPrefs.SetInt("Level4", 1);
        }
        else if (level == 5)
        {
            PlayerPrefs.SetInt("Level5", 1);
        }
        print("level-> " + level);
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
