using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class WaterFloat : MonoBehaviour
{
	// Token: 0x06000506 RID: 1286 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00029734 File Offset: 0x00027934
	private void Update()
	{
		if (base.transform.position.y < this.WaterHeight)
		{
			base.transform.position = new Vector3(base.transform.position.x, this.WaterHeight, base.transform.position.z);
		}
	}

	// Token: 0x040006BA RID: 1722
	public float WaterHeight = 15.5f;
}
