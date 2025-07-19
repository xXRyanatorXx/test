using System;
using System.Collections.Generic;

// Token: 0x020001E3 RID: 483
internal class SimplifiedModObjectComparer : IEqualityComparer<SimplifiedModObject>
{
	// Token: 0x06000B54 RID: 2900 RVA: 0x0007C3AC File Offset: 0x0007A5AC
	public bool Equals(SimplifiedModObject x, SimplifiedModObject y)
	{
		return x.ModId == y.ModId;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x0007C3BF File Offset: 0x0007A5BF
	public int GetHashCode(SimplifiedModObject obj)
	{
		return obj.ModId.GetHashCode();
	}
}
