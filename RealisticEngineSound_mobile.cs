using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200025B RID: 603
public class RealisticEngineSound_mobile : MonoBehaviour
{
	// Token: 0x06000E46 RID: 3654 RVA: 0x000985B4 File Offset: 0x000967B4
	private void Start()
	{
		this.spatialBlend = 1f;
		this._wait = new WaitForSeconds(0.15f);
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
		if (this.mainCamera != null && Vector3.Distance(this.mainCamera.transform.position, base.gameObject.transform.position) <= this.maxDistance)
		{
			this.isCameraNear = true;
		}
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x0009864C File Offset: 0x0009684C
	private void Update()
	{
		if (this.isCameraNear)
		{
			this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
			if (this.idleClip != null)
			{
				if (this.engineIdle == null && this.idleVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateIdle();
				}
				if (this.engineIdle != null)
				{
					this.engineIdle.volume = this.idleVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
					this.engineIdle.pitch = this.idlePitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
				if (this.engineIdle != null && this.destroyAudioSources && this.engineIdle.volume < this.optimisationLevel)
				{
					UnityEngine.Object.Destroy(this.engineIdle);
				}
			}
			if (this.lowOnClip != null)
			{
				if (this.lowOn == null && this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateLowOn();
				}
				if (this.lowOn != null)
				{
					this.lowOn.volume = this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
					this.lowOn.pitch = this.lowPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
			}
			if (this.lowOn != null && this.destroyAudioSources && this.lowOn.volume < this.optimisationLevel)
			{
				UnityEngine.Object.Destroy(this.lowOn);
			}
			if (this.medOnClip != null)
			{
				if (this.medOn == null && this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateMedOn();
				}
				if (this.medOn != null)
				{
					this.medOn.volume = this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
					this.medOn.pitch = this.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
				if (this.medOn != null && this.destroyAudioSources && this.medOn.volume < this.optimisationLevel)
				{
					UnityEngine.Object.Destroy(this.medOn);
				}
			}
			if (this.highOnClip != null)
			{
				if (this.highOn == null && this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateHighOn();
				}
				if (this.highOn != null)
				{
					if (this.maxRPM != null)
					{
						if (this.maxRPM.volume < 0.95f)
						{
							this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
						}
						else
						{
							this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume / 3.3f;
						}
					}
					else
					{
						this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
					}
					this.highOn.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
				if (this.highOn != null && this.destroyAudioSources && this.highOn.volume < this.optimisationLevel)
				{
					UnityEngine.Object.Destroy(this.highOn);
				}
			}
			if (this.maxRPMClip != null)
			{
				if (this.useRPMLimit)
				{
					if (this.maxRPMVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
					{
						if (this.maxRPM == null)
						{
							this.CreateRPMLimit();
						}
						else
						{
							this.maxRPM.volume = this.maxRPMVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
							this.maxRPM.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
						}
					}
					else
					{
						if (this.maxRPM != null && this.maxRPM.volume > this.maxRPMVolCurve.Evaluate(this.clipsValue))
						{
							this.maxRPM.volume = this.maxRPMVolCurve.Evaluate(this.clipsValue);
						}
						if (this.destroyAudioSources && this.maxRPM != null)
						{
							UnityEngine.Object.Destroy(this.maxRPM);
						}
					}
				}
				else if (this.maxRPM != null)
				{
					UnityEngine.Object.Destroy(this.maxRPM);
				}
			}
			else
			{
				this.useRPMLimit = false;
			}
			if (this.enableReverseGear)
			{
				if (!(this.reversingClip != null))
				{
					this.isReversing = false;
					this.enableReverseGear = false;
					return;
				}
				if (this.isReversing)
				{
					if (this.reversingVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
					{
						if (this.reversing == null)
						{
							this.CreateReverse();
							return;
						}
						this.reversing.volume = this.reversingVolCurve.Evaluate(this.carCurrentSpeed / 55f) * this.masterVolume;
						this.reversing.pitch = this.reversingPitchCurve.Evaluate(this.carCurrentSpeed / 55f);
						return;
					}
					else if (this.destroyAudioSources && this.reversing != null)
					{
						UnityEngine.Object.Destroy(this.reversing);
						return;
					}
				}
				else if (this.reversing != null)
				{
					UnityEngine.Object.Destroy(this.reversing);
					return;
				}
			}
			else
			{
				if (this.isReversing)
				{
					this.isReversing = false;
				}
				if (this.reversing != null)
				{
					UnityEngine.Object.Destroy(this.reversing);
					return;
				}
			}
		}
		else if (!this.alreadyDestroyed)
		{
			this.DestroyAll();
			this.alreadyDestroyed = true;
		}
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x00098C80 File Offset: 0x00096E80
	private void FixedUpdate()
	{
		if (this.mainCamera != null)
		{
			if (Vector3.Distance(this.mainCamera.transform.position, base.gameObject.transform.position) > this.maxDistance)
			{
				this.isCameraNear = false;
			}
			else
			{
				this.isCameraNear = true;
				if (this.alreadyDestroyed)
				{
					this.alreadyDestroyed = false;
				}
			}
			if (!this.enableReverseGear && this.reversing != null)
			{
				UnityEngine.Object.Destroy(this.reversing);
			}
			if (!this.useRPMLimit && this.maxRPM != null)
			{
				UnityEngine.Object.Destroy(this.maxRPM);
			}
		}
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x00098D2C File Offset: 0x00096F2C
	private void OnEnable()
	{
		base.StartCoroutine(this.WaitForStart());
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x00098D3B File Offset: 0x00096F3B
	private void OnDisable()
	{
		this.DestroyAll();
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x00098D44 File Offset: 0x00096F44
	private void DestroyAll()
	{
		if (this.engineIdle != null)
		{
			UnityEngine.Object.Destroy(this.engineIdle);
		}
		if (this.lowOn != null)
		{
			UnityEngine.Object.Destroy(this.lowOn);
		}
		if (this.medOn != null)
		{
			UnityEngine.Object.Destroy(this.medOn);
		}
		if (this.highOn != null)
		{
			UnityEngine.Object.Destroy(this.highOn);
		}
		if (this.useRPMLimit && this.maxRPM != null)
		{
			UnityEngine.Object.Destroy(this.maxRPM);
		}
		if (this.enableReverseGear && this.reversing != null)
		{
			UnityEngine.Object.Destroy(this.reversing);
		}
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00098DF7 File Offset: 0x00096FF7
	private IEnumerator WaitForStart()
	{
		yield return this._wait;
		if (this.engineIdle == null)
		{
			this.Start();
		}
		yield break;
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x00098E08 File Offset: 0x00097008
	private void CreateIdle()
	{
		if (this.idleClip != null)
		{
			this.engineIdle = base.gameObject.AddComponent<AudioSource>();
			this.engineIdle.spatialBlend = this.spatialBlend;
			this.engineIdle.rolloffMode = this.audioRolloffMode;
			this.engineIdle.dopplerLevel = this.dopplerLevel;
			this.engineIdle.volume = this.idleVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.engineIdle.pitch = this.idlePitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.engineIdle.minDistance = this.minDistance;
			this.engineIdle.maxDistance = this.maxDistance;
			this.engineIdle.loop = true;
			this.engineIdle.clip = this.idleClip;
			this.engineIdle.Play();
			if (this.audioMixer != null)
			{
				this.engineIdle.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x00098F1C File Offset: 0x0009711C
	private void CreateLowOn()
	{
		if (this.lowOnClip != null)
		{
			this.lowOn = base.gameObject.AddComponent<AudioSource>();
			this.lowOn.spatialBlend = this.spatialBlend;
			this.lowOn.rolloffMode = this.audioRolloffMode;
			this.lowOn.dopplerLevel = this.dopplerLevel;
			this.lowOn.volume = this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.lowOn.pitch = this.lowPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.lowOn.minDistance = this.minDistance;
			this.lowOn.maxDistance = this.maxDistance;
			this.lowOn.loop = true;
			this.lowOn.clip = this.lowOnClip;
			this.lowOn.Play();
			if (this.audioMixer != null)
			{
				this.lowOn.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x00099030 File Offset: 0x00097230
	private void CreateMedOn()
	{
		if (this.medOnClip != null)
		{
			this.medOn = base.gameObject.AddComponent<AudioSource>();
			this.medOn.spatialBlend = this.spatialBlend;
			this.medOn.rolloffMode = this.audioRolloffMode;
			this.medOn.dopplerLevel = this.dopplerLevel;
			this.medOn.volume = this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.medOn.pitch = this.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.medOn.minDistance = this.minDistance;
			this.medOn.maxDistance = this.maxDistance;
			this.medOn.loop = true;
			this.medOn.clip = this.medOnClip;
			this.medOn.Play();
			if (this.audioMixer != null)
			{
				this.medOn.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00099144 File Offset: 0x00097344
	private void CreateHighOn()
	{
		if (this.highOnClip != null)
		{
			this.highOn = base.gameObject.AddComponent<AudioSource>();
			this.highOn.spatialBlend = this.spatialBlend;
			this.highOn.rolloffMode = this.audioRolloffMode;
			this.highOn.dopplerLevel = this.dopplerLevel;
			this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.highOn.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.highOn.minDistance = this.minDistance;
			this.highOn.maxDistance = this.maxDistance;
			this.highOn.loop = true;
			this.highOn.clip = this.highOnClip;
			this.highOn.Play();
			if (this.audioMixer != null)
			{
				this.highOn.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x00099258 File Offset: 0x00097458
	private void CreateRPMLimit()
	{
		if (this.maxRPMClip != null)
		{
			this.maxRPM = base.gameObject.AddComponent<AudioSource>();
			this.maxRPM.spatialBlend = this.spatialBlend;
			this.maxRPM.rolloffMode = this.audioRolloffMode;
			this.maxRPM.dopplerLevel = this.dopplerLevel;
			this.maxRPM.volume = this.maxRPMVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.maxRPM.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.maxRPM.minDistance = this.minDistance;
			this.maxRPM.maxDistance = this.maxDistance;
			this.maxRPM.loop = true;
			this.maxRPM.clip = this.maxRPMClip;
			this.maxRPM.Play();
			if (this.audioMixer != null)
			{
				this.maxRPM.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x0009936C File Offset: 0x0009756C
	private void CreateReverse()
	{
		if (this.reversingClip != null)
		{
			this.reversing = base.gameObject.AddComponent<AudioSource>();
			this.reversing.spatialBlend = this.spatialBlend;
			this.reversing.rolloffMode = this.audioRolloffMode;
			this.reversing.dopplerLevel = this.dopplerLevel;
			this.reversing.volume = this.reversingVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
			this.reversing.pitch = this.reversingPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.reversing.minDistance = this.minDistance;
			this.reversing.maxDistance = this.maxDistance;
			this.reversing.loop = true;
			this.reversing.clip = this.reversingClip;
			this.reversing.Play();
			if (this.audioMixer != null)
			{
				this.reversing.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x04001738 RID: 5944
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x04001739 RID: 5945
	public AudioMixerGroup audioMixer;

	// Token: 0x0400173A RID: 5946
	public bool destroyAudioSources = true;

	// Token: 0x0400173B RID: 5947
	public float engineCurrentRPM;

	// Token: 0x0400173C RID: 5948
	public float maxRPMLimit = 7000f;

	// Token: 0x0400173D RID: 5949
	[Range(0f, 5f)]
	public float dopplerLevel = 1f;

	// Token: 0x0400173E RID: 5950
	[Range(0f, 1f)]
	[HideInInspector]
	public float spatialBlend = 1f;

	// Token: 0x0400173F RID: 5951
	[Range(0.1f, 2f)]
	public float pitchMultiplier = 1f;

	// Token: 0x04001740 RID: 5952
	[Range(0f, 0.25f)]
	public float optimisationLevel = 0.01f;

	// Token: 0x04001741 RID: 5953
	public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Custom;

	// Token: 0x04001742 RID: 5954
	public float minDistance = 1f;

	// Token: 0x04001743 RID: 5955
	public float maxDistance = 50f;

	// Token: 0x04001744 RID: 5956
	public bool isReversing;

	// Token: 0x04001745 RID: 5957
	public bool useRPMLimit = true;

	// Token: 0x04001746 RID: 5958
	public bool enableReverseGear;

	// Token: 0x04001747 RID: 5959
	[HideInInspector]
	public float carCurrentSpeed;

	// Token: 0x04001748 RID: 5960
	[HideInInspector]
	public float carMaxSpeed;

	// Token: 0x04001749 RID: 5961
	[HideInInspector]
	public bool isShifting;

	// Token: 0x0400174A RID: 5962
	public bool gasPedalPressing;

	// Token: 0x0400174B RID: 5963
	public AudioClip idleClip;

	// Token: 0x0400174C RID: 5964
	public AnimationCurve idleVolCurve;

	// Token: 0x0400174D RID: 5965
	public AnimationCurve idlePitchCurve;

	// Token: 0x0400174E RID: 5966
	public AudioClip lowOnClip;

	// Token: 0x0400174F RID: 5967
	public AnimationCurve lowVolCurve;

	// Token: 0x04001750 RID: 5968
	public AnimationCurve lowPitchCurve;

	// Token: 0x04001751 RID: 5969
	public AudioClip medOnClip;

	// Token: 0x04001752 RID: 5970
	public AnimationCurve medVolCurve;

	// Token: 0x04001753 RID: 5971
	public AnimationCurve medPitchCurve;

	// Token: 0x04001754 RID: 5972
	public AudioClip highOnClip;

	// Token: 0x04001755 RID: 5973
	public AnimationCurve highVolCurve;

	// Token: 0x04001756 RID: 5974
	public AnimationCurve highPitchCurve;

	// Token: 0x04001757 RID: 5975
	public AudioClip maxRPMClip;

	// Token: 0x04001758 RID: 5976
	public AnimationCurve maxRPMVolCurve;

	// Token: 0x04001759 RID: 5977
	public AudioClip reversingClip;

	// Token: 0x0400175A RID: 5978
	public AnimationCurve reversingVolCurve;

	// Token: 0x0400175B RID: 5979
	public AnimationCurve reversingPitchCurve;

	// Token: 0x0400175C RID: 5980
	private AudioSource engineIdle;

	// Token: 0x0400175D RID: 5981
	private AudioSource lowOn;

	// Token: 0x0400175E RID: 5982
	private AudioSource medOn;

	// Token: 0x0400175F RID: 5983
	private AudioSource highOn;

	// Token: 0x04001760 RID: 5984
	private AudioSource maxRPM;

	// Token: 0x04001761 RID: 5985
	private AudioSource reversing;

	// Token: 0x04001762 RID: 5986
	private float clipsValue;

	// Token: 0x04001763 RID: 5987
	public Camera mainCamera;

	// Token: 0x04001764 RID: 5988
	[HideInInspector]
	public bool isCameraNear;

	// Token: 0x04001765 RID: 5989
	private WaitForSeconds _wait;

	// Token: 0x04001766 RID: 5990
	private bool alreadyDestroyed;
}
