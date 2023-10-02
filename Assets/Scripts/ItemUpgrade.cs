using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemUpgrade : MonoBehaviour
{
    [SerializeField]
    private Color buff, debuff;
    [SerializeField]
    private List<Item> items;
    private int count;
    [SerializeField]
    private GameObject lenta;
    [SerializeField]
    private GameObject textArea;
    private List<GameObject> factors;
    private Vector3 startPosition;
    private bool initialized;

    private void Awake()
    {
        Initialization();
    }

    public void Initialization()
    {
        startPosition = lenta.transform.position;
        factors = new List<GameObject>();
        List<Item> newItems = new List<Item>(items);
        if (PlayerController.instance != null)
        {
            items.Clear();
            for (int i = 0; i < newItems.Count; i++)
            {
                switch (newItems[i].type)
                {
                    case PlayerController.Item.publicNet:
                        if (!PlayerController.instance.myInventory.publicNet)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.antivirus:
                        if (!PlayerController.instance.myInventory.antivirus)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.oldVersion:
                        if (!PlayerController.instance.myInventory.oldVersion)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.optialFiber:
                        if (!PlayerController.instance.myInventory.optialFiber)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.archive:
                        if (!PlayerController.instance.myInventory.archive)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.exploit:
                        if (!PlayerController.instance.myInventory.exploit)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.https:
                        if (!PlayerController.instance.myInventory.https)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.damagedHDD:
                        if (!PlayerController.instance.myInventory.damagedHDD)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.weakPassword:
                        if (!PlayerController.instance.myInventory.weakPassword)
                            items.Add(newItems[i]);
                        break;
                    case PlayerController.Item.cryptoDurability:
                        if (!PlayerController.instance.myInventory.cryptoDurability)
                            items.Add(newItems[i]);
                        break;
                    default: break;
                }
            }
        }
        count = 0;
        StartCoroutine(Spawn());
        initialized = true;
    }

    private void Update()
    {
        if (count < (int)Mathf.Abs((lenta.transform.position.y - startPosition.y) / 300) && initialized)
        {
            count++;
            GameObject newItem = Instantiate(textArea, transform.position, Quaternion.identity);
            newItem.transform.parent = lenta.transform;
            factors.Add(newItem);
            Item item = items[Random.Range(0, items.Count)];
            newItem.GetComponent<ItemHolder>().myItem = item;
            newItem.GetComponent<Text>().text = item.itemName;
            if (item.buff)
                newItem.GetComponent<Text>().color = buff;
            else
                newItem.GetComponent<Text>().color = debuff;
            if (factors.Count > 5)
            {
                Destroy(factors[0]);
                factors.RemoveAt(0);
            }
        }
    }

    private IEnumerator Spawn()
    {
        float speed = 0;
        while (speed < 100)
        {
            lenta.transform.position -= new Vector3(0, speed, 0);
            speed += Time.deltaTime * 100;
            yield return null;
        }
        while (speed > 0)
        {
            lenta.transform.position -= new Vector3(0, speed, 0);
            speed -= Time.deltaTime * 32f;
            yield return null;
        }
        StartCoroutine(NewFactor());
    }

    private IEnumerator NewFactor()
    {
        GameObject newFactor = factors[0];
        float minDistance = Vector2.Distance(factors[0].transform.position, startPosition);
        for (int i = 0; i < factors.Count; i++)
        {
            if (Vector2.Distance(factors[i].transform.position, startPosition) < minDistance)
            {
                newFactor = factors[i];
                minDistance = Vector2.Distance(factors[i].transform.position, startPosition);
            }
        }
        while (Vector2.Distance(newFactor.transform.position, startPosition) > 10)
        {
            yield return null;
            newFactor.transform.position = Vector2.MoveTowards(newFactor.transform.position, startPosition, 100 * Time.deltaTime);
        }
        float scale = 1;
        yield return new WaitForSeconds(0.5f);
        while (scale < 1.25f)
        {
            newFactor.transform.localScale = Vector3.one * scale;
            scale += 0.5f * Time.deltaTime;
            yield return null;
        }

        if (PlayerController.instance != null)
            PlayerController.instance.AddItem(newFactor.GetComponent<ItemHolder>().myItem.type);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game");
    }
}
