using UnityEngine;
using UnityEngine.UI;

public class TitleChecker : MonoBehaviour
{
    private void Awake()
    {
        Text text = GetComponent<Text>();
        if (PlayerController.instance.myInventory.level < 10)
            text.text = "Загрузка...";
        else
            text.text = "???????";
    }
}
