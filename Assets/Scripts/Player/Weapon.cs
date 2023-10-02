using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private AudioClip shotClip;
    [SerializeField]
    private GameObject shotSFX;
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private float reloadTime, spread;
    private float currentReloadTime;
    [SerializeField]
    private GameObject bullet, dulo;
    public static Weapon instance;

    public void Initialization()
    {
        if (instance == null)
            instance = this;
    }

    public void Shot(Vector3 direction)
    {
        if (currentReloadTime == 0)
        {
            Instantiate(shotSFX, transform.position, Quaternion.identity).GetComponent<SFXController>().Initialization(shotClip);
            if (instance == this)
                CameraController.instance.ShakeCaller();
            particle.Play();
            GameObject newBullet = Instantiate(bullet, dulo.transform.position, Quaternion.identity);
            newBullet.GetComponent<Bullet>().Initialization(direction + new Vector3(0, 0, Random.Range(-spread / 2, spread / 2)));
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        if (instance == this)
        {
            currentReloadTime = reloadTime;
            if (PlayerController.instance.myInventory.optialFiber)
                currentReloadTime = reloadTime / 2;
        }
        else
            currentReloadTime = reloadTime * Random.Range(1f, 3f);
        while (currentReloadTime > 0)
        {
            currentReloadTime -= Time.deltaTime;
            yield return null;
        }
        currentReloadTime = 0;
    }
}
