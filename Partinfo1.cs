using System;
using UnityEngine;

// Token: 0x02000164 RID: 356
public class Partinfo1 : MonoBehaviour
{
	// Token: 0x060007C5 RID: 1989 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x04000E63 RID: 3683
	public bool SmallObject;

	// Token: 0x04000E64 RID: 3684
	public bool DontShowInCatalog;

	// Token: 0x04000E65 RID: 3685
	public bool DontSpawnInJunyard;

	// Token: 0x04000E66 RID: 3686
	public bool CanPutInBox;

	// Token: 0x04000E67 RID: 3687
	public bool InBackpack;

	// Token: 0x04000E68 RID: 3688
	public Texture2D Thumbnail;

	// Token: 0x04000E69 RID: 3689
	public GameObject player;

	// Token: 0x04000E6A RID: 3690
	public float attachedparts;

	// Token: 0x04000E6B RID: 3691
	public float attachedbolts;

	// Token: 0x04000E6C RID: 3692
	public float tightnuts;

	// Token: 0x04000E6D RID: 3693
	public float attachedwelds;

	// Token: 0x04000E6E RID: 3694
	public float fixedwelds;

	// Token: 0x04000E6F RID: 3695
	public float ImportantBolts;

	// Token: 0x04000E70 RID: 3696
	public float fixedImportantBolts;

	// Token: 0x04000E71 RID: 3697
	public float ChildrenFixedBolts;

	// Token: 0x04000E72 RID: 3698
	public bool DontRotateWhenAttaching;

	// Token: 0x04000E73 RID: 3699
	public bool Openable;

	// Token: 0x04000E74 RID: 3700
	public GameObject HingePivot;

	// Token: 0x04000E75 RID: 3701
	public bool Rdoor;

	// Token: 0x04000E76 RID: 3702
	public bool Ldoor;

	// Token: 0x04000E77 RID: 3703
	public bool Trunk;

	// Token: 0x04000E78 RID: 3704
	public bool Hood;

	// Token: 0x04000E79 RID: 3705
	public bool HoodHalf;

	// Token: 0x04000E7A RID: 3706
	public bool EnabledTouchable;

	// Token: 0x04000E7B RID: 3707
	public GameObject AudioParent;

	// Token: 0x04000E7C RID: 3708
	public string RenamedPrefab;

	// Token: 0x04000E7D RID: 3709
	public float weight = 1f;

	// Token: 0x04000E7E RID: 3710
	public float price;

	// Token: 0x04000E7F RID: 3711
	public bool Suspension;

	// Token: 0x04000E80 RID: 3712
	public bool Brakes;

	// Token: 0x04000E81 RID: 3713
	public bool Engine;

	// Token: 0x04000E82 RID: 3714
	public bool BodyPanel;

	// Token: 0x04000E83 RID: 3715
	public bool Interior;

	// Token: 0x04000E84 RID: 3716
	public bool Accessories;

	// Token: 0x04000E85 RID: 3717
	public bool Light;

	// Token: 0x04000E86 RID: 3718
	public bool Window;

	// Token: 0x04000E87 RID: 3719
	public bool Rim;

	// Token: 0x04000E88 RID: 3720
	public bool Tire;

	// Token: 0x04000E89 RID: 3721
	public string[] FitsToCar;

	// Token: 0x04000E8A RID: 3722
	public string[] FitsToEngine;

	// Token: 0x04000E8B RID: 3723
	public bool AllowFall;
}
