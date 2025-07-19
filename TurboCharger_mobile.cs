using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000265 RID: 613
public class TurboCharger_mobile : MonoBehaviour
{
	// Token: 0x06000E97 RID: 3735 RVA: 0x0009B068 File Offset: 0x00099268
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
		this._playtime = new WaitForSeconds(0.15f);
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x0009B0EC File Offset: 0x000992EC
	private void Update()
	{
		if (this.res.enabled)
		{
			if (this.res.isCameraNear)
			{
				this.clipsValue = this.res.engineCurrentRPM / this.res.maxRPMLimit;
				if (this.res.gasPedalPressing)
				{
					this.oneShotController = 1;
					if (this.res.maxRPMVolCurve.Evaluate(this.clipsValue) * this.masterVolume < 0.5f)
					{
						if (this.turboLoop == null)
						{
							this.CreateTurboLoop();
						}
						else if (!this.turboLoop.isPlaying)
						{
							this.turboLoop.Play();
						}
						this.turboLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
						this.turboLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
						if (this.maxTurboLoop != null)
						{
							if (this.destroyAudioSources)
							{
								UnityEngine.Object.Destroy(this.maxTurboLoop);
							}
							else
							{
								this.maxTurboLoop.Stop();
							}
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
							this.maxTurboLoop.volume = this.chargerVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
							this.maxTurboLoop.pitch = this.chargerPitchCurve.Evaluate(this.clipsValue);
						}
						if (this.turboLoop != null)
						{
							if (this.destroyAudioSources)
							{
								UnityEngine.Object.Destroy(this.turboLoop);
							}
							else
							{
								this.turboLoop.Stop();
							}
						}
					}
				}
				else
				{
					if (this.turboLoop != null)
					{
						UnityEngine.Object.Destroy(this.turboLoop);
					}
					if (this.maxTurboLoop != null)
					{
						UnityEngine.Object.Destroy(this.maxTurboLoop);
					}
					if (this.oneShotController == 1)
					{
						if (this.oneShot == null)
						{
							this.CreateOneShot();
						}
						else
						{
							this.PlayOneShot();
						}
						this.oneShotController = 0;
					}
				}
				if (this.oneShot != null && !this.oneShot.isPlaying && this.destroyAudioSources)
				{
					UnityEngine.Object.Destroy(this.oneShot);
					return;
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

	// Token: 0x06000E99 RID: 3737 RVA: 0x0009B35B File Offset: 0x0009955B
	private void OnDisable()
	{
		this.DestroyAll();
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x0009B364 File Offset: 0x00099564
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

	// Token: 0x06000E9B RID: 3739 RVA: 0x0009B3BC File Offset: 0x000995BC
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

	// Token: 0x06000E9C RID: 3740 RVA: 0x0009B414 File Offset: 0x00099614
	private void OnEnable()
	{
		base.StartCoroutine(this.WaitForStart());
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x0009B423 File Offset: 0x00099623
	private IEnumerator WaitForStart()
	{
		yield return this._playtime;
		if (this.oneShot == null)
		{
			this.Start();
		}
		yield break;
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x0009B434 File Offset: 0x00099634
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

	// Token: 0x06000E9F RID: 3743 RVA: 0x0009B558 File Offset: 0x00099758
	private void CreateOneShot()
	{
		this.oneShot = base.gameObject.AddComponent<AudioSource>();
		this.oneShot.rolloffMode = this.res.audioRolloffMode;
		this.oneShot.spatialBlend = this.res.spatialBlend;
		this.oneShot.volume = this.oneShotVolCurve.Evaluate(this.clipsValue) * this.masterVolume;
		this.oneShot.pitch = this.oneShotPitchCurve.Evaluate(this.clipsValue) * UnityEngine.Random.Range(0.85f, 1.2f);
		this.oneShot.minDistance = this.res.minDistance;
		this.oneShot.maxDistance = this.res.maxDistance;
		this.oneShot.loop = false;
		if (this._audioMixer != null)
		{
			this.oneShot.outputAudioMixerGroup = this._audioMixer;
		}
		this.oneShot.Stop();
		if (this.clipsValue > this.longShotTreshold)
		{
			this.oneShot.clip = this.longShotClip;
		}
		else
		{
			this.oneShot.clip = this.shortShotClip;
		}
		this.oneShot.Play();
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0009B690 File Offset: 0x00099890
	private void PlayOneShot()
	{
		if (this.clipsValue > this.longShotTreshold)
		{
			this.oneShot.clip = this.longShotClip;
		}
		else
		{
			this.oneShot.clip = this.shortShotClip;
		}
		this.oneShot.Play();
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0009B6D0 File Offset: 0x000998D0
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

	// Token: 0x040017B5 RID: 6069
	private RealisticEngineSound_mobile res;

	// Token: 0x040017B6 RID: 6070
	private float clipsValue;

	// Token: 0x040017B7 RID: 6071
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x040017B8 RID: 6072
	public AudioMixerGroup audioMixer;

	// Token: 0x040017B9 RID: 6073
	private AudioMixerGroup _audioMixer;

	// Token: 0x040017BA RID: 6074
	public AudioClip turboLoopClip;

	// Token: 0x040017BB RID: 6075
	public AudioClip maxRpmLoopClip;

	// Token: 0x040017BC RID: 6076
	public AnimationCurve chargerVolCurve;

	// Token: 0x040017BD RID: 6077
	public AnimationCurve chargerPitchCurve;

	// Token: 0x040017BE RID: 6078
	[Range(0.4f, 1f)]
	public float longShotTreshold = 0.8f;

	// Token: 0x040017BF RID: 6079
	public AudioClip longShotClip;

	// Token: 0x040017C0 RID: 6080
	public AudioClip shortShotClip;

	// Token: 0x040017C1 RID: 6081
	public AnimationCurve oneShotVolCurve;

	// Token: 0x040017C2 RID: 6082
	public AnimationCurve oneShotPitchCurve;

	// Token: 0x040017C3 RID: 6083
	public bool destroyAudioSources;

	// Token: 0x040017C4 RID: 6084
	private AudioSource turboLoop;

	// Token: 0x040017C5 RID: 6085
	private AudioSource oneShot;

	// Token: 0x040017C6 RID: 6086
	private AudioSource maxTurboLoop;

	// Token: 0x040017C7 RID: 6087
	private int oneShotController;

	// Token: 0x040017C8 RID: 6088
	private WaitForSeconds _playtime;
}
