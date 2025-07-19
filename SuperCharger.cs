using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000261 RID: 609
public class SuperCharger : MonoBehaviour
{
	// Token: 0x06000E73 RID: 3699 RVA: 0x00099DEC File Offset: 0x00097FEC
	private void Start()
	{
		this.res = base.gameObject.transform.parent.GetComponent<RealisticEngineSound>();
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

	// Token: 0x06000E74 RID: 3700 RVA: 0x00099E60 File Offset: 0x00098060
	private void Update()
	{
		if (this.res.enabled)
		{
			this.clipsValue = this.res.engineCurrentRPM / this.res.maxRPMLimit;
			if (this.res.isCameraNear)
			{
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
					this.chargerOnLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.res.gasPedalValue;
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
					this.chargerOffLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume * (1f - this.res.gasPedalValue);
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

	// Token: 0x06000E75 RID: 3701 RVA: 0x0009A02D File Offset: 0x0009822D
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x0009A035 File Offset: 0x00098235
	private void OnDisable()
	{
		this.DestroyAll();
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x0009A03D File Offset: 0x0009823D
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

	// Token: 0x06000E78 RID: 3704 RVA: 0x0009A071 File Offset: 0x00098271
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

	// Token: 0x06000E79 RID: 3705 RVA: 0x0009A0A8 File Offset: 0x000982A8
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

	// Token: 0x06000E7A RID: 3706 RVA: 0x0009A1CC File Offset: 0x000983CC
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

	// Token: 0x04001786 RID: 6022
	private RealisticEngineSound res;

	// Token: 0x04001787 RID: 6023
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x04001788 RID: 6024
	public AudioMixerGroup audioMixer;

	// Token: 0x04001789 RID: 6025
	private AudioMixerGroup _audioMixer;

	// Token: 0x0400178A RID: 6026
	public AudioClip chargerOnLoopClip;

	// Token: 0x0400178B RID: 6027
	public AudioClip chargerOffLoopClip;

	// Token: 0x0400178C RID: 6028
	public AnimationCurve chargerVolCurve;

	// Token: 0x0400178D RID: 6029
	public AnimationCurve chargerPitchCurve;

	// Token: 0x0400178E RID: 6030
	public bool destroyAudioSources;

	// Token: 0x0400178F RID: 6031
	private AudioSource chargerOnLoop;

	// Token: 0x04001790 RID: 6032
	private AudioSource chargerOffLoop;

	// Token: 0x04001791 RID: 6033
	private float clipsValue;
}
