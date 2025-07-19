using System;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class WeldCut1 : MonoBehaviour
{
	// Token: 0x060007C8 RID: 1992 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x04000E8C RID: 3724
	public GameObject otherobject;

	// Token: 0x04000E8D RID: 3725
	public string otherobjectName;

	// Token: 0x04000E8E RID: 3726
	public bool CheckedForOther;

	// Token: 0x04000E8F RID: 3727
	public bool welded;

	// Token: 0x04000E90 RID: 3728
	public bool canweld;

	// Token: 0x04000E91 RID: 3729
	public FixedJoint joint;

	// Token: 0x04000E92 RID: 3730
	public GameObject AudioParent;

	// Token: 0x04000E93 RID: 3731
	public bool NotImportant;

	// Token: 0x04000E94 RID: 3732
	public GameObject WeldSparks;

	// Token: 0x04000E95 RID: 3733
	public GameObject Sparks;

	// Token: 0x04000E96 RID: 3734
	public GameObject SparksPefab;

	// Token: 0x04000E97 RID: 3735
	public GameObject WeldPefab;
}
