using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


public class BasicDamageable : MonoBehaviour
{
    private float HitPoints
    {
        get => hitPoints;
        set
        {
            hitPoints = value;
            healthBar?.UpdateBar(hitPoints);
        }
    }

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private AudioClip takeDamageSound;

    public float hitPoints;
    public float reductionPoints;
    public bool immune;

    public List<SpriteRenderer> hitFlashSprites;
    public float flashDuration = 0.05f;

    private Color baseColor;

    private float _startHitPoints;
    private Transform _startTransform;
    
    protected virtual void Start()
    {
        _startHitPoints = hitPoints;
        _startTransform = transform;
        healthBar.SetStart(_startHitPoints);
        
        if (hitFlashSprites.Count > 0 )
        {
            baseColor = hitFlashSprites[0].color;
        }
    }

    public virtual void GetHit(float amount)
    {
        Debug.Log("aaaaaaa");
        if (immune) return; // todo: feedback
        
        var netAmount = CalculateHitTaken(amount);
        Debug.Log($"{gameObject.name} took {netAmount}");
        if (netAmount >= HitPoints)
        {
            HitPoints = 0;
            Die();
            return;
        }

        DamageFlash();
        AudioManager.Instance.PlaySound(takeDamageSound);

        HitPoints -= netAmount;
    }

    private void DamageFlash()
    {
        foreach (var sprite in hitFlashSprites)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(sprite.DOColor(Color.red, flashDuration));
            sequence.Append(sprite.DOColor(baseColor, flashDuration));
        }
    }

    protected virtual float CalculateHitTaken(float amount)
    {
        var multiplier = amount / (reductionPoints + amount);
        return  amount * multiplier;
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} Died!");
        Reset();
    }

    private void Reset()
    {
        HitPoints = _startHitPoints;
        transform.position = _startTransform.position;
        transform.rotation = _startTransform.rotation;
    }
}
