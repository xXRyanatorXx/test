using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200025D RID: 605
public class ShiftingSound : MonoBehaviour
{
	// Token: 0x06000E5A RID: 3674 RVA: 0x00099574 File Offset: 0x00097774
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

	// Token: 0x06000E5B RID: 3675 RVA: 0x000995E8 File Offset: 0x000977E8
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
					if (this.destroyAudioSources && this.shiftingSound != null && !this.shiftingSound.isPlaying)
					{
						UnityEngine.Object.Destroy(this.shiftingSound);
						return;
					}
				}
			}
			else
			{
				this.playOnce = 0;
				if (this.destroyAudioSources && this.shiftingSound != null && !this.shiftingSound.isPlaying)
				{
					UnityEngine.Object.Destroy(this.shiftingSound);
					return;
				}
			}
		}
		else if (this.shiftingSound != null)
		{
			UnityEngine.Object.Destroy(this.shiftingSound);
		}
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x000996DE File Offset: 0x000978DE
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x000996E6 File Offset: 0x000978E6
	private void OnDisable()
	{
		if (this.shiftingSound != null)
		{
			UnityEngine.Object.Destroy(this.shiftingSound);
		}
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x00099704 File Offset: 0x00097904
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
		this.shiftingSound.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		this.shiftingSound.loop = false;
		this.shiftingSound.clip = this.shiftingSoundClip;
		this.shiftingSound.Play();
	}

	// Token: 0x0400176A RID: 5994
	private RealisticEngineSound res;

	// Token: 0x0400176B RID: 5995
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x0400176C RID: 5996
	public AudioMixerGroup audioMixer;

	// Token: 0x0400176D RID: 5997
	private AudioMixerGroup _audioMixer;

	// Token: 0x0400176E RID: 5998
	public AudioClip shiftingSoundClip;

	// Token: 0x0400176F RID: 5999
	private AudioSource shiftingSound;

	// Token: 0x04001770 RID: 6000
	private int playOnce;

	// Token: 0x04001771 RID: 6001
	public bool destroyAudioSources;
}
