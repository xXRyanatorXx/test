using System;
using PaintIn3D;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class testDust : MonoBehaviour
{
	// Token: 0x0600114D RID: 4429 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x000A5C3D File Offset: 0x000A3E3D
	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			this.RustRepairDecal.HandleHitPoint(false, 0, 1f, 0, base.transform.position, Quaternion.identity);
		}
	}

	// Token: 0x040018AC RID: 6316
	public P3dPaintSphere RustRepairDecal;
}
