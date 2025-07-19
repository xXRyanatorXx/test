using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000256 RID: 598
public class RealisticEngineSound : MonoBehaviour
{
	// Token: 0x06000E2D RID: 3629 RVA: 0x0009694C File Offset: 0x00094B4C
	private void Start()
	{
		this.spatialBlend = 1f;
		this._wait = new WaitForSeconds(0.15f);
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
		if (this.mainCamera != null)
		{
			if (Vector3.Distance(this.mainCamera.transform.position, base.gameObject.transform.position) <= this.maxDistance)
			{
				this.isCameraNear = true;
				this.reverbZoneControll = this.reverbZoneSetting;
				this.SetReverbZone();
			}
			else
			{
				this.isCameraNear = false;
			}
		}
		this.UpdateStartRange();
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x00096A04 File Offset: 0x00094C04
	private void Update()
	{
		if (this.isCameraNear)
		{
			if (this.gasPedalValueSetting == RealisticEngineSound.GasPedalValue.Simulated)
			{
				if (this.shakeVolumeChangeDetect != this.shakeVolumeChange)
				{
					this.UpdateStartRange();
				}
				if (this.engineShakeSetting == RealisticEngineSound.EngineShake.Off)
				{
					this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
					if (this.gasPedalPressing)
					{
						this.gasPedalValue = Mathf.Lerp(this.gasPedalValue, 1f, Time.deltaTime * this.gasPedalSimSpeed);
					}
					else
					{
						this.gasPedalValue = Mathf.Lerp(this.gasPedalValue, 0f, Time.deltaTime * this.gasPedalSimSpeed);
					}
				}
				if (this.engineShakeSetting == RealisticEngineSound.EngineShake.AllwaysOn)
				{
					if (this.gasPedalPressing)
					{
						if (this.lenght < 1f)
						{
							if (this.shakeLenghtSetting == RealisticEngineSound.ShakeLenghtType.Fix)
							{
								this.gasPedalValue = this._oscillateOffset + Mathf.Sin(Time.time * (this.shakeLength * this.clipsValue)) * this._oscillateRange;
								this.clipsValue2 = this.engineCurrentRPM / this.maxRPMLimit + Mathf.Sin(Time.time * this.shakeLength) * (this._oscillateRange / 10f);
							}
							if (this.shakeLenghtSetting == RealisticEngineSound.ShakeLenghtType.Random)
							{
								this.gasPedalValue = this._oscillateOffset + Mathf.Sin(Time.time * ((float)UnityEngine.Random.Range(10, 100) * this.clipsValue)) * this._oscillateRange;
								this.clipsValue2 = this.engineCurrentRPM / this.maxRPMLimit + Mathf.Sin(Time.time * (float)UnityEngine.Random.Range(10, 100)) * (this._oscillateRange / 10f);
							}
							this.lenght += UnityEngine.Random.Range(0.01f, 0.12f);
							this.clipsValue = this.clipsValue2;
						}
						else
						{
							this.gasPedalValue = Mathf.Lerp(this.gasPedalValue, 1f, Time.deltaTime * this.gasPedalSimSpeed);
							this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
						}
					}
					else
					{
						this.gasPedalValue = Mathf.Lerp(this.gasPedalValue, 0f, Time.deltaTime * this.gasPedalSimSpeed);
						this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
						this.lenght = 0f;
					}
				}
				if (this.engineShakeSetting == RealisticEngineSound.EngineShake.Random)
				{
					if (this.gasPedalPressing)
					{
						this.randomShakingValue2 = 0f;
						if (this.randomShakingValue == 0f)
						{
							this.randomShakingValue = UnityEngine.Random.Range(0.1f, 1f);
						}
						if (this.randomShakingValue < this.randomChance)
						{
							if (this.lenght < 1f)
							{
								if (this.shakeLenghtSetting == RealisticEngineSound.ShakeLenghtType.Fix)
								{
									this.gasPedalValue = this._oscillateOffset + Mathf.Sin(Time.time * (this.shakeLength * this.clipsValue)) * this._oscillateRange;
									this.clipsValue2 = this.engineCurrentRPM / this.maxRPMLimit + Mathf.Sin(Time.time * this.shakeLength) * (this._oscillateRange / 10f);
								}
								if (this.shakeLenghtSetting == RealisticEngineSound.ShakeLenghtType.Random)
								{
									this.gasPedalValue = this._oscillateOffset + Mathf.Sin(Time.time * ((float)UnityEngine.Random.Range(10, 100) * this.clipsValue)) * this._oscillateRange;
									this.clipsValue2 = this.engineCurrentRPM / this.maxRPMLimit + Mathf.Sin(Time.time * (float)UnityEngine.Random.Range(10, 100)) * (this._oscillateRange / 10f);
								}
								this.lenght += UnityEngine.Random.Range(0.01f, 0.12f);
								this.clipsValue = this.clipsValue2;
							}
							else
							{
								this.gasPedalValue = Mathf.Lerp(this.gasPedalValue, 1f, Time.deltaTime * this.gasPedalSimSpeed);
								this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
							}
						}
						else
						{
							this.gasPedalValue = Mathf.Lerp(this.gasPedalValue, 1f, Time.deltaTime * this.gasPedalSimSpeed);
							this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
						}
					}
					else
					{
						this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
						this.randomShakingValue = 0f;
						if (this.randomShakingValue2 == 0f)
						{
							this.randomShakingValue2 = UnityEngine.Random.Range(0.1f, 1f);
						}
						this.lenght = 0f;
						this.gasPedalValue = Mathf.Lerp(this.gasPedalValue, 0f, Time.deltaTime * this.gasPedalSimSpeed);
					}
				}
			}
			else
			{
				this.clipsValue = this.engineCurrentRPM / this.maxRPMLimit;
			}
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
					this.lowOn.volume = this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue;
					this.lowOn.pitch = this.lowPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
			}
			if (this.lowOn != null && this.destroyAudioSources && this.lowOn.volume < this.optimisationLevel)
			{
				UnityEngine.Object.Destroy(this.lowOn);
			}
			if (this.lowOffClip != null)
			{
				if (this.lowOff == null && this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateLowOff();
				}
				if (this.lowOff != null)
				{
					this.lowOff.volume = this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume * (1f - this.gasPedalValue);
					this.lowOff.pitch = this.lowPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
			}
			if (this.lowOff != null && this.destroyAudioSources && this.lowOff.volume < this.optimisationLevel)
			{
				UnityEngine.Object.Destroy(this.lowOff);
			}
			if (this.medOnClip != null)
			{
				if (this.medOn == null && this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateMedOn();
				}
				if (this.medOn != null)
				{
					this.medOn.volume = this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue;
					this.medOn.pitch = this.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
				if (this.medOn != null && this.destroyAudioSources && this.medOn.volume < this.optimisationLevel)
				{
					UnityEngine.Object.Destroy(this.medOn);
				}
			}
			if (this.medOffClip != null)
			{
				if (this.medOff == null && this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateMedOff();
				}
				if (this.medOff != null)
				{
					this.medOff.volume = this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume * (1f - this.gasPedalValue);
					this.medOff.pitch = this.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
				if (this.medOff != null && this.destroyAudioSources && this.medOff.volume < this.optimisationLevel)
				{
					UnityEngine.Object.Destroy(this.medOff);
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
							this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue;
						}
						else
						{
							this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue / 3.3f;
						}
					}
					else
					{
						this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue;
					}
					this.highOn.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
				if (this.highOn != null && this.destroyAudioSources && this.highOn.volume < this.optimisationLevel)
				{
					UnityEngine.Object.Destroy(this.highOn);
				}
			}
			if (this.highOffClip != null)
			{
				if (this.highOff == null && this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume > this.optimisationLevel)
				{
					this.CreateHighOff();
				}
				if (this.highOff != null)
				{
					this.highOff.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume * (1f - this.gasPedalValue);
					this.highOff.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
				}
				if (this.highOff != null && this.destroyAudioSources && this.highOff.volume < this.optimisationLevel)
				{
					UnityEngine.Object.Destroy(this.highOff);
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
						if (this.maxRPM != null && this.maxRPM.volume > this.maxRPMVolCurve.Evaluate(this.clipsValue) * this.masterVolume)
						{
							this.maxRPM.volume = this.maxRPMVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
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
			if (this.destroyAudioSources)
			{
				this.DestroyAll();
			}
			this.alreadyDestroyed = true;
		}
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x00097770 File Offset: 0x00095970
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
			if (!this.enableReverseGear && this.reversing != null && this.destroyAudioSources)
			{
				UnityEngine.Object.Destroy(this.reversing);
			}
			if (!this.useRPMLimit && this.maxRPM != null)
			{
				UnityEngine.Object.Destroy(this.maxRPM);
			}
			if (this.reverbZoneControll != this.reverbZoneSetting)
			{
				this.SetReverbZone();
				return;
			}
		}
		else
		{
			this.isCameraNear = false;
		}
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x00097840 File Offset: 0x00095A40
	private void OnEnable()
	{
		base.StartCoroutine(this.WaitForStart());
		this.SetReverbZone();
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x00097855 File Offset: 0x00095A55
	private void OnDisable()
	{
		this.DestroyAll();
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x00097860 File Offset: 0x00095A60
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
		if (this.lowOff != null)
		{
			UnityEngine.Object.Destroy(this.lowOff);
		}
		if (this.medOn != null)
		{
			UnityEngine.Object.Destroy(this.medOn);
		}
		if (this.medOff != null)
		{
			UnityEngine.Object.Destroy(this.medOff);
		}
		if (this.highOn != null)
		{
			UnityEngine.Object.Destroy(this.highOn);
		}
		if (this.highOff != null)
		{
			UnityEngine.Object.Destroy(this.highOff);
		}
		if (this.useRPMLimit && this.maxRPM != null)
		{
			UnityEngine.Object.Destroy(this.maxRPM);
		}
		if (this.enableReverseGear && this.reversing != null)
		{
			UnityEngine.Object.Destroy(this.reversing);
		}
		if (base.gameObject.GetComponent<AudioReverbZone>() != null)
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<AudioReverbZone>());
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x00097984 File Offset: 0x00095B84
	private void UpdateStartRange()
	{
		this._oscillateRange = (this._endRange - (1f - this.shakeVolumeChange)) / 2f;
		this._oscillateOffset = this._oscillateRange + (1f - this.shakeVolumeChange);
		this.shakeVolumeChangeDetect = this.shakeVolumeChange;
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x000979D8 File Offset: 0x00095BD8
	private void SetReverbZone()
	{
		if (this.reverbZoneSetting == AudioReverbPreset.Off)
		{
			if (base.gameObject.GetComponent<AudioReverbZone>() != null)
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<AudioReverbZone>());
			}
		}
		else if (base.gameObject.GetComponent<AudioReverbZone>() == null)
		{
			base.gameObject.AddComponent<AudioReverbZone>();
			base.gameObject.GetComponent<AudioReverbZone>().reverbPreset = this.reverbZoneSetting;
		}
		else
		{
			base.gameObject.GetComponent<AudioReverbZone>().reverbPreset = this.reverbZoneSetting;
		}
		this.reverbZoneControll = this.reverbZoneSetting;
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00097A6B File Offset: 0x00095C6B
	private IEnumerator WaitForStart()
	{
		yield return this._wait;
		if (this.engineIdle == null)
		{
			this.Start();
		}
		yield break;
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00097A7C File Offset: 0x00095C7C
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
			this.engineIdle.clip = this.idleClip;
			this.engineIdle.loop = true;
			this.engineIdle.Play();
			if (this.audioMixer != null)
			{
				this.engineIdle.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x00097B90 File Offset: 0x00095D90
	private void CreateLowOff()
	{
		if (this.lowOffClip != null)
		{
			this.lowOff = base.gameObject.AddComponent<AudioSource>();
			this.lowOff.spatialBlend = this.spatialBlend;
			this.lowOff.rolloffMode = this.audioRolloffMode;
			this.lowOff.dopplerLevel = this.dopplerLevel;
			this.lowOff.volume = this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume * (1f - this.gasPedalValue);
			this.lowOff.pitch = this.lowPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.lowOff.minDistance = this.minDistance;
			this.lowOff.maxDistance = this.maxDistance;
			this.lowOff.clip = this.lowOffClip;
			this.lowOff.loop = true;
			this.lowOff.Play();
			if (this.audioMixer != null)
			{
				this.lowOff.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x00097CB0 File Offset: 0x00095EB0
	private void CreateLowOn()
	{
		if (this.lowOnClip != null)
		{
			this.lowOn = base.gameObject.AddComponent<AudioSource>();
			this.lowOn.spatialBlend = this.spatialBlend;
			this.lowOn.rolloffMode = this.audioRolloffMode;
			this.lowOn.dopplerLevel = this.dopplerLevel;
			this.lowOn.volume = this.lowVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue;
			this.lowOn.pitch = this.lowPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.lowOn.minDistance = this.minDistance;
			this.lowOn.maxDistance = this.maxDistance;
			this.lowOn.clip = this.lowOnClip;
			this.lowOn.loop = true;
			this.lowOn.Play();
			if (this.audioMixer != null)
			{
				this.lowOn.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x00097DC8 File Offset: 0x00095FC8
	private void CreateMedOff()
	{
		if (this.medOffClip != null)
		{
			this.medOff = base.gameObject.AddComponent<AudioSource>();
			this.medOff.spatialBlend = this.spatialBlend;
			this.medOff.rolloffMode = this.audioRolloffMode;
			this.medOff.dopplerLevel = this.dopplerLevel;
			this.medOff.volume = this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume * (1f - this.gasPedalValue);
			this.medOff.pitch = this.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.medOff.minDistance = this.minDistance;
			this.medOff.maxDistance = this.maxDistance;
			this.medOff.clip = this.medOffClip;
			this.medOff.loop = true;
			this.medOff.Play();
			if (this.audioMixer != null)
			{
				this.medOff.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x00097EE8 File Offset: 0x000960E8
	private void CreateMedOn()
	{
		if (this.medOnClip != null)
		{
			this.medOn = base.gameObject.AddComponent<AudioSource>();
			this.medOn.spatialBlend = this.spatialBlend;
			this.medOn.rolloffMode = this.audioRolloffMode;
			this.medOn.dopplerLevel = this.dopplerLevel;
			this.medOn.volume = this.medVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue;
			this.medOn.pitch = this.medPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.medOn.minDistance = this.minDistance;
			this.medOn.maxDistance = this.maxDistance;
			this.medOn.clip = this.medOnClip;
			this.medOn.loop = true;
			this.medOn.Play();
			if (this.audioMixer != null)
			{
				this.medOn.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x00098000 File Offset: 0x00096200
	private void CreateHighOff()
	{
		if (this.highOffClip != null)
		{
			this.highOff = base.gameObject.AddComponent<AudioSource>();
			this.highOff.spatialBlend = this.spatialBlend;
			this.highOff.rolloffMode = this.audioRolloffMode;
			this.highOff.dopplerLevel = this.dopplerLevel;
			this.highOff.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume * (1f - this.gasPedalValue);
			this.highOff.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.highOff.minDistance = this.minDistance;
			this.highOff.maxDistance = this.maxDistance;
			this.highOff.clip = this.highOffClip;
			this.highOff.loop = true;
			this.highOff.Play();
			if (this.audioMixer != null)
			{
				this.highOff.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x00098120 File Offset: 0x00096320
	private void CreateHighOn()
	{
		if (this.highOnClip != null)
		{
			this.highOn = base.gameObject.AddComponent<AudioSource>();
			this.highOn.spatialBlend = this.spatialBlend;
			this.highOn.rolloffMode = this.audioRolloffMode;
			this.highOn.dopplerLevel = this.dopplerLevel;
			this.highOn.volume = this.highVolCurve.Evaluate(this.clipsValue) * this.masterVolume * this.gasPedalValue;
			this.highOn.pitch = this.highPitchCurve.Evaluate(this.clipsValue) * this.pitchMultiplier;
			this.highOn.minDistance = this.minDistance;
			this.highOn.maxDistance = this.maxDistance;
			this.highOn.clip = this.highOnClip;
			this.highOn.loop = true;
			this.highOn.Play();
			if (this.audioMixer != null)
			{
				this.highOn.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x00098238 File Offset: 0x00096438
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
			this.maxRPM.clip = this.maxRPMClip;
			this.maxRPM.loop = true;
			this.maxRPM.Play();
			if (this.audioMixer != null)
			{
				this.maxRPM.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x0009834C File Offset: 0x0009654C
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
			this.reversing.clip = this.reversingClip;
			this.reversing.loop = true;
			this.reversing.Play();
			if (this.audioMixer != null)
			{
				this.reversing.outputAudioMixerGroup = this.audioMixer;
			}
		}
	}

	// Token: 0x040016E4 RID: 5860
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x040016E5 RID: 5861
	public AudioMixerGroup audioMixer;

	// Token: 0x040016E6 RID: 5862
	public bool destroyAudioSources = true;

	// Token: 0x040016E7 RID: 5863
	public float engineCurrentRPM;

	// Token: 0x040016E8 RID: 5864
	public bool gasPedalPressing;

	// Token: 0x040016E9 RID: 5865
	[Range(0f, 1f)]
	public float gasPedalValue = 1f;

	// Token: 0x040016EA RID: 5866
	public RealisticEngineSound.GasPedalValue gasPedalValueSetting;

	// Token: 0x040016EB RID: 5867
	[Range(1f, 15f)]
	public float gasPedalSimSpeed = 5.5f;

	// Token: 0x040016EC RID: 5868
	public float maxRPMLimit = 7000f;

	// Token: 0x040016ED RID: 5869
	[Range(0f, 5f)]
	public float dopplerLevel = 1f;

	// Token: 0x040016EE RID: 5870
	[Range(0f, 1f)]
	[HideInInspector]
	public float spatialBlend = 1f;

	// Token: 0x040016EF RID: 5871
	[Range(0.1f, 2f)]
	public float pitchMultiplier = 1f;

	// Token: 0x040016F0 RID: 5872
	public AudioReverbPreset reverbZoneSetting;

	// Token: 0x040016F1 RID: 5873
	private AudioReverbPreset reverbZoneControll;

	// Token: 0x040016F2 RID: 5874
	[Range(0f, 0.25f)]
	public float optimisationLevel = 0.01f;

	// Token: 0x040016F3 RID: 5875
	public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Custom;

	// Token: 0x040016F4 RID: 5876
	public float minDistance = 1f;

	// Token: 0x040016F5 RID: 5877
	public float maxDistance = 50f;

	// Token: 0x040016F6 RID: 5878
	public bool isReversing;

	// Token: 0x040016F7 RID: 5879
	public bool useRPMLimit = true;

	// Token: 0x040016F8 RID: 5880
	public bool enableReverseGear = true;

	// Token: 0x040016F9 RID: 5881
	[HideInInspector]
	public float carCurrentSpeed = 1f;

	// Token: 0x040016FA RID: 5882
	[HideInInspector]
	public float carMaxSpeed = 250f;

	// Token: 0x040016FB RID: 5883
	[HideInInspector]
	public bool isShifting;

	// Token: 0x040016FC RID: 5884
	public AudioClip idleClip;

	// Token: 0x040016FD RID: 5885
	public AnimationCurve idleVolCurve;

	// Token: 0x040016FE RID: 5886
	public AnimationCurve idlePitchCurve;

	// Token: 0x040016FF RID: 5887
	public AudioClip lowOffClip;

	// Token: 0x04001700 RID: 5888
	public AudioClip lowOnClip;

	// Token: 0x04001701 RID: 5889
	public AnimationCurve lowVolCurve;

	// Token: 0x04001702 RID: 5890
	public AnimationCurve lowPitchCurve;

	// Token: 0x04001703 RID: 5891
	public AudioClip medOffClip;

	// Token: 0x04001704 RID: 5892
	public AudioClip medOnClip;

	// Token: 0x04001705 RID: 5893
	public AnimationCurve medVolCurve;

	// Token: 0x04001706 RID: 5894
	public AnimationCurve medPitchCurve;

	// Token: 0x04001707 RID: 5895
	public AudioClip highOffClip;

	// Token: 0x04001708 RID: 5896
	public AudioClip highOnClip;

	// Token: 0x04001709 RID: 5897
	public AnimationCurve highVolCurve;

	// Token: 0x0400170A RID: 5898
	public AnimationCurve highPitchCurve;

	// Token: 0x0400170B RID: 5899
	public AudioClip maxRPMClip;

	// Token: 0x0400170C RID: 5900
	public AnimationCurve maxRPMVolCurve;

	// Token: 0x0400170D RID: 5901
	public AudioClip reversingClip;

	// Token: 0x0400170E RID: 5902
	public AnimationCurve reversingVolCurve;

	// Token: 0x0400170F RID: 5903
	public AnimationCurve reversingPitchCurve;

	// Token: 0x04001710 RID: 5904
	private AudioSource engineIdle;

	// Token: 0x04001711 RID: 5905
	private AudioSource lowOff;

	// Token: 0x04001712 RID: 5906
	private AudioSource lowOn;

	// Token: 0x04001713 RID: 5907
	private AudioSource medOff;

	// Token: 0x04001714 RID: 5908
	private AudioSource medOn;

	// Token: 0x04001715 RID: 5909
	private AudioSource highOff;

	// Token: 0x04001716 RID: 5910
	private AudioSource highOn;

	// Token: 0x04001717 RID: 5911
	private AudioSource maxRPM;

	// Token: 0x04001718 RID: 5912
	private AudioSource reversing;

	// Token: 0x04001719 RID: 5913
	private float clipsValue;

	// Token: 0x0400171A RID: 5914
	private float clipsValue2;

	// Token: 0x0400171B RID: 5915
	public Camera mainCamera;

	// Token: 0x0400171C RID: 5916
	[HideInInspector]
	public bool isCameraNear;

	// Token: 0x0400171D RID: 5917
	public RealisticEngineSound.EngineShake engineShakeSetting;

	// Token: 0x0400171E RID: 5918
	[HideInInspector]
	public RealisticEngineSound.ShakeLenghtType shakeLenghtSetting;

	// Token: 0x0400171F RID: 5919
	[HideInInspector]
	public float shakeLength = 50f;

	// Token: 0x04001720 RID: 5920
	[HideInInspector]
	public float shakeVolumeChange = 0.35f;

	// Token: 0x04001721 RID: 5921
	[HideInInspector]
	public float randomChance = 0.5f;

	// Token: 0x04001722 RID: 5922
	private float _endRange = 1f;

	// Token: 0x04001723 RID: 5923
	private float shakeVolumeChangeDetect;

	// Token: 0x04001724 RID: 5924
	private float _oscillateRange;

	// Token: 0x04001725 RID: 5925
	private float _oscillateOffset;

	// Token: 0x04001726 RID: 5926
	private float lenght;

	// Token: 0x04001727 RID: 5927
	private float randomShakingValue;

	// Token: 0x04001728 RID: 5928
	private float randomShakingValue2;

	// Token: 0x04001729 RID: 5929
	private WaitForSeconds _wait;

	// Token: 0x0400172A RID: 5930
	private bool alreadyDestroyed;

	// Token: 0x02000257 RID: 599
	public enum GasPedalValue
	{
		// Token: 0x0400172C RID: 5932
		Simulated,
		// Token: 0x0400172D RID: 5933
		NotSimulated
	}

	// Token: 0x02000258 RID: 600
	public enum EngineShake
	{
		// Token: 0x0400172F RID: 5935
		Off,
		// Token: 0x04001730 RID: 5936
		Random,
		// Token: 0x04001731 RID: 5937
		AllwaysOn
	}

	// Token: 0x02000259 RID: 601
	public enum ShakeLenghtType
	{
		// Token: 0x04001733 RID: 5939
		Fix,
		// Token: 0x04001734 RID: 5940
		Random
	}
}
