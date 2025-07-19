using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000250 RID: 592
public class MufflerRandom : MonoBehaviour
{
	// Token: 0x06000E01 RID: 3585 RVA: 0x000955F8 File Offset: 0x000937F8
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

	// Token: 0x06000E02 RID: 3586 RVA: 0x00095680 File Offset: 0x00093880
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
									this.offLoop.clip = this.offClip[UnityEngine.Random.Range(0, this.offClip.Length)];
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
								this.offLoop.clip = this.offClip[UnityEngine.Random.Range(0, this.offClip.Length)];
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
							this.offLoop.clip = this.offClip[UnityEngine.Random.Range(0, this.offClip.Length)];
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
									this.onLoop.clip = this.onClip[UnityEngine.Random.Range(0, this.onClip.Length)];
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
								this.onLoop.clip = this.onClip[UnityEngine.Random.Range(0, this.onClip.Length)];
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

	// Token: 0x06000E03 RID: 3587 RVA: 0x00095BD0 File Offset: 0x00093DD0
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00095BD8 File Offset: 0x00093DD8
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

	// Token: 0x06000E05 RID: 3589 RVA: 0x00095C0C File Offset: 0x00093E0C
	private void CreateOff()
	{
		if (this.offClip != null)
		{
			this.offLoop = base.gameObject.AddComponent<AudioSource>();
			this.offLoop.spatialBlend = this.res.spatialBlend;
			this.offLoop.rolloffMode = this.res.audioRolloffMode;
			this.offLoop.dopplerLevel = this.res.dopplerLevel;
			this.offLoop.minDistance = this.res.minDistance;
			this.offLoop.maxDistance = this.res.maxDistance;
			this.offLoop.volume = this.mufflerOffVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.offLoop.pitch = this.pitchMultiplier;
			this.offLoop.clip = this.offClip[UnityEngine.Random.Range(0, this.offClip.Length)];
			this.offLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.offLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.offLoop.Play();
			base.StartCoroutine(this.WaitForOffLoop());
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x00095D3C File Offset: 0x00093F3C
	private void CreateOn()
	{
		if (this.onClip != null)
		{
			this.onLoop = base.gameObject.AddComponent<AudioSource>();
			this.onLoop.spatialBlend = this.res.spatialBlend;
			this.onLoop.rolloffMode = this.res.audioRolloffMode;
			this.onLoop.dopplerLevel = this.res.dopplerLevel;
			this.onLoop.minDistance = this.res.minDistance;
			this.onLoop.maxDistance = this.res.maxDistance;
			this.onLoop.volume = this.mufflerOnVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.onLoop.pitch = this.pitchMultiplier;
			this.onLoop.clip = this.onClip[UnityEngine.Random.Range(0, this.onClip.Length)];
			this.onLoop.loop = true;
			if (this._audioMixer != null)
			{
				this.onLoop.outputAudioMixerGroup = this._audioMixer;
			}
			this.onLoop.Play();
			base.StartCoroutine(this.WaitForOnLoop());
		}
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x00095E6A File Offset: 0x0009406A
	private void UpdateWaitTime()
	{
		this._playtime = new WaitForSeconds(this.playTime);
		this.playTime_ = this.playTime;
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x00095E89 File Offset: 0x00094089
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

	// Token: 0x06000E09 RID: 3593 RVA: 0x00095E98 File Offset: 0x00094098
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

	// Token: 0x040016B2 RID: 5810
	private RealisticEngineSound res;

	// Token: 0x040016B3 RID: 5811
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x040016B4 RID: 5812
	public bool playDuringShifting = true;

	// Token: 0x040016B5 RID: 5813
	public bool destroyAudioSources;

	// Token: 0x040016B6 RID: 5814
	private bool _destroyAudioSources;

	// Token: 0x040016B7 RID: 5815
	public AudioMixerGroup audioMixer;

	// Token: 0x040016B8 RID: 5816
	private AudioMixerGroup _audioMixer;

	// Token: 0x040016B9 RID: 5817
	[Range(0.5f, 2f)]
	public float pitchMultiplier = 1f;

	// Token: 0x040016BA RID: 5818
	[Range(0.5f, 4f)]
	public float playTime = 2f;

	// Token: 0x040016BB RID: 5819
	private float playTime_;

	// Token: 0x040016BC RID: 5820
	public AudioClip[] offClip;

	// Token: 0x040016BD RID: 5821
	public AudioClip[] onClip;

	// Token: 0x040016BE RID: 5822
	private AudioSource offLoop;

	// Token: 0x040016BF RID: 5823
	private AudioSource onLoop;

	// Token: 0x040016C0 RID: 5824
	public AnimationCurve mufflerOffVolCurve;

	// Token: 0x040016C1 RID: 5825
	public AnimationCurve mufflerOnVolCurve;

	// Token: 0x040016C2 RID: 5826
	private float clipsValue;

	// Token: 0x040016C3 RID: 5827
	private int oneShotController;

	// Token: 0x040016C4 RID: 5828
	private WaitForSeconds _playtime;
}
