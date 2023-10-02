using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    private void Awake()
    {
        SetInstance();
        GetComponent<AudioSource>().Play();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "LoseScene" || SceneManager.GetActiveScene().name == "WinnerScene")
        {
            instance = null;
            Destroy(gameObject);
        }
    }
    private void SetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
