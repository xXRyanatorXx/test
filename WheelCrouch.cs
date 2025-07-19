using System;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class WheelCrouch : MonoBehaviour
{
	// Token: 0x06000C93 RID: 3219 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x0008C704 File Offset: 0x0008A904
	private void Update()
	{
		float num = Input.GetAxis("Mouse ScrollWheel") / 2f;
		if (num != 0f)
		{
			base.transform.localScale -= Vector3.one * num;
			base.transform.localScale = Vector3.Max(base.transform.localScale, this.minScale);
			base.transform.localScale = Vector3.Min(base.transform.localScale, this.maxScale);
		}
	}

	// Token: 0x04001575 RID: 5493
	private Vector3 minScale = new Vector3(1f, 0.1f, 1f);

	// Token: 0x04001576 RID: 5494
	private Vector3 maxScale = new Vector3(1f, 1f, 1f);
}
