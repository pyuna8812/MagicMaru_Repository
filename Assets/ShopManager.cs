using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;
using System.Numerics;
using UnityEngine.EventSystems;

public enum ButtonSpriteType
{
    LevelUpOn,
    LevelUpOff,
    OpenOn,
    OpenOff
}
public enum ShopState
{
    Furniture,
    Deco,
    Prop,
    Balcony
}

public class ShopManager : MonoBehaviour
{
   /* private enum ShopState
    {
        Furniture,
        Deco,
        Prop,
        Balcony
    }*/
    public ShopState shopState = ShopState.Furniture;
    public Sprite imageBoxOnSprite;
    public Sprite buttonLevelUpOnSprite;
    public Sprite buttonLevelUpOffSprite;
    public Sprite buttonOpenOnSprite;
    public Sprite buttonOpenOffSprite;
    public List<Interior> currentSelectList = new List<Interior>();
    public List<Interior> furnitureList = new List<Interior>();
    public List<Interior> decoList = new List<Interior>();
    public List<Interior> propList = new List<Interior>();
    public List<Interior> balconyList = new List<Interior>();
    public GameObject[] currentArray;
    public GameObject[] furnitureArray;
    public GameObject[] decoArray;
    public GameObject[] propArray;
    public GameObject[] balconyArray;
    public GameObject[] scrollViewArray;
    public Image[] menuImgArray;
    public Sprite[] menuOnArray;
    public Sprite[] menuOffArray;

    private static ShopManager instance;

