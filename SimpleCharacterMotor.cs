using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterMotor : MonoBehaviour
{
	// Token: 0x0600016F RID: 367 RVA: 0x0000BA80 File Offset: 0x00009C80
	private void Awake()
	{
		this.controller = base.GetComponent<CharacterController>();
		Cursor.lockState = this.cursorLockMode;
		Cursor.visible = this.cursorVisible;
		this.targetRotation = (this.targetPivotRotation = Quaternion.identity);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000BAC3 File Offset: 0x00009CC3
	private void Update()
	{
		this.UpdateTranslation();
		this.UpdateLookRotation();
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000BAD4 File Offset: 0x00009CD4
	private void UpdateLookRotation()
	{
		float num = Input.GetAxis("Mouse Y");
		float axis = Input.GetAxis("Mouse X");
		num *= (float)(this.invertY ? -1 : 1);
		this.targetRotation = base.transform.localRotation * Quaternion.AngleAxis(axis * this.lookSpeed * Time.deltaTime, Vector3.up);
		this.targetPivotRotation = this.cameraPivot.localRotation * Quaternion.AngleAxis(num * this.lookSpeed * Time.deltaTime, Vector3.right);
		base.transform.localRotation = this.targetRotation;
		this.cameraPivot.localRotation = this.targetPivotRotation;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000BB88 File Offset: 0x00009D88
	private void UpdateTranslation()
	{
		if (this.controller.isGrounded)
		{
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			bool key = Input.GetKey(KeyCode.LeftShift);
			Vector3 a = new Vector3(axis, 0f, axis2);
			this.speed = (key ? this.runSpeed : this.walkSpeed);
			this.movement = base.transform.TransformDirection(a * this.speed);
		}
		else
		{
			this.movement.y = this.movement.y - this.gravity * Time.deltaTime;
		}
		this.finalMovement = Vector3.Lerp(this.finalMovement, this.movement, Time.deltaTime * this.movementAcceleration);
		this.controller.Move(this.finalMovement * Time.deltaTime);
	}

	// Token: 0x040001F4 RID: 500
	public CursorLockMode cursorLockMode = CursorLockMode.Locked;

	// Token: 0x040001F5 RID: 501
	public bool cursorVisible;

	// Token: 0x040001F6 RID: 502
	[Header("Movement")]
	public float walkSpeed = 2f;

	// Token: 0x040001F7 RID: 503
	public float runSpeed = 4f;

	// Token: 0x040001F8 RID: 504
	public float gravity = 9.8f;

	// Token: 0x040001F9 RID: 505
	[Space]
	[Header("Look")]
	public Transform cameraPivot;

	// Token: 0x040001FA RID: 506
	public float lookSpeed = 45f;

	// Token: 0x040001FB RID: 507
	public bool invertY = true;

	// Token: 0x040001FC RID: 508
	[Space]
	[Header("Smoothing")]
	public float movementAcceleration = 1f;

	// Token: 0x040001FD RID: 509
	private CharacterController controller;

	// Token: 0x040001FE RID: 510
	private Vector3 movement;

	// Token: 0x040001FF RID: 511
	private Vector3 finalMovement;

	// Token: 0x04000200 RID: 512
	private float speed;

	// Token: 0x04000201 RID: 513
	private Quaternion targetRotation;

	// Token: 0x04000202 RID: 514
	private Quaternion targetPivotRotation;
}
