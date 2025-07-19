using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000263 RID: 611
public class TurboCharger : MonoBehaviour
{
	// Token: 0x06000E85 RID: 3717 RVA: 0x0009A7FC File Offset: 0x000989FC
	private void Start()
	{
		this.res = base.gameObject.transform.parent.GetComponent<RealisticEngineSound>();
		if (this.audioMixer != null)
		{
			this._audioMixer = this.audioMixer;
		}
		else if (this.res.audioMixer != null)
		{
			this._audioMixer = this.res.audioMixer;
			this.audioMixer = this._audioMixer;
		}
		this._playtime = new WaitForSeconds(0.15f);
		this.CreateOneShot();
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x0009A888 File Offset: 0x00098A88
	private void Update()
	{
		if (this.res.enabled)
		{
			this.clipsValue = this.res.engineCurrentRPM / this.res.maxRPMLimit;
			if (this.res.isCameraNear)
			{
				if (this.res.gasPedalPressing)
				{
					this.oneShotController = 1;
					if (this.res.maxRPMVolCurve.Evaluate(this.clipsValue) < 0.5f)
					{
						if (this.turboLoop == null)
						{
							this.CreateTurboLoop();
						}
						else if (!this.turboLoop.isPlaying)
						{
							this.turboLoop.Play();
						}
						this.turboLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.res.gasPedalValue;
						this.turboLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
						if (this.destroyAudioSources)
						{
							if (this.maxTurboLoop != null)
							{
								UnityEngine.Object.Destroy(this.maxTurboLoop);
								return;
							}
						}
						else if (this.maxTurboLoop != null)
						{
							this.maxTurboLoop.Stop();
							return;
						}
					}
					else if (this.res.useRPMLimit)
					{
						if (this.maxTurboLoop == null)
						{
							this.CreateMaxTurboLoop();
						}
						else
						{
							if (!this.maxTurboLoop.isPlaying)
							{
								this.maxTurboLoop.Play();
							}
							this.maxTurboLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
							this.maxTurboLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
						}
						if (this.destroyAudioSources)
						{
							if (this.turboLoop != null)
							{
								UnityEngine.Object.Destroy(this.turboLoop);
								return;
							}
						}
						else if (this.turboLoop != null)
						{
							this.turboLoop.Stop();
							return;
						}
					}
				}
				else
				{
					if (this.destroyAudioSources)
					{
						if (this.turboLoop != null)
						{
							UnityEngine.Object.Destroy(this.turboLoop);
						}
						if (this.maxTurboLoop != null)
						{
							UnityEngine.Object.Destroy(this.maxTurboLoop);
						}
					}
					else
					{
						if (this.turboLoop != null)
						{
							this.turboLoop.Stop();
						}
						if (this.maxTurboLoop != null)
						{
							this.maxTurboLoop.Stop();
						}
					}
					if (this.oneShotController == 1)
					{
						this.PlayOneShot();
						this.oneShotController = 0;
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

	// Token: 0x06000E87 RID: 3719 RVA: 0x0009AB24 File Offset: 0x00098D24
	private void OnDisable()
	{
		this.DestroyAll();
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0009AB2C File Offset: 0x00098D2C
	private void OnEnable()
	{
		base.StartCoroutine(this.WaitForStart());
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0009AB3C File Offset: 0x00098D3C
	private void DestroyAll()
	{
		if (this.turboLoop != null)
		{
			UnityEngine.Object.Destroy(this.turboLoop);
		}
		if (this.oneShot != null)
		{
			UnityEngine.Object.Destroy(this.oneShot);
		}
		if (this.maxTurboLoop != null)
		{
			UnityEngine.Object.Destroy(this.maxTurboLoop);
		}
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0009AB94 File Offset: 0x00098D94
	private void StopAll()
	{
		if (this.turboLoop != null)
		{
			this.turboLoop.Stop();
		}
		if (this.oneShot != null)
		{
			this.oneShot.Stop();
		}
		if (this.maxTurboLoop != null)
		{
			this.maxTurboLoop.Stop();
		}
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0009ABEC File Offset: 0x00098DEC
	private IEnumerator WaitForStart()
	{
		yield return this._playtime;
		if (this.oneShot == null)
		{
			this.CreateOneShot();
		}
		yield break;
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0009ABFC File Offset: 0x00098DFC
	private void CreateTurboLoop()
	{
		if (this.turboLoopClip != null)
		{
			this.turboLoop = base.gameObject.AddComponent<AudioSource>();
			this.turboLoop.rolloffMode = this.res.audioRolloffMode;
			this.turboLoop.dopplerLevel = this.res.dopplerLevel;
			this.turboLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.turboLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
			this.turboLoop.minDistance = this.res.minDistance;
			this.turboLoop.maxDistance = this.res.maxDistance;
			this.turboLoop.spatialBlend = this.res.spatialBlend;
			this.turboLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.turboLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.turboLoop.clip = this.turboLoopClip;
			this.turboLoop.Play();
		}
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0009AD20 File Offset: 0x00098F20
	private void PlayOneShot()
	{
		if (this.oneShot != null)
		{
			this.oneShot.volume = this.oneShotVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.oneShot.pitch = this.oneShotPitchCurve.Evaluate(this.clipsValue) * UnityEngine.Random.Range(0.85f, 1.2f);
			if (this.clipsValue > this.longShotTreshold)
			{
				this.oneShot.clip = this.longShotClips[UnityEngine.Random.Range(0, this.longShotClips.Length)];
			}
			else
			{
				this.oneShot.clip = this.shortShotClips[UnityEngine.Random.Range(0, this.shortShotClips.Length)];
			}
			this.oneShot.Play();
			return;
		}
		this.CreateOneShot();
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0009ADF0 File Offset: 0x00098FF0
	private void CreateOneShot()
	{
		this.oneShot = base.gameObject.AddComponent<AudioSource>();
		this.oneShot.rolloffMode = this.res.audioRolloffMode;
		this.oneShot.dopplerLevel = this.res.dopplerLevel;
		this.oneShot.spatialBlend = this.res.spatialBlend;
		this.oneShot.minDistance = this.res.minDistance;
		this.oneShot.maxDistance = this.res.maxDistance;
		this.oneShot.loop = false;
		if (this._audioMixer != null)
		{
			this.oneShot.outputAudioMixerGroup = this._audioMixer;
		}
		this.oneShot.Stop();
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0009AEB4 File Offset: 0x000990B4
	private void CreateMaxTurboLoop()
	{
		if (this.maxRpmLoopClip != null)
		{
			this.maxTurboLoop = base.gameObject.AddComponent<AudioSource>();
			this.maxTurboLoop.rolloffMode = this.res.audioRolloffMode;
			this.maxTurboLoop.dopplerLevel = this.res.dopplerLevel;
			this.maxTurboLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.maxTurboLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
			this.maxTurboLoop.minDistance = this.res.minDistance;
			this.maxTurboLoop.maxDistance = this.res.maxDistance;
			this.maxTurboLoop.spatialBlend = this.res.spatialBlend;
			this.maxTurboLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.maxTurboLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.maxTurboLoop.clip = this.maxRpmLoopClip;
			this.maxTurboLoop.Play();
		}
	}

	// Token: 0x0400179E RID: 6046
	private RealisticEngineSound res;

	// Token: 0x0400179F RID: 6047
	private float clipsValue;

	// Token: 0x040017A0 RID: 6048
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x040017A1 RID: 6049
	public AudioMixerGroup audioMixer;

	// Token: 0x040017A2 RID: 6050
	private AudioMixerGroup _audioMixer;

	// Token: 0x040017A3 RID: 6051
	public AudioClip turboLoopClip;

	// Token: 0x040017A4 RID: 6052
	public AudioClip maxRpmLoopClip;

	// Token: 0x040017A5 RID: 6053
	public AnimationCurve chargerVolCurve;

	// Token: 0x040017A6 RID: 6054
	public AnimationCurve chargerPitchCurve;

	// Token: 0x040017A7 RID: 6055
	[Range(0.4f, 1f)]
	public float longShotTreshold = 0.8f;

	// Token: 0x040017A8 RID: 6056
	public AudioClip[] longShotClips;

	// Token: 0x040017A9 RID: 6057
	public AudioClip[] shortShotClips;

	// Token: 0x040017AA RID: 6058
	public AnimationCurve oneShotVolCurve;

	// Token: 0x040017AB RID: 6059
	public AnimationCurve oneShotPitchCurve;

	// Token: 0x040017AC RID: 6060
	public bool destroyAudioSources;

	// Token: 0x040017AD RID: 6061
	private AudioSource turboLoop;

	// Token: 0x040017AE RID: 6062
	private AudioSource oneShot;

	// Token: 0x040017AF RID: 6063
	private AudioSource maxTurboLoop;

	// Token: 0x040017B0 RID: 6064
	private int oneShotController;

	// Token: 0x040017B1 RID: 6065
	private WaitForSeconds _playtime;
}
