using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    [SerializeField]
    private Image sprite;
    [SerializeField]
    private Text text;

    public void InitializationSprite(Sprite description)
    {
        text.gameObject.SetActive(false);
        sprite.sprite = description;
    }

    public void InitializationText(string description)
    {
        sprite.gameObject.SetActive(false);
        text.text = description;
    }
}
