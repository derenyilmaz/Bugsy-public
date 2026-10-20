using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Sprite = UnityEngine.Sprite;

public class Boss : BasicDamageable
{
    public float moveSpeed;
    
    [SerializeField] private Transform headNode;
    [SerializeField] private List<Transform> tailNodes;
    
    [SerializeField] private AudioClip deathSound;
    
    [SerializeField] private Sprite _deathFace;
    
    private SpriteRenderer _spriteRenderer;
    private Vector3 _lookDir = Vector2.right;
    private Transform _target;

    private bool _defeated = false;
    private bool _started = false;
    
    protected override void Start()
    {
        base.Start();
        _target = FindAnyObjectByType<Character>().transform;
    }

    private void OnEnable()
    {
        StartWaiting();
    }

    private void Update()
    {
        if (!_defeated && _started)
        {
            Move();
        }
    }

    private void StartWaiting()
    {
        if (_waitingCoroutine == null) _waitingCoroutine = StartCoroutine(StartWaitingRoutine());
    }

    private Coroutine _waitingCoroutine;
    private IEnumerator StartWaitingRoutine()
    {
        _started = false;
        yield return new WaitForSeconds(0.5f);
        _started = true;
    }
    
    private void Move()
    {
        var moveVector = (_target.position - headNode.position).normalized * (moveSpeed * Time.deltaTime);
        _lookDir = moveVector.normalized;
        
        var tailBuffer = new Transform[tailNodes.Count];
        for (var index = 0; index < tailNodes.Count; index++)
        {
            tailBuffer[index] = tailNodes[index];
        }

        var headBuffer = headNode.transform.position;
        
        headNode.transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(_lookDir));
        headNode.transform.position += moveVector;
        
        var nextMoveVector = (headBuffer - tailNodes[0].position);
        tailNodes[0].position += nextMoveVector * (moveSpeed * Time.deltaTime);
        tailNodes[0].transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(nextMoveVector));
        
        for (int i = 1; i < tailNodes.Count; i++)
        {
            var target = tailBuffer[i-1];
            nextMoveVector = (target.position - tailNodes[i].position);
            tailNodes[i].position += nextMoveVector * (moveSpeed * Time.deltaTime);
            tailNodes[i].transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(nextMoveVector));
        }
    }

    private float GetRotationAngle(Vector3 lookDir)
    {
        return 90 + (math.atan2(lookDir.y, lookDir.x) * 180 / math.PI);
        
        if (lookDir.x > 0.92 && lookDir.y < 0.39 && lookDir.y > -0.39) return 90f;
        if (lookDir.x < 0.39 && lookDir.x > -0.39 && lookDir.y > 0.92) return 180f;
        if (lookDir.x < -0.92 && lookDir.y < 0.39 && lookDir.y > -0.39) return 270f;
        if (lookDir.x < 0.39 && lookDir.x > -0.39 && lookDir.y < -0.92) return 0f;
        // order matters ^
        if (lookDir.x > 0 && lookDir.y < 0) return 45f;
        if (lookDir.x > 0 && lookDir.y > 0) return 135f;
        if (lookDir.x < 0 && lookDir.y > 0) return 225f;
        if (lookDir.x < 0 && lookDir.y < 0) return 315f;
        return 0f;
    }

    protected override void Die()
    {
        // todo: death vfx 
        // todo: you win screen, send scene to end title & credits
        foreach (var sprite in hitFlashSprites)
        {
            //stop boss from hurting player
            sprite.transform.GetComponent<BoxCollider2D>().enabled = false;
        }
        _defeated = true;
        headNode.GetComponent<SpriteRenderer>().sprite = _deathFace;
        AudioManager.Instance.PlaySound(deathSound);

        StartCoroutine(WaitAndLoadEndTitleScene());
        // base.Die();
    }

    private IEnumerator WaitAndLoadEndTitleScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("EndingCardScene");
    }
}
