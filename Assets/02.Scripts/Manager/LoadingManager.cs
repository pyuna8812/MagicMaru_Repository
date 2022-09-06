using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("ÆÁ Àû´Â Ä­")]
    public string[] tips;

    public static string nextScene;
    public GameObject loadingObj;
    public GameObject firstCut;
    public GameObject secondCut;
    public Image fadeOut;
    public Text loading;
    public Text percent;
    public Text tip;
    public Image loadingBar;

    // Start is called before the first frame update
    private void Start()
    {
        bool test = false;
        test = bool.Parse(PlayerPrefs.GetString(DataManager.DATA_PATH_SECONDENTER, "false"));
        loadingObj.SetActive(test);
        firstCut.SetActive(!test);
        fadeOut.gameObject.SetActive(!test);
        if (!test)
        {
            StartCoroutine(Co_PlayComics());
        }
        else
        {
            StartCoroutine(Co_Loading());
        }
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
                loading.text = "·Îµù Áß..";
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
            timer += Time.deltaTime * 0.3f;
            loadingBar.fillAmount = timer;
        }
        SceneManager.LoadScene(nextScene);
        SoundManager.instance.PlayBGM(1);
    }
    private IEnumerator Co_PlayComics()
    {
        for (int i = 0; i < 3; i++)
        {
            firstCut.transform.GetChild(i).DOMove(Vector3.zero, 2f);
            yield return new WaitForSeconds(2f);
        }
        firstCut.transform.GetChild(3).GetComponent<Image>().DOColor(Color.white, 2f);
        yield return new WaitForSeconds(2f);
        firstCut.transform.DOMoveY(12, 1f);
        yield return new WaitForSeconds(1f);
        secondCut.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            secondCut.transform.GetChild(i).DOMove(Vector3.zero, 2f);
            yield return new WaitForSeconds(2f);
        }
        secondCut.transform.DOMoveY(12, 1f);
        yield return new WaitForSeconds(1f);
        loadingObj.SetActive(true);
        fadeOut.DOColor(Color.clear, 1f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Co_Loading());
    }
    private void Update()
    {
        percent.text = (loadingBar.fillAmount * 100).ToString("0") + "%";
    }
}
