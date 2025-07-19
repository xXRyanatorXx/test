using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class ActivateChildrenOnEnableAfterTime : MonoBehaviour
{
	// Token: 0x06000C9B RID: 3227 RVA: 0x0008C93C File Offset: 0x0008AB3C
	public void EnableChildren()
	{
		foreach (object obj in base.transform)
		{
			((Transform)obj).gameObject.SetActive(true);
		}
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0008C998 File Offset: 0x0008AB98
	private void OnEnable()
	{
		base.Invoke("EnableChildren", this.delay);
	}

	// Token: 0x0400157D RID: 5501
	public float delay = 5f;
}
