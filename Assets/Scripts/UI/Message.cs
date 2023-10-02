using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public void Initialization(string description)
    {
        text.text = description;
    }
}
