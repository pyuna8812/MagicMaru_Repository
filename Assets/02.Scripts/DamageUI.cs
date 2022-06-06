using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DamageUI : MonoBehaviour
{
    private Image image;
    private TextMeshProUGUI tmp;
    private Color color = new Color(255, 255, 255, 0);
    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        image.DOColor(color, Player.instance.attackDelay - 0.3f);
        tmp.DOColor(color, Player.instance.attackDelay - 0.3f);
        Invoke("ResetDamageUI", 2f);
    }
    private void ResetDamageUI()
    {
        transform.gameObject.SetActive(false);
        image.color = Color.white;
        tmp.color = Color.white;
    }
}
