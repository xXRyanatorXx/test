using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
[Serializable]
public class EnviroSeasons
{
	// Token: 0x04000226 RID: 550
	[Tooltip("When enabled the system will change seasons automaticly when enough days passed.")]
	public bool calcSeasons;

	// Token: 0x04000227 RID: 551
	[Tooltip("The current season.")]
	public EnviroSeasons.Seasons currentSeasons;

	// Token: 0x04000228 RID: 552
	[HideInInspector]
	public EnviroSeasons.Seasons lastSeason;

	// Token: 0x02000066 RID: 102
	public enum Seasons
	{
		// Token: 0x0400022A RID: 554
		Spring,
		// Token: 0x0400022B RID: 555
		Summer,
		// Token: 0x0400022C RID: 556
		Autumn,
		// Token: 0x0400022D RID: 557
		Winter
	}
}
