using System;
using System.Collections.Generic;

// Token: 0x020001E2 RID: 482
public class SimplifiedMods
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0007C388 File Offset: 0x0007A588
	// (set) Token: 0x06000B52 RID: 2898 RVA: 0x0007C390 File Offset: 0x0007A590
	public List<SimplifiedModObject> Mods { get; set; }

	// Token: 0x06000B53 RID: 2899 RVA: 0x0007C399 File Offset: 0x0007A599
	public SimplifiedMods()
	{
		this.Mods = new List<SimplifiedModObject>();
	}
}
