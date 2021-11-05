using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField]
    private Image loadingBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while(!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if(op.progress <0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress, timer);
                if(loadingBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timer);
                if(loadingBar.fillAmount == 1.0f)
                {
                    Player.inst.rigidbody.useGravity = true;
                    Player.inst.isMove = false;
                    Player.inst.animator.SetBool("isMove", false);
                    op.allowSceneActivation = true;
                    
                    yield break;
                }
            }
        }
    }
}
