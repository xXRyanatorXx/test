using System;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class BoltNut1 : MonoBehaviour
{
	// Token: 0x060007B6 RID: 1974 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x04000CC6 RID: 3270
	public GameObject otherobject;

	// Token: 0x04000CC7 RID: 3271
	public GameObject otherobjectL;

	// Token: 0x04000CC8 RID: 3272
	public GameObject otherobjectR;

	// Token: 0x04000CC9 RID: 3273
	public string otherobjectName;

	// Token: 0x04000CCA RID: 3274
	public string otherobjectNameL;

	// Token: 0x04000CCB RID: 3275
	public string otherobjectNameR;

	// Token: 0x04000CCC RID: 3276
	public bool Started;

	// Token: 0x04000CCD RID: 3277
	public bool DontDisableRenderer;

	// Token: 0x04000CCE RID: 3278
	public bool CheckedForOther;

	// Token: 0x04000CCF RID: 3279
	public bool tight;

	// Token: 0x04000CD0 RID: 3280
	public bool canfix;

	// Token: 0x04000CD1 RID: 3281
	public bool ChildrenHaveToBeRemoved;

	// Token: 0x04000CD2 RID: 3282
	public bool NothingFound;

	// Token: 0x04000CD3 RID: 3283
	public bool NotImportant;

	// Token: 0x04000CD4 RID: 3284
	public bool AffectsGrandParent1;

	// Token: 0x04000CD5 RID: 3285
	public bool AffectsGrandParent2;

	// Token: 0x04000CD6 RID: 3286
	public bool AffectsGrandParent3;

	// Token: 0x04000CD7 RID: 3287
	public bool MatchTypeToBolt;

	// Token: 0x04000CD8 RID: 3288
	public bool DisallowDistantBreaking;

	// Token: 0x04000CD9 RID: 3289
	public GameObject AudioParent;

	// Token: 0x04000CDA RID: 3290
	public FixedJoint joint;
}
