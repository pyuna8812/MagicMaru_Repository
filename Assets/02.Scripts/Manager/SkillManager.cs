using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;
using System.Numerics;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public double tapGoldReinforcePrice;
    public Text tapGoldText;
    public Text tapGoldLevelText;
    public Text tapGoldReinforcePriceText;
    public Image tapGoldReinforceImage;
    public Sprite tapGoldReinforceOnSprite;
    public Sprite tapGoldReinforceOffSprite;
    /// <summary>
    /// 0 = tapGoldLevel, 1 ~ = SkillLevel
    /// </summary>
    public int[] skillLevelArray = new int[5]; // 모든 스킬들의 레벨을 담을 배열 (0 = 탭골드, 1부터는 스킬 UI 왼쪽 상단 -> 순으로)
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.LoadingComplete);
        UpdateTapGoldUI();
        StartCoroutine(Co_UpdateTapGoldReinforceButtonImage());
    }
    private IEnumerator Co_UpdateTapGoldReinforceButtonImage()
    {
        bool isReady = false;
        yield return new WaitUntil(() => GameManager.Instance != null);
        while (true)
        {
            yield return null;
            if (GameManager.Instance.gold >= tapGoldReinforcePrice && !isReady)
            {
                isReady = true;
                tapGoldReinforceImage.sprite = tapGoldReinforceOnSprite;
            }
            else if (GameManager.Instance.gold < tapGoldReinforcePrice && isReady)
            {
                isReady = false;
                tapGoldReinforceImage.sprite = tapGoldReinforceOffSprite;
            }
        }
    }
    public void BtnEvt_ReinforceSkill(int index)
    {
        ReinforceSkill(index);
    }
    private void ReinforceSkill(int index)
    {
        if(GameManager.Instance.gold < tapGoldReinforcePrice)
        {
            return;
        }
        SoundManager.instance.PlaySE_UI(3);
        ReinforceTapGold();
    }
    private void UpdateTapGoldUI()
    {
        tapGoldReinforcePrice = GameManager.Instance.tapGold * 9;
        tapGoldText.text = GameManager.Instance.tapGold < 1000 ? GameManager.Instance.tapGold.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)GameManager.Instance.tapGold);
        tapGoldLevelText.text = skillLevelArray[0].ToString();
        tapGoldReinforcePriceText.text = tapGoldReinforcePrice < 1000 ? tapGoldReinforcePrice.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)tapGoldReinforcePrice);
    }
    private void ReinforceTapGold()
    {
        GameManager.Instance.UpdateGold(-tapGoldReinforcePrice);
        GameManager.Instance.tapGold = GameManager.Instance.tapGold * 1.2f;
        skillLevelArray[0]++;
        UpdateTapGoldUI();
        DataManager.SaveData(DataManager.DATA_PATH_SKILL + "0", skillLevelArray[0], 0);
        DataManager.SaveData(DataManager.DATA_PATH_TAPGOLD, GameManager.Instance.tapGold, 1);
    }
}
