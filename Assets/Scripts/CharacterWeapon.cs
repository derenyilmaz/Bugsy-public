using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CharacterWeapon : MonoBehaviour
{
    public float baseDamage;
    public float critChance;
    public float critMultiplier;
    
    private SlashCollider _slashCollider;
    private Coroutine _coroutine;

    private bool _dead = false;
    
    private void Start()
    {
        _slashCollider = GetComponent<SlashCollider>();
    }

    private void Update()
    {
        if (_dead)
        {
            return;
        }
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Hit();
        }
    }

    public void Die()
    {
        _dead = true;
    }
    
    private void Hit()
    {
        if (_coroutine != null) return;
        _coroutine = StartCoroutine(HitRoutine());
    }
    
    private IEnumerator HitRoutine()
    {
        Debug.Log("hit routine");
        yield return _slashCollider.Slash();
        var hits = _slashCollider.GetHits();

        Debug.Log("hits :" + hits.Count);
        
        foreach (var collidersHit in hits)
        {
            
            var damageable = collidersHit.GetComponentInParent<Boss>();
            Debug.Log("damageable: " + collidersHit.gameObject.name);
            if (damageable != null)
            {
                Debug.Log("before apply dfamage");
                ApplyDamage(damageable);
            }
        }
        
        _coroutine = null;
    }

    private void ApplyDamage(BasicDamageable damageable)
    {
        var amount = CalculateDamage();
        damageable.GetHit(amount);
    }

    private float CalculateDamage()
    {
        if (critChance < 0 || critChance > 100)
        {
            Debug.LogWarning("Crit Chance format incorrect, give a number between 0-100");
        }
        
        var rand = Random.Range(0f, 100f);
        if (rand > critChance) return baseDamage;
        
        Debug.Log("CRIT!!!");
        return baseDamage * critMultiplier;
    }
}
