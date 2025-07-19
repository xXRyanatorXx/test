using System;
using Rewired;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class carcam : MonoBehaviour
{
	// Token: 0x06000BB3 RID: 2995 RVA: 0x00082164 File Offset: 0x00080364
	public void Awake()
	{
		this.Player = ReInput.players.GetPlayer(0);
		if (PlayerPrefs.HasKey("masterInvertY"))
		{
			if (PlayerPrefs.GetInt("masterInvertY") == 0)
			{
				this.invertY = false;
			}
			if (PlayerPrefs.GetInt("masterInvertY") == 1)
			{
				this.invertY = true;
			}
			if (PlayerPrefs.HasKey("UseHeadbob") && PlayerPrefs.GetFloat("UseHeadbob") == 1f)
			{
				this.UseHeadBob = true;
				return;
			}
			this.UseHeadBob = false;
		}
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x000821E4 File Offset: 0x000803E4
	private void OnEnable()
	{
		this.Head = base.transform;
		this.Car = base.transform.root.GetComponent<Rigidbody>();
		this.TargetPosition = this.Head.localPosition;
		if (this.transitionSpeed == 0f)
		{
			this.transitionSpeed = 0.2f;
		}
		this.Velocity = this.Car.transform.InverseTransformDirection(this.Car.velocity);
		this.PreviousVelocity = this.Car.transform.InverseTransformDirection(this.Car.velocity);
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x0008227E File Offset: 0x0008047E
	private void OnDisable()
	{
		this.Head.localPosition = this.TargetPosition;
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00082294 File Offset: 0x00080494
	private void Update()
	{
		base.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * this.mouseSensitivity, Space.Self);
		base.transform.Rotate(Vector3.up, this.Player.GetAxis("LookingHorizontal") * this.mouseSensitivity, Space.Self);
		if (!this.invertY)
		{
			base.transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * this.mouseSensitivity, Space.Self);
			this.player.transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * this.mouseSensitivity, Space.Self);
			base.transform.Rotate(Vector3.right, -this.Player.GetAxis("LookingVertical") * this.mouseSensitivity, Space.Self);
			this.player.transform.Rotate(Vector3.right, -this.Player.GetAxis("LookingVertical") * this.mouseSensitivity, Space.Self);
		}
		else
		{
			base.transform.Rotate(Vector3.left, -Input.GetAxis("Mouse Y") * this.mouseSensitivity, Space.Self);
			this.player.transform.Rotate(Vector3.left, -Input.GetAxis("Mouse Y") * this.mouseSensitivity, Space.Self);
			base.transform.Rotate(Vector3.left, -this.Player.GetAxis("LookingVertical") * this.mouseSensitivity, Space.Self);
			this.player.transform.Rotate(Vector3.left, -this.Player.GetAxis("LookingVertical") * this.mouseSensitivity, Space.Self);
		}
		float z = base.transform.localEulerAngles.z;
		base.transform.Rotate(0f, 0f, -z);
		if (this.UseHeadBob)
		{
			this.Velocity = this.Car.transform.InverseTransformDirection(this.Car.velocity);
			this.VelocityDiff = this.PreviousVelocity - this.Velocity;
			this.Head.localPosition = Vector3.Lerp(this.Head.localPosition, this.TargetPosition + this.VelocityDiff, Time.deltaTime * this.transitionSpeed);
			this.PreviousVelocity = this.Car.transform.InverseTransformDirection(this.Car.velocity);
		}
	}

	// Token: 0x04001456 RID: 5206
	private float x;

	// Token: 0x04001457 RID: 5207
	private float y;

	// Token: 0x04001458 RID: 5208
	private Vector3 rotateValue;

	// Token: 0x04001459 RID: 5209
	public GameObject player;

	// Token: 0x0400145A RID: 5210
	private Player Player;

	// Token: 0x0400145B RID: 5211
	public bool invertY;

	// Token: 0x0400145C RID: 5212
	public bool UseHeadBob;

	// Token: 0x0400145D RID: 5213
	public float mouseSensitivity = 2f;

	// Token: 0x0400145E RID: 5214
	private Vector2 rotation = new Vector2(0f, 0f);

	// Token: 0x0400145F RID: 5215
	public float speed = 3f;

	// Token: 0x04001460 RID: 5216
	public Rigidbody Car;

	// Token: 0x04001461 RID: 5217
	public Vector3 TargetPosition;

	// Token: 0x04001462 RID: 5218
	public Vector3 PreviousVelocity;

	// Token: 0x04001463 RID: 5219
	public Vector3 Velocity;

	// Token: 0x04001464 RID: 5220
	public Vector3 VelocityDiff;

	// Token: 0x04001465 RID: 5221
	public float transitionSpeed;

	// Token: 0x04001466 RID: 5222
	public Transform Head;
}
