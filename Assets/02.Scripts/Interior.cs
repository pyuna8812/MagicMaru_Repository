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
    public const int MAX_LEVEL = 100; //���׸��� ��ȭ ���� �ִ�ġ
    public string[] typeNameArray;
    public Sprite[] typeSpriteArray; // ������ �������� ���׸����� �ٸ� ���� ��������Ʈ
    public Sprite[] typeIconArray;
    public InteriorType interiorType; //���׸��� Ÿ�� (������ ����������)
    public string name; // ���׸��� �̸�
    public Sprite icon; // ���׸��� ������ �̹���
    private int level = 0; // ���׸��� ��ȭ ����
    public long baseCost; // ���׸��� �ʱ� ���� ����
    public double currentCost; // ���׸��� ��ȭ ����
    public double defaultGoldPerSec; // ���׸��� �ʱ� �ʴ� ȹ�� ���
    public double currentGoldPerSec; // ���׸��� ���� �ʴ� ȹ�� ���
    public bool isActive { get; set; } //������ ���� Ȱ��ȭ ����(���� ���� �� true, ���� �� false)
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

    public bool isUnLock; //������ ��� ����(���� �� ��� true)
    public bool isOpenReady;
    public bool isLevelUpReady;

    public GameObject childObj;

    public bool LevelUp() //���� ������ �Լ�. ������ 1 �÷��ְ� ���� ������ �°� ��ȭ���, �ʴ� ȹ�� ��� ������Ʈ �� true ����
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
                    Debug.Log($"{name}���� ��");
                    //yield return new WaitUntil(() => GameManager.Instance.gold < baseCost);
                }
                else if(isOpenReady && GameManager.Instance.gold < baseCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.OpenOff);
                    isOpenReady = false;
                    Debug.Log($"{name}���� ����");
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
                    Debug.Log($"{name}������ ��");
                    //yield return new WaitUntil(() => GameManager.Instance.gold < currentCost);
                }
                else if(isLevelUpReady && GameManager.Instance.gold < currentCost)
                {
                    ShopManager.Instance.ChangeButtonSprite(this, ButtonSpriteType.LevelUpOff);
                    isLevelUpReady = false;
                    Debug.Log($"{name}������ ����");
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
