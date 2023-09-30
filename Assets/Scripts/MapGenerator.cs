using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int radius;
    public float currentRadius;
    private List<Vector2Int> walls;
    private List<GameObject> wallsSubjects;
    [SerializeField]
    private GameObject wall;
    private GameObject wallManager;
    [SerializeField]
    private float resizeSpeed, minRadius;
    [SerializeField]
    private List<EnemyBasic> enemies;
    public static MapGenerator instance;
    public PlayerController player;

    private void Awake()
    {
        SetInstance();
        player.Initialization();
        Initialization();
        EnemyInitializator();
        gameObject.SetActive(false);
    }

    private void SetInstance()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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

    private void EnemyInitializator()
    {
        for (int i = 0; i < enemies.Count; i++)
            if (enemies[i] != null)
                enemies[i].Initialization();
    }

    private IEnumerator ResizeMapZone()
    {
        currentRadius = radius - 1;
        for (int i = 0; i < wallsSubjects.Count; i++)
            wallsSubjects[i].SetActive(false);
        while (currentRadius > minRadius)
        {
            for (int i = 0; i < wallsSubjects.Count; i++)
            {
                if (Vector2.Distance((Vector2)wallsSubjects[i].transform.position, Vector2.zero) >= currentRadius)
                    wallsSubjects[i].SetActive(true);
                if (Vector2.Distance((Vector2)wallsSubjects[i].transform.position, Vector2.zero) > currentRadius + 1)
                    wallsSubjects[i].SetActive(false);
            }
            if (currentRadius == radius - 1)
                yield return new WaitForSeconds(2);
            currentRadius -= (1 / (resizeSpeed + 1)) * Time.deltaTime;
            yield return null;
        }
    }
}
