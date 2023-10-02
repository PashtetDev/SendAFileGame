using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemFactor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject msgPosition;
    [SerializeField]
    private GameObject msgBox;
    [SerializeField]
    private Color buff, debuff;
    private string description;
    private GameObject currentMessage;

    private void Awake()
    {
        Item myItem = GetComponent<ItemHolder>().myItem;
        Text txt = GetComponent<Text>();
        description = myItem.description;
        txt.text = myItem.itemName;
        if (myItem.buff)
            txt.color = buff;
        else
            txt.color = debuff;
    }

    public string Description()
    {
        return description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentMessage = Instantiate(msgBox, msgPosition.transform.position, Quaternion.identity);
        currentMessage.transform.parent = msgPosition.transform;
        currentMessage.GetComponent<Message>().Initialization(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(currentMessage);
        currentMessage = null;
    }
}
