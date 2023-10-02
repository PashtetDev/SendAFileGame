using System.Collections;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particle;

    public void Initialization()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Play();
        StartCoroutine(WaitDestroy());
    }

    private IEnumerator WaitDestroy()
    {
        while (particle.isPlaying)
            yield return null;
        Destroy(gameObject);
    }
}
