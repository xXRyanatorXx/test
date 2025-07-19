using System;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class CharController_Motor : MonoBehaviour
{
	// Token: 0x060004FE RID: 1278 RVA: 0x0002948E File Offset: 0x0002768E
	private void Start()
	{
		this.character = base.GetComponent<CharacterController>();
		if (Application.isEditor)
		{
			this.webGLRightClickRotation = false;
			this.sensitivity *= 1.5f;
		}
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000294BC File Offset: 0x000276BC
	private void CheckForWaterHeight()
	{
		if (base.transform.position.y < this.WaterHeight)
		{
			this.gravity = 0f;
			return;
		}
		this.gravity = -9.8f;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000294F0 File Offset: 0x000276F0
	private void Update()
	{
		this.moveFB = Input.GetAxis("Horizontal") * this.speed;
		this.moveLR = Input.GetAxis("Vertical") * this.speed;
		this.rotX = Input.GetAxis("Mouse X") * this.sensitivity;
		this.rotY = Input.GetAxis("Mouse Y") * this.sensitivity;
		this.CheckForWaterHeight();
		Vector3 vector = new Vector3(this.moveFB, this.gravity, this.moveLR);
		if (this.webGLRightClickRotation)
		{
			if (Input.GetKey(KeyCode.Mouse0))
			{
				this.CameraRotation(this.cam, this.rotX, this.rotY);
			}
		}
		else if (!this.webGLRightClickRotation)
		{
			this.CameraRotation(this.cam, this.rotX, this.rotY);
		}
		vector = base.transform.rotation * vector;
		this.character.Move(vector * Time.deltaTime);
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000295EF File Offset: 0x000277EF
	private void CameraRotation(GameObject cam, float rotX, float rotY)
	{
		base.transform.Rotate(0f, rotX * Time.deltaTime, 0f);
		cam.transform.Rotate(-rotY * Time.deltaTime, 0f, 0f);
	}

	// Token: 0x040006AE RID: 1710
	public float speed = 10f;

	// Token: 0x040006AF RID: 1711
	public float sensitivity = 30f;

	// Token: 0x040006B0 RID: 1712
	public float WaterHeight = 15.5f;

	// Token: 0x040006B1 RID: 1713
	private CharacterController character;

	// Token: 0x040006B2 RID: 1714
	public GameObject cam;

	// Token: 0x040006B3 RID: 1715
	private float moveFB;

	// Token: 0x040006B4 RID: 1716
	private float moveLR;

	// Token: 0x040006B5 RID: 1717
	private float rotX;

	// Token: 0x040006B6 RID: 1718
	private float rotY;

	// Token: 0x040006B7 RID: 1719
	public bool webGLRightClickRotation = true;

	// Token: 0x040006B8 RID: 1720
	private float gravity = -9.8f;
}
