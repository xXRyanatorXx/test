using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000B8 RID: 184
[AddComponentMenu("Enviro/Interior Zone")]
public class EnviroInterior : MonoBehaviour
{
	// Token: 0x0600039D RID: 925 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x0600039E RID: 926 RVA: 0x00018984 File Offset: 0x00016B84
	public void CreateNewTrigger()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Trigger " + this.triggers.Count.ToString();
		gameObject.transform.SetParent(base.transform, false);
		gameObject.AddComponent<BoxCollider>().isTrigger = true;
		EnviroTrigger enviroTrigger = gameObject.AddComponent<EnviroTrigger>();
		enviroTrigger.myZone = this;
		enviroTrigger.name = gameObject.name;
		this.triggers.Add(enviroTrigger);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000189FE File Offset: 0x00016BFE
	public void RemoveTrigger(EnviroTrigger id)
	{
		UnityEngine.Object.DestroyImmediate(id.gameObject);
		this.triggers.Remove(id);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00018A18 File Offset: 0x00016C18
	public void Enter()
	{
		EnviroSkyMgr.instance.interiorMode = true;
		EnviroSkyMgr.instance.lastInteriorZone = this;
		if (this.directLighting)
		{
			this.fadeOutDirectLight = false;
			this.fadeInDirectLight = true;
		}
		if (this.ambientLighting)
		{
			this.fadeOutAmbientLight = false;
			this.fadeInAmbientLight = true;
		}
		if (this.skybox)
		{
			this.fadeOutSkybox = false;
			this.fadeInSkybox = true;
		}
		if (this.ambientAudio)
		{
			EnviroSkyMgr.instance.ambientAudioVolumeModifier = this.ambientVolume;
		}
		if (this.weatherAudio)
		{
			EnviroSkyMgr.instance.weatherAudioVolumeModifier = this.weatherVolume;
		}
		if (this.zoneAudioClip != null)
		{
			EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorZoneAudioFadingSpeed = this.zoneAudioFadingSpeed;
			EnviroSkyMgr.instance.AudioSettings.AudioSourceZone.FadeIn(this.zoneAudioClip);
			EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorZoneAudioVolume = this.zoneAudioVolume;
		}
		if (this.fog)
		{
			this.fadeOutFog = false;
			this.fadeInFog = true;
		}
		if (this.fogColor)
		{
			this.fadeOutFogColor = false;
			this.fadeInFogColor = true;
		}
		if (this.weatherEffects)
		{
			this.fadeOutWeather = false;
			this.fadeInWeather = true;
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00018B44 File Offset: 0x00016D44
	public void Exit()
	{
		EnviroSkyMgr.instance.interiorMode = false;
		if (this.directLighting)
		{
			this.fadeInDirectLight = false;
			this.fadeOutDirectLight = true;
		}
		if (this.ambientLighting)
		{
			this.fadeOutAmbientLight = true;
			this.fadeInAmbientLight = false;
		}
		if (this.skybox)
		{
			this.fadeOutSkybox = true;
			this.fadeInSkybox = false;
		}
		if (this.ambientAudio)
		{
			EnviroSkyMgr.instance.ambientAudioVolumeModifier = 0f;
		}
		if (this.weatherAudio)
		{
			EnviroSkyMgr.instance.weatherAudioVolumeModifier = 0f;
		}
		if (this.zoneAudioClip != null)
		{
			EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorZoneAudioFadingSpeed = this.zoneAudioFadingSpeed;
			EnviroSkyMgr.instance.AudioSettings.AudioSourceZone.FadeOut();
		}
		if (this.fog)
		{
			this.fadeOutFog = true;
			this.fadeInFog = false;
		}
		if (this.fogColor)
		{
			this.fadeOutFogColor = true;
			this.fadeInFogColor = false;
		}
		if (this.weatherEffects)
		{
			this.fadeOutWeather = true;
			this.fadeInWeather = false;
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00018C48 File Offset: 0x00016E48
	public void StopAllFading()
	{
		if (this.directLighting)
		{
			this.fadeInDirectLight = false;
			this.fadeOutDirectLight = false;
		}
		if (this.ambientLighting)
		{
			this.fadeOutAmbientLight = false;
			this.fadeInAmbientLight = false;
		}
		if (this.zoneAudioClip != null)
		{
			EnviroSkyMgr.instance.AudioSettings.AudioSourceZone.FadeOut();
		}
		if (this.skybox)
		{
			this.fadeOutSkybox = false;
			this.fadeInSkybox = false;
		}
		if (this.fog)
		{
			this.fadeOutFog = false;
			this.fadeInFog = false;
		}
		if (this.fogColor)
		{
			this.fadeOutFogColor = false;
			this.fadeInFogColor = false;
		}
		if (this.weatherEffects)
		{
			this.fadeOutWeather = false;
			this.fadeInWeather = false;
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00018CFC File Offset: 0x00016EFC
	private void Update()
	{
		if (EnviroSkyMgr.instance == null || !EnviroSkyMgr.instance.IsAvailable())
		{
			return;
		}
		if (this.directLighting)
		{
			if (this.fadeInDirectLight)
			{
				this.curDirectLightingMod = Color.Lerp(this.curDirectLightingMod, this.directLightingMod, this.directLightFadeSpeed * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorDirectLightMod = this.curDirectLightingMod;
				if (this.curDirectLightingMod == this.directLightingMod)
				{
					this.fadeInDirectLight = false;
				}
			}
			else if (this.fadeOutDirectLight)
			{
				this.curDirectLightingMod = Color.Lerp(this.curDirectLightingMod, this.fadeOutColor, this.directLightFadeSpeed * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorDirectLightMod = this.curDirectLightingMod;
				if (this.curDirectLightingMod == this.fadeOutColor)
				{
					this.fadeOutDirectLight = false;
				}
			}
		}
		if (this.ambientLighting)
		{
			if (this.fadeInAmbientLight)
			{
				this.curAmbientLightingMod = Color.Lerp(this.curAmbientLightingMod, this.ambientLightingMod, this.ambientLightFadeSpeed * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorAmbientLightMod = this.curAmbientLightingMod;
				if (EnviroSkyMgr.instance.LightSettings.ambientMode == AmbientMode.Trilight)
				{
					this.curAmbientEQLightingMod = Color.Lerp(this.curAmbientEQLightingMod, this.ambientEQLightingMod, this.ambientLightFadeSpeed * Time.deltaTime);
					EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorAmbientEQLightMod = this.curAmbientEQLightingMod;
					this.curAmbientGRLightingMod = Color.Lerp(this.curAmbientGRLightingMod, this.ambientGRLightingMod, this.ambientLightFadeSpeed * Time.deltaTime);
					EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorAmbientGRLightMod = this.curAmbientGRLightingMod;
				}
				if (this.curAmbientLightingMod == this.ambientLightingMod)
				{
					this.fadeInAmbientLight = false;
				}
			}
			else if (this.fadeOutAmbientLight)
			{
				this.curAmbientLightingMod = Color.Lerp(this.curAmbientLightingMod, this.fadeOutColor, 2f * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorAmbientLightMod = this.curAmbientLightingMod;
				if (EnviroSkyMgr.instance.LightSettings.ambientMode == AmbientMode.Trilight)
				{
					this.curAmbientEQLightingMod = Color.Lerp(this.curAmbientEQLightingMod, this.fadeOutColor, 2f * Time.deltaTime);
					EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorAmbientEQLightMod = this.curAmbientEQLightingMod;
					this.curAmbientGRLightingMod = Color.Lerp(this.curAmbientGRLightingMod, this.fadeOutColor, 2f * Time.deltaTime);
					EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorAmbientGRLightMod = this.curAmbientGRLightingMod;
				}
				if (this.curAmbientLightingMod == this.fadeOutColor)
				{
					this.fadeOutAmbientLight = false;
				}
			}
		}
		if (this.skybox)
		{
			if (this.fadeInSkybox)
			{
				this.curskyboxColorMod = Color.Lerp(this.curskyboxColorMod, this.skyboxColorMod, this.skyboxFadeSpeed * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorSkyboxMod = this.curskyboxColorMod;
				if (this.curskyboxColorMod == this.skyboxColorMod)
				{
					this.fadeInSkybox = false;
				}
			}
			else if (this.fadeOutSkybox)
			{
				this.curskyboxColorMod = Color.Lerp(this.curskyboxColorMod, this.fadeOutColor, this.skyboxFadeSpeed * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorSkyboxMod = this.curskyboxColorMod;
				if (this.curskyboxColorMod == this.fadeOutColor)
				{
					this.fadeOutSkybox = false;
				}
			}
		}
		if (this.fog)
		{
			if (this.fadeInFog)
			{
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogMod = Mathf.Lerp(EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogMod, this.minFogMod, this.fogFadeSpeed * Time.deltaTime);
				if ((double)EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogMod <= (double)this.minFogMod + 0.001)
				{
					this.fadeInFog = false;
				}
			}
			else if (this.fadeOutFog)
			{
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogMod = Mathf.Lerp(EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogMod, 1f, this.fogFadeSpeed * 2f * Time.deltaTime);
				if ((double)EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogMod >= 0.999)
				{
					this.fadeOutFog = false;
				}
			}
		}
		if (this.fogColor)
		{
			if (this.fadeInFogColor)
			{
				this.curFogColorMod = Color.Lerp(this.curFogColorMod, this.fogColorMod, this.fogFadeSpeed * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogColorMod = this.curFogColorMod;
				if (this.curFogColorMod == this.fogColorMod)
				{
					this.fadeInFogColor = false;
				}
			}
			else if (this.fadeOutFogColor)
			{
				this.curFogColorMod = Color.Lerp(this.curFogColorMod, this.fadeOutColor, this.fogFadeSpeed * Time.deltaTime);
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorFogColorMod = this.curFogColorMod;
				if (this.curFogColorMod == this.fadeOutColor)
				{
					this.fadeOutFogColor = false;
				}
			}
		}
		if (this.weatherEffects)
		{
			if (this.fadeInWeather)
			{
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod = Mathf.Lerp(EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod, 0f, this.weatherFadeSpeed * Time.deltaTime);
				if ((double)EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod <= 0.001)
				{
					this.fadeInWeather = false;
					return;
				}
			}
			else if (this.fadeOutWeather)
			{
				EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod = Mathf.Lerp(EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod, 1f, this.weatherFadeSpeed * 2f * Time.deltaTime);
				if ((double)EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod >= 0.999)
				{
					this.fadeOutWeather = false;
				}
			}
		}
	}

	// Token: 0x04000530 RID: 1328
	public EnviroInterior.ZoneTriggerType zoneTriggerType;

	// Token: 0x04000531 RID: 1329
	public bool directLighting;

	// Token: 0x04000532 RID: 1330
	public bool ambientLighting;

	// Token: 0x04000533 RID: 1331
	public bool weatherAudio;

	// Token: 0x04000534 RID: 1332
	public bool ambientAudio;

	// Token: 0x04000535 RID: 1333
	public bool fog;

	// Token: 0x04000536 RID: 1334
	public bool fogColor;

	// Token: 0x04000537 RID: 1335
	public bool skybox;

	// Token: 0x04000538 RID: 1336
	public bool weatherEffects;

	// Token: 0x04000539 RID: 1337
	public Color directLightingMod = Color.black;

	// Token: 0x0400053A RID: 1338
	public Color ambientLightingMod = Color.black;

	// Token: 0x0400053B RID: 1339
	public Color ambientEQLightingMod = Color.black;

	// Token: 0x0400053C RID: 1340
	public Color ambientGRLightingMod = Color.black;

	// Token: 0x0400053D RID: 1341
	private Color curDirectLightingMod;

	// Token: 0x0400053E RID: 1342
	private Color curAmbientLightingMod;

	// Token: 0x0400053F RID: 1343
	private Color curAmbientEQLightingMod;

	// Token: 0x04000540 RID: 1344
	private Color curAmbientGRLightingMod;

	// Token: 0x04000541 RID: 1345
	public float directLightFadeSpeed = 2f;

	// Token: 0x04000542 RID: 1346
	public float ambientLightFadeSpeed = 2f;

	// Token: 0x04000543 RID: 1347
	public Color skyboxColorMod = Color.black;

	// Token: 0x04000544 RID: 1348
	private Color curskyboxColorMod;

	// Token: 0x04000545 RID: 1349
	public float skyboxFadeSpeed = 2f;

	// Token: 0x04000546 RID: 1350
	private bool fadeInDirectLight;

	// Token: 0x04000547 RID: 1351
	private bool fadeOutDirectLight;

	// Token: 0x04000548 RID: 1352
	private bool fadeInAmbientLight;

	// Token: 0x04000549 RID: 1353
	private bool fadeOutAmbientLight;

	// Token: 0x0400054A RID: 1354
	private bool fadeInSkybox;

	// Token: 0x0400054B RID: 1355
	private bool fadeOutSkybox;

	// Token: 0x0400054C RID: 1356
	public float ambientVolume;

	// Token: 0x0400054D RID: 1357
	public float weatherVolume;

	// Token: 0x0400054E RID: 1358
	public AudioClip zoneAudioClip;

	// Token: 0x0400054F RID: 1359
	public float zoneAudioVolume = 1f;

	// Token: 0x04000550 RID: 1360
	public float zoneAudioFadingSpeed = 1f;

	// Token: 0x04000551 RID: 1361
	public Color fogColorMod = Color.black;

	// Token: 0x04000552 RID: 1362
	private Color curFogColorMod;

	// Token: 0x04000553 RID: 1363
	public float fogFadeSpeed = 2f;

	// Token: 0x04000554 RID: 1364
	public float minFogMod;

	// Token: 0x04000555 RID: 1365
	private bool fadeInFog;

	// Token: 0x04000556 RID: 1366
	private bool fadeOutFog;

	// Token: 0x04000557 RID: 1367
	private bool fadeInFogColor;

	// Token: 0x04000558 RID: 1368
	private bool fadeOutFogColor;

	// Token: 0x04000559 RID: 1369
	public float weatherFadeSpeed = 2f;

	// Token: 0x0400055A RID: 1370
	private bool fadeInWeather;

	// Token: 0x0400055B RID: 1371
	private bool fadeOutWeather;

	// Token: 0x0400055C RID: 1372
	public List<EnviroTrigger> triggers = new List<EnviroTrigger>();

	// Token: 0x0400055D RID: 1373
	private Color fadeOutColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x020000B9 RID: 185
	public enum ZoneTriggerType
	{
		// Token: 0x0400055F RID: 1375
		Entry_Exit,
		// Token: 0x04000560 RID: 1376
		Zone
	}
}
