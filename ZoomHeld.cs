using System;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class ZoomHeld : MonoBehaviour
{
	// Token: 0x06000C96 RID: 3222 RVA: 0x0008C7CC File Offset: 0x0008A9CC
	private void Update()
	{
		if (!(tools.helditem == "nothing"))
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f && base.transform.localPosition.x < 0.05f)
			{
				base.transform.Translate(-Vector3.right * Time.deltaTime * this.zoomspeed, Space.Self);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f && base.transform.localPosition.x > 0.033f)
			{
				base.transform.Translate(Vector3.right * Time.deltaTime * this.zoomspeed, Space.Self);
			}
		}
	}

	// Token: 0x04001577 RID: 5495
	public float zoomspeed;
}
