using System;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class Open : MonoBehaviour
{
	// Token: 0x06000BD0 RID: 3024 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0008316F File Offset: 0x0008136F
	public void RUN()
	{
		base.transform.RotateAround(base.transform.position, base.transform.right, 90f);
	}

	// Token: 0x0400147B RID: 5243
	public bool open;
}
