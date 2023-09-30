using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricZone : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float lifeDuration, reloadTime;
    private bool playerInZone;
    private Coroutine currentCorotine;

    public void Initialization()
    {
        playerInZone = false;
        StartCoroutine(WaitDestroy());
    }

    private IEnumerator WaitDestroy()
    {
        while (lifeDuration > 0)
        {
            lifeDuration -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator Visitor(GameObject player)
    {
        playerInZone = true;
        while (playerInZone)
        {
            yield return new WaitForSeconds(reloadTime);
            if (playerInZone)
            {
                Debug.Log("Bzzzz");
                player.GetComponent<PlayerController>().GetDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerInZone)
        {
            currentCorotine = StartCoroutine(Visitor(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerInZone)
        {
            playerInZone = false;
            if (currentCorotine != null)
                StopCoroutine(currentCorotine);
            currentCorotine = null;
        }
    }
}
