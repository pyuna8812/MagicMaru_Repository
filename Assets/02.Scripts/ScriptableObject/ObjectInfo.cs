using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ObjectInfo",menuName ="ScriptableObject",order = int.MaxValue)]
public class ObjectInfo : ScriptableObject
{
    private enum EKind
    {
        Furniture,
        Deco,
        Prop,
        Balcony
    }
    [Header("오브젝트 종류")]
    [SerializeField] private EKind eKind;
    [Header("오브젝트 이름")]
    [SerializeField] private string name;
    [Header("오브젝트 스프라이트")]
    [SerializeField] private Sprite sprite;
    [Header("오브젝트 레벨")]
    [SerializeField] private int level;
    [Header("초당 획득 골드")]
    [SerializeField] private double goldPerSec;
    [Header("구매 가격")]
    [SerializeField] private float purchaseCost;
    [Header("잠금 해제 여부")]
    [SerializeField] private bool unlock;
    public string Name { get => name; set => name = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public int Level { get => level;
        set
        {
            level = value;
            goldPerSec = goldPerSec * Mathf.Pow(1.15f, level);
        }
    }
    public double GoldPerSec { get => goldPerSec; set => goldPerSec = value; }
    public float PurchaseCost { get => purchaseCost; set => purchaseCost = value; }
    public bool Unlock { get => unlock; set => unlock = value; }
}
