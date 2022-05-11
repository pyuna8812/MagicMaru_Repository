using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CAH.GameSystem.BigNumber;
using System.Numerics;
using DG.Tweening;

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
    public InteriorUI[] currentArray;
    public InteriorUI[] furnitureArray;
    public InteriorUI[] decoArray;
    public InteriorUI[] propArray;
    public InteriorUI[] balconyArray;
    public GameObject[] scrollViewArray;
    public Image[] menuImgArray;
    public Sprite[] menuOnArray;
    public Sprite[] menuOffArray;
    public Image unlockImage;
    public Text unlockText;
    public GameObject unlockUI;
    public GameObject typeChangeUI;
    public GameObject typeInfoPool;
    public ContentSizeFitter typeChangeContentSizeFitter;
    public Sprite typeApplySprite;
    public Sprite typeUnappliedSprite;
    public Image batchReinforcementImage;
    public Text batchReinforcementText;
    public Sprite batchReinforcementOnSprite;
    public Sprite batchReinforcementOffSprite;

    private double batchReinforcementCost;
    private Interior currentTypeInterior;
    private static ShopManager instance;

    private float rect;

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
        StartCoroutine(Co_UpdateBatchReinforcementImage());
        rect = typeChangeContentSizeFitter.transform.GetComponent<RectTransform>().rect.x;
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
    private void InitInteriorInfo(InteriorUI target, Interior interior)
    {
        if (interior.isUnLock)
        {
            ChangeInteriorUIBoxToOn(target, interior);
        }
        else
        {
            target.levelUpImage.sprite = buttonOpenOffSprite;
            target.priceText.text = interior.baseCost < 1000 ? interior.baseCost.ToString() : BigIntegerManager.GetUnit(interior.baseCost);
        }
        target.nameText.text = interior.name;
    }
    public void ChangeButtonSprite(Interior target, ButtonSpriteType type)
    {
        var button = currentArray[currentSelectList.FindIndex(x => x == target)].levelUpImage;
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
    private void ChangeInteriorUIBoxToOn(InteriorUI target, Interior interior)
    {
        target.boxImage.sprite = imageBoxOnSprite;
        target.iconImage.sprite = interior.icon;
        target.iconImage.gameObject.SetActive(true);
        UpdateLevelAndGetText(target, interior);
        target.levelUpImage.sprite = buttonLevelUpOnSprite;
        if (interior.interiorType == InteriorType.Change)
        {
            target.changeObj.gameObject.SetActive(true);
        }
    }
    private void UpdateLevelAndGetText(InteriorUI target, Interior interior)
    {
        target.levelText.text = $"{interior.Level}";
        target.levelText.gameObject.SetActive(true);
        target.getText.text = interior.currentGoldPerSec < 1000 ? interior.currentGoldPerSec.ToString("F1") : BigIntegerManager.GetUnit((BigInteger)interior.currentGoldPerSec);
        target.priceText.text = interior.currentCost < 1000 ? interior.currentCost.ToString("F1") : BigIntegerManager.GetUnit((long)interior.currentCost);
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
        UpdateBatchReinforcementCost();
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
        currentSelectList[index].LevelUp();
        ChangeInteriorUIBoxToOn(currentArray[index], currentSelectList[index]);
        ChangeButtonSprite(currentSelectList[index], ButtonSpriteType.LevelUpOff);//GameManager.Instance.gold >= interiors[index].currentCost ? ButtonSpriteType.LevelUpOn : ButtonSpriteType.LevelUpOff);
        currentSelectList[index].childObj.SetActive(true);
        UpdateUnlockUI(currentSelectList[index], true);
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
    public void BtnEvt_ChangeType(int index)
    {
        ChangeType(index);
    }
    public void BtnEvt_ChangeInteriorType(int index)
    {
        ChangeInteriorTyep(index);
    }
    private void ChangeInteriorTyep(int index)
    {
        if(currentTypeInterior.CurrentSprite == currentTypeInterior.typeSpriteArray[index])
        {
            return;
        }
        currentTypeInterior.CurrentSprite = currentTypeInterior.typeSpriteArray[index];
        for (int i = 0; i < typeChangeContentSizeFitter.transform.childCount; i++)
        {
            var obj = typeChangeContentSizeFitter.transform.GetChild(i).GetChild(2).GetComponent<Image>();
            obj.sprite = typeApplySprite;
            if (currentTypeInterior.CurrentSprite == currentTypeInterior.typeSpriteArray[i])
            {
                obj.sprite = typeUnappliedSprite;
            }
        }
    }
    private void ChangeType(int index)
    {
        if (typeChangeUI.activeSelf)
        {
            typeChangeUI.SetActive(false);
            int count = typeChangeContentSizeFitter.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var obj = typeChangeContentSizeFitter.transform.GetChild(0);//parent = typeInfoPool.transform;
                obj.SetParent(typeInfoPool.transform);
                obj.SetSiblingIndex(i);
                obj.gameObject.SetActive(false);              
            }
            var r = typeChangeContentSizeFitter.transform.GetComponent<RectTransform>().rect;
            r.x = rect;
            return;
        }
        currentTypeInterior = currentSelectList[index];
        typeChangeUI.SetActive(true);
        typeChangeContentSizeFitter.horizontalFit = currentTypeInterior.typeNameArray.Length > 3 ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
        for (int i = 0; i < currentTypeInterior.typeSpriteArray.Length; i++)
        {
            var obj = typeInfoPool.transform.GetChild(0);
            obj.transform.SetParent(typeChangeContentSizeFitter.transform);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = currentTypeInterior.typeIconArray[i];
            obj.transform.GetChild(1).GetComponent<Text>().text = currentTypeInterior.typeNameArray[i];
            obj.GetChild(2).GetComponent<Image>().sprite = typeApplySprite;
            if (currentTypeInterior.CurrentSprite == currentTypeInterior.typeSpriteArray[i])
            {
                obj.GetChild(2).GetComponent<Image>().sprite = typeUnappliedSprite;
            }
            obj.gameObject.SetActive(true);
        }
       // currentSelectList[index]
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
    private void UpdateShopState(List<Interior> interiors, InteriorUI[] array, int index)
    {
        shopState = (ShopState)index;
        currentSelectList = interiors;
        UpdateBatchReinforcementCost();
        currentArray = array;
        for (int i = 0; i < scrollViewArray.Length; i++)
        {
            scrollViewArray[i].SetActive(false);
            menuImgArray[i].sprite = menuOffArray[i];
        }
        scrollViewArray[index].SetActive(true);
        menuImgArray[index].sprite = menuOnArray[index];
    }
    private void UpdateUnlockUI(Interior interior, bool isEnlarge)
    {
        if (isEnlarge)
        {
            unlockImage.transform.parent.localScale = UnityEngine.Vector3.zero;
            unlockImage.sprite = interior.icon;
            unlockText.text = interior.name;
            unlockUI.SetActive(isEnlarge);
            unlockImage.transform.parent.DOScale(UnityEngine.Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }
        else
        {
            unlockUI.SetActive(isEnlarge);
        }
    }
    public void BtnEvt_BatchReinforcement()
    {
        BatchReinforcement();
    }
    private void BatchReinforcement()
    {
        if(GameManager.Instance.gold < batchReinforcementCost)
        {
            return;
        }
       /* for (int i = 0; i < currentSelectList.Count; i++)
        {
            if (currentSelectList[i].isUnLock)
            {
                currentSelectList[i].LevelUp();

            }
        }*/
        foreach (var item in currentSelectList)
        {
            if (item.isUnLock)
            {
                item.LevelUp();
                UpdateLevelAndGetText(currentArray[currentSelectList.FindIndex(x => x == item)], item);
            }
        }
        UpdateBatchReinforcementCost();
    }
    private void UpdateBatchReinforcementCost()
    {
        batchReinforcementCost = 0;
        foreach (var item in currentSelectList)
        {
            if (item.isUnLock)
            {
                batchReinforcementCost += item.currentCost;
            }
        }
        batchReinforcementText.text = batchReinforcementCost < 1000 ? batchReinforcementCost.ToString("F1") : BigIntegerManager.GetUnit((long)batchReinforcementCost);
    }
    private IEnumerator Co_UpdateBatchReinforcementImage()
    {
        bool isReady = false;
        yield return new WaitUntil(() => GameManager.Instance != null);
        while (true)
        {
            yield return null;
            if (currentSelectList != null)
            {
                /*if(batchReinforcementCost <= 0)
                {
                    batchReinforcementImage.sprite = batchReinforcementOffSprite;
                    continue;
                }*/
                if (GameManager.Instance.gold >= batchReinforcementCost && !isReady)
                {
                    isReady = true;
                    batchReinforcementImage.sprite = batchReinforcementOnSprite;
                }
                else if (GameManager.Instance.gold < batchReinforcementCost && isReady)
                {
                    isReady = false;
                    batchReinforcementImage.sprite = batchReinforcementOffSprite;
                }
            }
        }
    }
    public void LateUpdate()
    {
        if (unlockUI.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0)
            {
                UpdateUnlockUI(null, false);
            }
        }
    }
}
