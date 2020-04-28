using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform projectileParent;
    public Transform enemyParent;
    private List<float> playAreaBounds;
    public List<float> PlayAreaBounds { get { return playAreaBounds; } private set {; } }
    private Camera cam;
    public GameObject playerPrefab;
    public GameObject playerObj;

    void Start()
    {
        cam = Camera.main;
        playerObj = Instantiate(playerPrefab, new Vector3(1.0f, 0, 0), playerPrefab.transform.rotation);
        PopulatePlayAreaBounds();
        //SpawnEnemies();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        GameObject enemyObj = ObjectPooler.Instance.SpawnFromPool("enemy1", new Vector3(0, 0, playAreaBounds[0] + 2.0f), Quaternion.Euler(0, 180f, 0), enemyParent);
        Enemy newEnemy = enemyObj.GetComponent<Enemy>();
        newEnemy.InitBezier(new Vector3(Random.Range(PlayAreaBounds[2], PlayAreaBounds[3]), 0,
                                    Random.Range(PlayAreaBounds[0], PlayAreaBounds[1])));
        enemyObj = ObjectPooler.Instance.SpawnFromPool("enemy4", new Vector3(0, 0, playAreaBounds[0] + 2.0f), Quaternion.Euler(0, 180f, 0), enemyParent);
        newEnemy = enemyObj.GetComponent<Enemy>();
        newEnemy.InitBezier(new Vector3(Random.Range(PlayAreaBounds[2], PlayAreaBounds[3]), 0,
                            Random.Range(PlayAreaBounds[0], PlayAreaBounds[1])));
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
