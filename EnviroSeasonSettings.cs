using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
[Serializable]
public class EnviroSeasonSettings
{
	// Token: 0x040002F7 RID: 759
	[Header("Spring")]
	[Tooltip("Start Day of Year for Spring")]
	[Range(0f, 366f)]
	public int SpringStart = 60;

	// Token: 0x040002F8 RID: 760
	[Tooltip("End Day of Year for Spring")]
	[Range(0f, 366f)]
	public int SpringEnd = 92;

	// Token: 0x040002F9 RID: 761
	[Tooltip("Base Temperature in Spring")]
	public AnimationCurve springBaseTemperature = new AnimationCurve();

	// Token: 0x040002FA RID: 762
	[Header("Summer")]
	[Tooltip("Start Day of Year for Summer")]
	[Range(0f, 366f)]
	public int SummerStart = 93;

	// Token: 0x040002FB RID: 763
	[Tooltip("End Day of Year for Summer")]
	[Range(0f, 366f)]
	public int SummerEnd = 185;

	// Token: 0x040002FC RID: 764
	[Tooltip("Base Temperature in Summer")]
	public AnimationCurve summerBaseTemperature = new AnimationCurve();

	// Token: 0x040002FD RID: 765
	[Header("Autumn")]
	[Tooltip("Start Day of Year for Autumn")]
	[Range(0f, 366f)]
	public int AutumnStart = 186;

	// Token: 0x040002FE RID: 766
	[Tooltip("End Day of Year for Autumn")]
	[Range(0f, 366f)]
	public int AutumnEnd = 276;

	// Token: 0x040002FF RID: 767
	[Tooltip("Base Temperature in Autumn")]
	public AnimationCurve autumnBaseTemperature = new AnimationCurve();

	// Token: 0x04000300 RID: 768
	[Header("Winter")]
	[Tooltip("Start Day of Year for Winter")]
	[Range(0f, 366f)]
	public int WinterStart = 277;

	// Token: 0x04000301 RID: 769
	[Tooltip("End Day of Year for Winter")]
	[Range(0f, 366f)]
	public int WinterEnd = 59;

	// Token: 0x04000302 RID: 770
	[Tooltip("Base Temperature in Winter")]
	public AnimationCurve winterBaseTemperature = new AnimationCurve();
}
