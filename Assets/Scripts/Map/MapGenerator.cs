using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int radius;
    private List<Vector2Int> walls;
    [SerializeField]
    private GameObject wall;
    private GameObject wallManager;

    private void Awake()
    {
        Initialization();
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
        for (int i = 0; i < map.Count; i++)
        {
            GameObject newWall = Instantiate(wall, (Vector2)map[i], Quaternion.identity);
            newWall.transform.parent = wallManager.transform;
            newWall.GetComponent<Wall>().Initialization();
        }
    }

    private IEnumerator ResizeMapZone()
    {
        GameObject mapZone = new GameObject("MapZone");
        mapZone.tag = "MapZone";
        //mapZone.transform.parent = transform;
        Rigidbody2D rb = mapZone.AddComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.gravityScale = 0;
        CircleCollider2D circleCollider2D = mapZone.AddComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        circleCollider2D.radius = radius;
        while (circleCollider2D.radius > radius - 2)
        {
            circleCollider2D.radius -= 2 * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        while (circleCollider2D.radius > 5)
        {
            circleCollider2D.radius -= 0.125f * Time.deltaTime;
            yield return null;
        }
    }
}
