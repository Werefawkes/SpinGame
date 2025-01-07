using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomInspector;

public class PlayerController : MonoBehaviour, IDamageable, IShooter
{
	[HorizontalLine("Stats"), Foldout]
	public WeaponSO currentWeapon;
	float primaryCooldownEnd, secondaryCooldownEnd;

	public float speed = 1;

	[HorizontalLine("References", color: FixedColor.Black)]
	[ForceFill]
	public Camera playerCam;
	[SelfFill(true)]
	public Rigidbody2D rb;
	[ForceFill]
	public Transform pointer;
	public float pointerAngleOffset = 90;

	[HorizontalLine]
	[ReadOnly]
	public Vector2 moveInput, aimInput;
	[ReadOnly]
	public bool isPrimaryActionHeld = false, isSecondaryActionHeld = false;

	// interface
	public Vector3 ProjectileOrigin { get { return pointer.transform.position; } }
	public Vector2 AimDirection { get { return aimInput; } }
	public Rigidbody2D Rigidbody { get { return rb; } }

	private void Start()
	{
		GameManager.player = this;
	}

	private void Update()
	{
		// Movement
		rb.velocity = speed * moveInput;

		// Aim
		if (aimInput != Vector2.zero)
		{
			pointer.localPosition = aimInput;
			Vector3 angles = Vector3.zero;
			angles.z = Vector2.SignedAngle(transform.up, aimInput) + pointerAngleOffset;
			pointer.localEulerAngles = angles;
		}

		// Attack
		if (isPrimaryActionHeld && Time.time >= primaryCooldownEnd)
		{
			PrimaryAttack();
		}

		if (isSecondaryActionHeld && Time.time >= secondaryCooldownEnd)
		{
			SecondaryAttack();
		}
	}

	public void Damage(float amount, Vector2 knockback, Vector2 hitPosition)
	{
		Debug.Log("Took damage of amount " + amount);
		rb.AddForce(knockback, ForceMode2D.Impulse);
	}

	void PrimaryAttack()
	{
		if (Time.time >= primaryCooldownEnd)
		{
			currentWeapon.PrimaryAction(this);
			primaryCooldownEnd = Time.time + currentWeapon.primaryCooldown;
		}
	}

	void SecondaryAttack()
	{
		if (Time.time >= secondaryCooldownEnd)
		{
			currentWeapon.SecondaryAction(this);
			secondaryCooldownEnd = Time.time + currentWeapon.secondaryCooldown;
		}
	}

	#region Input methods
	public void OnMove(InputValue val)
	{
		moveInput = val.Get<Vector2>();
	}

	public void OnPoint(InputValue val)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(val.Get<Vector2>());
		Vector2 dir = pos - transform.position;
		aimInput = dir.normalized;
	}

	public void OnAim(InputValue val)
	{
		aimInput = val.Get<Vector2>();
	}

	public void OnPrimaryAction(InputValue val)
	{
		isPrimaryActionHeld = val.Get<float>() == 1;
	}
	public void OnSecondaryAction(InputValue val)
	{
		isSecondaryActionHeld = val.Get<float>() == 1;
	}
	#endregion

}
