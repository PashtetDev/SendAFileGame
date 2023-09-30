using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private GameObject sprite;
    private BoxCollider2D _collider;

    public void Initialization()
    {
        _collider = GetComponent<BoxCollider2D>();
        sprite.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MapZone"))
        {
            _collider.enabled = true;
            sprite.SetActive(true);
        }
    }
}
