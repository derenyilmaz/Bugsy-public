using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossMovementInput : MonoBehaviour
{
    public float moveSpeed;
    public float dashMultiplier;
    public int dashCooldownFrames;
    
    [SerializeField] private Transform headNode;
    [SerializeField] private List<Transform> tailNodes;

    private const float MinX = -7.7f;
    private const float MaxX = 7.7f;
    private const float MinY = -4.15f;
    private const float MaxY = 4.08f;
    
    private SpriteRenderer _spriteRenderer;
    private Vector3 _lookDir = Vector2.right;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
	    
        if(Keyboard.current.dKey.isPressed)
        {
            _lookDir = Vector3.right;
            _spriteRenderer.flipX = false;
        }
        else if(Keyboard.current.aKey.isPressed)
        {
            _lookDir = Vector3.left;
            _spriteRenderer.flipX = true;
        }
        else
        {
            _lookDir = Vector3.zero;
        }
		
        if(Keyboard.current.wKey.isPressed)
        {
            _lookDir += Vector3.up;
        }
        else if(Keyboard.current.sKey.isPressed)
        {
            _lookDir += Vector3.down;
        }

        if (_lookDir != Vector3.zero)
        {
            Move();
        }
    }

    private void Move()
    {
        var moveVector = _lookDir.normalized * (moveSpeed * Time.deltaTime);
        if (!CheckBounds(headNode.position + moveVector)) return;
        
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
    
    private bool CheckBounds(Vector3 pos)
    {
        if (MinX < pos.x && pos.x < MaxX &&
            MinY < pos.y && pos.y < MaxY) return true;
        return false;
    }

    private float GetRotationAngle(Vector3 lookDir)
    {
        if (lookDir.x > 0 && lookDir.y < 0) return 45f;
        if (lookDir.x > 0 && lookDir.y == 0) return 90f;
        if (lookDir.x > 0 && lookDir.y > 0) return 135f;
        if (lookDir.x == 0 && lookDir.y > 0) return 180f;
        if (lookDir.x < 0 && lookDir.y > 0) return 225f;
        if (lookDir.x < 0 && lookDir.y == 0) return 270f;
        if (lookDir.x < 0 && lookDir.y < 0) return 315f;
        return 0f;
    }

}
