using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000262 RID: 610
public class SuperCharger_mobile : MonoBehaviour
{
	// Token: 0x06000E7C RID: 3708 RVA: 0x0009A304 File Offset: 0x00098504
	private void Start()
	{
		this.res = base.gameObject.transform.parent.GetComponent<RealisticEngineSound_mobile>();
		if (this.audioMixer != null)
		{
			this._audioMixer = this.audioMixer;
			return;
		}
		if (this.res.audioMixer != null)
		{
			this._audioMixer = this.res.audioMixer;
			this.audioMixer = this._audioMixer;
		}
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0009A378 File Offset: 0x00098578
	private void Update()
	{
		if (this.res.enabled)
		{
			if (this.res.isCameraNear)
			{
				this.clipsValue = this.res.engineCurrentRPM / this.res.maxRPMLimit;
				if (this.res.gasPedalPressing)
				{
					if (this.chargerOnLoop == null)
					{
						this.CreateChargerOn();
					}
					else if (!this.chargerOnLoop.isPlaying)
					{
						this.chargerOnLoop.Play();
					}
					this.chargerOnLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
					this.chargerOnLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
					if (this.chargerOffLoop != null)
					{
						if (this.destroyAudioSources)
						{
							UnityEngine.Object.Destroy(this.chargerOffLoop);
							return;
						}
						this.chargerOffLoop.Stop();
						return;
					}
				}
				else
				{
					if (this.chargerOffLoop == null)
					{
						this.CreateChargerOff();
					}
					else if (!this.chargerOffLoop.isPlaying)
					{
						this.chargerOffLoop.Play();
					}
					this.chargerOffLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
					this.chargerOffLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
					if (this.chargerOnLoop != null)
					{
						if (this.destroyAudioSources)
						{
							UnityEngine.Object.Destroy(this.chargerOnLoop);
							return;
						}
						this.chargerOnLoop.Stop();
						return;
					}
				}
			}
			else
			{
				if (this.destroyAudioSources)
				{
					this.DestroyAll();
					return;
				}
				this.StopAll();
				return;
			}
		}
		else
		{
			this.DestroyAll();
		}
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0009A527 File Offset: 0x00098727
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x0009A52F File Offset: 0x0009872F
	private void OnDisable()
	{
		this.DestroyAll();
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0009A537 File Offset: 0x00098737
	private void DestroyAll()
	{
		if (this.chargerOnLoop != null)
		{
			UnityEngine.Object.Destroy(this.chargerOnLoop);
		}
		if (this.chargerOffLoop != null)
		{
			UnityEngine.Object.Destroy(this.chargerOffLoop);
		}
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x0009A56B File Offset: 0x0009876B
	private void StopAll()
	{
		if (this.chargerOnLoop != null)
		{
			this.chargerOnLoop.Stop();
		}
		if (this.chargerOffLoop != null)
		{
			this.chargerOffLoop.Stop();
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0009A5A0 File Offset: 0x000987A0
	private void CreateChargerOn()
	{
		if (this.chargerOnLoopClip != null)
		{
			this.chargerOnLoop = base.gameObject.AddComponent<AudioSource>();
			this.chargerOnLoop.rolloffMode = this.res.audioRolloffMode;
			this.chargerOnLoop.dopplerLevel = this.res.dopplerLevel;
			this.chargerOnLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.chargerOnLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
			this.chargerOnLoop.minDistance = this.res.minDistance;
			this.chargerOnLoop.maxDistance = this.res.maxDistance;
			this.chargerOnLoop.spatialBlend = this.res.spatialBlend;
			this.chargerOnLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.chargerOnLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.chargerOnLoop.clip = this.chargerOnLoopClip;
			this.chargerOnLoop.Play();
		}
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x0009A6C4 File Offset: 0x000988C4
	private void CreateChargerOff()
	{
		if (this.chargerOffLoopClip != null)
		{
			this.chargerOffLoop = base.gameObject.AddComponent<AudioSource>();
			this.chargerOffLoop.rolloffMode = this.res.audioRolloffMode;
			this.chargerOffLoop.dopplerLevel = this.res.dopplerLevel;
			this.chargerOffLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.chargerOffLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
			this.chargerOffLoop.minDistance = this.res.minDistance;
			this.chargerOffLoop.maxDistance = this.res.maxDistance;
			this.chargerOffLoop.spatialBlend = this.res.spatialBlend;
			this.chargerOffLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.chargerOffLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.chargerOffLoop.clip = this.chargerOffLoopClip;
			this.chargerOffLoop.Play();
		}
	}

	// Token: 0x04001792 RID: 6034
	private RealisticEngineSound_mobile res;

	// Token: 0x04001793 RID: 6035
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x04001794 RID: 6036
	public AudioMixerGroup audioMixer;

	// Token: 0x04001795 RID: 6037
	private AudioMixerGroup _audioMixer;

	// Token: 0x04001796 RID: 6038
	public AudioClip chargerOnLoopClip;

	// Token: 0x04001797 RID: 6039
	public AudioClip chargerOffLoopClip;

	// Token: 0x04001798 RID: 6040
	public AnimationCurve chargerVolCurve;

	// Token: 0x04001799 RID: 6041
	public AnimationCurve chargerPitchCurve;

	// Token: 0x0400179A RID: 6042
	public bool destroyAudioSources;

	// Token: 0x0400179B RID: 6043
	private AudioSource chargerOnLoop;

	// Token: 0x0400179C RID: 6044
	private AudioSource chargerOffLoop;

	// Token: 0x0400179D RID: 6045
	private float clipsValue;
}
