using System.Collections;
using UnityEngine;

public class Bug : EnemyBasic
{
    public float followingDistance, detectedDistance;
    private float jumpTime;
    private bool activated;
    [SerializeField]
    private GameObject electricZone;
    private GameObject currentZone;

    public override void Movement()
    {
        if (!activated)
        {
            activated = true;
            StartCoroutine(RandomJump());
        }
        else
        {
            if (Vector2.Distance(player.transform.position, transform.position) < followingDistance)
                BugJump();
        }
    }

    public IEnumerator RandomJump()
    {
        while (true)
        {
            while (jumpTime > 0)
            {
                yield return null;
                jumpTime -= Time.deltaTime;
            }
            BugJump();
        }
    }

    public override void WeaponInit() { }

    public override void WeaponRotateToPlayer() { }

    public override void FreeWeaponRotate() { }

    private void BugJump()
    {
        jumpTime = Random.Range(1f, 3f);
        transform.position = MapGenerator.instance.RandomPlace(Random.Range(1f, 4f), transform.position);
        currentZone = Instantiate(electricZone, transform.position, Quaternion.identity);
        currentZone.GetComponent<ElectricZone>().Initialization();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
            BugJump();
    }
}
