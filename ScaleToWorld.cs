using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class ScaleToWorld : MonoBehaviour
{
	// Token: 0x06000600 RID: 1536 RVA: 0x0002FAC8 File Offset: 0x0002DCC8
	private void Update()
	{
		base.transform.localScale = new Vector3(this.FixeScale / this.parent.transform.localScale.x, this.FixeScale / this.parent.transform.localScale.y, this.FixeScale / this.parent.transform.localScale.z);
	}

	// Token: 0x04000900 RID: 2304
	public float FixeScale = 1f;

	// Token: 0x04000901 RID: 2305
	public GameObject parent;
}
