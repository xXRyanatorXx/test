using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class DisableRenderer : MonoBehaviour
{
	// Token: 0x060004FB RID: 1275 RVA: 0x00029480 File Offset: 0x00027680
	private void Start()
	{
		base.GetComponent<Renderer>().enabled = false;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}
}
