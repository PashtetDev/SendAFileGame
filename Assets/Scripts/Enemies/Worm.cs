using System.Collections;
using UnityEngine;

public class Worm : EnemyBasic
{
    public SpriteRenderer sprite;
    [SerializeField]
    private float touchDamage;
    public float reloadTime;
    private Coroutine reload = null;
    public float speed, followingDistance, detectedDistance;

    public override void Movement()
    {
        if (PlayerIsVisible() && PlayerController.instance != null
            && Vector2.Distance(transform.position, player.transform.position) < detectedDistance
            && !PlayerController.instance.isLose)
            WalkToPlayer();
        else
            FreeWalk();
    }

    public void FreeWalk()
    {
        FreeWeaponRotate();
        Debug.DrawRay(transform.position, (Vector3)targetPosition - transform.position, Color.yellow);
        if (Vector2.Distance(transform.position, targetPosition) < 1)
            targetPosition = MapGenerator.instance.RandomPlace(followingDistance, transform.position);
        rb.velocity = ((Vector3)targetPosition - transform.position).normalized * speed;

        if (targetPosition.x > transform.position.x)
            sprite.flipX = true;
        if (targetPosition.x < transform.position.x)
            sprite.flipX = false;
    }

    public void WalkToPlayer()
    {
        WeaponRotateToPlayer();
        if (Vector2.Distance(transform.position, player.transform.position) > followingDistance)
            rb.velocity = (player.transform.position - transform.position).normalized * speed;
        else
            rb.velocity = Vector2.zero;

        if (player.transform.position.x > transform.position.x)
            sprite.flipX = true;
        if (player.transform.position.x < transform.position.x)
            sprite.flipX = false;
    }

    public override void WeaponRotateToPlayer() { }

    public override void WeaponInit() { }

    public override void FreeWeaponRotate() { }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        reload = null;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Enemy"))
            targetPosition = MapGenerator.instance.RandomPlace(followingDistance, transform.position);
        if (collision.transform.CompareTag("Player") && reload == null)
        {
            collision.transform.GetComponent<PlayerController>().GetDamage(touchDamage);
            reload = StartCoroutine(Reload());
        }
    }
}
