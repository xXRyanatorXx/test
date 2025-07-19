using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
[Serializable]
public class EnviroAudio
{
	// Token: 0x04000230 RID: 560
	[Tooltip("The prefab with AudioSources used by Enviro. Will be instantiated at runtime.")]
	public GameObject SFXHolderPrefab;

	// Token: 0x04000231 RID: 561
	[Range(0f, 1f)]
	[Tooltip("The volume of ambient sounds played by enviro.")]
	public float ambientSFXVolume = 0.5f;

	// Token: 0x04000232 RID: 562
	[Range(0f, 1f)]
	[Tooltip("The volume of weather sounds played by enviro.")]
	public float weatherSFXVolume = 1f;

	// Token: 0x04000233 RID: 563
	[HideInInspector]
	public EnviroAudioSource currentAmbientSource;

	// Token: 0x04000234 RID: 564
	[HideInInspector]
	public float ambientSFXVolumeMod;

	// Token: 0x04000235 RID: 565
	[HideInInspector]
	public float weatherSFXVolumeMod;

	// Token: 0x04000236 RID: 566
	[HideInInspector]
	public EnviroAudioSource AudioSourceWeather;

	// Token: 0x04000237 RID: 567
	[HideInInspector]
	public EnviroAudioSource AudioSourceWeather2;

	// Token: 0x04000238 RID: 568
	[HideInInspector]
	public EnviroAudioSource AudioSourceAmbient;

	// Token: 0x04000239 RID: 569
	[HideInInspector]
	public EnviroAudioSource AudioSourceAmbient2;

	// Token: 0x0400023A RID: 570
	[HideInInspector]
	public EnviroAudioSource AudioSourceThunder;

	// Token: 0x0400023B RID: 571
	[HideInInspector]
	public EnviroAudioSource AudioSourceZone;
}
