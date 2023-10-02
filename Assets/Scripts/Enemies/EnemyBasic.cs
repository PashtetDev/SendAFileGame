using System.Collections;
using UnityEngine;

public abstract class EnemyBasic : MonoBehaviour
{
    [SerializeField]
    private GameObject deadParticles;
    public float health;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Vector2 targetPosition;

    public void Initialization()
    {
        StartCoroutine(SpawnInitialization());
    }

    private IEnumerator SpawnInitialization()
    {
        rb = GetComponent<Rigidbody2D>();
        while (PlayerController.instance == null)
            yield return null;
        player = PlayerController.instance.gameObject;
        WeaponInit();
    }

    public abstract void WeaponInit();

    private void Update()
    {
        if (player != null)
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

    public void GetDamage(float damage)
    {
        if (health > damage)
            health -= damage;
        else
            Death();
    }

    private void Death()
    {
        Instantiate(deadParticles, transform.position, Quaternion.identity).GetComponent<ParticleController>().Initialization();
        MapGenerator.instance.DestroyEnemy(gameObject);
    }
}
