using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class RotateMoveCamera : MonoBehaviour
{
	// Token: 0x060000D6 RID: 214 RVA: 0x00008FE0 File Offset: 0x000071E0
	private void Update()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		float axis = Input.GetAxis("Mouse X");
		float axis2 = Input.GetAxis("Mouse Y");
		if (axis != this.MouseX || axis2 != this.MouseY)
		{
			this.rotationX += axis * this.sensX * Time.deltaTime;
			this.rotationY += axis2 * this.sensY * Time.deltaTime;
			this.rotationY = Mathf.Clamp(this.rotationY, this.minY, this.maxY);
			this.MouseX = axis;
			this.MouseY = axis2;
			this.Camera.transform.localEulerAngles = new Vector3(-this.rotationY, this.rotationX, 0f);
		}
		if (Input.GetKey(KeyCode.W))
		{
			base.transform.Translate(new Vector3(0f, 0f, 0.1f));
		}
		else if (Input.GetKey(KeyCode.S))
		{
			base.transform.Translate(new Vector3(0f, 0f, -0.1f));
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Translate(new Vector3(0.1f, 0f, 0f));
			return;
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Translate(new Vector3(-0.1f, 0f, 0f));
		}
	}

	// Token: 0x04000153 RID: 339
	public GameObject Camera;

	// Token: 0x04000154 RID: 340
	public float minX = -360f;

	// Token: 0x04000155 RID: 341
	public float maxX = 360f;

	// Token: 0x04000156 RID: 342
	public float minY = -45f;

	// Token: 0x04000157 RID: 343
	public float maxY = 45f;

	// Token: 0x04000158 RID: 344
	public float sensX = 100f;

	// Token: 0x04000159 RID: 345
	public float sensY = 100f;

	// Token: 0x0400015A RID: 346
	private float rotationY;

	// Token: 0x0400015B RID: 347
	private float rotationX;

	// Token: 0x0400015C RID: 348
	private float MouseX;

	// Token: 0x0400015D RID: 349
	private float MouseY;
}
