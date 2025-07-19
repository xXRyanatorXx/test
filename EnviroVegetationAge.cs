using System;

// Token: 0x020000A5 RID: 165
[Serializable]
public class EnviroVegetationAge
{
	// Token: 0x0400047F RID: 1151
	public float maxAgeHours = 24f;

	// Token: 0x04000480 RID: 1152
	public float maxAgeDays = 60f;

	// Token: 0x04000481 RID: 1153
	public float maxAgeYears;

	// Token: 0x04000482 RID: 1154
	public bool randomStartAge;

	// Token: 0x04000483 RID: 1155
	public float startAgeinHours;

	// Token: 0x04000484 RID: 1156
	public double birthdayInHours;

	// Token: 0x04000485 RID: 1157
	public bool Loop = true;

	// Token: 0x04000486 RID: 1158
	public int LoopFromGrowStage;
}
