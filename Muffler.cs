using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200024D RID: 589
public class Muffler : MonoBehaviour
{
	// Token: 0x06000DEB RID: 3563 RVA: 0x00094CAC File Offset: 0x00092EAC
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
		this.playTime_ = this.playTime;
		this.UpdateWaitTime();
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x00094D34 File Offset: 0x00092F34
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

	// Token: 0x06000DED RID: 3565 RVA: 0x000951D5 File Offset: 0x000933D5
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x000951DD File Offset: 0x000933DD
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

	// Token: 0x06000DEF RID: 3567 RVA: 0x00095214 File Offset: 0x00093414
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

	// Token: 0x06000DF0 RID: 3568 RVA: 0x0009533C File Offset: 0x0009353C
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

	// Token: 0x06000DF1 RID: 3569 RVA: 0x00095461 File Offset: 0x00093661
	private void UpdateWaitTime()
	{
		this._playtime = new WaitForSeconds(this.playTime);
		this.playTime_ = this.playTime;
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x00095480 File Offset: 0x00093680
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

	// Token: 0x06000DF3 RID: 3571 RVA: 0x0009548F File Offset: 0x0009368F
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

	// Token: 0x04001699 RID: 5785
	private RealisticEngineSound res;

	// Token: 0x0400169A RID: 5786
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x0400169B RID: 5787
	public bool playDuringShifting = true;

	// Token: 0x0400169C RID: 5788
	public bool destroyAudioSources;

	// Token: 0x0400169D RID: 5789
	private bool _destroyAudioSources;

	// Token: 0x0400169E RID: 5790
	public AudioMixerGroup audioMixer;

	// Token: 0x0400169F RID: 5791
	private AudioMixerGroup _audioMixer;

	// Token: 0x040016A0 RID: 5792
	[Range(0.5f, 2f)]
	public float pitchMultiplier = 1f;

	// Token: 0x040016A1 RID: 5793
	[Range(0.5f, 4f)]
	public float playTime = 2f;

	// Token: 0x040016A2 RID: 5794
	private float playTime_;

	// Token: 0x040016A3 RID: 5795
	public AudioClip offClip;

	// Token: 0x040016A4 RID: 5796
	public AudioClip onClip;

	// Token: 0x040016A5 RID: 5797
	private AudioSource offLoop;

	// Token: 0x040016A6 RID: 5798
	private AudioSource onLoop;

	// Token: 0x040016A7 RID: 5799
	public AnimationCurve mufflerOffVolCurve;

	// Token: 0x040016A8 RID: 5800
	public AnimationCurve mufflerOnVolCurve;

	// Token: 0x040016A9 RID: 5801
	private float clipsValue;

	// Token: 0x040016AA RID: 5802
	private int oneShotController;

	// Token: 0x040016AB RID: 5803
	private WaitForSeconds _playtime;
}
