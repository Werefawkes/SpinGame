using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomInspector;

public class PlayerController : MonoBehaviour, IDamageable
{
	public WeaponSO currentWeapon;
	float cooldownEnd;

	public bool isAttacking = false;

	[ForceFill]
	public Transform pointer;
	public float pointerAngleOffset = 90;

	public float speed = 1;
	[ReadOnly(DisableStyle.OnlyText)]
	public Vector2 moveInput;
	[ReadOnly(DisableStyle.OnlyText)]
	public Vector2 aimInput;

	[SelfFill(true)]
	public Rigidbody2D rb;
	[SelfFill(true)]
	public Camera playerCam;

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
		if (isAttacking && Time.time >= cooldownEnd)
		{
			Attack();
		}
	}
	public void Damage(float amount, Vector2 knockback, Vector2 hitPosition)
	{
		Debug.Log("Took damage of amount " + amount);
		rb.AddForce(knockback, ForceMode2D.Impulse);
	}

	public void Attack()
	{
		if (Time.time >= cooldownEnd)
		{
			currentWeapon.SpawnProjectile(pointer.transform.position, aimInput);
			cooldownEnd = Time.time + currentWeapon.cooldown;
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

	public void OnFire(InputValue val)
	{
		isAttacking = val.Get<float>() == 1;
	}
	#endregion

}
