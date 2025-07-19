using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
[CreateAssetMenu]
public class RampAsset : ScriptableObject
{
	// Token: 0x040001AD RID: 429
	public Gradient gradient = new Gradient();

	// Token: 0x040001AE RID: 430
	public int size = 16;

	// Token: 0x040001AF RID: 431
	public bool up;

	// Token: 0x040001B0 RID: 432
	public bool overwriteExisting = true;
}
