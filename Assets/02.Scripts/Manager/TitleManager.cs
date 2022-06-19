using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public RectTransform logo;
    public RectTransform maru;
    public RectTransform monster;
    public Image start;
    private bool sceneMove;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_TitleTweening());        
    }
    private IEnumerator Co_TitleTweening()
    {
        monster.DOLocalMove(Vector3.zero, 1f);
        yield return new WaitForSeconds(1f);
        maru.DOLocalMove(Vector3.zero, 1f);
        yield return new WaitForSeconds(1f);
        logo.DOLocalMove(new Vector3(20.6f, 196), 2.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(2.5f);
        sceneMove = true;
        while (true)
        {
            start.DOColor(Color.white, 1f);
            yield return new WaitForSeconds(1f);
            start.DOColor(Color.clear, 1f);
            yield return new WaitForSeconds(1f);
        }
    }
    private void Update()
    {
        if (sceneMove)
        {
            if(Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                LoadingManager.LoadScene("Main");
            }
        }
    }
}
