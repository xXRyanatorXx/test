using System;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class FOLLOWcar : MonoBehaviour
{
	// Token: 0x060007D0 RID: 2000 RVA: 0x000446D5 File Offset: 0x000428D5
	private void FixedUpdate()
	{
		base.transform.position = this.target.position;
		base.transform.rotation = this.target.rotation;
	}

	// Token: 0x04000EC2 RID: 3778
	public Transform target;
}
