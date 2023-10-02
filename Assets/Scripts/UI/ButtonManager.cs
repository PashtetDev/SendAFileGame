using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject disk, about, howToPlay;
    private bool load;

    private void Awake()
    {
        disk.SetActive(false);
        about.SetActive(false);
        howToPlay.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(WaitAndPlay());
    }

    public void ToMenu()
    {
        StartCoroutine(WaitAndExitToMenu());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void About()
    {
        if (!load)
        {
            about.SetActive(true);
        }
    }

    public void HowToPlay()
    {
        if (!load)
        {
            howToPlay.SetActive(true);
        }
    }

    public void Close()
    {
        about.SetActive(false);
        howToPlay.SetActive(false);
    }

    private IEnumerator WaitAndPlay()
    {
        load = true;
        disk.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game");
    }

    private IEnumerator WaitAndExitToMenu()
    {
        load = true;
        disk.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }

}
