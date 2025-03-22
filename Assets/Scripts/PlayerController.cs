using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomInspector;
using Mirror;
using ReadOnlyAttribute = CustomInspector.ReadOnlyAttribute;

public class PlayerController : NetworkBehaviour, IDamageable
{
	[HorizontalLine("Stats")]
	public float currentHealth = 20;
	public float standardViewDistance = 8;

	[HorizontalLine("Technical")]
	[Range(0, 1)]
	public float cameraLerpStep = 0.35f;
	public float knockbackDecayMult = 2;

	[HorizontalLine("References", color: FixedColor.Black)]
	[ForceFill]	public Camera playerCam;
	[SelfFill(true)] public Rigidbody2D rb;
	[SelfFill(true)] public Shooter shooter;
	[SelfFill(true)] public CharacterStats stats;
	[SelfFill(true)] public PlayerInput input;
	[ForceFill] public Transform pointer;
	[ForceFill] public CrosshairController crosshair;

	[HorizontalLine, ReadOnly]
	public Vector2 moveInput;
	[ReadOnly]
	public Vector2 aimInput;
	[ReadOnly]
	public Vector2 knockbackForce;
	[ReadOnly]
	public bool isPrimaryActionHeld = false, isSecondaryActionHeld = false;

	// interface
	public Vector3 ProjectileOrigin { get { return pointer.transform.position; } }
	public Vector2 AimDirection { get { return aimInput; } }
	public Rigidbody2D Rigidbody { get { return rb; } }

	private void Start()
	{
		currentHealth = stats.maxHealth;

		if (isLocalPlayer)
		{
			GameManager.localPlayer = this;
		}
		else
		{
			gameObject.layer = 8;
		}
	}

	private void Update()
	{
		if (!isLocalPlayer) return;

		// Movement
		Vector2 move = stats.moveSpeed * moveInput;
		knockbackForce = Vector2.Lerp(knockbackForce, Vector2.zero, knockbackDecayMult * Time.deltaTime);

		rb.linearVelocity = move + knockbackForce;

		// Aim
		if (aimInput != Vector2.zero)
		{
			pointer.localPosition = aimInput;
			Vector3 angles = Vector3.zero;
			angles.z = Vector2.SignedAngle(transform.right, aimInput);
			pointer.localEulerAngles = angles;

			shooter.aimDirection = aimInput;
		}

		Point(screenPos);
	}

	private void LateUpdate()
	{
		if (!isLocalPlayer) return;

		Vector2 camPos = Vector2.Lerp(transform.position, crosshair.transform.position, cameraLerpStep);
		playerCam.transform.position = new Vector3(camPos.x, camPos.y, -10);
	}

	public void TakeDamage(float amount, Vector2 knockback, Vector2 hitPosition)
	{
		currentHealth -= amount;
		knockbackForce = knockback;

		if (currentHealth <= 0)
		{
			// die
		}
	}

	#region Input methods
	public void OnMove(InputValue val)
	{
		if (ApplicationManager.IsUIOpen) return;

		moveInput = val.Get<Vector2>();
	}

	Vector2 screenPos;
	public void OnPoint(InputValue val)
	{
		Point(val.Get<Vector2>());
	}

	// Called every frame
	public void Point(Vector2 input)
	{
		if (ApplicationManager.IsUIOpen) return;

		screenPos = input;
		// Clamp the position to the screen
		screenPos = new(Math.Clamp(screenPos.x, 0, Screen.width), Math.Clamp(screenPos.y, 0, Screen.height));

		Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
		worldPos.z = 0;
		shooter.targetPosititon = worldPos;
		Vector2 dir = worldPos - transform.position;
		aimInput = dir.normalized;

		if (crosshair)
		{
			crosshair.transform.position = worldPos;

			crosshair.SetSize(Vector2.Distance(pointer.transform.position, crosshair.transform.position), shooter.currentFiringAngle / 2);
		}
	}

	public void OnAim(InputValue val)
	{
		if (ApplicationManager.IsUIOpen) return;

		aimInput = val.Get<Vector2>();
	}

	public void OnPrimaryAction(InputValue val)
	{
		if (ApplicationManager.IsUIOpen) return;

		isPrimaryActionHeld = val.Get<float>() == 1;
		shooter.isAttacking = isPrimaryActionHeld;
	}

	public void OnSecondaryAction(InputValue val)
	{
		if (ApplicationManager.IsUIOpen) return;

		isSecondaryActionHeld = val.Get<float>() == 1;
	}

	public void OnOpenMenu()
	{
		ApplicationManager.SetUIState(true);

		input.SwitchCurrentActionMap("UI");
	}

	public void OnCloseMenu()
	{
		ApplicationManager.SetUIState(false);

		input.SwitchCurrentActionMap("Player");
	}
	#endregion
}