using System;
using UnityEngine;

// Token: 0x020001FF RID: 511
[Serializable]
public class Item
{
	// Token: 0x04001494 RID: 5268
	public ShopItem i;

	// Token: 0x04001495 RID: 5269
	public int amountMin;

	// Token: 0x04001496 RID: 5270
	public int amountMax;

	// Token: 0x04001497 RID: 5271
	public float price;

	// Token: 0x04001498 RID: 5272
	[HideInInspector]
	public int finalAmount;

	// Token: 0x04001499 RID: 5273
	[HideInInspector]
	public int position;

	// Token: 0x0400149A RID: 5274
	[HideInInspector]
	public bool randomSelected;
}
