using System.Collections;
using UnityEngine;

public abstract class EnemyBasic : MonoBehaviour
{
    public int health;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Vector2 targetPosition;

    public void Initialization()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.instance.gameObject;
        WeaponInit();
    }

    public abstract void WeaponInit();

    private void Update()
    {
        Movement();
    }

    public abstract void WeaponRotateToPlayer();

    public abstract void FreeWeaponRotate();

    public abstract void Movement();

    public bool PlayerIsVisible()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Player"))
                return true;
            if (hits[i].transform.CompareTag("Wall"))
                return false;
        }
        return false;
    }

    public Vector2 RandomPlace(float minimalDistance)
    {
        float radius = MapGenerator.instance.currentRadius;
        float distance;
        Vector2 position;
        do
        {
            position = new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));
            distance = Vector2.Distance(transform.position, position);
        } while (distance < minimalDistance);
        return position;
    }

    public void GetDamage(int damage)
    {
        if (health > damage)
            health -= damage;
        else
            Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
