using System;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class RotateSideways : MonoBehaviour
{
	// Token: 0x06000AE6 RID: 2790 RVA: 0x0006FF1C File Offset: 0x0006E11C
	private void LateUpdate()
	{
		if (!Input.GetKey(this.crouchKey))
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				base.transform.Rotate(Vector3.forward * this.rotationspeed * Time.deltaTime);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				base.transform.Rotate(-Vector3.forward * this.rotationspeed * Time.deltaTime);
			}
		}
	}

	// Token: 0x0400134E RID: 4942
	public float rotationspeed;

	// Token: 0x0400134F RID: 4943
	public KeyCode crouchKey = KeyCode.LeftControl;
}
