using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CAH.GameSystem.BigNumber;
using DG.Tweening;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager instance;
    public GameObject shop;
    public GameObject skillPopUp;
    public CameraUtility camera;
    public Text goldText;
    public Text goldPerSecText;
    public Slider playerHpSlider;
    public Text playerHpText;
    public List<Transform> playerDamageUI = new List<Transform>();
    public List<Transform> monsterDamageUI = new List<Transform>();
    private Vector3 playerDamageUIPos;
    private Vector3 monsterDamageUIPos;
    //public Text tapGoldText;
    private void Awake()
    {
        instance = this;
        camera = Camera.main.GetComponent<CameraUtility>();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdatePlayerHpBar());
    }
    private void LateUpdate()
    {
        goldText.text = GameManager.Instance.gold < 1000 ? GameManager.Instance.gold.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.gold);
        goldPerSecText.text = GameManager.Instance.goldPerSec < 1000 ? GameManager.Instance.goldPerSec.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.goldPerSec);
        foreach (var item in playerDamageUI)
        {
            if (item.gameObject.activeSelf)
            {
                item.position = Camera.main.WorldToScreenPoint(playerDamageUIPos);
                playerDamageUIPos = new Vector3(playerDamageUIPos.x, playerDamageUIPos.y + 0.001f);
            }
        }
        foreach (var item in monsterDamageUI)
        {
            if (item.gameObject.activeSelf)
            {
                item.position = Camera.main.WorldToScreenPoint(monsterDamageUIPos);
                monsterDamageUIPos = new Vector3(monsterDamageUIPos.x, monsterDamageUIPos.y + 0.001f);
            }
        }

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
    public void BtnEvt_FixCam()
    {
        FixCam();
    }
    public void EventTrigger_MoveLeft(bool isLeft)
    {
        camera.isLeft = isLeft;
    }
    public void EventTrigger_MoveRight(bool isRight)
    {
        camera.isRight = isRight;
    }
    private void FixCam()
    {
        camera.transform.position = new Vector3(Player.instance.transform.position.x, 0, -10);
    }
    private IEnumerator Co_UpdatePlayerHpBar()
    {
        yield return new WaitUntil(() => Player.instance.commonStatus.currentHp != 0);
        while (true)
        {
            yield return null;
            playerHpSlider.value = Player.instance.commonStatus.currentHp;
            playerHpText.text = playerHpSlider.value.ToString();
        }
    }
    public void ShowDamageUI(Vector3 position,string value, bool isPlayer)
    {
        if (isPlayer)
        {
            var damageUI = playerDamageUI.Find(x => !x.gameObject.activeSelf);
            var mesh = damageUI.GetComponentInChildren<TextMeshProUGUI>();
            mesh.text = value;
            Vector3 pos = position + new Vector3(0, 4, 0);
            playerDamageUIPos = pos;
            damageUI.gameObject.SetActive(true);
        }
        else
        {
            var damageUI = monsterDamageUI.Find(x => !x.gameObject.activeSelf);
            var mesh = damageUI.GetComponentInChildren<TextMeshProUGUI>();
            mesh.text = value;
            Vector3 pos = position + new Vector3(0, 1, 0);
            monsterDamageUIPos = pos;
            damageUI.gameObject.SetActive(true);
        }
    }
}
