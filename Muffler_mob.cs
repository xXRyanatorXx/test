using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000253 RID: 595
public class Muffler_mob : MonoBehaviour
{
	// Token: 0x06000E17 RID: 3607 RVA: 0x00096000 File Offset: 0x00094200
	private void Start()
	{
		this.res = base.gameObject.transform.parent.GetComponent<RealisticEngineSound_mobile>();
		if (this.audioMixer != null)
		{
			this._audioMixer = this.audioMixer;
		}
		else if (this.res.audioMixer != null)
		{
			this._audioMixer = this.res.audioMixer;
			this.audioMixer = this._audioMixer;
		}
		this.playTime_ = this.playTime;
		this.UpdateWaitTime();
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x00096088 File Offset: 0x00094288
	private void Update()
	{
		if (this._destroyAudioSources != this.destroyAudioSources)
		{
			this._destroyAudioSources = this.destroyAudioSources;
			if (this.destroyAudioSources)
			{
				if (this.onLoop != null)
				{
					UnityEngine.Object.Destroy(this.onLoop);
				}
				if (this.offLoop != null)
				{
					UnityEngine.Object.Destroy(this.offLoop);
				}
			}
		}
		if (this.res.enabled)
		{
			this.clipsValue = this.res.engineCurrentRPM / this.res.maxRPMLimit;
			if (this.res.isCameraNear)
			{
				if (this.res.gasPedalPressing)
				{
					if (this.oneShotController != 2)
					{
						this.oneShotController = 1;
					}
				}
				else if (this.mufflerOffVolCurve.Evaluate(this.clipsValue) * this.masterVolume > 0.09f)
				{
					if (!this.playDuringShifting)
					{
						if (this.oneShotController == 2)
						{
							if (this.offLoop == null)
							{
								if (!this.res.isShifting)
								{
									this.CreateOff();
								}
							}
							else if (!this.res.isShifting)
							{
								if (!this.destroyAudioSources && !this.offLoop.isPlaying)
								{
									this.offLoop.Play();
									base.StartCoroutine(this.WaitForOffLoop());
								}
								this.offLoop.pitch = this.pitchMultiplier;
								this.offLoop.volume = this.mufflerOffVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
							}
						}
					}
					else if (this.oneShotController == 2)
					{
						if (this.offLoop == null)
						{
							this.CreateOff();
						}
						else if (!this._destroyAudioSources)
						{
							if (!this.offLoop.isPlaying)
							{
								this.offLoop.Play();
								base.StartCoroutine(this.WaitForOffLoop());
							}
							this.offLoop.pitch = this.res.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
							this.offLoop.volume = this.mufflerOffVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
						}
					}
				}
				if (this.res.isShifting && this.playDuringShifting)
				{
					if (this.offLoop == null)
					{
						this.CreateOff();
					}
					else
					{
						if (!this.destroyAudioSources && !this.offLoop.isPlaying)
						{
							this.offLoop.Play();
							base.StartCoroutine(this.WaitForOffLoop());
						}
						this.offLoop.pitch = this.pitchMultiplier;
						this.offLoop.volume = this.mufflerOffVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
					}
				}
				if (this.mufflerOnVolCurve.Evaluate(this.clipsValue) * this.masterVolume > 0.09f)
				{
					if (!this.playDuringShifting)
					{
						if (this.oneShotController == 1)
						{
							if (this.onLoop == null)
							{
								if (!this.res.isShifting)
								{
									this.CreateOn();
									this.oneShotController = 2;
								}
							}
							else
							{
								if (!this._destroyAudioSources && !this.onLoop.isPlaying)
								{
									this.onLoop.Play();
									this.oneShotController = 2;
									base.StartCoroutine(this.WaitForOnLoop());
								}
								if (!this.res.isShifting)
								{
									this.onLoop.pitch = this.pitchMultiplier;
									this.onLoop.volume = this.mufflerOnVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
								}
								this.oneShotController = 2;
							}
						}
					}
					else if (this.oneShotController == 1)
					{
						if (this.onLoop == null)
						{
							this.CreateOn();
							this.oneShotController = 2;
						}
						else if (!this._destroyAudioSources)
						{
							if (!this.onLoop.isPlaying)
							{
								this.onLoop.Play();
								this.oneShotController = 2;
								base.StartCoroutine(this.WaitForOnLoop());
							}
							this.onLoop.pitch = this.res.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
							this.onLoop.volume = this.mufflerOnVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
						}
					}
				}
			}
		}
		else
		{
			if (this.onLoop != null)
			{
				UnityEngine.Object.Destroy(this.onLoop);
			}
			if (this.offLoop != null)
			{
				UnityEngine.Object.Destroy(this.offLoop);
			}
		}
		if (this.playTime_ != this.playTime)
		{
			this.UpdateWaitTime();
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x00096529 File Offset: 0x00094729
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x00096531 File Offset: 0x00094731
	private void OnDisable()
	{
		if (this.onLoop != null)
		{
			UnityEngine.Object.Destroy(this.onLoop);
		}
		if (this.offLoop != null)
		{
			UnityEngine.Object.Destroy(this.offLoop);
		}
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x00096568 File Offset: 0x00094768
	private void CreateOff()
	{
		if (this.offClip != null)
		{
			this.offLoop = base.gameObject.AddComponent<AudioSource>();
			this.offLoop.rolloffMode = this.res.audioRolloffMode;
			this.offLoop.minDistance = this.res.minDistance;
			this.offLoop.maxDistance = this.res.maxDistance;
			this.offLoop.spatialBlend = this.res.spatialBlend;
			this.offLoop.dopplerLevel = this.res.dopplerLevel;
			this.offLoop.volume = this.mufflerOffVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.offLoop.pitch = this.pitchMultiplier;
			this.offLoop.clip = this.offClip;
			this.offLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.offLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.offLoop.Play();
			base.StartCoroutine(this.WaitForOffLoop());
		}
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x00096690 File Offset: 0x00094890
	private void CreateOn()
	{
		if (this.onClip != null)
		{
			this.onLoop = base.gameObject.AddComponent<AudioSource>();
			this.onLoop.rolloffMode = this.res.audioRolloffMode;
			this.onLoop.minDistance = this.res.minDistance;
			this.onLoop.maxDistance = this.res.maxDistance;
			this.onLoop.spatialBlend = this.res.spatialBlend;
			this.onLoop.dopplerLevel = this.res.dopplerLevel;
			this.onLoop.volume = this.mufflerOnVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.onLoop.pitch = this.pitchMultiplier;
			this.onLoop.clip = this.onClip;
			this.onLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.onLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.onLoop.Play();
			base.StartCoroutine(this.WaitForOnLoop());
		}
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x000967B5 File Offset: 0x000949B5
	private void UpdateWaitTime()
	{
		this._playtime = new WaitForSeconds(this.playTime);
		this.playTime_ = this.playTime;
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x000967D4 File Offset: 0x000949D4
	private IEnumerator WaitForOnLoop()
	{
		yield return this._playtime;
		if (this.onLoop != null)
		{
			if (this._destroyAudioSources)
			{
				UnityEngine.Object.Destroy(this.onLoop);
			}
			else
			{
				this.onLoop.Stop();
			}
		}
		yield break;
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x000967E3 File Offset: 0x000949E3
	private IEnumerator WaitForOffLoop()
	{
		yield return this._playtime;
		this.oneShotController = 0;
		if (this.offLoop != null)
		{
			if (this._destroyAudioSources)
			{
				UnityEngine.Object.Destroy(this.offLoop);
			}
			else
			{
				this.offLoop.Stop();
			}
		}
		yield break;
	}

	// Token: 0x040016CB RID: 5835
	private RealisticEngineSound_mobile res;

	// Token: 0x040016CC RID: 5836
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x040016CD RID: 5837
	public bool playDuringShifting = true;

	// Token: 0x040016CE RID: 5838
	public bool destroyAudioSources;

	// Token: 0x040016CF RID: 5839
	private bool _destroyAudioSources;

	// Token: 0x040016D0 RID: 5840
	public AudioMixerGroup audioMixer;

	// Token: 0x040016D1 RID: 5841
	private AudioMixerGroup _audioMixer;

	// Token: 0x040016D2 RID: 5842
	[Range(0.5f, 2f)]
	public float pitchMultiplier = 1f;

	// Token: 0x040016D3 RID: 5843
	[Range(0.5f, 4f)]
	public float playTime = 2f;

	// Token: 0x040016D4 RID: 5844
	private float playTime_;

	// Token: 0x040016D5 RID: 5845
	public AudioClip offClip;

	// Token: 0x040016D6 RID: 5846
	public AudioClip onClip;

	// Token: 0x040016D7 RID: 5847
	private AudioSource offLoop;

	// Token: 0x040016D8 RID: 5848
	private AudioSource onLoop;

	// Token: 0x040016D9 RID: 5849
	public AnimationCurve mufflerOffVolCurve;

	// Token: 0x040016DA RID: 5850
	public AnimationCurve mufflerOnVolCurve;

	// Token: 0x040016DB RID: 5851
	private float clipsValue;

	// Token: 0x040016DC RID: 5852
	private int oneShotController;

	// Token: 0x040016DD RID: 5853
	private WaitForSeconds _playtime;
}
