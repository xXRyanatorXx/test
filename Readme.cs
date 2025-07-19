using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class Readme : ScriptableObject
{
	// Token: 0x04000203 RID: 515
	public Texture2D icon;

	// Token: 0x04000204 RID: 516
	public string title;

	// Token: 0x04000205 RID: 517
	public Readme.Section[] sections;

	// Token: 0x04000206 RID: 518
	public bool loadedLayout;

	// Token: 0x02000053 RID: 83
	[Serializable]
	public class Section
	{
		// Token: 0x04000207 RID: 519
		public string heading;

		// Token: 0x04000208 RID: 520
		public string text;

		// Token: 0x04000209 RID: 521
		public string linkText;

		// Token: 0x0400020A RID: 522
		public string url;
	}
}
