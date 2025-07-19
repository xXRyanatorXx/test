using System;

// Token: 0x020001E1 RID: 481
public class SimplifiedModObject
{
	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0007C338 File Offset: 0x0007A538
	// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0007C340 File Offset: 0x0007A540
	public string ModId { get; set; }

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0007C349 File Offset: 0x0007A549
	// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0007C351 File Offset: 0x0007A551
	public string ModName { get; set; }

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0007C35A File Offset: 0x0007A55A
	// (set) Token: 0x06000B4F RID: 2895 RVA: 0x0007C362 File Offset: 0x0007A562
	public string ModVersion { get; set; }

	// Token: 0x06000B50 RID: 2896 RVA: 0x0007C36B File Offset: 0x0007A56B
	public SimplifiedModObject(string id, string name, string version)
	{
		this.ModId = id;
		this.ModName = name;
		this.ModVersion = version;
	}
}
