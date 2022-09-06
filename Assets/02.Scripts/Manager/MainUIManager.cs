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
    public RectTransform[] tapGoldRectArray;
    public List<Image> tapGoldImageList = new List<Image>();
    public List<Text> tapGoldTextList = new List<Text>();
    private Text resurrectionCountTxt;
    private UnityEngine.Vector3 playerDamageUIPos;
    private UnityEngine.Vector3 monsterDamageUIPos;
    private UnityEngine.Vector2 screenPoint;
    public Slider slider_BGM;
    public Slider slider_SE;
    public RectTransform filter;
    private int tapGoldCount = 0;
    private Color textColor;
    private Color imageColor;
    public Text offlineRewardText;
    public GameObject offlineRewardObj;
    public Image fadeOutImage;
    private void Awake()
    {
        instance = this;
        camera = Camera.main.GetComponent<CameraUtility>();
        resurrectionCountTxt = dieUI.GetComponentInChildren<Text>();
        for (int i = 0; i < tapGoldRectArray.Length; i++)
        {
            tapGoldImageList.Add(tapGoldRectArray[i].GetComponentInChildren<Image>());
            tapGoldTextList.Add(tapGoldRectArray[i].GetComponentInChildren<Text>());
        }
        textColor = tapGoldTextList[0].color;
        textColor = new Color(textColor.r, textColor.g, textColor.b, 1);
        imageColor = tapGoldImageList[0].color;
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.LoadingComplete);
        if (GameManager.Instance.OfflineReward)
        {
            ShowOfflineReward();
        }
        StartCoroutine(Co_UpdatePlayerHpBar());
        FixCam();
        fadeOutImage.DOFade(0, 2f);
        yield return new WaitForSeconds(2f);
        fadeOutImage.gameObject.SetActive(false);
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
                item.position = playerDamageUIPos;
            }
        }
        foreach (var item in monsterDamageUI)
        {
            if (item.gameObject.activeSelf)
            {
                item.position = monsterDamageUIPos;
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
    public void BtnEvt_ConfirmOfflineReward()
    {
        GameManager.Instance.UpdateGold(GameManager.Instance.OfflineRewardGold);
        SoundManager.instance.PlaySE_UI(0);
        offlineRewardObj.SetActive(false);
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
        ShowTapGold();
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
    private void ShowTapGold()
    {
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(filter, Input.mousePosition, Camera.main, out screenPoint);
        tapGoldRectArray[tapGoldCount].localPosition = new UnityEngine.Vector2(Random.Range(-600f, 600f), Random.Range(-300f, 300f));
        tapGoldImageList[tapGoldCount].color = Color.white;
        tapGoldTextList[tapGoldCount].color = textColor;
        tapGoldTextList[tapGoldCount].text = "+" + (GameManager.Instance.tapGold < 1000 ? GameManager.Instance.tapGold.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)GameManager.Instance.tapGold));
        tapGoldRectArray[tapGoldCount].DOLocalMoveY(tapGoldRectArray[tapGoldCount].localPosition.y + 20, 1f);
        tapGoldImageList[tapGoldCount].DOColor(imageColor, 1f);
        tapGoldTextList[tapGoldCount].DOColor(Color.clear, 1f);
        tapGoldCount++;
        if (tapGoldCount >= tapGoldRectArray.Length) tapGoldCount = 0;
    }
    private void FixCam()
    {
        if(Player.instance.transform.position.x <= CameraUtility.LEFT_MAX)
        {
            camera.transform.DOMove(new UnityEngine.Vector3(CameraUtility.LEFT_MAX, 0, -10), 2f);
            return;
        }
        if(Player.instance.transform.position.x >= CameraUtility.RiGHT_MAX)
        {
            camera.transform.DOMove(new UnityEngine.Vector3(CameraUtility.RiGHT_MAX, 0, -10), 2f);
            return;
        }
        camera.transform.DOMove(new UnityEngine.Vector3(Player.instance.transform.position.x, 0, -10), 1f);
    }
    private void ShowOfflineReward()
    {
        offlineRewardText.text = GameManager.Instance.OfflineRewardGold < 1000 ? GameManager.Instance.OfflineRewardGold.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)GameManager.Instance.OfflineRewardGold);
        offlineRewardObj.SetActive(true);
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
    public void UpdateBGMValue()
    {
        SoundManager.instance.audioSource_BGM.volume = slider_BGM.value;
    }
    public void UpdateSFXValue()
    {
        SoundManager.instance.audioSource_SE_InGame.volume = slider_SE.value;
        SoundManager.instance.audioSource_SE_UI.volume = slider_SE.value;
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
            damageUI.transform.position = pos;
        }
        else
        {
            var damageUI = monsterDamageUI.Find(x => !x.gameObject.activeSelf);
            var mesh = damageUI.GetComponentInChildren<TextMeshProUGUI>();
            mesh.text = value;
            UnityEngine.Vector3 pos = position + new UnityEngine.Vector3(0, 1, 0);
            monsterDamageUIPos = pos;
            damageUI.gameObject.SetActive(true);
            damageUI.transform.position = pos;
        }
    }
}
