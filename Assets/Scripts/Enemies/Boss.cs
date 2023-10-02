using UnityEngine;

public class Boss : USB
{
    public WeaponHolder myWeapon;
    public override void WeaponInit()
    {
        if (myWeapon != null)
            myWeapon.Initialization();
    }

    public override void WeaponRotateToPlayer()
    {
        if (myWeapon != null)
        {
            myWeapon.Rotate(player.transform.position);
            myWeapon.WeaponShot();
        }
    }

    public override void FreeWeaponRotate()
    {
        if (myWeapon != null)
            myWeapon.Rotate(targetPosition);
    }

    public new void Walk()
    {
        if (!wait)
        {
            if (!PlayerController.instance.isLose)
                WeaponRotateToPlayer();
            else
                FreeWeaponRotate();
            Debug.DrawRay(transform.position, (Vector3)targetPosition - transform.position, Color.yellow);
            if (Vector2.Distance(transform.position, targetPosition) < 1)
                targetPosition = MapGenerator.instance.RandomPlace(followingDistance, transform.position);
            rb.velocity = ((Vector3)targetPosition - transform.position).normalized * speed;

            if (player.transform.position.x > transform.position.x)
                sprite.flipX = true;
            if (player.transform.position.x < transform.position.x)
                sprite.flipX = false;
        }
        else
            rb.velocity = Vector2.zero;
    }

    public override void Movement()
    {
        if (!PlayerController.instance.isLose)
        {
            Walk();
            if (currentReloadTime == 0)
                StartCoroutine(WormSpawner());
        }
        else
            base.Walk();
    }
}
