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
    public long baseCost; // 인테리어 초기 구매 가격
    public double currentCost; // 인테리어 강화 가격
    public double defaultGoldPerSec; // 인테리어 초기 초당 획득 골드
    public double currentGoldPerSec; // 인테리어 현재 초당 획득 골드
    public bool isActive { get; set; } //가구의 현재 활성화 상태(가구 장착 시 true, 해제 시 false)
    public int Level { get => level; }
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

    public bool isUnLock; //가구의 잠금 상태(구매 시 계속 true)
    public bool isOpenReady;
    public bool isLevelUpReady;

    public GameObject childObj;

    public bool LevelUp() //가구 레벨업 함수. 레벨을 1 올려주고 현재 레벨에 맞게 강화비용, 초당 획득 골드 업데이트 후 true 리턴
    {
        level++;
        GameManager.Instance.UpdateGold((long)-currentCost);
        UpdateCurrentCostByLevel();
        UpdateCurrentGoldPerSecByLevel();
        GameManager.Instance.UpdateGoldPerSec(currentGoldPerSec);
        return true;
    }
    private void UpdateCurrentCostByLevel()
    {
        currentCost = baseCost * (Mathf.Pow(1.07f, level));
    }
    private void UpdateCurrentGoldPerSecByLevel()
    {
        currentGoldPerSec = defaultGoldPerSec * (Mathf.Pow(1.15f, level));
    }
    private void Awake()
    {
        childObj = transform.GetChild(0).gameObject;
        if(level < 1)
        {
            currentGoldPerSec = defaultGoldPerSec;
            currentCost = baseCost;
        }
        currentSprite = childObj.GetComponent<SpriteRenderer>().sprite;
    }
    private void Start()
    {
        // transform.gameObject.SetActive(isActive);
        StartCoroutine(Co_SetIsUnLock());
        StartCoroutine(Co_SetLevelUp());
    }
    private void Update()
    {
        /*if (isUnLock && GameManager.Instance.gold >= currentCost)
        {
            ShopManager.Instance.ChangeButton(this, ButtonSpriteType.LevelUpOn);
        }*/
       /* if(!isUnLock && GameManager.Instance.gold >= baseCost)
        {
            Debug.Log("tlqkf");
            ShopManager.Instance.ChangeButton(this, ButtonSpriteType.OpenOn);
        }*/
    }
    private IEnumerator Co_SetIsUnLock()
    {
        while (true)
        {
            yield return new WaitUntil(() => ShopManager.Instance.shopState == shopState);
            while (!isUnLock)
            {
                if(ShopManager.Instance.shopState != shopState)
                {
                    break;
                }
                if (!isOpenReady && GameManager.Instance.gold >= baseCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.OpenOn);
                    isOpenReady = true;
                    Debug.Log($"{name}오픈 온");
                    //yield return new WaitUntil(() => GameManager.Instance.gold < baseCost);
                }
                else if(isOpenReady && GameManager.Instance.gold < baseCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.OpenOff);
                    isOpenReady = false;
                    Debug.Log($"{name}오픈 오프");
                   // yield return new WaitUntil(() => GameManager.Instance.gold >= baseCost);
                }
                yield return null;
            }
        }
    }
    private IEnumerator Co_SetLevelUp()
    {
        yield return new WaitUntil(() => isUnLock);
        while (true)
        {
            yield return new WaitUntil(() => ShopManager.Instance.shopState == shopState);
            while (true)
            {
                if (ShopManager.Instance.shopState != shopState)
                {
                    break;
                }
                if (!isLevelUpReady && GameManager.Instance.gold >= currentCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.LevelUpOn);
                    isLevelUpReady = true;
                    Debug.Log($"{name}레벨업 온");
                    //yield return new WaitUntil(() => GameManager.Instance.gold < currentCost);
                }
                else if(isLevelUpReady && GameManager.Instance.gold < currentCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.LevelUpOff);
                    isLevelUpReady = false;
                    Debug.Log($"{name}레벨업 오프");
                    //yield return new WaitUntil(() => GameManager.Instance.gold >= currentCost);
                }
                yield return null;
            }
        }
    }
    private void LateUpdate()
    {

    }
}
