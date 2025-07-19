using System;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class transparents1 : MonoBehaviour
{
	// Token: 0x060007CB RID: 1995 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x04000E98 RID: 3736
	public GameObject RealParent;

	// Token: 0x04000E99 RID: 3737
	public Mesh ChildrenMesh;

	// Token: 0x04000E9A RID: 3738
	public Mesh ChildrenMesh1;

	// Token: 0x04000E9B RID: 3739
	public Mesh ChildrenMesh2;

	// Token: 0x04000E9C RID: 3740
	public Mesh ChildrenMesh3;

	// Token: 0x04000E9D RID: 3741
	public int SavePosition;

	// Token: 0x04000E9E RID: 3742
	public int Type;

	// Token: 0x04000E9F RID: 3743
	public bool NotImportantPart;

	// Token: 0x04000EA0 RID: 3744
	public bool invert;

	// Token: 0x04000EA1 RID: 3745
	public int DependsOn;

	// Token: 0x04000EA2 RID: 3746
	public bool CanAttach;

	// Token: 0x04000EA3 RID: 3747
	public transparents.dependantObjects[] DEPENDANTS;

	// Token: 0x04000EA4 RID: 3748
	public bool ATTACHED;

	// Token: 0x04000EA5 RID: 3749
	public bool IsSpring;

	// Token: 0x04000EA6 RID: 3750
	public bool HaveAttached;

	// Token: 0x04000EA7 RID: 3751
	public bool R;

	// Token: 0x04000EA8 RID: 3752
	public bool L;

	// Token: 0x04000EA9 RID: 3753
	public bool FL;

	// Token: 0x04000EAA RID: 3754
	public bool FR;

	// Token: 0x04000EAB RID: 3755
	public bool RL;

	// Token: 0x04000EAC RID: 3756
	public bool RR;

	// Token: 0x04000EAD RID: 3757
	public bool FRONT;

	// Token: 0x04000EAE RID: 3758
	public bool REAR;

	// Token: 0x04000EAF RID: 3759
	public bool BrakePadA;

	// Token: 0x04000EB0 RID: 3760
	public bool BrakePadB;

	// Token: 0x04000EB1 RID: 3761
	public bool RemovesSpringFL;

	// Token: 0x04000EB2 RID: 3762
	public bool RemovesSpringFR;

	// Token: 0x04000EB3 RID: 3763
	public bool RemovesSpringRL;

	// Token: 0x04000EB4 RID: 3764
	public bool RemovesSpringRR;

	// Token: 0x04000EB5 RID: 3765
	public bool PARENTHaveToBeREmoved;

	// Token: 0x04000EB6 RID: 3766
	public bool PartHaveToBeREmoved;

	// Token: 0x04000EB7 RID: 3767
	public bool PartHaveToBeREmovedDependsOnSide;

	// Token: 0x04000EB8 RID: 3768
	public GameObject PartThatNeedsToBeOff;

	// Token: 0x04000EB9 RID: 3769
	public GameObject PartThatNeedsToBeOffL;

	// Token: 0x04000EBA RID: 3770
	public GameObject PartThatNeedsToBeOffR;

	// Token: 0x04000EBB RID: 3771
	public string PartThatNeedsToBeOffname;

	// Token: 0x04000EBC RID: 3772
	public string PartThatNeedsToBeOffnameL;

	// Token: 0x04000EBD RID: 3773
	public string PartThatNeedsToBeOffnameR;

	// Token: 0x04000EBE RID: 3774
	public bool CanAttachBecauseOfPart;

	// Token: 0x04000EBF RID: 3775
	public transparents.AttachingObjects[] ATTACHABLES;

	// Token: 0x02000167 RID: 359
	[Serializable]
	public class dependantObjects
	{
		// Token: 0x04000EC0 RID: 3776
		public GameObject dependant;
	}

	// Token: 0x02000168 RID: 360
	[Serializable]
	public class AttachingObjects
	{
		// Token: 0x04000EC1 RID: 3777
		public GameObject Attachable;
	}
}
