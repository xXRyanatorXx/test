using System;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class ExtendedFlycam : MonoBehaviour
{
	// Token: 0x06000F33 RID: 3891 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0009E408 File Offset: 0x0009C608
	private void Update()
	{
		this.rotationX += Input.GetAxis("Mouse X") * this.cameraSensitivity * Time.deltaTime;
		this.rotationY += Input.GetAxis("Mouse Y") * this.cameraSensitivity * Time.deltaTime;
		this.rotationY = Mathf.Clamp(this.rotationY, -90f, 90f);
		base.transform.localRotation = Quaternion.AngleAxis(this.rotationX, Vector3.up);
		base.transform.localRotation *= Quaternion.AngleAxis(this.rotationY, Vector3.left);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			base.transform.position += base.transform.right * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			base.transform.position += base.transform.right * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else
		{
			base.transform.position += base.transform.forward * this.normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
			base.transform.position += base.transform.right * this.normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			base.transform.position += base.transform.up * this.climbSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.E))
		{
			base.transform.position -= base.transform.up * this.climbSpeed * Time.deltaTime;
		}
		if (Input.GetKeyDown(KeyCode.End))
		{
			if (Cursor.lockState == CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.Locked;
				return;
			}
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Token: 0x04001859 RID: 6233
	public float cameraSensitivity = 90f;

	// Token: 0x0400185A RID: 6234
	public float climbSpeed = 4f;

	// Token: 0x0400185B RID: 6235
	public float normalMoveSpeed = 10f;

	// Token: 0x0400185C RID: 6236
	public float slowMoveFactor = 0.25f;

	// Token: 0x0400185D RID: 6237
	public float fastMoveFactor = 3f;

	// Token: 0x0400185E RID: 6238
	private float rotationX;

	// Token: 0x0400185F RID: 6239
	private float rotationY;
}
