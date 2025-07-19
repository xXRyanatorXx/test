using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class BuildingParent : MonoBehaviour
{
	// Token: 0x060000A5 RID: 165 RVA: 0x00007DD6 File Offset: 0x00005FD6
	public void NextWall()
	{
		this.currentIndex++;
		if (this.currentIndex > this.WallParts.Length - 1)
		{
			this.currentIndex = 0;
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00007DFF File Offset: 0x00005FFF
	public void PreviousWall()
	{
		this.currentIndex--;
		if (this.currentIndex < 0)
		{
			this.currentIndex = this.WallParts.Length - 1;
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00007E28 File Offset: 0x00006028
	public void NextRoof()
	{
		this.currentIndex1++;
		if (this.currentIndex1 > this.RoofParts.Length - 1)
		{
			this.currentIndex1 = 0;
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00007E51 File Offset: 0x00006051
	public void PreviousRoof()
	{
		this.currentIndex1--;
		if (this.currentIndex1 < 0)
		{
			this.currentIndex1 = this.RoofParts.Length - 1;
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00007E7A File Offset: 0x0000607A
	public void NextDoor()
	{
		this.currentIndex2++;
		if (this.currentIndex2 > this.DoorParts.Length - 1)
		{
			this.currentIndex2 = 0;
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00007EA3 File Offset: 0x000060A3
	public void PreviousDoor()
	{
		this.currentIndex2--;
		if (this.currentIndex2 < 0)
		{
			this.currentIndex2 = this.DoorParts.Length - 1;
		}
	}

	// Token: 0x0400012B RID: 299
	public int currentIndex;

	// Token: 0x0400012C RID: 300
	public int currentIndex1;

	// Token: 0x0400012D RID: 301
	public int currentIndex2;

	// Token: 0x0400012E RID: 302
	public GameObject[] WallParts;

	// Token: 0x0400012F RID: 303
	public GameObject[] RoofParts;

	// Token: 0x04000130 RID: 304
	public GameObject[] DoorParts;
}
