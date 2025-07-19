using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000260 RID: 608
public class ShiftingSound_mobile : MonoBehaviour
{
	// Token: 0x06000E6D RID: 3693 RVA: 0x00099B1C File Offset: 0x00097D1C
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

	// Token: 0x06000E6E RID: 3694 RVA: 0x00099B90 File Offset: 0x00097D90
	private void Update()
	{
		if (this.res.enabled)
		{
			if (this.res.isCameraNear)
			{
				if (this.res.isShifting)
				{
					if (this.playOnce == 0)
					{
						if (this.shiftingSound == null)
						{
							this.CreateShiftSound();
						}
						else
						{
							this.shiftingSound.PlayOneShot(this.shiftingSoundClip);
						}
						this.playOnce = 1;
						return;
					}
				}
				else
				{
					this.playOnce = 0;
					if (this.shiftingSound != null && !this.shiftingSound.isPlaying)
					{
						if (this.destroyAudioSources)
						{
							UnityEngine.Object.Destroy(this.shiftingSound);
							return;
						}
						this.shiftingSound.Stop();
						return;
					}
				}
			}
			else
			{
				this.playOnce = 0;
				if (this.shiftingSound != null && !this.shiftingSound.isPlaying)
				{
					if (this.destroyAudioSources)
					{
						UnityEngine.Object.Destroy(this.shiftingSound);
						return;
					}
					this.shiftingSound.Stop();
					return;
				}
			}
		}
		else if (this.shiftingSound != null && !this.shiftingSound.isPlaying)
		{
			UnityEngine.Object.Destroy(this.shiftingSound);
		}
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x00099CB4 File Offset: 0x00097EB4
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00099CBC File Offset: 0x00097EBC
	private void OnDisable()
	{
		if (this.shiftingSound != null)
		{
			UnityEngine.Object.Destroy(this.shiftingSound);
		}
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x00099CD8 File Offset: 0x00097ED8
	private void CreateShiftSound()
	{
		this.shiftingSound = base.gameObject.AddComponent<AudioSource>();
		this.shiftingSound.rolloffMode = this.res.audioRolloffMode;
		this.shiftingSound.minDistance = this.res.minDistance;
		this.shiftingSound.maxDistance = this.res.maxDistance;
		this.shiftingSound.spatialBlend = this.res.spatialBlend;
		this.shiftingSound.dopplerLevel = this.res.dopplerLevel;
		this.shiftingSound.volume = this.masterVolume;
		if (this._audioMixer != null)
		{
			this.shiftingSound.outputAudioMixerGroup = this._audioMixer;
		}
		this.shiftingSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		this.shiftingSound.loop = false;
		this.shiftingSound.clip = this.shiftingSoundClip;
		this.shiftingSound.Play();
	}

	// Token: 0x0400177E RID: 6014
	private RealisticEngineSound_mobile res;

	// Token: 0x0400177F RID: 6015
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x04001780 RID: 6016
	public AudioMixerGroup audioMixer;

	// Token: 0x04001781 RID: 6017
	private AudioMixerGroup _audioMixer;

	// Token: 0x04001782 RID: 6018
	public AudioClip shiftingSoundClip;

	// Token: 0x04001783 RID: 6019
	public bool destroyAudioSources;

	// Token: 0x04001784 RID: 6020
	private AudioSource shiftingSound;

	// Token: 0x04001785 RID: 6021
	private int playOnce;
}
