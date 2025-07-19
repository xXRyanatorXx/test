using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class EngineAudioAI : MonoBehaviour
{
	// Token: 0x06000066 RID: 102 RVA: 0x00006994 File Offset: 0x00004B94
	private void Start()
	{
		this.ACAI = base.transform.GetComponent<AnyCarAI>();
		this.engineVolume = this.ACAI.engineVolume;
		this.lowAccelClip = this.ACAI.lowAcceleration;
		this.lowDecelClip = this.ACAI.lowDeceleration;
		this.highAccelClip = this.ACAI.highAcceleration;
		this.highDecelClip = this.ACAI.highDeceleration;
		this.highAccelSource = this.SetUpEngineAudioSource(this.highAccelClip);
		this.lowAccelSource = this.SetUpEngineAudioSource(this.lowAccelClip);
		this.lowDecelSource = this.SetUpEngineAudioSource(this.lowDecelClip);
		this.highDecelSource = this.SetUpEngineAudioSource(this.highDecelClip);
		if (this.ACAI.turboON)
		{
			this.turboVolume = this.ACAI.turboVolume;
			this.turboAudioClip = this.ACAI.turboAudioClip;
			this.turboAudioSource = this.SetUpEngineAudioSource(this.turboAudioClip);
			this.turboAudioSource.volume = this.turboVolume;
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00006AA1 File Offset: 0x00004CA1
	private void Update()
	{
		this.PlayEngineSound();
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00006AAC File Offset: 0x00004CAC
	private void PlayEngineSound()
	{
		float num = EngineAudioAI.SoundLerp(this.lowPitchMin, this.lowPitchMax, this.ACAI.RPM);
		num = Mathf.Min(this.lowPitchMax, num);
		this.lowAccelSource.pitch = num * this.pitchMultiplier;
		this.lowDecelSource.pitch = num * this.pitchMultiplier;
		this.highAccelSource.pitch = num * this.highPitchMultiplier * this.pitchMultiplier;
		this.highDecelSource.pitch = num * this.highPitchMultiplier * this.pitchMultiplier;
		float num2 = Mathf.Abs(this.ACAI.AccelInput);
		float num3 = 1f - num2;
		float num4 = Mathf.InverseLerp(0.2f, 0.8f, this.ACAI.RPM);
		float num5 = 1f - num4;
		num4 = 1f - (1f - num4) * (1f - num4);
		num5 = 1f - (1f - num5) * (1f - num5);
		num2 = 1f - (1f - num2) * (1f - num2);
		num3 = 1f - (1f - num3) * (1f - num3);
		this.lowAccelSource.volume = num5 * num2 * this.engineVolume;
		this.lowDecelSource.volume = num5 * num3 * this.engineVolume;
		this.highAccelSource.volume = num4 * num2 * this.engineVolume;
		this.highDecelSource.volume = num4 * num3 * this.engineVolume;
		if (this.ACAI.turboON)
		{
			this.turboAudioSource.pitch = num * this.highPitchMultiplier * this.pitchMultiplier;
			this.turboAudioSource.volume = num4 * num2 * this.turboVolume + 0.1f;
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00006C70 File Offset: 0x00004E70
	private AudioSource SetUpEngineAudioSource(AudioClip clip)
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = 0f;
		audioSource.loop = true;
		audioSource.time = UnityEngine.Random.Range(0f, clip.length);
		audioSource.Play();
		audioSource.minDistance = 5f;
		audioSource.dopplerLevel = 0f;
		return audioSource;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000045F0 File Offset: 0x000027F0
	private static float SoundLerp(float from, float to, float value)
	{
		return (1f - value) * from + value * to;
	}

	// Token: 0x040000D6 RID: 214
	private AudioClip lowAccelClip;

	// Token: 0x040000D7 RID: 215
	private AudioClip lowDecelClip;

	// Token: 0x040000D8 RID: 216
	private AudioClip highAccelClip;

	// Token: 0x040000D9 RID: 217
	private AudioClip highDecelClip;

	// Token: 0x040000DA RID: 218
	private AudioClip nosAudioClip;

	// Token: 0x040000DB RID: 219
	private AudioClip turboAudioClip;

	// Token: 0x040000DC RID: 220
	private AudioSource lowAccelSource;

	// Token: 0x040000DD RID: 221
	private AudioSource lowDecelSource;

	// Token: 0x040000DE RID: 222
	private AudioSource highAccelSource;

	// Token: 0x040000DF RID: 223
	private AudioSource highDecelSource;

	// Token: 0x040000E0 RID: 224
	private AudioSource nosAudioSource;

	// Token: 0x040000E1 RID: 225
	private AudioSource turboAudioSource;

	// Token: 0x040000E2 RID: 226
	private AnyCarAI ACAI;

	// Token: 0x040000E3 RID: 227
	private float engineVolume;

	// Token: 0x040000E4 RID: 228
	private float nosVolume;

	// Token: 0x040000E5 RID: 229
	private float turboVolume;

	// Token: 0x040000E6 RID: 230
	private float lowPitchMin = 1f;

	// Token: 0x040000E7 RID: 231
	private float lowPitchMax = 6f;

	// Token: 0x040000E8 RID: 232
	private float pitchMultiplier = 1f;

	// Token: 0x040000E9 RID: 233
	private float highPitchMultiplier = 0.25f;
}
