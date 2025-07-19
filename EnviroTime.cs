using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
[Serializable]
public class EnviroTime
{
	// Token: 0x0400024A RID: 586
	[Tooltip("None = No time auto time progressing, Simulated = Time calculated with DayLenghtInMinutes, SystemTime = uses your systemTime.")]
	public EnviroTime.TimeProgressMode ProgressTime = EnviroTime.TimeProgressMode.Simulated;

	// Token: 0x0400024B RID: 587
	[Tooltip("Current Time: minutes")]
	[Range(0f, 60f)]
	public int Seconds;

	// Token: 0x0400024C RID: 588
	[Tooltip("Current Time: minutes")]
	[Range(0f, 60f)]
	public int Minutes;

	// Token: 0x0400024D RID: 589
	[Tooltip("Current Time: hours")]
	[Range(0f, 24f)]
	public int Hours = 12;

	// Token: 0x0400024E RID: 590
	[Tooltip("Current Time: Days")]
	public int Days = 1;

	// Token: 0x0400024F RID: 591
	[Tooltip("Current Time: Years")]
	public int Years = 1;

	// Token: 0x04000250 RID: 592
	[Tooltip("How many days in one year?")]
	public int DaysInYear = 365;

	// Token: 0x04000251 RID: 593
	[Tooltip("Ful 24h cycle in realtime minutes.")]
	public float cycleLengthInMinutes = 5f;

	// Token: 0x04000252 RID: 594
	[Tooltip("Day lenght modifier.")]
	[Range(0.1f, 10f)]
	public float dayLengthModifier = 1f;

	// Token: 0x04000253 RID: 595
	[Tooltip("Night lenght modifier.")]
	[Range(0.1f, 10f)]
	public float nightLengthModifier = 1f;

	// Token: 0x04000254 RID: 596
	[Range(-13f, 13f)]
	[Tooltip("Time offset for timezones")]
	public int utcOffset;

	// Token: 0x04000255 RID: 597
	[Range(-90f, 90f)]
	[Tooltip("-90,  90   Horizontal earth lines")]
	public float Latitude;

	// Token: 0x04000256 RID: 598
	[Range(-180f, 180f)]
	[Tooltip("-180, 180  Vertical earth line")]
	public float Longitude;

	// Token: 0x04000257 RID: 599
	[HideInInspector]
	public float solarTime;

	// Token: 0x04000258 RID: 600
	[HideInInspector]
	public float lunarTime;

	// Token: 0x04000259 RID: 601
	[Tooltip("This setting will change the timing when system switches from day to night and night to day. It uses the sun position in sky: 0 -> Night, ~0.5 -> Dawn/Dusk, 1 -> Midday.")]
	[Range(0.3f, 0.7f)]
	public float dayNightSwitch = 0.45f;

	// Token: 0x0200006C RID: 108
	public enum TimeProgressMode
	{
		// Token: 0x0400025B RID: 603
		None,
		// Token: 0x0400025C RID: 604
		Simulated,
		// Token: 0x0400025D RID: 605
		OneDay,
		// Token: 0x0400025E RID: 606
		SystemTime
	}
}
