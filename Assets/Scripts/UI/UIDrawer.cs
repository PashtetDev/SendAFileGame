using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawer : MonoBehaviour
{
    [SerializeField]
    private Image health, healthBackground;
    [SerializeField]
    private Image shield, shieldBackground;
    public static UIDrawer instance;
    private Coroutine lowerHp, lowerSh;
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
        if (lowerHp == null)
            lowerHp = StartCoroutine(HealthLower());
    }

    public void UpdateUIShield(float relation)
    {
        if (shield.fillAmount > 0)
            shield.transform.parent.gameObject.SetActive(true);
        shield.fillAmount = relation;
        if (lowerSh == null)
            lowerSh = StartCoroutine(ShieldLower());
    }

    private IEnumerator HealthLower()
    {
        float speed = 0;
        while (healthBackground.fillAmount > health.fillAmount)
        {
            speed += 10 * Time.deltaTime;
            healthBackground.fillAmount -= speed * Time.deltaTime;
            yield return null;
        }
        healthBackground.fillAmount = health.fillAmount;
        lowerHp = null;
    }

    private IEnumerator ShieldLower()
    {
        float speed = 0;
        while (shieldBackground.fillAmount > shield.fillAmount)
        {
            speed += 10 * Time.deltaTime;
            shieldBackground.fillAmount -= speed * Time.deltaTime;
            yield return null;
        }
        shieldBackground.fillAmount = shield.fillAmount;
        lowerSh = null;
        if (shield.fillAmount == 0)
            shield.transform.parent.gameObject.SetActive(false);
    }
}
