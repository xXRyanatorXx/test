using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class EnviroAudioSource : MonoBehaviour
{
	// Token: 0x0600038E RID: 910 RVA: 0x000182C4 File Offset: 0x000164C4
	private void Start()
	{
		if (EnviroSkyMgr.instance == null)
		{
			Debug.Log("EnviroSky Manager not found. Deactivate enviro AudioSource");
			base.enabled = false;
			return;
		}
		if (this.audiosrc == null)
		{
			this.audiosrc = base.GetComponent<AudioSource>();
		}
		if (this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather1 || this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather2)
		{
			this.audiosrc.loop = true;
			this.audiosrc.volume = 0f;
		}
		this.currentAmbientVolume = EnviroSkyMgr.instance.ambientAudioVolume;
		this.currentWeatherVolume = EnviroSkyMgr.instance.weatherAudioVolume;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00018357 File Offset: 0x00016557
	public void FadeOut()
	{
		this.isFadingOut = true;
		this.isFadingIn = false;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00018367 File Offset: 0x00016567
	public void FadeIn(AudioClip clip)
	{
		this.isFadingIn = true;
		this.isFadingOut = false;
		this.audiosrc.clip = clip;
		this.audiosrc.Play();
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001838E File Offset: 0x0001658E
	public void PlayOneShot(AudioClip clip)
	{
		this.audiosrc.loop = false;
		this.audiosrc.clip = clip;
		this.audiosrc.Play();
	}

	// Token: 0x06000392 RID: 914 RVA: 0x000183B4 File Offset: 0x000165B4
	private void Update()
	{
		if (EnviroSkyMgr.instance == null && !EnviroSkyMgr.instance.IsStarted())
		{
			return;
		}
		this.currentAmbientVolume = Mathf.Lerp(this.currentAmbientVolume, EnviroSkyMgr.instance.ambientAudioVolume + EnviroSkyMgr.instance.ambientAudioVolumeModifier, 10f * Time.deltaTime);
		this.currentWeatherVolume = Mathf.Lerp(this.currentWeatherVolume, EnviroSkyMgr.instance.weatherAudioVolume + EnviroSkyMgr.instance.weatherAudioVolumeModifier, 10f * Time.deltaTime);
		if (this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather1 || this.myFunction == EnviroAudioSource.AudioSourceFunction.Weather2 || this.myFunction == EnviroAudioSource.AudioSourceFunction.Thunder)
		{
			if (this.isFadingIn && this.audiosrc.volume < this.currentWeatherVolume)
			{
				this.audiosrc.volume += EnviroSkyMgr.instance.audioTransitionSpeed * Time.deltaTime;
			}
			else if (this.isFadingIn && this.audiosrc.volume >= this.currentWeatherVolume - 0.01f)
			{
				this.isFadingIn = false;
			}
			if (this.isFadingOut && this.audiosrc.volume > 0f)
			{
				this.audiosrc.volume -= EnviroSkyMgr.instance.audioTransitionSpeed * Time.deltaTime;
			}
			else if (this.isFadingOut && this.audiosrc.volume == 0f)
			{
				this.audiosrc.Stop();
				this.isFadingOut = false;
			}
			if (this.audiosrc.isPlaying && !this.isFadingOut && !this.isFadingIn)
			{
				this.audiosrc.volume = this.currentWeatherVolume;
				return;
			}
		}
		else if (this.myFunction == EnviroAudioSource.AudioSourceFunction.Ambient || this.myFunction == EnviroAudioSource.AudioSourceFunction.Ambient2)
		{
			if (this.isFadingIn && this.audiosrc.volume < this.currentAmbientVolume)
			{
				this.audiosrc.volume += EnviroSkyMgr.instance.audioTransitionSpeed * Time.deltaTime;
			}
			else if (this.isFadingIn && this.audiosrc.volume >= this.currentAmbientVolume - 0.01f)
			{
				this.isFadingIn = false;
			}
			if (this.isFadingOut && this.audiosrc.volume > 0f)
			{
				this.audiosrc.volume -= EnviroSkyMgr.instance.audioTransitionSpeed * Time.deltaTime;
			}
			else if (this.isFadingOut && this.audiosrc.volume == 0f)
			{
				this.audiosrc.Stop();
				this.isFadingOut = false;
			}
			if (this.audiosrc.isPlaying && !this.isFadingOut && !this.isFadingIn)
			{
				this.audiosrc.volume = this.currentAmbientVolume;
				return;
			}
		}
		else if (this.myFunction == EnviroAudioSource.AudioSourceFunction.ZoneAmbient)
		{
			if (this.isFadingIn && this.audiosrc.volume < EnviroSkyMgr.instance.interiorZoneAudioVolume)
			{
				this.audiosrc.volume += EnviroSkyMgr.instance.interiorZoneAudioFadingSpeed * Time.deltaTime;
			}
			else if (this.isFadingIn && this.audiosrc.volume >= EnviroSkyMgr.instance.interiorZoneAudioVolume - 0.01f)
			{
				this.isFadingIn = false;
			}
			if (this.isFadingOut && this.audiosrc.volume > 0f)
			{
				this.audiosrc.volume -= EnviroSkyMgr.instance.interiorZoneAudioFadingSpeed * Time.deltaTime;
			}
			else if (this.isFadingOut && this.audiosrc.volume == 0f)
			{
				this.audiosrc.Stop();
				this.isFadingOut = false;
			}
			if (this.audiosrc.isPlaying && !this.isFadingOut && !this.isFadingIn)
			{
				this.audiosrc.volume = EnviroSkyMgr.instance.interiorZoneAudioVolume;
			}
		}
	}

	// Token: 0x0400051F RID: 1311
	public EnviroAudioSource.AudioSourceFunction myFunction;

	// Token: 0x04000520 RID: 1312
	public AudioSource audiosrc;

	// Token: 0x04000521 RID: 1313
	public bool isFadingIn;

	// Token: 0x04000522 RID: 1314
	public bool isFadingOut;

	// Token: 0x04000523 RID: 1315
	private float currentAmbientVolume;

	// Token: 0x04000524 RID: 1316
	private float currentWeatherVolume;

	// Token: 0x04000525 RID: 1317
	private float currentZoneVolume;

	// Token: 0x020000B5 RID: 181
	public enum AudioSourceFunction
	{
		// Token: 0x04000527 RID: 1319
		Weather1,
		// Token: 0x04000528 RID: 1320
		Weather2,
		// Token: 0x04000529 RID: 1321
		Ambient,
		// Token: 0x0400052A RID: 1322
		Ambient2,
		// Token: 0x0400052B RID: 1323
		Thunder,
		// Token: 0x0400052C RID: 1324
		ZoneAmbient
	}
}
