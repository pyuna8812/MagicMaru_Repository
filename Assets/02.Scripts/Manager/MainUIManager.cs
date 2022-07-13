using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CAH.GameSystem.BigNumber;
using DG.Tweening;
using System.Numerics;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager instance;
    public GameObject setting;
    public GameObject shop;
    public GameObject skillPopUp;
    public CameraUtility camera;
    public Text goldText;
    public Text goldPerSecText;
    public Slider playerHpSlider;
    public Text playerHpText;
    public List<Transform> playerDamageUI = new List<Transform>();
    public List<Transform> monsterDamageUI = new List<Transform>();
    public GameObject dieUI;
    public GameObject exitUI;
    private Text resurrectionCountTxt;
    private UnityEngine.Vector3 playerDamageUIPos;
    private UnityEngine.Vector3 monsterDamageUIPos;

    public Slider slider_BGM;
    public Slider slider_SE;
    //public Text tapGoldText;
    private void Awake()
    {
        instance = this;
        camera = Camera.main.GetComponent<CameraUtility>();
        resurrectionCountTxt = dieUI.GetComponentInChildren<Text>();
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.LoadingComplete);
        StartCoroutine(Co_UpdatePlayerHpBar());
        StartCoroutine(Co_UpdateSound());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.instance.PlaySE_UI(0);
            exitUI.SetActive(!exitUI.activeSelf);
        }
    }
    private void LateUpdate()
    {
        foreach (var item in playerDamageUI)
        {
            if (item.gameObject.activeSelf)
            {
                item.position = Camera.main.WorldToScreenPoint(playerDamageUIPos);
                playerDamageUIPos = new UnityEngine.Vector3(playerDamageUIPos.x, playerDamageUIPos.y + 0.001f);
            }
        }
        foreach (var item in monsterDamageUI)
        {
            if (item.gameObject.activeSelf)
            {
                item.position = Camera.main.WorldToScreenPoint(monsterDamageUIPos);
                monsterDamageUIPos = new UnityEngine.Vector3(monsterDamageUIPos.x, monsterDamageUIPos.y + 0.001f);
            }
        }
    }
    public void UpdateGoldUI()
    {
        goldText.text = GameManager.Instance.gold < 1000 ? GameManager.Instance.gold.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)GameManager.Instance.gold);
    }
    public void UpdateGoldPerSecUI()
    {     
        goldPerSecText.text = GameManager.Instance.goldPerSec < 1000 ? GameManager.Instance.goldPerSec.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)GameManager.Instance.goldPerSec);
    }
    public void BtnEvt_ActiveShop()
    {
        shop.SetActive(!shop.activeSelf);
        if (!shop.activeSelf)
        {
            SoundManager.instance.PlaySE_UI(1);
            return;
        }
        SoundManager.instance.PlaySE_UI(0);
    }
    public void BtnEvt_ActiveSkillPopUp()
    {
        skillPopUp.SetActive(!skillPopUp.activeSelf);
        if (!skillPopUp.activeSelf)
        {
            SoundManager.instance.PlaySE_UI(1);
            return;
        }
        SoundManager.instance.PlaySE_UI(0);
    }
    public void BtnEvt_Tapping()
    {
        GameManager.Instance.Tapping();
        SoundManager.instance.PlaySE_UI(2);
    }
    public void BtnEvt_FixCam()
    {
        FixCam();
        SoundManager.instance.PlaySE_UI(0);
    }
    public void BtnEvt_ActiveSetting()
    {
        setting.SetActive(!setting.activeSelf);
        if (!setting.activeSelf)
        {
            SoundManager.instance.PlaySE_UI(1);
            DataManager.SaveData(DataManager.DATA_PATH_BGM, slider_BGM.value, 2);
            DataManager.SaveData(DataManager.DATA_PATH_SE, slider_SE.value, 2);
            return;
        }
        SoundManager.instance.PlaySE_UI(0);
    }
    public void BtnEvt_ExitGame()
    {
        Application.Quit();
    }
    public void BtnEvt_ActiveExitUI()
    {
        SoundManager.instance.PlaySE_UI(1);
        exitUI.SetActive(!exitUI.activeSelf);
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
        camera.transform.position = new UnityEngine.Vector3(Player.instance.transform.position.x, 0, -10);
        if(camera.transform.position.x <= CameraUtility.LEFT_MAX)
        {
            camera.transform.position = new UnityEngine.Vector3(CameraUtility.LEFT_MAX, 0, -10);
        }
        else if(camera.transform.position.x >= CameraUtility.RiGHT_MAX)
        {
            camera.transform.position = new UnityEngine.Vector3(CameraUtility.RiGHT_MAX, 0, -10);
        }
    }
    private IEnumerator Co_UpdatePlayerHpBar()
    {
        yield return new WaitUntil(() => Player.instance.commonStatus.currentHp != 0);
        while (true)
        {
            yield return null;
            playerHpSlider.value = Player.instance.commonStatus.currentHp;
            playerHpText.text = playerHpSlider.value.ToString();
            if(Player.instance.isDie)
            {
                print("이프문 들어옴");
                dieUI.SetActive(true);
                while (Player.instance.isDie)
                {
                    resurrectionCountTxt.text = Player.instance.resurrectionCount.ToString();
                    if(Player.instance.resurrectionCount == 0)
                    {
                        dieUI.SetActive(false);
                        yield return new WaitUntil(() => !Player.instance.isDie);
                        break;
                    }
                    yield return null;
                }
            }
        }
    }
    private IEnumerator Co_UpdateSound()
    {
        while (true)
        {
            SoundManager.instance.audioSource_BGM.volume = slider_BGM.value;
            SoundManager.instance.audioSource_SE_InGame.volume = slider_SE.value;
            SoundManager.instance.audioSource_SE_UI.volume = slider_SE.value;
            yield return null;
        }
    }
    public void ShowDamageUI(UnityEngine.Vector3 position,string value, bool isPlayer)
    {
        if (isPlayer)
        {
            var damageUI = playerDamageUI.Find(x => !x.gameObject.activeSelf);
            var mesh = damageUI.GetComponentInChildren<TextMeshProUGUI>();
            mesh.text = value;
            UnityEngine.Vector3 pos = position + new UnityEngine.Vector3(0, 4, 0);
            playerDamageUIPos = pos;
            damageUI.gameObject.SetActive(true);
        }
        else
        {
            var damageUI = monsterDamageUI.Find(x => !x.gameObject.activeSelf);
            var mesh = damageUI.GetComponentInChildren<TextMeshProUGUI>();
            mesh.text = value;
            UnityEngine.Vector3 pos = position + new UnityEngine.Vector3(0, 1, 0);
            monsterDamageUIPos = pos;
            damageUI.gameObject.SetActive(true);
        }
    }
}