    public static ShopManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
    }
    private bool InitAllInteriorList()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    furnitureList = GameManager.Instance.furnitureList;
                    furnitureList = furnitureList.OrderBy(x => x.baseCost).ToList();
                    for (int j = 0; j < furnitureArray.Length; j++)
                    {
                        InitInteriorInfo(furnitureArray[j], furnitureList[j]);
                    }
                    break;
                case 1:
                    decoList = GameManager.Instance.decoList;
                    decoList = decoList.OrderBy(x => x.baseCost).ToList();
                    for (int j = 0; j < decoArray.Length; j++)
                    {
                        InitInteriorInfo(decoArray[j], decoList[j]);
                    }
                    break;
                case 2:
                    propList = GameManager.Instance.propList;
                    propList = propList.OrderBy(x => x.baseCost).ToList();
                    for (int j = 0; j < propArray.Length; j++)
                    {
                        InitInteriorInfo(propArray[j], propList[j]);
                    }
                    break;
                case 3:
                    balconyList = GameManager.Instance.balconyList;
                    balconyList = balconyList.OrderBy(x => x.baseCost).ToList();
                    for (int j = 0; j < balconyArray.Length; j++)
                    {
                        InitInteriorInfo(balconyArray[j], balconyList[j]);
                    }
                    break;
                default:
                    Debug.LogError("Out of Index Range!");
                    return false;
                    break;
            }
        }
        return true;
    }
    private void Start()
    {
        if (InitAllInteriorList())
        {
            ChangeShopState((int)shopState);
        }
    }
    private void InitInteriorInfo(GameObject target, Interior interior)
    {
        if (interior.isUnLock)
        {
            ChangeInteriorUIBoxToOn(target, interior);
        }
        else
        {
            target.transform.Find("Button_LevelUp").transform.GetComponentInChildren<Text>().text = interior.baseCost < 1000 ? interior.baseCost.ToString() : BigIntegerManager.GetUnit(interior.baseCost);
        }
        target.transform.Find("Text_Name").GetComponent<Text>().text = interior.name;
    }
    public void ChangeButtonSprite(Interior target, ButtonSpriteType type)
    {
        var button = currentArray[currentSelectList.FindIndex(x => x == target)].transform.Find("Button_LevelUp").GetComponent<Image>();
        Debug.Log(currentSelectList.FindIndex(x => x == target));
        switch (type)
        {
            case ButtonSpriteType.LevelUpOn:
                button.sprite = buttonLevelUpOnSprite;
                break;
            case ButtonSpriteType.LevelUpOff:
                button.sprite = buttonLevelUpOffSprite;
                break;
            case ButtonSpriteType.OpenOn:
                button.sprite = buttonOpenOnSprite;
                break;
            case ButtonSpriteType.OpenOff:
                button.sprite = buttonOpenOffSprite;
                break;
            default:
                break;
        }
    }
    private void ChangeInteriorUIBoxToOn(GameObject target, Interior interior)
    {
        target.transform.Find("Image_Box").GetComponent<Image>().sprite = imageBoxOnSprite;
        var icon = target.transform.Find("Image_Icon").GetComponent<Image>();
        icon.sprite = interior.icon;
        icon.gameObject.SetActive(true);
        UpdateLevelAndGetText(target, interior);
        target.transform.Find("Button_LevelUp").GetComponent<Image>().sprite = buttonLevelUpOnSprite;
        if (interior.interiorType == InteriorType.Change)
        {
            target.transform.Find("Button_Change").gameObject.SetActive(true);
        }
    }
    private void UpdateLevelAndGetText(GameObject target, Interior interior)
    {
        var level = target.transform.Find("Text_Level").GetComponent<Text>();
        level.text = $"{interior.Level}";
        level.gameObject.SetActive(true);
        target.transform.Find("Text_Get").GetComponent<Text>().text = interior.currentGoldPerSec < 1000 ? interior.currentGoldPerSec.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)interior.currentGoldPerSec);
        target.transform.Find("Button_LevelUp").transform.GetComponentInChildren<Text>().text = interior.currentCost < 1000 ? interior.currentCost.ToString("F1") : BigIntegerManager.GetUnit((long)interior.currentCost);
    }
    public void BtnEvt_InteriorInteraction(int index)
    {
        InteriorInteraction(index);
    }
    private void InteriorInteraction(int index)
    {
        switch (shopState)
        {
            case ShopState.Furniture:
                InteriorInteractionByInteriorList(furnitureList, index);
                break;
            case ShopState.Deco:
                InteriorInteractionByInteriorList(decoList, index);
                break;
            case ShopState.Prop:
                InteriorInteractionByInteriorList(propList, index);
                break;
            case ShopState.Balcony:
                InteriorInteractionByInteriorList(balconyList, index);
                break;
            default:
                break;
        }
    }
    private void InteriorInteractionByInteriorList(List<Interior> interiors, int index)
    {
        if (!interiors[index].isUnLock && interiors[index].isOpenReady)
        {
            OpenInterior(index);
            return;
        }
        if (interiors[index].isUnLock)
        {
            LevelUpInterior(index);
        }
    }
    private void OpenInterior(int index)
    {
        if (GameManager.Instance.gold < currentSelectList[index].baseCost)
        {
            return;
        }
        currentSelectList[index].isUnLock = true;
        GameManager.Instance.UpdateGold(-currentSelectList[index].baseCost);
        currentSelectList[index].LevelUp();
        ChangeInteriorUIBoxToOn(currentArray[index], currentSelectList[index]);
        ChangeButtonSprite(currentSelectList[index], ButtonSpriteType.LevelUpOff);//GameManager.Instance.gold >= interiors[index].currentCost ? ButtonSpriteType.LevelUpOn : ButtonSpriteType.LevelUpOff);
        currentSelectList[index].childObj.SetActive(true);
    }
    private void LevelUpInterior(int index)
    {
        if(GameManager.Instance.gold < currentSelectList[index].currentCost)
        {
            return;
        }
        if (currentSelectList[index].LevelUp())
        {
            UpdateLevelAndGetText(currentArray[index], currentSelectList[index]);
        }
    }
    public void BtnEvt_ChangeType()
    {
        //DoSomething
    }
    public void BtnEvt_ChangeShopState(int index)
    {
        ChangeShopState(index);
    }
    private void ChangeShopState(int index)
    {
        switch (index)
        {
            case 0:
                UpdateShopState(furnitureList, furnitureArray, index);
                break;
            case 1:
                UpdateShopState(decoList, decoArray, index);
                break;
            case 2:
                UpdateShopState(propList, propArray, index);
                break;
            case 3:
                UpdateShopState(balconyList, balconyArray, index);
                break;
            default:
                break;
        }
    }
    private void UpdateShopState(List<Interior> interiors, GameObject[] array, int index)
    {
        shopState = (ShopState)index;
        currentSelectList = interiors;
        currentArray = array;
        for (int i = 0; i < scrollViewArray.Length; i++)
        {
            scrollViewArray[i].SetActive(false);
            menuImgArray[i].sprite = menuOffArray[i];
        }
        scrollViewArray[index].SetActive(true);
        menuImgArray[index].sprite = menuOnArray[index];
    }
    public void LateUpdate()
    {
        
    }
}
