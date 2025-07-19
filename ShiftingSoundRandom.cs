using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200025E RID: 606
public class ShiftingSoundRandom : MonoBehaviour
{
	// Token: 0x06000E60 RID: 3680 RVA: 0x00099818 File Offset: 0x00097A18
	private void Start()
	{
		this.res = base.gameObject.transform.parent.GetComponent<RealisticEngineSound>();
		this._playtime = new WaitForSeconds(0.05f);
		if (this.audioMixer != null)
		{
			this._audioMixer = this.audioMixer;
		}
		if (this.audioMixer == null && this.res.audioMixer != null)
		{
			this._audioMixer = this.res.audioMixer;
			this.audioMixer = this._audioMixer;
		}
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
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x00099958 File Offset: 0x00097B58
	private void Update()
	{
		if (this.res.enabled)
		{
			if (this.res.isCameraNear)
			{
				if (!this.res.isShifting)
				{
					this.playOnce = 0;
					return;
				}
				if (this.playOnce == 0)
				{
					this.PlayShiftSound();
					this.playOnce = 1;
					return;
				}
			}
			else if (this.shiftingSound != null)
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
		else
		{
			UnityEngine.Object.Destroy(this.shiftingSound);
		}
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x000999E3 File Offset: 0x00097BE3
	private void OnDisable()
	{
		if (this.shiftingSound != null)
		{
			UnityEngine.Object.Destroy(this.shiftingSound);
		}
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x000999FE File Offset: 0x00097BFE
	private void OnEnable()
	{
		base.StartCoroutine(this.WaitForStart());
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x00099A0D File Offset: 0x00097C0D
	private IEnumerator WaitForStart()
	{
		yield return this._playtime;
		if (this.shiftingSound == null)
		{
			this.Start();
		}
		yield break;
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00099A1C File Offset: 0x00097C1C
	private void PlayShiftSound()
	{
		if (this.shiftingSound != null)
		{
			this.shiftingSound.clip = this.shiftingSoundClips[UnityEngine.Random.Range(0, this.shiftingSoundClips.Length)];
			this.shiftingSound.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
			this.shiftingSound.loop = false;
			this.shiftingSound.Play();
			return;
		}
		base.StartCoroutine(this.WaitForStart());
	}

	// Token: 0x04001772 RID: 6002
	private RealisticEngineSound res;

	// Token: 0x04001773 RID: 6003
	[Range(0.1f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x04001774 RID: 6004
	public AudioMixerGroup audioMixer;

	// Token: 0x04001775 RID: 6005
	private AudioMixerGroup _audioMixer;

	// Token: 0x04001776 RID: 6006
	public AudioClip[] shiftingSoundClips;

	// Token: 0x04001777 RID: 6007
	public bool destroyAudioSources;

	// Token: 0x04001778 RID: 6008
	private AudioSource shiftingSound;

	// Token: 0x04001779 RID: 6009
	private int playOnce;

	// Token: 0x0400177A RID: 6010
	private WaitForSeconds _playtime;
}
