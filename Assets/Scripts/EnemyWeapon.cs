using System;

using DG.Tweening;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public float baseDamage;
    public float rotationSpeed;
    
    [SerializeField] Collider2D rotatingObjCollider;

    private void Start()
    {
        // rotatingObjCollider.transform.DORotate(Vector3.forward * rotationSpeed, 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.GetHit(baseDamage);
        }
    }
}
