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
    public const int MAX_LEVEL = 100; //���׸��� ��ȭ ���� �ִ�ġ
    public string[] typeNameArray;
    public Sprite[] typeSpriteArray; // ������ �������� ���׸����� �ٸ� ���� ��������Ʈ
    public Sprite[] typeIconArray;
    public InteriorType interiorType; //���׸��� Ÿ�� (������ ����������)
    public string name; // ���׸��� �̸�
    public Sprite icon; // ���׸��� ������ �̹���
    private int level = 0; // ���׸��� ��ȭ ����
    public double baseCost; // ���׸��� �ʱ� ���� ����
    public double currentCost; // ���׸��� ��ȭ ����
    public double defaultGoldPerSec; // ���׸��� �ʱ� �ʴ� ȹ�� ���
    public double currentGoldPerSec; // ���׸��� ���� �ʴ� ȹ�� ���
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
    }//������ ��� ����(���� �� ��� true)
    public bool isOpenReady;
    public bool isLevelUpReady;

    public GameObject childObj;

    public bool LevelUp() //���� ������ �Լ�. ������ 1 �÷��ְ� ���� ������ �°� ��ȭ���, �ʴ� ȹ�� ��� ������Ʈ �� true ����
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
            CurrentSprite = typeSpriteArray[PlayerPrefs.GetInt(name + DataManager.DATA_PATH_TYPE, 0)]; //���� ���� �����Ƽ� �׳� ���ο��� ������Ƽ�� ���� ���� ���� ������
            Debug.Log("Ÿ�� �ε��� : " + PlayerPrefs.GetInt(name + DataManager.DATA_PATH_TYPE, 0));
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
