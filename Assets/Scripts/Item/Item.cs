using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;
    public PlayerController.Item type;
    public bool buff;

    public bool haveImage;
    public Sprite sprite;
    public string gameMeaning;
}
