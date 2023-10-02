using System.Collections;
using UnityEngine;

public class USB : Worm
{
    public GameObject worm;
    [HideInInspector]
    public float currentReloadTime;
    [HideInInspector]
    public bool wait;

    public override void Movement()
    {
        Walk();
        if (currentReloadTime == 0)
            StartCoroutine(WormSpawner());
    }

    public IEnumerator WormSpawner()
    {
        wait = true;
        currentReloadTime = reloadTime * Random.Range(0.75f, 1.5f);
        yield return new WaitForSeconds(1f);
        GameObject newEnemy = Instantiate(worm, transform.position, Quaternion.identity);
        MapGenerator.instance.AddEnemy(newEnemy);
        yield return new WaitForSeconds(1f);
        if (newEnemy != null)
        newEnemy.GetComponent<EnemyBasic>().Initialization();
        wait = false;
        while (currentReloadTime > 0)
        {
            currentReloadTime -= Time.deltaTime;
            yield return null;
        }
        currentReloadTime = 0;
    }
    
    public void Walk()
    {
        if (!wait)
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
        else
            rb.velocity = Vector2.zero;
    }
}
