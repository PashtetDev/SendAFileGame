using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemFactor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject msgPosition, bottomPosition;
    [SerializeField]
    private GameObject msgBox, image;
    [SerializeField]
    private Color buff, debuff;
    private string description;
    private string gameMeaning;
    private Sprite sprite;
    private GameObject currentMessage, currentDescription;

    private void Awake()
    {
        Item myItem = GetComponent<ItemHolder>().myItem;
        Text txt = GetComponent<Text>();
        description = myItem.description;
        sprite = myItem.sprite;
        gameMeaning = myItem.gameMeaning;
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
        GameObject canvas;
        currentMessage = Instantiate(msgBox, Vector3.zero, Quaternion.identity);
        canvas = currentMessage;
        currentMessage = currentMessage.transform.GetChild(0).gameObject;
        currentMessage.transform.position = msgPosition.transform.position;
        currentMessage.transform.parent = msgPosition.transform;
        currentMessage.GetComponent<Message>().Initialization(description);
        Destroy(canvas);

        currentDescription = Instantiate(image, Vector3.zero, Quaternion.identity);
        canvas = currentDescription;
        currentDescription = currentDescription.transform.GetChild(0).gameObject;
        currentDescription.transform.position = bottomPosition.transform.position;
        currentDescription.transform.parent = bottomPosition.transform;
        if (sprite != null)
            currentDescription.GetComponent<Description>().InitializationSprite(sprite);
        else
            currentDescription.GetComponent<Description>().InitializationText(gameMeaning);
        Destroy(canvas);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(currentMessage);
        currentMessage = null;
        Destroy(currentDescription);
        currentDescription = null;
    }
}
