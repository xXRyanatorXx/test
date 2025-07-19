using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000071 RID: 113
public class EnviroCore : MonoBehaviour
{
	// Token: 0x060001BE RID: 446 RVA: 0x0000D2D0 File Offset: 0x0000B4D0
	public void UpdateEnviroment()
	{
		if (this.Seasons.calcSeasons)
		{
			this.UpdateSeason();
		}
		if (this.EnviroVegetationInstances.Count > 0)
		{
			for (int i = 0; i < this.EnviroVegetationInstances.Count; i++)
			{
				if (this.EnviroVegetationInstances[i] != null)
				{
					this.EnviroVegetationInstances[i].UpdateInstance();
				}
			}
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000D33C File Offset: 0x0000B53C
	public void PlayAmbient(AudioClip sfx)
	{
		if (sfx == this.Audio.currentAmbientSource.audiosrc.clip)
		{
			if (!this.Audio.currentAmbientSource.audiosrc.isPlaying)
			{
				this.Audio.currentAmbientSource.audiosrc.Play();
			}
			return;
		}
		if (this.Audio.currentAmbientSource == this.Audio.AudioSourceAmbient)
		{
			this.Audio.AudioSourceAmbient.FadeOut();
			this.Audio.AudioSourceAmbient2.FadeIn(sfx);
			this.Audio.currentAmbientSource = this.Audio.AudioSourceAmbient2;
			return;
		}
		if (this.Audio.currentAmbientSource == this.Audio.AudioSourceAmbient2)
		{
			this.Audio.AudioSourceAmbient2.FadeOut();
			this.Audio.AudioSourceAmbient.FadeIn(sfx);
			this.Audio.currentAmbientSource = this.Audio.AudioSourceAmbient;
		}
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000D43C File Offset: 0x0000B63C
	public void TryPlayAmbientSFX()
	{
		if (this.Weather.currentActiveWeatherPreset == null)
		{
			return;
		}
		if (this.isNight)
		{
			switch (this.Seasons.currentSeasons)
			{
			case EnviroSeasons.Seasons.Spring:
				if (this.Weather.currentActiveWeatherPreset.SpringNightAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.SpringNightAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			case EnviroSeasons.Seasons.Summer:
				if (this.Weather.currentActiveWeatherPreset.SummerNightAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.SummerNightAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			case EnviroSeasons.Seasons.Autumn:
				if (this.Weather.currentActiveWeatherPreset.AutumnNightAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.AutumnNightAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			case EnviroSeasons.Seasons.Winter:
				if (this.Weather.currentActiveWeatherPreset.WinterNightAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.WinterNightAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			default:
				return;
			}
		}
		else
		{
			switch (this.Seasons.currentSeasons)
			{
			case EnviroSeasons.Seasons.Spring:
				if (this.Weather.currentActiveWeatherPreset.SpringDayAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.SpringDayAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			case EnviroSeasons.Seasons.Summer:
				if (this.Weather.currentActiveWeatherPreset.SummerDayAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.SummerDayAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			case EnviroSeasons.Seasons.Autumn:
				if (this.Weather.currentActiveWeatherPreset.AutumnDayAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.AutumnDayAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			case EnviroSeasons.Seasons.Winter:
				if (this.Weather.currentActiveWeatherPreset.WinterDayAmbient != null)
				{
					this.PlayAmbient(this.Weather.currentActiveWeatherPreset.WinterDayAmbient);
					return;
				}
				this.Audio.AudioSourceAmbient.FadeOut();
				this.Audio.AudioSourceAmbient2.FadeOut();
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000D730 File Offset: 0x0000B930
	public void CreateWeatherEffectHolder(string name)
	{
		if (this.Weather.VFXHolder == null)
		{
			this.Weather.VFXHolder = GameObject.Find(name + "/VFX");
		}
		if (this.Weather.VFXHolder == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "VFX";
			gameObject.transform.parent = this.EffectsHolder.transform;
			gameObject.transform.localPosition = Vector3.zero;
			this.Weather.VFXHolder = gameObject;
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000D7C4 File Offset: 0x0000B9C4
	public void CreateEffects(string name)
	{
		if (this.EffectsHolder == null)
		{
			this.EffectsHolder = GameObject.Find(name);
		}
		if (this.EffectsHolder == null)
		{
			this.EffectsHolder = new GameObject();
			this.EffectsHolder.name = name;
			this.EffectsHolder.transform.parent = base.transform;
			this.EffectsHolder.transform.parent = null;
		}
		this.CreateWeatherEffectHolder(name);
		if (Application.isPlaying && EnviroSkyMgr.instance.dontDestroy)
		{
			UnityEngine.Object.DontDestroyOnLoad(this.EffectsHolder);
		}
		if (this.Player != null)
		{
			this.EffectsHolder.transform.position = this.Player.transform.position;
		}
		else
		{
			this.EffectsHolder.transform.position = base.transform.position;
		}
		GameObject gameObject = GameObject.Find(name + "/SFX Holder(Clone)");
		if (gameObject == null)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Audio.SFXHolderPrefab, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = this.EffectsHolder.transform;
		}
		EnviroAudioSource[] componentsInChildren = gameObject.GetComponentsInChildren<EnviroAudioSource>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			switch (componentsInChildren[i].myFunction)
			{
			case EnviroAudioSource.AudioSourceFunction.Weather1:
				this.Audio.AudioSourceWeather = componentsInChildren[i];
				break;
			case EnviroAudioSource.AudioSourceFunction.Weather2:
				this.Audio.AudioSourceWeather2 = componentsInChildren[i];
				break;
			case EnviroAudioSource.AudioSourceFunction.Ambient:
				this.Audio.AudioSourceAmbient = componentsInChildren[i];
				break;
			case EnviroAudioSource.AudioSourceFunction.Ambient2:
				this.Audio.AudioSourceAmbient2 = componentsInChildren[i];
				break;
			case EnviroAudioSource.AudioSourceFunction.Thunder:
				this.Audio.AudioSourceThunder = componentsInChildren[i];
				break;
			case EnviroAudioSource.AudioSourceFunction.ZoneAmbient:
				this.Audio.AudioSourceZone = componentsInChildren[i];
				break;
			}
		}
		this.Weather.currentAudioSource = this.Audio.AudioSourceWeather;
		this.Audio.currentAmbientSource = this.Audio.AudioSourceAmbient;
		this.TryPlayAmbientSFX();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
	public Transform CreateDirectionalLight(bool additional)
	{
		GameObject gameObject = new GameObject();
		if (!additional)
		{
			gameObject.name = "Enviro Directional Light";
		}
		else
		{
			gameObject.name = "Enviro Directional Light - Moon";
		}
		gameObject.transform.parent = base.transform;
		gameObject.transform.parent = null;
		Light light = gameObject.AddComponent<Light>();
		light.type = LightType.Directional;
		light.shadows = LightShadows.Soft;
		return gameObject.transform;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000DA2C File Offset: 0x0000BC2C
	public Vector3 BetaRay(Vector3 waveLength)
	{
		Vector3 vector = waveLength * 1E-09f;
		Vector3 result;
		result.x = 8f * Mathf.Pow(3.1415927f, 3f) * Mathf.Pow(Mathf.Pow(1.0003f, 2f) - 1f, 2f) * 6.105f / (7.635E+25f * Mathf.Pow(vector.x, 4f) * 5.755f) * 2000f;
		result.y = 8f * Mathf.Pow(3.1415927f, 3f) * Mathf.Pow(Mathf.Pow(1.0003f, 2f) - 1f, 2f) * 6.105f / (7.635E+25f * Mathf.Pow(vector.y, 4f) * 5.755f) * 2000f;
		result.z = 8f * Mathf.Pow(3.1415927f, 3f) * Mathf.Pow(Mathf.Pow(1.0003f, 2f) - 1f, 2f) * 6.105f / (7.635E+25f * Mathf.Pow(vector.z, 4f) * 5.755f) * 2000f;
		return result;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000DB78 File Offset: 0x0000BD78
	public Vector3 BetaMie(float turbidity, Vector3 waveLength)
	{
		float num = 0.2f * turbidity * 10f;
		Vector3 vector;
		vector.x = 434f * num * 3.1415927f * Mathf.Pow(6.2831855f / waveLength.x, 2f) * this.K.x;
		vector.y = 434f * num * 3.1415927f * Mathf.Pow(6.2831855f / waveLength.y, 2f) * this.K.y;
		vector.z = 434f * num * 3.1415927f * Mathf.Pow(6.2831855f / waveLength.z, 2f) * this.K.z;
		vector.x = Mathf.Pow(vector.x, -1f);
		vector.y = Mathf.Pow(vector.y, -1f);
		vector.z = Mathf.Pow(vector.z, -1f);
		return vector;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000DC7E File Offset: 0x0000BE7E
	public Vector3 GetMieG(float g)
	{
		if (g == 1f)
		{
			g = 0.99f;
		}
		return new Vector3(1f - g * g, 1f + g * g, 2f * g);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000DC7E File Offset: 0x0000BE7E
	public Vector3 GetMieGScene(float g)
	{
		if (g == 1f)
		{
			g = 0.99f;
		}
		return new Vector3(1f - g * g, 1f + g * g, 2f * g);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
	public void UpdateTime(int daysInYear)
	{
		if (Application.isPlaying)
		{
			float cycleLengthInMinutes = this.GameTime.cycleLengthInMinutes;
			float num;
			if (!this.isNight)
			{
				num = 0.4f / (cycleLengthInMinutes * this.GameTime.dayLengthModifier);
			}
			else
			{
				num = 0.4f / (cycleLengthInMinutes * this.GameTime.nightLengthModifier);
			}
			this.hourTime = num * Time.deltaTime;
			switch (this.GameTime.ProgressTime)
			{
			case EnviroTime.TimeProgressMode.None:
				this.SetTime(this.GameTime.Years, this.GameTime.Days, this.GameTime.Hours, this.GameTime.Minutes, this.GameTime.Seconds);
				break;
			case EnviroTime.TimeProgressMode.Simulated:
				this.internalHour += this.hourTime;
				this.SetGameTime();
				break;
			case EnviroTime.TimeProgressMode.OneDay:
				this.internalHour += this.hourTime;
				this.SetGameTime();
				break;
			case EnviroTime.TimeProgressMode.SystemTime:
				this.SetTime(DateTime.Now);
				break;
			}
		}
		else
		{
			this.SetTime(this.GameTime.Years, this.GameTime.Days, this.GameTime.Hours, this.GameTime.Minutes, this.GameTime.Seconds);
		}
		if (this.internalHour > this.lastHourUpdate + 1f)
		{
			this.lastHourUpdate = this.internalHour;
			EnviroSkyMgr.instance.NotifyHourPassed();
		}
		if (this.GameTime.Days >= daysInYear)
		{
			this.GameTime.Years = this.GameTime.Years + 1;
			this.GameTime.Days = 0;
			EnviroSkyMgr.instance.NotifyYearPassed();
		}
		this.currentHour = this.internalHour;
		this.currentDay = (float)this.GameTime.Days;
		this.currentYear = (float)this.GameTime.Years;
		this.currentTimeInHours = this.GetInHours(this.internalHour, this.currentDay, this.currentYear, daysInYear);
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000DEB8 File Offset: 0x0000C0B8
	public void SetInternalTime(int year, int dayOfYear, int hour, int minute, int seconds)
	{
		this.GameTime.Years = year;
		this.GameTime.Days = dayOfYear;
		this.GameTime.Minutes = minute;
		this.GameTime.Hours = hour;
		this.internalHour = (float)hour + (float)minute * 0.0166667f + (float)seconds * 0.000277778f;
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000DF14 File Offset: 0x0000C114
	public void SetGameTime()
	{
		if (this.internalHour >= 24f)
		{
			this.internalHour -= 24f;
			EnviroSkyMgr.instance.NotifyHourPassed();
			this.lastHourUpdate = this.internalHour;
			if (this.GameTime.ProgressTime != EnviroTime.TimeProgressMode.OneDay)
			{
				this.GameTime.Days = this.GameTime.Days + 1;
				EnviroSkyMgr.instance.NotifyDayPassed();
			}
		}
		else if (this.internalHour < 0f)
		{
			this.internalHour = 24f + this.internalHour;
			this.lastHourUpdate = this.internalHour;
			if (this.GameTime.ProgressTime != EnviroTime.TimeProgressMode.OneDay)
			{
				this.GameTime.Days = this.GameTime.Days - 1;
				EnviroSkyMgr.instance.NotifyDayPassed();
			}
		}
		float num = this.internalHour;
		this.GameTime.Hours = (int)num;
		num -= (float)this.GameTime.Hours;
		this.GameTime.Minutes = (int)(num * 60f);
		num -= (float)this.GameTime.Minutes * 0.0166667f;
		this.GameTime.Seconds = (int)(num * 3600f);
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000E044 File Offset: 0x0000C244
	public void SetTime(DateTime date)
	{
		this.GameTime.Years = date.Year;
		this.GameTime.Days = date.DayOfYear;
		this.GameTime.Minutes = date.Minute;
		this.GameTime.Seconds = date.Second;
		this.GameTime.Hours = date.Hour;
		this.internalHour = (float)date.Hour + (float)date.Minute * 0.0166667f + (float)date.Second * 0.000277778f;
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000E0D7 File Offset: 0x0000C2D7
	public void ResetHourEventTimer()
	{
		this.lastHourUpdate = this.internalHour;
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000E0E8 File Offset: 0x0000C2E8
	public void SetTime(int year, int dayOfYear, int hour, int minute, int seconds)
	{
		this.GameTime.Years = year;
		this.GameTime.Days = dayOfYear;
		this.GameTime.Minutes = minute;
		this.GameTime.Hours = hour;
		this.GameTime.Seconds = seconds;
		this.internalHour = (float)hour + (float)minute * 0.0166667f + (float)seconds * 0.000277778f;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000E150 File Offset: 0x0000C350
	public void SetInternalTimeOfDay(float inHours)
	{
		this.internalHour = inHours;
		this.GameTime.Hours = (int)inHours;
		inHours -= (float)this.GameTime.Hours;
		this.GameTime.Minutes = (int)(inHours * 60f);
		inHours -= (float)this.GameTime.Minutes * 0.0166667f;
		this.GameTime.Seconds = (int)(inHours * 3600f);
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000E1BD File Offset: 0x0000C3BD
	public string GetTimeStringWithSeconds()
	{
		return string.Format("{0:00}:{1:00}:{2:00}", this.GameTime.Hours, this.GameTime.Minutes, this.GameTime.Seconds);
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000E1F9 File Offset: 0x0000C3F9
	public string GetTimeString()
	{
		return string.Format("{0:00}:{1:00}", this.GameTime.Hours, this.GameTime.Minutes);
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000E228 File Offset: 0x0000C428
	public DateTime CreateSystemDate()
	{
		return default(DateTime).AddYears(this.GameTime.Years - 1).AddDays((double)(this.GameTime.Days - 1));
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000E269 File Offset: 0x0000C469
	public float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000E27C File Offset: 0x0000C47C
	public Vector3 OrbitalToLocal(float theta, float phi)
	{
		float num = Mathf.Sin(theta);
		float y = Mathf.Cos(theta);
		float num2 = Mathf.Sin(phi);
		float num3 = Mathf.Cos(phi);
		Vector3 result;
		result.z = num * num3;
		result.y = y;
		result.x = num * num2;
		return result;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
	public void CalculateSunPosition(float d, float ecl, bool simpleMoon)
	{
		float num = 282.9404f + 4.70935E-05f * d;
		float num2 = 0.016709f - 1.151E-09f * d;
		float num3 = 356.047f + 0.98560023f * d;
		float num4 = num3 + num2 * 57.29578f * Mathf.Sin(0.017453292f * num3) * (1f + num2 * Mathf.Cos(0.017453292f * num3));
		float num5 = Mathf.Cos(0.017453292f * num4) - num2;
		float num6 = Mathf.Sin(0.017453292f * num4) * Mathf.Sqrt(1f - num2 * num2);
		float num7 = 57.29578f * Mathf.Atan2(num6, num5);
		float num8 = Mathf.Sqrt(num5 * num5 + num6 * num6);
		float num9 = num7 + num;
		float num10 = num8 * Mathf.Cos(0.017453292f * num9);
		float num11 = num8 * Mathf.Sin(0.017453292f * num9);
		float num12 = num10;
		float num13 = num11 * Mathf.Cos(0.017453292f * ecl);
		float f = Mathf.Atan2(num11 * Mathf.Sin(0.017453292f * ecl), Mathf.Sqrt(num12 * num12 + num13 * num13));
		float num14 = Mathf.Sin(f);
		float num15 = Mathf.Cos(f);
		float num16 = num9 + 180f + this.GetUniversalTimeOfDay() * 15f;
		this.LST = num16 + this.GameTime.Longitude;
		float num17 = this.LST - 57.29578f * Mathf.Atan2(num13, num12);
		float f2 = 0.017453292f * num17;
		float num18 = Mathf.Sin(f2);
		float num19 = Mathf.Cos(f2) * num15;
		float num20 = num18 * num15;
		float num21 = num14;
		float num22 = Mathf.Sin(0.017453292f * this.GameTime.Latitude);
		float num23 = Mathf.Cos(0.017453292f * this.GameTime.Latitude);
		float num24 = num19 * num22 - num21 * num23;
		float num25 = num20;
		float y = num19 * num23 + num21 * num22;
		float num26 = Mathf.Atan2(num25, num24) + 3.1415927f;
		float num27 = Mathf.Atan2(y, Mathf.Sqrt(num24 * num24 + num25 * num25));
		float num28 = 1.5707964f - num27;
		float phi = num26;
		this.GameTime.solarTime = Mathf.Clamp01(this.Remap(num28, -1.5f, 0f, 1.5f, 1f));
		this.Components.Sun.transform.localPosition = this.OrbitalToLocal(num28, phi);
		this.Components.Sun.transform.LookAt(base.transform.position);
		if (simpleMoon)
		{
			this.Components.Moon.transform.localPosition = this.OrbitalToLocal(num28 - 3.1415927f, phi);
			this.GameTime.lunarTime = Mathf.Clamp01(this.Remap(num28 - 3.1415927f, -3f, 0f, 0f, 1f));
			this.Components.Moon.transform.LookAt(base.transform.position);
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000E5AC File Offset: 0x0000C7AC
	public void CalculateMoonPosition(float d, float ecl)
	{
		float num = 125.1228f - 0.05295381f * d;
		float num2 = 5.1454f;
		float num3 = 318.0634f + 0.16435732f * d;
		float num4 = 60.2666f;
		float num5 = 0.0549f;
		float num6 = 115.3654f + 13.064993f * d;
		float num7 = 0.017453292f * num6;
		float f = num7 + num5 * Mathf.Sin(num7) * (1f + num5 * Mathf.Cos(num7));
		float num8 = num4 * (Mathf.Cos(f) - num5);
		float num9 = num4 * (Mathf.Sqrt(1f - num5 * num5) * Mathf.Sin(f));
		float num10 = 57.29578f * Mathf.Atan2(num9, num8);
		float num11 = Mathf.Sqrt(num8 * num8 + num9 * num9);
		float f2 = 0.017453292f * num;
		float num12 = Mathf.Sin(f2);
		float num13 = Mathf.Cos(f2);
		float f3 = 0.017453292f * (num10 + num3);
		float num14 = Mathf.Sin(f3);
		float num15 = Mathf.Cos(f3);
		float f4 = 0.017453292f * num2;
		float num16 = Mathf.Cos(f4);
		float num17 = num11 * (num13 * num15 - num12 * num14 * num16);
		float num18 = num11 * (num12 * num15 + num13 * num14 * num16);
		float num19 = num11 * (num14 * Mathf.Sin(f4));
		float num20 = Mathf.Cos(0.017453292f * ecl);
		float num21 = Mathf.Sin(0.017453292f * ecl);
		float num22 = num17;
		float num23 = num18 * num20 - num19 * num21;
		float y = num18 * num21 + num19 * num20;
		float num24 = Mathf.Atan2(num23, num22);
		float f5 = Mathf.Atan2(y, Mathf.Sqrt(num22 * num22 + num23 * num23));
		float f6 = 0.017453292f * this.LST - num24;
		float num25 = Mathf.Cos(f6) * Mathf.Cos(f5);
		float num26 = Mathf.Sin(f6) * Mathf.Cos(f5);
		float num27 = Mathf.Sin(f5);
		float f7 = 0.017453292f * this.GameTime.Latitude;
		float num28 = Mathf.Sin(f7);
		float num29 = Mathf.Cos(f7);
		float num30 = num25 * num28 - num27 * num29;
		float num31 = num26;
		float y2 = num25 * num29 + num27 * num28;
		float num32 = Mathf.Atan2(num31, num30) + 3.1415927f;
		float num33 = Mathf.Atan2(y2, Mathf.Sqrt(num30 * num30 + num31 * num31));
		float num34 = 1.5707964f - num33;
		float phi = num32;
		this.Components.Moon.transform.localPosition = this.OrbitalToLocal(num34, phi);
		this.GameTime.lunarTime = Mathf.Clamp01(this.Remap(num34, -1.5f, 0f, 1.5f, 1f));
		this.Components.Moon.transform.LookAt(base.transform.position);
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000E844 File Offset: 0x0000CA44
	public void CalculateSatPosition(float d, float ecl, Transform sat, Transform satRota, EnviroSatellite satSetting)
	{
		float num = 125.1228f - 0.05295381f * d;
		float num2 = 5.1454f;
		float num3 = 318.0634f + 0.16435732f * d;
		float num4 = 60.2666f;
		float num5 = 0.0549f;
		float num6 = 115.3654f + 13.064993f * d;
		float num7 = 0.017453292f * num6;
		float f = num7 + num5 * Mathf.Sin(num7) * (1f + num5 * Mathf.Cos(num7));
		float num8 = num4 * (Mathf.Cos(f) - num5);
		float num9 = num4 * (Mathf.Sqrt(1f - num5 * num5) * Mathf.Sin(f));
		float num10 = 57.29578f * Mathf.Atan2(num9, num8);
		float num11 = Mathf.Sqrt(num8 * num8 + num9 * num9);
		float f2 = 0.017453292f * num;
		float num12 = Mathf.Sin(f2);
		float num13 = Mathf.Cos(f2);
		float f3 = 0.017453292f * (num10 + num3);
		float num14 = Mathf.Sin(f3);
		float num15 = Mathf.Cos(f3);
		float f4 = 0.017453292f * num2;
		float num16 = Mathf.Cos(f4);
		float num17 = num11 * (num13 * num15 - num12 * num14 * num16);
		float num18 = num11 * (num12 * num15 + num13 * num14 * num16);
		float num19 = num11 * (num14 * Mathf.Sin(f4));
		float num20 = Mathf.Cos(0.017453292f * ecl);
		float num21 = Mathf.Sin(0.017453292f * ecl);
		float num22 = num17;
		float num23 = num18 * num20 - num19 * num21;
		float y = num18 * num21 + num19 * num20;
		float num24 = Mathf.Atan2(num23, num22);
		float f5 = Mathf.Atan2(y, Mathf.Sqrt(num22 * num22 + num23 * num23));
		float f6 = 0.017453292f * this.LST - num24;
		float num25 = Mathf.Cos(f6) * Mathf.Cos(f5);
		float num26 = Mathf.Sin(f6) * Mathf.Cos(f5);
		float num27 = Mathf.Sin(f5);
		float f7 = 0.017453292f * this.GameTime.Latitude;
		float num28 = Mathf.Sin(f7);
		float num29 = Mathf.Cos(f7);
		float num30 = num25 * num28 - num27 * num29;
		float num31 = num26;
		float y2 = num25 * num29 + num27 * num28;
		float num32 = Mathf.Atan2(num31, num30) + 3.1415927f;
		float num33 = Mathf.Atan2(y2, Mathf.Sqrt(num30 * num30 + num31 * num31));
		float theta = 1.5707964f - num33;
		float phi = num32;
		sat.localPosition = this.OrbitalToLocal(theta, phi);
		sat.LookAt(base.transform.position);
		Vector3 eulerAngles = new Vector3(satSetting.xRot, satSetting.yRot, satSetting.zRot);
		satRota.eulerAngles = eulerAngles;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000EAB8 File Offset: 0x0000CCB8
	public void CalculateStarsPosition(float siderealTime)
	{
		if (siderealTime > 24f)
		{
			siderealTime -= 24f;
		}
		else if (siderealTime < 0f)
		{
			siderealTime += 24f;
		}
		Quaternion quaternion = Quaternion.Euler(90f - this.GameTime.Latitude, 0.017453292f * this.GameTime.Longitude, 0f);
		quaternion *= Quaternion.Euler(0f, siderealTime, 0f);
		this.Components.starsRotation.localRotation = quaternion;
		Shader.SetGlobalMatrix("_StarsMatrix", this.Components.starsRotation.worldToLocalMatrix);
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000EB58 File Offset: 0x0000CD58
	public void UpdateSunAndMoonPosition()
	{
		DateTime dateTime = this.CreateSystemDate();
		float num = (float)(367 * dateTime.Year - 7 * (dateTime.Year + (dateTime.Month / 12 + 9) / 12) / 4 + 275 * dateTime.Month / 9 + dateTime.Day - 730530);
		num += this.GetUniversalTimeOfDay() / 24f;
		float ecl = 23.4393f - 3.563E-07f * num;
		if (this.skySettings.sunAndMoonPosition == EnviroSkySettings.SunAndMoonCalc.Realistic)
		{
			this.CalculateSunPosition(num, ecl, false);
			this.CalculateMoonPosition(num, ecl);
		}
		else
		{
			this.CalculateSunPosition(num, ecl, true);
		}
		this.CalculateStarsPosition(this.LST);
		this.CalculateSatPositions(this.LST);
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000EC16 File Offset: 0x0000CE16
	public float GetUniversalTimeOfDay()
	{
		return this.internalHour - (float)this.GameTime.utcOffset;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000EC2B File Offset: 0x0000CE2B
	public float GetTimeOfDay()
	{
		return this.internalHour;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000EC33 File Offset: 0x0000CE33
	public double GetInHours(float hours, float days, float years, int daysInYear)
	{
		return (double)(hours + days * 24f + years * (float)daysInYear * 24f);
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000EC4C File Offset: 0x0000CE4C
	public void UpdateSeason()
	{
		if (this.currentDay >= (float)this.seasonsSettings.SpringStart && this.currentDay <= (float)this.seasonsSettings.SpringEnd)
		{
			this.ChangeSeason(EnviroSeasons.Seasons.Spring);
			return;
		}
		if (this.currentDay >= (float)this.seasonsSettings.SummerStart && this.currentDay <= (float)this.seasonsSettings.SummerEnd)
		{
			this.ChangeSeason(EnviroSeasons.Seasons.Summer);
			return;
		}
		if (this.currentDay >= (float)this.seasonsSettings.AutumnStart && this.currentDay <= (float)this.seasonsSettings.AutumnEnd)
		{
			this.ChangeSeason(EnviroSeasons.Seasons.Autumn);
			return;
		}
		if (this.currentDay >= (float)this.seasonsSettings.WinterStart || this.currentDay <= (float)this.seasonsSettings.WinterEnd)
		{
			this.ChangeSeason(EnviroSeasons.Seasons.Winter);
		}
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000ED18 File Offset: 0x0000CF18
	public void ChangeSeason(EnviroSeasons.Seasons season)
	{
		if (this.Seasons.currentSeasons != season)
		{
			EnviroSkyMgr.instance.NotifySeasonChanged(season);
			this.Seasons.lastSeason = this.Seasons.currentSeasons;
			this.Seasons.currentSeasons = season;
		}
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000ED58 File Offset: 0x0000CF58
	public void ApplyProfile(EnviroProfile p)
	{
		this.profile = p;
		this.lightSettings = JsonUtility.FromJson<EnviroLightSettings>(JsonUtility.ToJson(p.lightSettings));
		this.volumeLightSettings = JsonUtility.FromJson<EnviroVolumeLightingSettings>(JsonUtility.ToJson(p.volumeLightSettings));
		this.distanceBlurSettings = JsonUtility.FromJson<EnviroDistanceBlurSettings>(JsonUtility.ToJson(p.distanceBlurSettings));
		this.skySettings = JsonUtility.FromJson<EnviroSkySettings>(JsonUtility.ToJson(p.skySettings));
		this.cloudsSettings = JsonUtility.FromJson<EnviroCloudSettings>(JsonUtility.ToJson(p.cloudsSettings));
		this.weatherSettings = JsonUtility.FromJson<EnviroWeatherSettings>(JsonUtility.ToJson(p.weatherSettings));
		this.fogSettings = JsonUtility.FromJson<EnviroFogSettings>(JsonUtility.ToJson(p.fogSettings));
		this.lightshaftsSettings = JsonUtility.FromJson<EnviroLightShaftsSettings>(JsonUtility.ToJson(p.lightshaftsSettings));
		this.audioSettings = JsonUtility.FromJson<EnviroAudioSettings>(JsonUtility.ToJson(p.audioSettings));
		this.satelliteSettings = JsonUtility.FromJson<EnviroSatellitesSettings>(JsonUtility.ToJson(p.satelliteSettings));
		this.qualitySettings = JsonUtility.FromJson<EnviroQualitySettings>(JsonUtility.ToJson(p.qualitySettings));
		this.seasonsSettings = JsonUtility.FromJson<EnviroSeasonSettings>(JsonUtility.ToJson(p.seasonsSettings));
		this.reflectionSettings = JsonUtility.FromJson<EnviroReflectionSettings>(JsonUtility.ToJson(p.reflectionSettings));
		this.auroraSettings = JsonUtility.FromJson<EnviroAuroraSettings>(JsonUtility.ToJson(p.auroraSettings));
		this.profileLoaded = true;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
	public void SaveProfile()
	{
		this.profile.lightSettings = JsonUtility.FromJson<EnviroLightSettings>(JsonUtility.ToJson(this.lightSettings));
		this.profile.volumeLightSettings = JsonUtility.FromJson<EnviroVolumeLightingSettings>(JsonUtility.ToJson(this.volumeLightSettings));
		this.profile.distanceBlurSettings = JsonUtility.FromJson<EnviroDistanceBlurSettings>(JsonUtility.ToJson(this.distanceBlurSettings));
		this.profile.skySettings = JsonUtility.FromJson<EnviroSkySettings>(JsonUtility.ToJson(this.skySettings));
		this.profile.cloudsSettings = JsonUtility.FromJson<EnviroCloudSettings>(JsonUtility.ToJson(this.cloudsSettings));
		this.profile.weatherSettings = JsonUtility.FromJson<EnviroWeatherSettings>(JsonUtility.ToJson(this.weatherSettings));
		this.profile.fogSettings = JsonUtility.FromJson<EnviroFogSettings>(JsonUtility.ToJson(this.fogSettings));
		this.profile.lightshaftsSettings = JsonUtility.FromJson<EnviroLightShaftsSettings>(JsonUtility.ToJson(this.lightshaftsSettings));
		this.profile.audioSettings = JsonUtility.FromJson<EnviroAudioSettings>(JsonUtility.ToJson(this.audioSettings));
		this.profile.satelliteSettings = JsonUtility.FromJson<EnviroSatellitesSettings>(JsonUtility.ToJson(this.satelliteSettings));
		this.profile.qualitySettings = JsonUtility.FromJson<EnviroQualitySettings>(JsonUtility.ToJson(this.qualitySettings));
		this.profile.seasonsSettings = JsonUtility.FromJson<EnviroSeasonSettings>(JsonUtility.ToJson(this.seasonsSettings));
		this.profile.reflectionSettings = JsonUtility.FromJson<EnviroReflectionSettings>(JsonUtility.ToJson(this.reflectionSettings));
		this.profile.auroraSettings = JsonUtility.FromJson<EnviroAuroraSettings>(JsonUtility.ToJson(this.auroraSettings));
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000F030 File Offset: 0x0000D230
	public void UpdateReflections()
	{
		if (this.Components.GlobalReflectionProbe == null)
		{
			Debug.Log("Global Reflection Probe not assigned in 'Components' menu of Enviro Sky Instance!");
			return;
		}
		if (EnviroSkyMgr.instance != null && EnviroSkyMgr.instance.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			this.Components.GlobalReflectionProbe.customRendering = this.reflectionSettings.globalReflectionCustomRendering;
			if (this.reflectionSettings.reflectionCloudsQuality != null)
			{
				this.Components.GlobalReflectionProbe.customCloudsQuality = this.reflectionSettings.reflectionCloudsQuality;
			}
			this.Components.GlobalReflectionProbe.useFog = this.reflectionSettings.globalReflectionUseFog;
		}
		if (!this.reflectionSettings.globalReflections)
		{
			this.Components.GlobalReflectionProbe.enabled = false;
			return;
		}
		if (!this.Components.GlobalReflectionProbe.isActiveAndEnabled)
		{
			this.Components.GlobalReflectionProbe.enabled = true;
		}
		this.Components.GlobalReflectionProbe.myProbe.cullingMask = this.reflectionSettings.globalReflectionLayers;
		this.Components.GlobalReflectionProbe.myProbe.intensity = this.reflectionSettings.globalReflectionsIntensity;
		this.Components.GlobalReflectionProbe.myProbe.size = base.transform.localScale * this.reflectionSettings.globalReflectionsScale;
		switch (this.reflectionSettings.globalReflectionResolution)
		{
		case EnviroReflectionSettings.GlobalReflectionResolution.R16:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 16;
			break;
		case EnviroReflectionSettings.GlobalReflectionResolution.R32:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 32;
			break;
		case EnviroReflectionSettings.GlobalReflectionResolution.R64:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 64;
			break;
		case EnviroReflectionSettings.GlobalReflectionResolution.R128:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 128;
			break;
		case EnviroReflectionSettings.GlobalReflectionResolution.R256:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 256;
			break;
		case EnviroReflectionSettings.GlobalReflectionResolution.R512:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 512;
			break;
		case EnviroReflectionSettings.GlobalReflectionResolution.R1024:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 1024;
			break;
		case EnviroReflectionSettings.GlobalReflectionResolution.R2048:
			this.Components.GlobalReflectionProbe.myProbe.resolution = 2048;
			break;
		}
		if ((this.currentTimeInHours > this.lastRelfectionUpdate + (double)this.reflectionSettings.globalReflectionsTimeTreshold || this.currentTimeInHours < this.lastRelfectionUpdate - (double)this.reflectionSettings.globalReflectionsTimeTreshold) && this.reflectionSettings.globalReflectionsUpdateOnGameTime)
		{
			this.lastRelfectionUpdate = this.currentTimeInHours;
			this.Components.GlobalReflectionProbe.RefreshReflection(this.reflectionSettings.globalReflectionTimeSlicing);
			return;
		}
		if ((base.transform.position.magnitude > this.lastRelfectionPositionUpdate.magnitude + this.reflectionSettings.globalReflectionsPositionTreshold || base.transform.position.magnitude < this.lastRelfectionPositionUpdate.magnitude - this.reflectionSettings.globalReflectionsPositionTreshold) && this.reflectionSettings.globalReflectionsUpdateOnPosition)
		{
			this.lastRelfectionPositionUpdate = base.transform.position;
			this.Components.GlobalReflectionProbe.RefreshReflection(this.reflectionSettings.globalReflectionTimeSlicing);
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000F3A4 File Offset: 0x0000D5A4
	public void UpdateAmbientLight()
	{
		float num = Mathf.Lerp(this.lightSettings.ambientIntensity.Evaluate(this.GameTime.solarTime), this.lightSettings.ambientIntensity.Evaluate(this.GameTime.solarTime) * 0.25f, this.GameTime.lunarTime * (1f - this.GameTime.solarTime));
		switch (this.lightSettings.ambientMode)
		{
		case AmbientMode.Skybox:
			RenderSettings.ambientIntensity = num;
			if (this.lastAmbientSkyUpdate < this.internalHour || this.lastAmbientSkyUpdate > this.internalHour + 0.101f)
			{
				DynamicGI.UpdateEnvironment();
				this.lastAmbientSkyUpdate = this.internalHour + 0.1f;
			}
			break;
		case AmbientMode.Trilight:
			RenderSettings.ambientSkyColor = Color.Lerp(Color.Lerp(this.lightSettings.ambientSkyColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, this.currentWeatherLightMod.a) * num, this.interiorZoneSettings.currentInteriorAmbientLightMod, this.interiorZoneSettings.currentInteriorAmbientLightMod.a);
			RenderSettings.ambientEquatorColor = Color.Lerp(Color.Lerp(this.lightSettings.ambientEquatorColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, this.currentWeatherLightMod.a) * num, this.interiorZoneSettings.currentInteriorAmbientEQLightMod, this.interiorZoneSettings.currentInteriorAmbientEQLightMod.a);
			RenderSettings.ambientGroundColor = Color.Lerp(Color.Lerp(this.lightSettings.ambientGroundColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, this.currentWeatherLightMod.a) * num, this.interiorZoneSettings.currentInteriorAmbientGRLightMod, this.interiorZoneSettings.currentInteriorAmbientGRLightMod.a);
			return;
		case (AmbientMode)2:
			break;
		case AmbientMode.Flat:
			RenderSettings.ambientSkyColor = Color.Lerp(Color.Lerp(this.lightSettings.ambientSkyColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, this.currentWeatherLightMod.a) * num, this.interiorZoneSettings.currentInteriorAmbientLightMod, this.interiorZoneSettings.currentInteriorAmbientLightMod.a);
			return;
		default:
			return;
		}
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
	public void CalculateDirectLight()
	{
		if (this.MainLight == null)
		{
			this.MainLight = this.Components.DirectLight.GetComponent<Light>();
		}
		if (this.lightSettings.directionalLightMode == EnviroLightSettings.LightingMode.Single || this.Components.AdditionalDirectLight == null)
		{
			Color a = Color.Lerp(this.lightSettings.LightColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, this.currentWeatherLightMod.a);
			this.MainLight.color = Color.Lerp(a, this.interiorZoneSettings.currentInteriorDirectLightMod, this.interiorZoneSettings.currentInteriorDirectLightMod.a);
			float b;
			if (!this.isNight)
			{
				b = this.lightSettings.directLightSunIntensity.Evaluate(this.GameTime.solarTime);
				this.Components.Sun.transform.LookAt(new Vector3(base.transform.position.x, base.transform.position.y - this.lightSettings.directLightAngleOffset, base.transform.position.z));
				if (!this.lightSettings.stopRotationAtHigh || (this.lightSettings.stopRotationAtHigh && this.GameTime.solarTime >= this.lightSettings.rotationStopHigh))
				{
					this.Components.DirectLight.rotation = this.Components.Sun.transform.rotation;
				}
			}
			else
			{
				b = this.lightSettings.directLightMoonIntensity.Evaluate(this.GameTime.lunarTime);
				this.Components.Moon.transform.LookAt(new Vector3(base.transform.position.x, base.transform.position.y - this.lightSettings.directLightAngleOffset, base.transform.position.z));
				if (!this.lightSettings.stopRotationAtHigh || (this.lightSettings.stopRotationAtHigh && this.GameTime.lunarTime >= this.lightSettings.rotationStopHigh))
				{
					this.Components.DirectLight.rotation = this.Components.Moon.transform.rotation;
				}
			}
			this.MainLight.intensity = Mathf.Lerp(this.MainLight.intensity, b, Time.deltaTime * this.lightSettings.lightIntensityTransitionSpeed);
			this.MainLight.shadowStrength = Mathf.Clamp01(this.lightSettings.shadowIntensity.Evaluate(this.GameTime.solarTime) + this.shadowIntensityMod);
			return;
		}
		if (this.lightSettings.directionalLightMode == EnviroLightSettings.LightingMode.Dual && this.Components.AdditionalDirectLight != null)
		{
			if (this.AdditionalLight == null)
			{
				this.AdditionalLight = this.Components.AdditionalDirectLight.GetComponent<Light>();
			}
			Color a2 = Color.Lerp(this.lightSettings.LightColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, this.currentWeatherLightMod.a);
			this.MainLight.color = Color.Lerp(a2, this.interiorZoneSettings.currentInteriorDirectLightMod, this.interiorZoneSettings.currentInteriorDirectLightMod.a);
			this.AdditionalLight.color = this.MainLight.color;
			float b2 = this.lightSettings.directLightSunIntensity.Evaluate(this.GameTime.solarTime);
			this.lightSettings.directLightMoonIntensity.Evaluate(this.GameTime.lunarTime);
			float solarTime = this.GameTime.solarTime;
			this.Components.Sun.transform.LookAt(new Vector3(base.transform.position.x, base.transform.position.y - this.lightSettings.directLightAngleOffset, base.transform.position.z));
			if (!this.lightSettings.stopRotationAtHigh || (this.lightSettings.stopRotationAtHigh && this.GameTime.solarTime >= this.lightSettings.rotationStopHigh))
			{
				this.Components.DirectLight.rotation = this.Components.Sun.transform.rotation;
			}
			this.Components.Moon.transform.LookAt(new Vector3(base.transform.position.x, base.transform.position.y - this.lightSettings.directLightAngleOffset, base.transform.position.z));
			if (!this.lightSettings.stopRotationAtHigh || (this.lightSettings.stopRotationAtHigh && this.GameTime.lunarTime >= this.lightSettings.rotationStopHigh))
			{
				this.Components.AdditionalDirectLight.rotation = this.Components.Moon.transform.rotation;
			}
			this.MainLight.intensity = Mathf.Lerp(this.MainLight.intensity, b2, Time.deltaTime * this.lightSettings.lightIntensityTransitionSpeed);
			this.MainLight.shadowStrength = Mathf.Clamp01(this.lightSettings.shadowIntensity.Evaluate(this.GameTime.solarTime) + this.shadowIntensityMod);
			this.AdditionalLight.shadowStrength = Mathf.Clamp01(this.lightSettings.shadowIntensity.Evaluate(this.GameTime.solarTime) + this.shadowIntensityMod);
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000FB8C File Offset: 0x0000DD8C
	public void UpdateSceneView()
	{
		if (this.Weather.startWeatherPreset != null && !Application.isPlaying)
		{
			this.currentWeatherSkyMod = this.Weather.startWeatherPreset.weatherSkyMod.Evaluate(this.GameTime.solarTime);
			this.currentWeatherFogMod = this.Weather.startWeatherPreset.weatherFogMod.Evaluate(this.GameTime.solarTime);
			this.currentWeatherLightMod = this.Weather.startWeatherPreset.weatherLightMod.Evaluate(this.GameTime.solarTime);
		}
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000FC28 File Offset: 0x0000DE28
	public void UpdateWind(EnviroWeatherPreset preset)
	{
		if (preset != null)
		{
			this.windIntensity = Mathf.Lerp(this.windIntensity, preset.WindStrenght, this.weatherSettings.windIntensityTransitionSpeed * Time.deltaTime);
		}
		if (this.cloudsSettings.useWindZoneDirection)
		{
			this.cloudsSettings.cloudsWindDirectionX = -this.Components.windZone.transform.forward.x;
			this.cloudsSettings.cloudsWindDirectionY = -this.Components.windZone.transform.forward.z;
		}
		this.cloudAnim += new Vector3(this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionX) * this.cloudsSettings.cloudsWindIntensity * Time.deltaTime, this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionY) * this.cloudsSettings.cloudsWindIntensity * Time.deltaTime, this.cloudsSettings.cloudsTimeScale * -1f * this.cloudsSettings.cloudsUpwardsWindIntensity * Time.deltaTime);
		this.cloudAnimNonScaled += new Vector2(this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionX) * this.cloudsSettings.cloudsWindIntensity * Time.deltaTime * 0.1f, this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionY) * this.cloudsSettings.cloudsWindIntensity * Time.deltaTime * 0.1f);
		this.cloudFlatBaseAnim += new Vector2(this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionX) * this.cloudsSettings.cloudsWindIntensity * Time.deltaTime * 0.1f, this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionY) * this.cloudsSettings.cloudsWindIntensity * Time.deltaTime * 0.1f);
		this.cloudFlatDetailAnim += new Vector2(this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionX) * this.cloudsSettings.cloudsDetailWindIntensity * Time.deltaTime, this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionY) * this.cloudsSettings.cloudsDetailWindIntensity * Time.deltaTime);
		this.cirrusAnim += new Vector2(this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionX) * this.cloudsSettings.cirrusWindIntensity * Time.deltaTime * 0.1f, this.cloudsSettings.cloudsTimeScale * (this.windIntensity * this.cloudsSettings.cloudsWindDirectionY) * this.cloudsSettings.cirrusWindIntensity * Time.deltaTime * 0.1f);
		if (this.cloudAnim.x > 1f)
		{
			this.cloudAnim.x = -1f;
		}
		else if (this.cloudAnim.x < -1f)
		{
			this.cloudAnim.x = 1f;
		}
		if (this.cloudAnim.y > 1f)
		{
			this.cloudAnim.y = -1f;
		}
		else if (this.cloudAnim.y < -1f)
		{
			this.cloudAnim.y = 1f;
		}
		if (this.cloudAnim.z > 1f)
		{
			this.cloudAnim.z = -1f;
		}
		else if (this.cloudAnim.z < -1f)
		{
			this.cloudAnim.z = 1f;
		}
		if (this.cirrusAnim.x > 1f)
		{
			this.cirrusAnim.x = -1f;
		}
		else if (this.cirrusAnim.x < -1f)
		{
			this.cirrusAnim.x = 1f;
		}
		if (this.cirrusAnim.y > 1f)
		{
			this.cirrusAnim.y = -1f;
		}
		else if (this.cirrusAnim.y < -1f)
		{
			this.cirrusAnim.y = 1f;
		}
		if (this.cloudFlatBaseAnim.x > 1f)
		{
			this.cloudFlatBaseAnim.x = -1f;
		}
		else if (this.cloudFlatBaseAnim.x < -1f)
		{
			this.cloudFlatBaseAnim.x = 1f;
		}
		if (this.cloudFlatBaseAnim.y > 1f)
		{
			this.cloudFlatBaseAnim.y = -1f;
		}
		else if (this.cloudFlatBaseAnim.y < -1f)
		{
			this.cloudFlatBaseAnim.y = 1f;
		}
		if (this.cloudFlatDetailAnim.x > 1f)
		{
			this.cloudFlatDetailAnim.x = -1f;
		}
		else if (this.cloudFlatDetailAnim.x < -1f)
		{
			this.cloudFlatDetailAnim.x = 1f;
		}
		if (this.cloudFlatDetailAnim.y > 1f)
		{
			this.cloudFlatDetailAnim.y = -1f;
		}
		else if (this.cloudFlatDetailAnim.y < -1f)
		{
			this.cloudFlatDetailAnim.y = 1f;
		}
		this.Components.windZone.windMain = this.windIntensity;
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x000101E0 File Offset: 0x0000E3E0
	public int GetActiveWeatherID()
	{
		for (int i = 0; i < this.Weather.WeatherPrefabs.Count; i++)
		{
			if (this.Weather.WeatherPrefabs[i].weatherPreset == this.Weather.currentActiveWeatherPreset)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00010234 File Offset: 0x0000E434
	public void UpdateWeatherVariables(EnviroWeatherPreset p)
	{
		this.UpdateWind(p);
		if (this.Weather.wetness < p.wetnessLevel)
		{
			this.Weather.wetness = Mathf.Lerp(this.Weather.curWetness, p.wetnessLevel, this.weatherSettings.wetnessAccumulationSpeed * Time.deltaTime);
		}
		else
		{
			this.Weather.wetness = Mathf.Lerp(this.Weather.curWetness, p.wetnessLevel, this.weatherSettings.wetnessDryingSpeed * Time.deltaTime);
		}
		this.Weather.wetness = Mathf.Clamp(this.Weather.wetness, 0f, 1f);
		this.Weather.curWetness = this.Weather.wetness;
		if (this.Weather.snowStrength < p.snowLevel)
		{
			this.Weather.snowStrength = Mathf.Lerp(this.Weather.curSnowStrength, p.snowLevel, this.weatherSettings.snowAccumulationSpeed * Time.deltaTime);
		}
		else if (this.Weather.currentTemperature > this.weatherSettings.snowMeltingTresholdTemperature)
		{
			this.Weather.snowStrength = Mathf.Lerp(this.Weather.curSnowStrength, p.snowLevel, this.weatherSettings.snowMeltingSpeed * Time.deltaTime);
		}
		this.Weather.snowStrength = Mathf.Clamp(this.Weather.snowStrength, 0f, 1f);
		this.Weather.curSnowStrength = this.Weather.snowStrength;
		Shader.SetGlobalFloat("_EnviroGrassSnow", this.Weather.curSnowStrength);
		float num = 0f;
		switch (this.Seasons.currentSeasons)
		{
		case EnviroSeasons.Seasons.Spring:
			num = this.seasonsSettings.springBaseTemperature.Evaluate(this.GetUniversalTimeOfDay() / 24f);
			break;
		case EnviroSeasons.Seasons.Summer:
			num = this.seasonsSettings.summerBaseTemperature.Evaluate(this.GetUniversalTimeOfDay() / 24f);
			break;
		case EnviroSeasons.Seasons.Autumn:
			num = this.seasonsSettings.autumnBaseTemperature.Evaluate(this.GetUniversalTimeOfDay() / 24f);
			break;
		case EnviroSeasons.Seasons.Winter:
			num = this.seasonsSettings.winterBaseTemperature.Evaluate(this.GetUniversalTimeOfDay() / 24f);
			break;
		}
		num += p.temperatureLevel;
		num += this.Weather.temperatureModifier;
		this.Weather.currentTemperature = Mathf.Lerp(this.Weather.currentTemperature, num, Time.deltaTime * this.weatherSettings.temperatureChangingSpeed);
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x000104C5 File Offset: 0x0000E6C5
	public IEnumerator PlayThunderRandom()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(this.Weather.currentActiveWeatherPreset.lightningInterval, this.Weather.currentActiveWeatherPreset.lightningInterval * 2f));
		if (this.Weather.currentActiveWeatherPrefab.weatherPreset.isLightningStorm)
		{
			if (this.Weather.weatherFullyChanged)
			{
				this.PlayLightning();
			}
			base.StartCoroutine(this.PlayThunderRandom());
		}
		else
		{
			base.StopCoroutine(this.PlayThunderRandom());
			this.Components.LightningGenerator.StopLightning();
		}
		yield break;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x000104D4 File Offset: 0x0000E6D4
	public IEnumerator PlayLightningEffect(Vector3 position)
	{
		this.lightningEffect.transform.position = position;
		this.lightningEffect.transform.eulerAngles = new Vector3(UnityEngine.Random.Range(-80f, -100f), 0f, 0f);
		this.lightningEffect.Play();
		yield return new WaitForSeconds(0.5f);
		this.lightningEffect.Stop();
		yield break;
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x000104EC File Offset: 0x0000E6EC
	public void PlayLightning()
	{
		if (this.lightningEffect != null)
		{
			base.StartCoroutine(this.PlayLightningEffect(new Vector3(base.transform.position.x + UnityEngine.Random.Range(-this.weatherSettings.lightningRange, this.weatherSettings.lightningRange), this.weatherSettings.lightningHeight, base.transform.position.z + UnityEngine.Random.Range(-this.weatherSettings.lightningRange, this.weatherSettings.lightningRange))));
		}
		int index = UnityEngine.Random.Range(0, this.audioSettings.ThunderSFX.Count);
		this.Audio.AudioSourceThunder.PlayOneShot(this.audioSettings.ThunderSFX[index]);
		this.Components.LightningGenerator.Lightning();
	}

	// Token: 0x060001EA RID: 490 RVA: 0x000105C8 File Offset: 0x0000E7C8
	public void ForceWeatherUpdate()
	{
		this.Weather.lastActiveWeatherPreset = this.Weather.currentActiveWeatherPreset;
		this.Weather.lastActiveWeatherPrefab = this.Weather.currentActiveWeatherPrefab;
		this.Weather.currentActiveWeatherPreset = this.Weather.currentActiveZone.currentActiveZoneWeatherPreset;
		this.Weather.currentActiveWeatherPrefab = this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab;
		if (this.Weather.currentActiveWeatherPreset != null)
		{
			EnviroSkyMgr.instance.NotifyWeatherChanged(this.Weather.currentActiveWeatherPreset);
			this.Weather.weatherFullyChanged = false;
			if (!this.serverMode)
			{
				if (this.Weather.currentActiveWeatherPrefab.weatherPreset.isLightningStorm)
				{
					base.StartCoroutine(this.PlayThunderRandom());
					return;
				}
				base.StopCoroutine(this.PlayThunderRandom());
				this.Components.LightningGenerator.StopLightning();
			}
		}
	}

	// Token: 0x060001EB RID: 491 RVA: 0x000106B4 File Offset: 0x0000E8B4
	public void CalcWeatherTransitionState()
	{
		bool weatherFullyChanged = false;
		if (EnviroSkyMgr.instance != null && EnviroSkyMgr.instance.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			weatherFullyChanged = (this.cloudsConfig.coverage >= this.Weather.currentActiveWeatherPreset.cloudsConfig.coverage - 0.01f);
		}
		else if (EnviroSkyMgr.instance != null && EnviroSkyMgr.instance.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.LW)
		{
			weatherFullyChanged = (this.cloudsConfig.particleLayer1Alpha >= this.Weather.currentActiveWeatherPreset.cloudsConfig.particleLayer1Alpha - 0.01f && this.cloudsConfig.particleLayer2Alpha >= this.Weather.currentActiveWeatherPreset.cloudsConfig.particleLayer2Alpha - 0.01f);
		}
		this.Weather.weatherFullyChanged = weatherFullyChanged;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0001078C File Offset: 0x0000E98C
	public void SetWeatherOverwrite(int weatherId)
	{
		if (weatherId < 0 || weatherId > this.Weather.WeatherPrefabs.Count)
		{
			return;
		}
		if (this.Weather.WeatherPrefabs[weatherId] != this.Weather.currentActiveWeatherPrefab)
		{
			this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab = this.Weather.WeatherPrefabs[weatherId];
			this.Weather.currentActiveZone.currentActiveZoneWeatherPreset = this.Weather.WeatherPrefabs[weatherId].weatherPreset;
			EnviroSkyMgr.instance.NotifyZoneWeatherChanged(this.Weather.WeatherPrefabs[weatherId].weatherPreset, this.Weather.currentActiveZone);
		}
		EnviroSkyMgr.instance.InstantWeatherChange(this.Weather.currentActiveZone.currentActiveZoneWeatherPreset, this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab);
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00010870 File Offset: 0x0000EA70
	public void SetWeatherOverwrite(EnviroWeatherPreset preset)
	{
		if (preset == null)
		{
			return;
		}
		if (preset != this.Weather.currentActiveWeatherPreset)
		{
			for (int i = 0; i < this.Weather.WeatherPrefabs.Count; i++)
			{
				if (preset == this.Weather.WeatherPrefabs[i].weatherPreset)
				{
					this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab = this.Weather.WeatherPrefabs[i];
					this.Weather.currentActiveZone.currentActiveZoneWeatherPreset = preset;
					EnviroSkyMgr.instance.NotifyZoneWeatherChanged(preset, this.Weather.currentActiveZone);
				}
			}
		}
		EnviroSkyMgr.instance.InstantWeatherChange(this.Weather.currentActiveZone.currentActiveZoneWeatherPreset, this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab);
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00010948 File Offset: 0x0000EB48
	public void ChangeWeather(int weatherId)
	{
		if (weatherId < 0 || weatherId > this.Weather.WeatherPrefabs.Count)
		{
			return;
		}
		if (this.Weather.WeatherPrefabs[weatherId] != this.Weather.currentActiveWeatherPrefab)
		{
			this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab = this.Weather.WeatherPrefabs[weatherId];
			this.Weather.currentActiveZone.currentActiveZoneWeatherPreset = this.Weather.WeatherPrefabs[weatherId].weatherPreset;
			EnviroSkyMgr.instance.NotifyZoneWeatherChanged(this.Weather.WeatherPrefabs[weatherId].weatherPreset, this.Weather.currentActiveZone);
		}
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00010A04 File Offset: 0x0000EC04
	public void ChangeWeather(EnviroWeatherPreset preset)
	{
		if (preset == null)
		{
			return;
		}
		if (preset != this.Weather.currentActiveWeatherPreset)
		{
			for (int i = 0; i < this.Weather.WeatherPrefabs.Count; i++)
			{
				if (preset == this.Weather.WeatherPrefabs[i].weatherPreset)
				{
					this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab = this.Weather.WeatherPrefabs[i];
					this.Weather.currentActiveZone.currentActiveZoneWeatherPreset = preset;
					EnviroSkyMgr.instance.NotifyZoneWeatherChanged(preset, this.Weather.currentActiveZone);
				}
			}
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00010AB4 File Offset: 0x0000ECB4
	public void ChangeWeather(string weatherName)
	{
		for (int i = 0; i < this.Weather.WeatherPrefabs.Count; i++)
		{
			if (this.Weather.WeatherPrefabs[i].weatherPreset.Name == weatherName && this.Weather.WeatherPrefabs[i] != this.Weather.currentActiveWeatherPrefab)
			{
				this.ChangeWeather(i);
				EnviroSkyMgr.instance.NotifyZoneWeatherChanged(this.Weather.WeatherPrefabs[i].weatherPreset, this.Weather.currentActiveZone);
			}
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00010B58 File Offset: 0x0000ED58
	public void UpdateAudioSource(EnviroWeatherPreset i)
	{
		if (i != null && i.weatherSFX != null)
		{
			if (i.weatherSFX == this.Weather.currentAudioSource.audiosrc.clip)
			{
				if (this.Weather.currentAudioSource.audiosrc.volume < 0.1f)
				{
					this.Weather.currentAudioSource.FadeIn(i.weatherSFX);
				}
				return;
			}
			if (this.Weather.currentAudioSource == this.Audio.AudioSourceWeather)
			{
				this.Audio.AudioSourceWeather.FadeOut();
				this.Audio.AudioSourceWeather2.FadeIn(i.weatherSFX);
				this.Weather.currentAudioSource = this.Audio.AudioSourceWeather2;
				return;
			}
			if (this.Weather.currentAudioSource == this.Audio.AudioSourceWeather2)
			{
				this.Audio.AudioSourceWeather2.FadeOut();
				this.Audio.AudioSourceWeather.FadeIn(i.weatherSFX);
				this.Weather.currentAudioSource = this.Audio.AudioSourceWeather;
				return;
			}
		}
		else
		{
			this.Audio.AudioSourceWeather.FadeOut();
			this.Audio.AudioSourceWeather2.FadeOut();
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00010CAB File Offset: 0x0000EEAB
	public void RegisterZone(EnviroZone zoneToAdd)
	{
		this.Weather.zones.Add(zoneToAdd);
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00010CBE File Offset: 0x0000EEBE
	public void EnterZone(EnviroZone zone)
	{
		this.Weather.currentActiveZone = zone;
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000245B File Offset: 0x0000065B
	public void ExitZone()
	{
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00010CCC File Offset: 0x0000EECC
	public void UpdateParticleClouds(bool active)
	{
		if (this.particleClouds.layer1System == null || this.particleClouds.layer2System == null)
		{
			return;
		}
		if (active)
		{
			if (!this.cloudsSettings.dualLayerParticleClouds)
			{
				if (this.particleClouds.layer2System.gameObject.activeSelf)
				{
					this.particleClouds.layer2System.gameObject.SetActive(false);
				}
				this.particleClouds.layer1System.transform.position = new Vector3(this.particleClouds.layer1System.transform.position.x, base.transform.localScale.y * this.cloudsSettings.ParticleCloudsLayer1.height, this.particleClouds.layer1System.transform.position.z);
				Color value = this.cloudsSettings.ParticleCloudsLayer1.particleCloudsColor.Evaluate(this.GameTime.solarTime) * this.cloudsConfig.particleLayer1Brightness;
				value.a = this.cloudsConfig.particleLayer1Alpha;
				this.particleClouds.layer1Material.SetColor("_CloudsColor", value);
				if (!this.particleClouds.layer1System.gameObject.activeSelf)
				{
					this.particleClouds.layer1System.gameObject.SetActive(true);
					return;
				}
			}
			else
			{
				this.particleClouds.layer1System.transform.position = new Vector3(this.particleClouds.layer1System.transform.position.x, base.transform.localScale.y * this.cloudsSettings.ParticleCloudsLayer1.height, this.particleClouds.layer1System.transform.position.z);
				this.particleClouds.layer2System.transform.position = new Vector3(this.particleClouds.layer2System.transform.position.x, base.transform.localScale.y * this.cloudsSettings.ParticleCloudsLayer2.height, this.particleClouds.layer2System.transform.position.z);
				if (this.cloudsSettings.ParticleCloudsLayer1.height >= this.cloudsSettings.ParticleCloudsLayer2.height)
				{
					this.particleClouds.layer1Material.renderQueue = 3001;
					this.particleClouds.layer2Material.renderQueue = 3002;
				}
				else
				{
					this.particleClouds.layer1Material.renderQueue = 3002;
					this.particleClouds.layer2Material.renderQueue = 3001;
				}
				Color value2 = this.cloudsSettings.ParticleCloudsLayer1.particleCloudsColor.Evaluate(this.GameTime.solarTime) * this.cloudsConfig.particleLayer1Brightness;
				value2.a = this.cloudsConfig.particleLayer1Alpha;
				this.particleClouds.layer1Material.SetColor("_CloudsColor", value2);
				Color value3 = this.cloudsSettings.ParticleCloudsLayer2.particleCloudsColor.Evaluate(this.GameTime.solarTime) * this.cloudsConfig.particleLayer2Brightness;
				value3.a = this.cloudsConfig.particleLayer2Alpha;
				this.particleClouds.layer2Material.SetColor("_CloudsColor", value3);
				if (!this.particleClouds.layer1System.gameObject.activeSelf)
				{
					this.particleClouds.layer1System.gameObject.SetActive(true);
				}
				if (!this.particleClouds.layer2System.gameObject.activeSelf)
				{
					this.particleClouds.layer2System.gameObject.SetActive(true);
					return;
				}
			}
		}
		else
		{
			if (this.particleClouds.layer1System != null && this.particleClouds.layer1System.isPlaying)
			{
				this.particleClouds.layer1System.gameObject.SetActive(false);
			}
			if (this.particleClouds.layer2System != null && this.particleClouds.layer2System.isPlaying)
			{
				this.particleClouds.layer2System.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00011118 File Offset: 0x0000F318
	public void CreateSatellite(int id)
	{
		if (this.satelliteSettings.additionalSatellites[id].prefab == null)
		{
			Debug.Log("Satellite without prefab! Please assign a prefab to all satellites.");
			return;
		}
		GameObject gameObject = new GameObject();
		gameObject.name = this.satelliteSettings.additionalSatellites[id].name;
		gameObject.transform.parent = this.Components.satellites;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		this.satellitesRotation.Add(gameObject);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.satelliteSettings.additionalSatellites[id].prefab, gameObject.transform);
		gameObject2.layer = this.satelliteRenderingLayer;
		this.satellites.Add(gameObject2);
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x000111EC File Offset: 0x0000F3EC
	public void CheckSatellites()
	{
		this.satellites = new List<GameObject>();
		for (int i = this.Components.satellites.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.DestroyImmediate(this.Components.satellites.GetChild(i).gameObject);
		}
		this.satellites.Clear();
		this.satellitesRotation.Clear();
		for (int j = 0; j < this.satelliteSettings.additionalSatellites.Count; j++)
		{
			this.CreateSatellite(j);
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00011274 File Offset: 0x0000F474
	public void CalculateSatPositions(float siderealTime)
	{
		for (int i = 0; i < this.satelliteSettings.additionalSatellites.Count; i++)
		{
			Quaternion quaternion = Quaternion.Euler(90f - this.GameTime.Latitude, this.GameTime.Longitude, 0f);
			quaternion *= Quaternion.Euler(this.satelliteSettings.additionalSatellites[i].xRot, siderealTime, this.satelliteSettings.additionalSatellites[i].yRot);
			if (this.satellites.Count >= 1 && this.satellites.Count >= i)
			{
				this.satellites[i].transform.localPosition = new Vector3(0f, this.satelliteSettings.additionalSatellites[i].orbit, 0f);
			}
			if (this.satellitesRotation.Count >= 1 && this.satellitesRotation.Count >= i)
			{
				this.satellitesRotation[i].transform.localRotation = quaternion;
			}
		}
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0001138C File Offset: 0x0000F58C
	public void SetCameraHDR(Camera cam, bool hdr)
	{
		cam.allowHDR = hdr;
	}

	// Token: 0x060001FA RID: 506 RVA: 0x00011395 File Offset: 0x0000F595
	public bool GetCameraHDR(Camera cam)
	{
		return cam.allowHDR;
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0001139D File Offset: 0x0000F59D
	private Quaternion LightLookAt(Quaternion inputRotation, Quaternion newRotation)
	{
		return Quaternion.Lerp(inputRotation, newRotation, 500f * Time.deltaTime);
	}

	// Token: 0x060001FC RID: 508 RVA: 0x000113B1 File Offset: 0x0000F5B1
	public int RegisterMe(EnviroVegetationInstance me)
	{
		this.EnviroVegetationInstances.Add(me);
		return this.EnviroVegetationInstances.Count - 1;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x000113CC File Offset: 0x0000F5CC
	public void Save()
	{
		PlayerPrefs.SetFloat("Time_Hours", this.internalHour);
		PlayerPrefs.SetInt("Time_Days", this.GameTime.Days);
		PlayerPrefs.SetInt("Time_Years", this.GameTime.Years);
		for (int i = 0; i < this.Weather.WeatherPrefabs.Count; i++)
		{
			if (this.Weather.WeatherPrefabs[i] == this.Weather.currentActiveWeatherPrefab)
			{
				PlayerPrefs.SetInt("currentWeather", i);
			}
		}
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0001145C File Offset: 0x0000F65C
	public void Load()
	{
		if (PlayerPrefs.HasKey("Time_Hours"))
		{
			this.SetInternalTimeOfDay(PlayerPrefs.GetFloat("Time_Hours"));
		}
		if (PlayerPrefs.HasKey("Time_Days"))
		{
			this.GameTime.Days = PlayerPrefs.GetInt("Time_Days");
		}
		if (PlayerPrefs.HasKey("Time_Years"))
		{
			this.GameTime.Years = PlayerPrefs.GetInt("Time_Years");
		}
		if (PlayerPrefs.HasKey("currentWeather"))
		{
			this.SetWeatherOverwrite(PlayerPrefs.GetInt("currentWeather"));
		}
	}

	// Token: 0x0400028B RID: 651
	[Header("Profile")]
	public EnviroProfile profile;

	// Token: 0x0400028C RID: 652
	[HideInInspector]
	public bool profileLoaded;

	// Token: 0x0400028D RID: 653
	[Tooltip("Assign your player gameObject here. Required Field! or enable AssignInRuntime!")]
	public GameObject Player;

	// Token: 0x0400028E RID: 654
	[Tooltip("Assign your main camera here. Required Field! or enable AssignInRuntime!")]
	public Camera PlayerCamera;

	// Token: 0x0400028F RID: 655
	[Tooltip("If enabled Enviro will search for your Player and Camera by Tag!")]
	public bool AssignInRuntime;

	// Token: 0x04000290 RID: 656
	[Tooltip("Your Player Tag")]
	public string PlayerTag = "";

	// Token: 0x04000291 RID: 657
	[Tooltip("Your CameraTag")]
	public string CameraTag = "MainCamera";

	// Token: 0x04000292 RID: 658
	[Tooltip("Enables enviro tonemapping. Disable that one later and use a third party tonemapping effect for best results!")]
	public bool tonemapping;

	// Token: 0x04000293 RID: 659
	public EnviroCore.EnviroStartMode startMode;

	// Token: 0x04000294 RID: 660
	[HideInInspector]
	public bool started;

	// Token: 0x04000295 RID: 661
	[HideInInspector]
	public bool serverMode;

	// Token: 0x04000296 RID: 662
	[HideInInspector]
	public EnviroWeatherCloudsConfig cloudsConfig;

	// Token: 0x04000297 RID: 663
	[HideInInspector]
	public float thunder;

	// Token: 0x04000298 RID: 664
	[HideInInspector]
	public bool isNight = true;

	// Token: 0x04000299 RID: 665
	[HideInInspector]
	public List<GameObject> satellites = new List<GameObject>();

	// Token: 0x0400029A RID: 666
	[HideInInspector]
	public List<GameObject> satellitesRotation = new List<GameObject>();

	// Token: 0x0400029B RID: 667
	[HideInInspector]
	public List<EnviroVegetationInstance> EnviroVegetationInstances = new List<EnviroVegetationInstance>();

	// Token: 0x0400029C RID: 668
	[HideInInspector]
	public EnviroLightSettings lightSettings = new EnviroLightSettings();

	// Token: 0x0400029D RID: 669
	[HideInInspector]
	public EnviroVolumeLightingSettings volumeLightSettings = new EnviroVolumeLightingSettings();

	// Token: 0x0400029E RID: 670
	[HideInInspector]
	public EnviroSkySettings skySettings = new EnviroSkySettings();

	// Token: 0x0400029F RID: 671
	[HideInInspector]
	public EnviroReflectionSettings reflectionSettings = new EnviroReflectionSettings();

	// Token: 0x040002A0 RID: 672
	[HideInInspector]
	public EnviroCloudSettings cloudsSettings = new EnviroCloudSettings();

	// Token: 0x040002A1 RID: 673
	[HideInInspector]
	public EnviroWeatherSettings weatherSettings = new EnviroWeatherSettings();

	// Token: 0x040002A2 RID: 674
	[HideInInspector]
	public EnviroFogSettings fogSettings = new EnviroFogSettings();

	// Token: 0x040002A3 RID: 675
	[HideInInspector]
	public EnviroLightShaftsSettings lightshaftsSettings = new EnviroLightShaftsSettings();

	// Token: 0x040002A4 RID: 676
	[HideInInspector]
	public EnviroSeasonSettings seasonsSettings = new EnviroSeasonSettings();

	// Token: 0x040002A5 RID: 677
	[HideInInspector]
	public EnviroAudioSettings audioSettings = new EnviroAudioSettings();

	// Token: 0x040002A6 RID: 678
	[HideInInspector]
	public EnviroSatellitesSettings satelliteSettings = new EnviroSatellitesSettings();

	// Token: 0x040002A7 RID: 679
	[HideInInspector]
	public EnviroQualitySettings qualitySettings = new EnviroQualitySettings();

	// Token: 0x040002A8 RID: 680
	[HideInInspector]
	public EnviroInteriorZoneSettings interiorZoneSettings = new EnviroInteriorZoneSettings();

	// Token: 0x040002A9 RID: 681
	[HideInInspector]
	public EnviroDistanceBlurSettings distanceBlurSettings = new EnviroDistanceBlurSettings();

	// Token: 0x040002AA RID: 682
	[HideInInspector]
	public EnviroAuroraSettings auroraSettings = new EnviroAuroraSettings();

	// Token: 0x040002AB RID: 683
	[HideInInspector]
	public DateTime dateTime;

	// Token: 0x040002AC RID: 684
	[HideInInspector]
	public float internalHour;

	// Token: 0x040002AD RID: 685
	[HideInInspector]
	public float currentHour;

	// Token: 0x040002AE RID: 686
	[HideInInspector]
	public float currentDay;

	// Token: 0x040002AF RID: 687
	[HideInInspector]
	public float currentYear;

	// Token: 0x040002B0 RID: 688
	[HideInInspector]
	public double currentTimeInHours;

	// Token: 0x040002B1 RID: 689
	[HideInInspector]
	public float LST;

	// Token: 0x040002B2 RID: 690
	[HideInInspector]
	public float lastHourUpdate;

	// Token: 0x040002B3 RID: 691
	[HideInInspector]
	public float hourTime;

	// Token: 0x040002B4 RID: 692
	[HideInInspector]
	public Vector3 cloudAnim;

	// Token: 0x040002B5 RID: 693
	[HideInInspector]
	public Vector2 cloudFlatBaseAnim;

	// Token: 0x040002B6 RID: 694
	[HideInInspector]
	public Vector2 cloudFlatDetailAnim;

	// Token: 0x040002B7 RID: 695
	[HideInInspector]
	public Vector2 cloudAnimNonScaled;

	// Token: 0x040002B8 RID: 696
	[HideInInspector]
	public Vector2 cirrusAnim;

	// Token: 0x040002B9 RID: 697
	[HideInInspector]
	public float windIntensity;

	// Token: 0x040002BA RID: 698
	[HideInInspector]
	public float shadowIntensityMod;

	// Token: 0x040002BB RID: 699
	[HideInInspector]
	public bool interiorMode;

	// Token: 0x040002BC RID: 700
	[HideInInspector]
	public EnviroInterior lastInteriorZone;

	// Token: 0x040002BD RID: 701
	[HideInInspector]
	public bool updateFogDensity = true;

	// Token: 0x040002BE RID: 702
	[HideInInspector]
	public Color customFogColor = Color.black;

	// Token: 0x040002BF RID: 703
	[HideInInspector]
	public float customFogIntensity;

	// Token: 0x040002C0 RID: 704
	[HideInInspector]
	public Color currentWeatherSkyMod;

	// Token: 0x040002C1 RID: 705
	[HideInInspector]
	public Color currentWeatherLightMod;

	// Token: 0x040002C2 RID: 706
	[HideInInspector]
	public Color currentWeatherFogMod;

	// Token: 0x040002C3 RID: 707
	[HideInInspector]
	[Range(-2f, 2f)]
	public float customMoonPhase;

	// Token: 0x040002C4 RID: 708
	public Light MainLight;

	// Token: 0x040002C5 RID: 709
	public Light AdditionalLight;

	// Token: 0x040002C6 RID: 710
	public Transform MoonTransform;

	// Token: 0x040002C7 RID: 711
	[HideInInspector]
	public float lastAmbientSkyUpdate;

	// Token: 0x040002C8 RID: 712
	[HideInInspector]
	public double lastRelfectionUpdate;

	// Token: 0x040002C9 RID: 713
	[HideInInspector]
	public Vector3 lastRelfectionPositionUpdate;

	// Token: 0x040002CA RID: 714
	[HideInInspector]
	public GameObject EffectsHolder;

	// Token: 0x040002CB RID: 715
	public ParticleSystem lightningEffect;

	// Token: 0x040002CC RID: 716
	public const float pi = 3.1415927f;

	// Token: 0x040002CD RID: 717
	private Vector3 K = new Vector3(686f, 678f, 666f);

	// Token: 0x040002CE RID: 718
	private const float n = 1.0003f;

	// Token: 0x040002CF RID: 719
	private const float N = 2.545E+25f;

	// Token: 0x040002D0 RID: 720
	private const float pn = 0.035f;

	// Token: 0x040002D1 RID: 721
	public EnviroTime GameTime;

	// Token: 0x040002D2 RID: 722
	public EnviroAudio Audio;

	// Token: 0x040002D3 RID: 723
	public EnviroWeather Weather;

	// Token: 0x040002D4 RID: 724
	public EnviroSeasons Seasons;

	// Token: 0x040002D5 RID: 725
	public EnviroComponents Components;

	// Token: 0x040002D6 RID: 726
	public EnviroFogging Fog;

	// Token: 0x040002D7 RID: 727
	public EnviroLightshafts LightShafts;

	// Token: 0x040002D8 RID: 728
	public EnviroParticleCloud particleClouds;

	// Token: 0x040002D9 RID: 729
	[HideInInspector]
	public EnviroPostProcessing EnviroPostProcessing;

	// Token: 0x040002DA RID: 730
	[Header("Layer Setup")]
	[Tooltip("This is the layer id forfor the moon.")]
	public int moonRenderingLayer = 29;

	// Token: 0x040002DB RID: 731
	[Tooltip("This is the layer id for additional satellites like moons, planets.")]
	public int satelliteRenderingLayer = 30;

	// Token: 0x040002DC RID: 732
	[Tooltip("Activate to set recommended maincamera clear flag.")]
	public bool setCameraClearFlags = true;

	// Token: 0x040002DD RID: 733
	[HideInInspector]
	public int frames;

	// Token: 0x040002DE RID: 734
	[HideInInspector]
	public int lightingUpdateEachFrames = 10;

	// Token: 0x040002DF RID: 735
	public float currentSceneExposureMod = 1f;

	// Token: 0x040002E0 RID: 736
	public float currentSkyExposureMod = 1f;

	// Token: 0x040002E1 RID: 737
	public float currentLightIntensityMod = 1f;

	// Token: 0x02000072 RID: 114
	public enum EnviroStartMode
	{
		// Token: 0x040002E3 RID: 739
		Started,
		// Token: 0x040002E4 RID: 740
		Paused,
		// Token: 0x040002E5 RID: 741
		PausedButTimeProgress
	}
}
