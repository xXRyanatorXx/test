using System;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class turnOffBuildCanvas : MonoBehaviour
{
	// Token: 0x06001150 RID: 4432 RVA: 0x000A5C6A File Offset: 0x000A3E6A
	private void Update()
	{
		if (tools.tool != 41 && tools.tool != 43 && tools.tool != 44)
		{
			base.gameObject.SetActive(false);
		}
	}
}
