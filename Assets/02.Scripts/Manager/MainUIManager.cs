using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;

public class MainUIManager : MonoBehaviour
{
    public GameObject shop;
    public GameObject skillPopUp;
    public CameraUtility camera;
    public Text goldText;
    public Text goldPerSecText;
    public Slider playerHpSlider;
    public Player player;
    //public Text tapGoldText;
    private void Awake()
    {
        camera = Camera.main.GetComponent<CameraUtility>();
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdatePlayerHpBar());
    }
    private void LateUpdate()
    {
        goldText.text = GameManager.Instance.gold < 1000 ? GameManager.Instance.gold.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.gold);
        goldPerSecText.text = GameManager.Instance.goldPerSec < 1000 ? GameManager.Instance.goldPerSec.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.goldPerSec);
       // tapGoldText.text = GameManager.Instance.tapGold < 1000 ? GameManager.Instance.tapGold.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.tapGold);
    }
    public void BtnEvt_ActiveShop()
    {
        shop.SetActive(!shop.activeSelf);
    }
    public void BtnEvt_ActiveSkillPopUp()
    {
        skillPopUp.SetActive(!skillPopUp.activeSelf);
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
    private IEnumerator Co_UpdatePlayerHpBar()
    {
        yield return new WaitUntil(() => player.commonStatus.currentHp != 0);
        while (true)
        {
            yield return null;
            playerHpSlider.value = player.commonStatus.currentHp;
        }
    }
}
