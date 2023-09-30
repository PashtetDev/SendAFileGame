using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : EnemyBasic
{
    public float speed, followingDistance, detectedDistance;

    public override void Movement()
    {
        if (PlayerIsVisible() && PlayerController.instance != null
            && Vector2.Distance(transform.position, player.transform.position) < detectedDistance)
            WalkToPlayer();
        else
            FreeWalk();
    }

    public void FreeWalk()
    {
        FreeWeaponRotate();
        Debug.DrawRay(transform.position, (Vector3)targetPosition - transform.position, Color.yellow);
        if (Vector2.Distance(transform.position, targetPosition) < 1)
            targetPosition = RandomPlace(followingDistance);
        rb.velocity = ((Vector3)targetPosition - transform.position).normalized * speed;
    }

    public void WalkToPlayer()
    {
        WeaponRotateToPlayer();
        if (Vector2.Distance(transform.position, player.transform.position) > followingDistance)
            rb.velocity = (player.transform.position - transform.position).normalized * speed;
        else
            rb.velocity = Vector2.zero;
    }

    public override void WeaponRotateToPlayer() { }

    public override void WeaponInit() { }

    public override void FreeWeaponRotate() { }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Enemy"))
            targetPosition = RandomPlace(followingDistance);
    }
}
