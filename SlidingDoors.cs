using System;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class SlidingDoors : MonoBehaviour
{
	// Token: 0x06000B66 RID: 2918 RVA: 0x0007D703 File Offset: 0x0007B903
	private void OnTriggerEnter(Collider other)
	{
		this.door1.JustOpen();
		this.door2.JustOpen();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0007D71B File Offset: 0x0007B91B
	private void OnTriggerExit(Collider other)
	{
		this.door1.JusClose();
		this.door2.JusClose();
	}

	// Token: 0x040013D5 RID: 5077
	public OpenGarage door1;

	// Token: 0x040013D6 RID: 5078
	public OpenGarage door2;
}
