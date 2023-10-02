using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject cam;
    [SerializeField]
    private float radius;
    private GameObject player;
    [SerializeField]
    private float shakeDuration, shakeMagnitude;
    public static CameraController instance;

    public void Initialization()
    {
        SetInstance();
        cam = transform.GetChild(0).gameObject;
        player = PlayerController.instance.gameObject;
    }

    private void SetInstance()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (!PlayerController.instance.isLose)
        {
            Vector3 camPosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position) / 2;
            camPosition.z = -10;
            transform.position = player.transform.position + camPosition.normalized * radius;
        }
    }

    public void ShakeCaller()
    {
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 startPosition = cam.transform.localPosition;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            cam.transform.localPosition = startPosition + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) * magnitude;
            yield return null;
        }
        cam.transform.localPosition = startPosition;
    }
}
