using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
[Serializable]
public class EnviroWeatherSettings
{
	// Token: 0x04000303 RID: 771
	[Header("Zones Setup:")]
	[Tooltip("Tag for zone triggers. Create and assign a tag to this gameObject")]
	public bool useTag;

	// Token: 0x04000304 RID: 772
	[Header("Weather Transition Settings:")]
	[Tooltip("Defines the speed of wetness will raise when it is raining.")]
	public float wetnessAccumulationSpeed = 0.05f;

	// Token: 0x04000305 RID: 773
	[Tooltip("Defines the speed of wetness will dry when it is not raining.")]
	public float wetnessDryingSpeed = 0.05f;

	// Token: 0x04000306 RID: 774
	[Tooltip("Defines the speed of snow will raise when it is snowing.")]
	public float snowAccumulationSpeed = 0.05f;

	// Token: 0x04000307 RID: 775
	[Tooltip("Defines the speed of snow will meld when it is not snowing.")]
	public float snowMeltingSpeed = 0.05f;

	// Token: 0x04000308 RID: 776
	[Tooltip("Defines the temperature when snow starts to melt.")]
	public float snowMeltingTresholdTemperature = 1f;

	// Token: 0x04000309 RID: 777
	[Tooltip("Defines the speed of clouds will change when weather conditions changed.")]
	public float cloudTransitionSpeed = 1f;

	// Token: 0x0400030A RID: 778
	[Tooltip("Defines the speed of fog will change when weather conditions changed.")]
	public float fogTransitionSpeed = 1f;

	// Token: 0x0400030B RID: 779
	[Tooltip("Defines the speed of wind intensity will change when weather conditions changed.")]
	public float windIntensityTransitionSpeed = 1f;

	// Token: 0x0400030C RID: 780
	[Tooltip("Defines the speed of particle effects will change when weather conditions changed.")]
	public float effectTransitionSpeed = 1f;

	// Token: 0x0400030D RID: 781
	[Tooltip("Defines the speed of sfx will fade in and out when weather conditions changed.")]
	public float audioTransitionSpeed = 0.1f;

	// Token: 0x0400030E RID: 782
	[Header("Lightning Effect:")]
	public GameObject lightningEffect;

	// Token: 0x0400030F RID: 783
	[Range(500f, 10000f)]
	public float lightningRange = 500f;

	// Token: 0x04000310 RID: 784
	[Range(500f, 5000f)]
	public float lightningHeight = 750f;

	// Token: 0x04000311 RID: 785
	[Header("Temperature:")]
	[Tooltip("Defines the speed of temperature changes.")]
	public float temperatureChangingSpeed = 10f;
}
