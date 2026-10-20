using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementInput : MonoBehaviour
{
    public float moveSpeed;
    public float dashMultiplier;
    public int dashCooldownFrames;
    
    public SpriteRenderer _spriteRenderer;

    [SerializeField] private AudioClip dashSound;
    
    private const float MinX = -8f;
    private const float MaxX = 8f;
    private const float MinY = -4.3f;
    private const float MaxY = 4.08f;
    

	private Vector3 _lookDir = Vector2.right;
	private int _frameCounter = 0;
	private int _framesSinceLastDash = 0;

	private bool canMove = true;

    private void Start()
    {
	    _framesSinceLastDash = dashCooldownFrames;
    }

	private void Update()
    {
	    if (!canMove)
	    {
		    return;
	    }
	    
	    _frameCounter++;
	    _framesSinceLastDash++;
	    
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
		
		if (Keyboard.current.spaceKey.wasPressedThisFrame)
		{
			Dash();
		}
    }

	private void Move()
	{
		var moveVector = _lookDir.normalized * (moveSpeed * Time.deltaTime);
		if (!CheckBounds(transform.position + moveVector)) return;
		
		transform.position += moveVector;
		// Debug.Log($"Moving on frame {_frameCounter}\n" +
		//           $"Look Vector: {_lookDir}\n" +
		//           $"Move Vector: {moveVector}\n" +
		//           $"Delta Time: {Time.deltaTime}");
	}

	private void Dash()
	{
		// >0 check here is a show-off (in case of overflow, what if user waits int.max frames and then decides to dash??)
		if (_framesSinceLastDash <= dashCooldownFrames && _framesSinceLastDash > 0) return;
		
		AudioManager.Instance.PlaySound(dashSound);
		// we use fixedDeltaTime here because dashing repeatedly causes deltaTime to differ so consecutive dashes
		// have high variance. decide if we want to use this dash logic and if it's ok to use fixedDeltaTime for movement
		var dashVector = _lookDir.normalized * (dashMultiplier * moveSpeed * Time.fixedDeltaTime);
		if (!CheckBounds(transform.position + dashVector)) return;
		
		transform.position += dashVector;
		_framesSinceLastDash = 0;
		// Debug.Log($"Dashing on frame {_frameCounter}\n" +
		//           $"Look Vector: {_lookDir}\n" +
		//           $"Dash Vector: {dashVector}\n" +
		//           $"Delta Time: {Time.deltaTime}");
	}
	
	private bool CheckBounds(Vector3 pos)
	{
		if (MinX < pos.x && pos.x < MaxX &&
		    MinY < pos.y && pos.y < MaxY) return true;
		return false;
	}

	public void StopMovement()
	{
		canMove = false;
	}
}	
