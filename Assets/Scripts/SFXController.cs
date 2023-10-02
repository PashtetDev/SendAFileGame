using System.Collections;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    private AudioSource sound;

    public void Initialization(AudioClip audioClip)
    {
        sound = GetComponent<AudioSource>();
        sound.clip = audioClip;
        sound.Play();
        StartCoroutine(WaitDestroy());
    }

    private IEnumerator WaitDestroy()
    {
        while (sound.isPlaying)
            yield return null;
        Destroy(gameObject);
    }
}
