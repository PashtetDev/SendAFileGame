using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private GameObject boss;
    [SerializeField]
    private GameObject portal;
    [SerializeField]
    private List<GameObject> enemiesInstance;
    private List<GameObject> enemies;

    [SerializeField]
    private int radius;
    [HideInInspector]
    public float currentRadius;
    private List<Vector2Int> walls;
    private List<GameObject> wallsSubjects;
    [SerializeField]
    private GameObject wall;
    private GameObject wallManager;
    [SerializeField]
    private float resizeSpeed, minRadius;
    public static MapGenerator instance;
    private PlayerController player;
    public UIDrawer uiDrawer;

    private void Awake()
    {
        SetInstance();
        uiDrawer.Initialization();
        currentRadius = radius - 1;
        StartCoroutine(InitializationCorotine());
    }

    private IEnumerator InitializationCorotine()
    {
        if (PlayerController.instance == null)
        {
            player = Instantiate(playerObject, Vector2.zero, Quaternion.identity).GetComponent<PlayerController>();
            player.Initialization();
        }
        if (PlayerController.instance.myInventory.level < 11)
            EnemyFiller();
        else
            BossIn();
        Initialization();
        float duration = 1.5f;
        float percent = 0;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
            percent += Random.Range(25f, 75f) * Time.deltaTime;
            uiDrawer.Load(percent);
        }
        if (percent < 100)
            uiDrawer.Load(100);
        yield return new WaitForSeconds(0.5f);
        EnemyInitializator();
    }

    private void SetInstance()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void BossIn()
    {
        enemies = new List<GameObject>();
        GameObject enemyManager = new GameObject("EnemyManager");
        enemyManager.transform.parent = transform;
        GameObject newEnemy = Instantiate(boss, RandomPlace(7, Vector3.zero), Quaternion.identity);
        enemies.Add(newEnemy);
        newEnemy.transform.parent = enemyManager.transform;
    }

    private void EnemyFiller()
    {
        enemies = new List<GameObject>();
        GameObject enemyManager = new GameObject("EnemyManager");
        enemyManager.transform.parent = transform;
        int enemyTypes = enemiesInstance.Count;
        if (PlayerController.instance.myInventory.level < enemiesInstance.Count)
            enemyTypes = PlayerController.instance.myInventory.level;
        int enemyCount = (int)(3f * Mathf.Log(PlayerController.instance.myInventory.level + 3));
        if (PlayerController.instance.myInventory.oldVersion)
            enemyCount = (int)(1.5f * enemyCount) + Random.Range(0, 3);

        while (enemies.Count < enemyCount)
        {
            int enemyNumber = Random.Range(0, enemyTypes); 
            GameObject newEnemy;
            newEnemy = Instantiate(enemiesInstance[enemyNumber], RandomPlace(5, Vector3.zero), Quaternion.identity);
            enemies.Add(newEnemy);
            newEnemy.transform.parent = enemyManager.transform;
        }
    }

    private void EnemyInitializator()
    {
        for (int i = 0; i < enemies.Count; i++)
            if (enemies[i] != null)
                enemies[i].GetComponent<EnemyBasic>().Initialization();
    }

    public Vector2 RandomPlace(float minimalDistance, Vector3 origin)
    {
        float radius = instance.currentRadius - 2;
        float distance;
        Vector2 position;
        do
        {
            position = new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius)).normalized * radius;
            distance = Vector2.Distance(origin, position);
        } while (distance < minimalDistance);
        return position;
    }

    public void Initialization()
    {
        walls = new List<Vector2Int>();
        wallManager = new GameObject("Wall");
        wallManager.transform.parent = transform;

        RoomCreator();
        DrawMap(walls);
        StartCoroutine(ResizeMapZone());

        gameObject.name = "Map";
    }

    public void AddEnemy(GameObject newEnemy)
    {
        enemies.Add(newEnemy);
    }

    private void RoomCreator()
    {
        for (int i = -radius + 1; i < radius; i++)
        {
            for (int j = -radius + 1; j < radius; j++)
            {
                if (Vector2Int.Distance(new Vector2Int(i, j), Vector2Int.zero) < radius)
                    walls.Add(new Vector2Int(i, j));
            }
        }
    }

    private void DrawMap(List<Vector2Int> map)
    {
        wallsSubjects = new List<GameObject>();
        for (int i = 0; i < map.Count; i++)
        {
            GameObject newWall = Instantiate(wall, (Vector2)map[i], Quaternion.identity);
            newWall.transform.parent = wallManager.transform;
            wallsSubjects.Add(newWall);
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            StartCoroutine(PortalSpawn());
        }
    }

    private IEnumerator PortalSpawn()
    {
        yield return new WaitForSeconds(1);
        Instantiate(portal, Vector3.zero, Quaternion.identity);
    }

    private IEnumerator ResizeMapZone()
    {
        for (int i = 0; i < wallsSubjects.Count; i++)
            wallsSubjects[i].SetActive(false);
        while (currentRadius > minRadius)
        {
            for (int i = 0; i < wallsSubjects.Count; i++)
            {
                if (Vector2.Distance((Vector2)wallsSubjects[i].transform.position, Vector2.zero) >= currentRadius)
                    wallsSubjects[i].SetActive(true);
                if (Vector2.Distance((Vector2)wallsSubjects[i].transform.position, Vector2.zero) > currentRadius + 1)
                    wallsSubjects[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            if (currentRadius == radius - 1)
                yield return new WaitForSeconds(2);
            currentRadius -= resizeSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
