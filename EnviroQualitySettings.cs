using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
[Serializable]
public class EnviroQualitySettings
{
	// Token: 0x040002F5 RID: 757
	[Range(0f, 1f)]
	[Tooltip("Modifies the amount of particles used in weather effects.")]
	public float GlobalParticleEmissionRates = 1f;

	// Token: 0x040002F6 RID: 758
	[Tooltip("How often Enviro Growth Instances should be updated. Lower value = smoother growth and more frequent updates but more perfomance hungry!")]
	public float UpdateInterval = 0.5f;
}
