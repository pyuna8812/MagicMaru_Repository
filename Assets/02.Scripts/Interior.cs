using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public enum InteriorType
{
    None,
    Change
}
public class Interior : MonoBehaviour
{
    private bool isUnlock;
    private Sprite currentSprite;
    public ShopState shopState;
    public const int MAX_LEVEL = 100; //인테리어 강화 레벨 최대치
    public string[] typeNameArray;
    public Sprite[] typeSpriteArray; // 종류가 여러개인 인테리어의 다른 종류 스프라이트
    public Sprite[] typeIconArray;
    public InteriorType interiorType; //인테리어 타입 (종류가 여러개인지)
    public string name; // 인테리어 이름
    public Sprite icon; // 인테리어 아이콘 이미지
    private int level = 0; // 인테리어 강화 레벨
    public double baseCost; // 인테리어 초기 구매 가격
    public double currentCost; // 인테리어 강화 가격
    public double defaultGoldPerSec; // 인테리어 초기 초당 획득 골드
    public double currentGoldPerSec; // 인테리어 현재 초당 획득 골드
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            isLevelUpReady = true;
            UpdateCurrentCostByLevel();
            UpdateCurrentGoldPerSecByLevel();
        }
    }
    public Sprite CurrentSprite
    {
        get
        {
            return currentSprite;
        }
        set
        {
            currentSprite = value;
            childObj.GetComponent<SpriteRenderer>().sprite = currentSprite;
        }
    }

    public bool IsUnlock
    {
        get
        {
            return isUnlock;
        }
        set
        {
            isUnlock = value;
            DataManager.SaveData(name + DataManager.DATA_PATH_ISUNLOCK, isUnlock, 1);
        }
    }//가구의 잠금 상태(구매 시 계속 true)
    public bool isOpenReady;
    public bool isLevelUpReady;

    public GameObject childObj;

    public bool LevelUp() //가구 레벨업 함수. 레벨을 1 올려주고 현재 레벨에 맞게 강화비용, 초당 획득 골드 업데이트 후 true 리턴
    {
        level++;
        GameManager.Instance.UpdateGold(-currentCost);
        UpdateCurrentCostByLevel();
        UpdateCurrentGoldPerSecByLevel();
        GameManager.Instance.UpdateGoldPerSec(currentGoldPerSec);
        isLevelUpReady = false;
        DataManager.SaveData(name + DataManager.DATA_PATH_LEVEL, level, 0);
        return true;
    }
    private void UpdateCurrentCostByLevel()
    {
        currentCost = baseCost * Mathf.Pow(1.15f, level);
    }
    private void UpdateCurrentGoldPerSecByLevel()
    {
        currentGoldPerSec = defaultGoldPerSec * Mathf.Pow(1.15f, level);
    }
    private void Awake()
    {
        childObj = transform.GetChild(0).gameObject;
        if (level < 1)
        {
            currentGoldPerSec = defaultGoldPerSec;
            currentCost = baseCost;
        }
        currentSprite = childObj.GetComponent<SpriteRenderer>().sprite;
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.LoadingComplete);
        if (isUnlock)
        {
            childObj.SetActive(true);
        }
        else
        {
            childObj.SetActive(false);
        }
        if (interiorType == InteriorType.Change)
        {
            CurrentSprite = typeSpriteArray[PlayerPrefs.GetInt(name + DataManager.DATA_PATH_TYPE, 0)]; //두줄 쓰기 귀찮아서 그냥 내부에서 프로퍼티로 세터 내부 로직 실행함
            Debug.Log("타입 인덱스 : " + PlayerPrefs.GetInt(name + DataManager.DATA_PATH_TYPE, 0));
        }
        StartCoroutine(Co_SetIsUnLock());
        StartCoroutine(Co_SetLevelUp());
    }
    private IEnumerator Co_SetIsUnLock()
    {
        while (true)
        {
            yield return new WaitUntil(() => ShopManager.Instance.shopState == shopState);
            while (!isUnlock)
            {
                if(ShopManager.Instance.shopState != shopState)
                {
                    break;
                }
                if (!isOpenReady && GameManager.Instance.gold >= baseCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.OpenOn);
                    isOpenReady = true;
                }
                else if(isOpenReady && GameManager.Instance.gold < baseCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.OpenOff);
                    isOpenReady = false;
                }
                yield return null;
            }
        }
    }
    private IEnumerator Co_SetLevelUp()
    {
        yield return new WaitUntil(() => isUnlock);
        while (true)
        {
            yield return new WaitUntil(() => ShopManager.Instance.shopState == shopState);
            while (true)
            {
                if (ShopManager.Instance.shopState != shopState)
                {
                    break;
                }
                if (GameManager.Instance.gold >= currentCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.LevelUpOn);
                    isLevelUpReady = true;
                }
                else if (GameManager.Instance.gold < currentCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.LevelUpOff);
                    isLevelUpReady = false;
                }
                if (level == MAX_LEVEL)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.LevelUpOff);
                    yield break;
                }
                yield return null;
            }
        }
    }
    private void LateUpdate()
    {

    }
}
