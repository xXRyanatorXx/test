using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
[Serializable]
public class ParticleExamples
{
	// Token: 0x040001D9 RID: 473
	public string title;

	// Token: 0x040001DA RID: 474
	[TextArea]
	public string description;

	// Token: 0x040001DB RID: 475
	public bool isWeaponEffect;

	// Token: 0x040001DC RID: 476
	public GameObject particleSystemGO;

	// Token: 0x040001DD RID: 477
	public Vector3 particlePosition;

	// Token: 0x040001DE RID: 478
	public Vector3 particleRotation;
}
