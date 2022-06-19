using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("팁 적는 칸")]
    public string[] tips;

    public static string nextScene;

    public Text loading;
    public Text percent;
    public Text tip;
    public Image loadingBar;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Co_Loading());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }
    private IEnumerator Co_LoadingText()
    {
        int dotCount = 0;
        while (true)
        {
            if(dotCount > 2)
            {
                loading.text = "로딩 중..";
                dotCount = 0;
            }
            else
            {
                loading.text += ".";
                dotCount++;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator Co_Loading()
    {
        StartCoroutine(Co_LoadingText());
        tip.text = tips[Random.Range(0, tips.Length)];
        float timer = 0f;
        while(loadingBar.fillAmount < 1)
        {
            yield return null;
            timer += 0.0005f;
            loadingBar.fillAmount = timer;
        }
        SceneManager.LoadScene(nextScene);
 
      /*  AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);  -> 데이터 연결 작업 후 수정 (6/19)
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress, timer);
                if (loadingBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timer);
                if (loadingBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }*/
    }
    private void Update()
    {
        percent.text = (loadingBar.fillAmount * 100).ToString("0") + "%";
    }
}
