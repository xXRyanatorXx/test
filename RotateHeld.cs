using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class RotateHeld : MonoBehaviour
{
	// Token: 0x06000A3C RID: 2620 RVA: 0x00064D60 File Offset: 0x00062F60
	private void LateUpdate()
	{
		if (!Input.GetKey(this.crouchKey))
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				base.transform.Rotate(Vector3.right * this.rotationspeed * Time.deltaTime);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				base.transform.Rotate(-Vector3.right * this.rotationspeed * Time.deltaTime);
			}
		}
	}

	// Token: 0x04001212 RID: 4626
	public float rotationspeed;

	// Token: 0x04001213 RID: 4627
	public KeyCode crouchKey = KeyCode.LeftControl;
}
