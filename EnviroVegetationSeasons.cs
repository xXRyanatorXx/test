using System;

// Token: 0x020000A6 RID: 166
[Serializable]
public class EnviroVegetationSeasons
{
	// Token: 0x04000487 RID: 1159
	public EnviroVegetationSeasons.SeasonAction seasonAction;

	// Token: 0x04000488 RID: 1160
	public bool GrowInSpring = true;

	// Token: 0x04000489 RID: 1161
	public bool GrowInSummer = true;

	// Token: 0x0400048A RID: 1162
	public bool GrowInAutumn = true;

	// Token: 0x0400048B RID: 1163
	public bool GrowInWinter = true;

	// Token: 0x020000A7 RID: 167
	public enum SeasonAction
	{
		// Token: 0x0400048D RID: 1165
		SpawnDeadPrefab,
		// Token: 0x0400048E RID: 1166
		Deactivate,
		// Token: 0x0400048F RID: 1167
		Destroy
	}
}
