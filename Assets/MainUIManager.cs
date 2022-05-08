using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;

public class MainUIManager : MonoBehaviour
{
    public GameObject shop;
    public CameraUtility camera;
    public Text goldText;
    public Text goldPerSecText;
    private void Awake()
    {
        camera = Camera.main.GetComponent<CameraUtility>();
    }
    private void LateUpdate()
    {
        goldText.text = GameManager.Instance.gold < 1000 ? GameManager.Instance.gold.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.gold);
        goldPerSecText.text = GameManager.Instance.goldPerSec < 1000 ? GameManager.Instance.goldPerSec.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.goldPerSec);
    }
    public void BtnEvt_ActiveShop()
    {
        shop.SetActive(!shop.activeSelf);
    }
    public void BtnEvt_Tapping()
    {
        GameManager.Instance.Tapping();
    }
    public void EventTrigger_MoveLeft(bool isLeft)
    {
        camera.isLeft = isLeft;
    }
    public void EventTrigger_MoveRight(bool isRight)
    {
        camera.isRight = isRight;
    }
}
