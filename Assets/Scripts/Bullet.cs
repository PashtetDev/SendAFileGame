using System.Collections;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float lifeTime, speed;
    private Rigidbody2D rb;

    public void Initialization(Vector3 direction)
    {
        rb= GetComponent<Rigidbody2D>();
        transform.eulerAngles = direction;
        StartCoroutine(WaitDestroy(direction.z));
    }

    private IEnumerator WaitDestroy(float direction)
    {
        rb.velocity = new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)).normalized * speed;
        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(particle, transform.position, transform.rotation).GetComponent<ParticleController>().Initialization();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("Bullet"))
        {
            switch (collision.tag)
            {
                case "Wall":
                    Destroy(gameObject);
                    break;
                case "Enemy":
                    collision.GetComponent<EnemyBasic>().GetDamage(damage);
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (collision.tag)
            {
                case "Player":
                    collision.GetComponent<PlayerController>().GetDamage(damage);
                    Destroy(gameObject);
                    break;
                case "Wall":
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
