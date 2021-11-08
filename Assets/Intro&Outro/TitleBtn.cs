using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleBtn : MonoBehaviour
{
    [SerializeField]
    private Image startFade;

    public void GameStart()
    {
        StartCoroutine(StartFade());
        SceneManager.LoadScene("Intro");
    }
   
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator StartFade()
    {
        yield return null;
        Color color = startFade.color;
        color.a = 0;
    }

}
