using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawer : MonoBehaviour
{
    [SerializeField]
    private Image health;
    [SerializeField]
    private Image healthBackground;
    public static UIDrawer instance;
    private Coroutine lower;
    [SerializeField]
    private Text percents;
    [SerializeField]
    private Image percentImage;
    [SerializeField]
    private GameObject load;

    public void Initialization()
    {
        SetInstance();
        load.SetActive(true);
    }

    public void Load(float percent)
    {
        percentImage.fillAmount = percent / 100;
        percents.text = System.Convert.ToString((int)percent) + "%";
        if (percent >= 100)
            StartCoroutine(HideLoad());
    }

    private IEnumerator HideLoad()
    { 
        yield return new WaitForSeconds(0.5f);
        load.SetActive(false);
        PlayerController.instance.activated = true;
    }


    private void SetInstance()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateUI(float relation)
    {
        health.fillAmount = relation;
        if (lower == null)
            lower = StartCoroutine(HealthLower());
    }

    private IEnumerator HealthLower()
    {
        float speed = 0;
        while (healthBackground.fillAmount > health.fillAmount)
        {
            speed += Time.deltaTime;
            healthBackground.fillAmount -= speed;
            yield return null;
        }
        healthBackground.fillAmount = health.fillAmount;
        lower = null;
    }
}
