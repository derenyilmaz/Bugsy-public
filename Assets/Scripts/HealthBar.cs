using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] TextMeshProUGUI text;

    private float _maxAmount;
    
    private void Start()
    {
        fillImage.fillAmount = 1;
    }

    public void UpdateBar(float amount)
    {
        text.text = $"{amount}/{_maxAmount}";
        var amt = amount/_maxAmount;
        fillImage.rectTransform.anchorMax = new Vector2(amt, 1);
    }
    
    public void SetStart(float start)
    {
        _maxAmount = start;
        UpdateBar(_maxAmount);
    }
}
