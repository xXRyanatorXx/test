using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class WheelsFXAI : MonoBehaviour
{
	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000090 RID: 144 RVA: 0x0000783B File Offset: 0x00005A3B
	// (set) Token: 0x06000091 RID: 145 RVA: 0x00007843 File Offset: 0x00005A43
	public bool skidding { get; private set; }

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000092 RID: 146 RVA: 0x0000784C File Offset: 0x00005A4C
	// (set) Token: 0x06000093 RID: 147 RVA: 0x00007854 File Offset: 0x00005A54
	public bool playingAudio { get; private set; }

	// Token: 0x06000094 RID: 148 RVA: 0x00007860 File Offset: 0x00005A60
	private void Start()
	{
		this.skidParticles = base.transform.parent.parent.GetComponentInChildren<ParticleSystem>();
		this.skidTrailPrefab = Resources.Load<Transform>("SkidTrail");
		this.ACAI = base.transform.parent.parent.GetComponent<AnyCarAI>();
		if (this.skidParticles == null)
		{
			Debug.LogWarning(" no particle system found on car to generate smoke particles", base.gameObject);
		}
		else if (!this.ACAI.smokeOn)
		{
			this.skidParticles.Stop();
		}
		this.wheelCollider = base.transform.GetComponent<WheelCollider>();
		if (WheelsFXAI.skidTrailsDetachedParent == null)
		{
			WheelsFXAI.skidTrailsDetachedParent = new GameObject("TemporarySkids").transform;
		}
		this.skidAudio = this.ACAI.skidSound;
		this.skidAudioVolume = this.ACAI.skidVolume;
		if (this.ACAI.skidSource == null)
		{
			this.SetUpSkidAudioSource(this.skidAudio);
		}
		else
		{
			this.skidAudioSource = this.ACAI.skidSource;
		}
		this.playingAudio = false;
		this.suspensionsSound = this.ACAI.suspensionsSound;
		this.suspensionsAudioVolume = this.ACAI.suspensionsVolume;
		if (this.ACAI.suspensionsSource == null)
		{
			this.SetUpSuspensionsAudioSource(this.suspensionsSound);
			return;
		}
		this.suspensionsAudioSource = this.ACAI.suspensionsSource;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x000079D0 File Offset: 0x00005BD0
	private void Update()
	{
		WheelHit wheelHit;
		base.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
		if (wheelHit.normal != this.wheelHit.normal)
		{
			if (this.gameStarted)
			{
				this.suspensionSoundOn = true;
			}
			else
			{
				this.gameStarted = true;
			}
		}
		else
		{
			this.suspensionSoundOn = false;
		}
		this.wheelHit.normal = wheelHit.normal;
		this.SuspensionsSound();
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00007A40 File Offset: 0x00005C40
	public void EmitTyreSmoke()
	{
		this.skidParticles.transform.position = base.transform.position - base.transform.up * this.wheelCollider.radius;
		this.skidParticles.Emit(1);
		if (!this.skidding)
		{
			base.StartCoroutine(this.StartSkidTrail());
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00007AA9 File Offset: 0x00005CA9
	public IEnumerator StartSkidTrail()
	{
		this.skidTrail = UnityEngine.Object.Instantiate<Transform>(this.skidTrailPrefab);
		this.skidding = true;
		while (this.skidTrail == null)
		{
			yield return null;
		}
		this.skidTrail.parent = base.gameObject.transform;
		this.skidTrail.localPosition = -Vector3.up * this.wheelCollider.radius;
		yield break;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00007AB8 File Offset: 0x00005CB8
	public void EndSkidTrail()
	{
		if (!this.skidding)
		{
			return;
		}
		this.skidding = false;
		this.skidTrail.parent = WheelsFXAI.skidTrailsDetachedParent;
		UnityEngine.Object.Destroy(this.skidTrail.gameObject, 15f);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00007AEF File Offset: 0x00005CEF
	public void PlayAudio()
	{
		this.skidAudioSource.Play();
		this.playingAudio = true;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00007B03 File Offset: 0x00005D03
	public void StopAudio()
	{
		this.skidAudioSource.Stop();
		this.playingAudio = false;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00007B17 File Offset: 0x00005D17
	private void SuspensionsSound()
	{
		if (this.suspensionSoundOn && !this.suspensionsAudioSource.isPlaying)
		{
			this.suspensionsAudioSource.PlayOneShot(this.suspensionsSound);
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00007B40 File Offset: 0x00005D40
	private AudioSource SetUpSkidAudioSource(AudioClip clip)
	{
		if (this.ACAI.skidSource == null)
		{
			this.skidAudioSource = base.transform.parent.gameObject.AddComponent<AudioSource>();
			this.ACAI.skidSource = this.skidAudioSource;
		}
		else
		{
			this.skidAudioSource = this.ACAI.skidSource;
		}
		this.skidAudioSource.clip = clip;
		this.skidAudioSource.volume = this.skidAudioVolume;
		this.skidAudioSource.loop = false;
		this.skidAudioSource.pitch = 1f;
		this.skidAudioSource.playOnAwake = false;
		this.skidAudioSource.minDistance = 5f;
		this.skidAudioSource.reverbZoneMix = 1.5f;
		this.skidAudioSource.maxDistance = 600f;
		this.skidAudioSource.dopplerLevel = 2f;
		return this.skidAudioSource;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00007C2C File Offset: 0x00005E2C
	private AudioSource SetUpSuspensionsAudioSource(AudioClip clip)
	{
		if (this.ACAI.suspensionsSource == null)
		{
			this.suspensionsAudioSource = base.transform.parent.gameObject.AddComponent<AudioSource>();
			this.ACAI.suspensionsSource = this.suspensionsAudioSource;
		}
		else
		{
			this.suspensionsAudioSource = this.ACAI.suspensionsSource;
		}
		this.suspensionsAudioSource.clip = clip;
		this.suspensionsAudioSource.volume = this.suspensionsAudioVolume;
		this.suspensionsAudioSource.loop = false;
		this.suspensionsAudioSource.pitch = 1f;
		this.suspensionsAudioSource.playOnAwake = false;
		this.suspensionsAudioSource.minDistance = 5f;
		this.suspensionsAudioSource.reverbZoneMix = 1.5f;
		this.suspensionsAudioSource.maxDistance = 600f;
		this.suspensionsAudioSource.dopplerLevel = 2f;
		return this.suspensionsAudioSource;
	}

	// Token: 0x04000117 RID: 279
	private AudioClip skidAudio;

	// Token: 0x04000118 RID: 280
	private AudioSource skidAudioSource;

	// Token: 0x04000119 RID: 281
	private float skidAudioVolume;

	// Token: 0x0400011A RID: 282
	private AudioClip suspensionsSound;

	// Token: 0x0400011B RID: 283
	private AudioSource suspensionsAudioSource;

	// Token: 0x0400011C RID: 284
	private float suspensionsAudioVolume;

	// Token: 0x0400011D RID: 285
	private WheelHit wheelHit;

	// Token: 0x0400011E RID: 286
	private bool gameStarted;

	// Token: 0x0400011F RID: 287
	private bool suspensionSoundOn;

	// Token: 0x04000120 RID: 288
	private ParticleSystem skidParticles;

	// Token: 0x04000121 RID: 289
	private static Transform skidTrailsDetachedParent;

	// Token: 0x04000122 RID: 290
	private Transform skidTrail;

	// Token: 0x04000123 RID: 291
	private Transform skidTrailPrefab;

	// Token: 0x04000124 RID: 292
	private WheelCollider wheelCollider;

	// Token: 0x04000125 RID: 293
	private AnyCarAI ACAI;
}
