using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AA RID: 170
[Serializable]
public class EnviroWeatherPrefab : MonoBehaviour
{
	// Token: 0x040004A8 RID: 1192
	public EnviroWeatherPreset weatherPreset;

	// Token: 0x040004A9 RID: 1193
	[HideInInspector]
	public List<ParticleSystem> effectSystems = new List<ParticleSystem>();

	// Token: 0x040004AA RID: 1194
	[HideInInspector]
	public List<float> effectEmmisionRates = new List<float>();
}
