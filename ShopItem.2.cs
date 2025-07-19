using System;
using UnityEngine;

// Token: 0x02000201 RID: 513
[CreateAssetMenu(fileName = "New ShopItem", menuName = "ShopItem")]
public class ShopItem : ScriptableObject
{
	// Token: 0x0400149C RID: 5276
	public int id;

	// Token: 0x0400149D RID: 5277
	public string itemName;

	// Token: 0x0400149E RID: 5278
	public string description;

	// Token: 0x0400149F RID: 5279
	public Sprite Image;
}
