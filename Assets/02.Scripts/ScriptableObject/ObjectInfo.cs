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
    [Header("������Ʈ ����")]
    [SerializeField] private EKind eKind;
    [Header("������Ʈ �̸�")]
    [SerializeField] private string name;
    [Header("������Ʈ ��������Ʈ")]
    [SerializeField] private Sprite sprite;
    [Header("������Ʈ ����")]
    [SerializeField] private int level;
    [Header("�ʴ� ȹ�� ���")]
    [SerializeField] private double goldPerSec;
    [Header("���� ����")]
    [SerializeField] private float purchaseCost;
    [Header("��� ���� ����")]
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
