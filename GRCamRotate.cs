using System;
using UnityEngine;

// Token: 0x0200022A RID: 554
public class GRCamRotate : MonoBehaviour
{
	// Token: 0x06000CC0 RID: 3264 RVA: 0x0008D129 File Offset: 0x0008B329
	private void Update()
	{
		base.transform.Rotate(Vector3.up * 0.3f);
	}
}
