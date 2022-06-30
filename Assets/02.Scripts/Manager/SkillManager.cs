using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;

public class SkillManager : MonoBehaviour
{
    public double tapGoldReinforcePrice;

    public Text tapGoldText;
    public Text tapGoldLevelText;
    public Text tapGoldReinforcePriceText;
    public Image tapGoldReinforceImage;
    public Sprite tapGoldReinforceOnSprite;
    public Sprite tapGoldReinforceOffSprite;

    // Start is called before the first frame update
    void Start()
    {
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
        GameManager.Instance.UpdateGold(-tapGoldReinforcePrice);
        GameManager.Instance.ReinforceTapGold();
        UpdateTapGoldUI();
    }
    private void UpdateTapGoldUI()
    {
        tapGoldReinforcePrice = GameManager.Instance.tapGold * 9;
        tapGoldText.text = GameManager.Instance.tapGold < 1000 ? GameManager.Instance.tapGold.ToString("F1") : BigIntegerManager.GetUnit((long)GameManager.Instance.tapGold);
        tapGoldLevelText.text = GameManager.Instance.skillLevelArray[0].ToString();
        tapGoldReinforcePriceText.text = tapGoldReinforcePrice < 1000 ? tapGoldReinforcePrice.ToString("F1") : BigIntegerManager.GetUnit((long)tapGoldReinforcePrice);
    }
}
