using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlashCollider : MonoBehaviour
{
    [SerializeField] Collider2D slashObjCollider;
    [SerializeField] AudioClip slashSound;

    public float rotationAngle;
    
    private HashSet<Collider2D> _hits = new HashSet<Collider2D>();
    private Vector3 _startRotation;
    private Coroutine _coroutine;
    private bool _slashing;
    

    private void Start()
    {
        slashObjCollider.enabled = false;
        _startRotation = transform.eulerAngles;
    }
    
    public IEnumerator Slash()
    {
        if (_slashing) yield break;
        _slashing = true;
        AudioManager.Instance.PlaySound(slashSound);
        
        _hits.Clear();
        slashObjCollider.enabled = true;
        
        slashObjCollider.transform.rotation = Quaternion.Euler(_startRotation);
        slashObjCollider.transform.DORotate(_startRotation + Vector3.back * rotationAngle, .25f)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                slashObjCollider.enabled = false;
                _slashing = false;
            });
        yield return new WaitUntil(() => !_slashing);
    }
    
    public HashSet<Collider2D> GetHits() => _hits;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        _hits.Add(collision);
    }
}
