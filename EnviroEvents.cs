using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000075 RID: 117
public class EnviroEvents : MonoBehaviour
{
	// Token: 0x0600020C RID: 524 RVA: 0x000117E8 File Offset: 0x0000F9E8
	private void Start()
	{
		EnviroSkyMgr.instance.OnHourPassed += delegate()
		{
			this.HourPassed();
		};
		EnviroSkyMgr.instance.OnDayPassed += delegate()
		{
			this.DayPassed();
		};
		EnviroSkyMgr.instance.OnYearPassed += delegate()
		{
			this.YearPassed();
		};
		EnviroSkyMgr.instance.OnWeatherChanged += delegate(EnviroWeatherPreset type)
		{
			this.WeatherChanged();
		};
		EnviroSkyMgr.instance.OnSeasonChanged += delegate(EnviroSeasons.Seasons season)
		{
			this.SeasonsChanged();
		};
		EnviroSkyMgr.instance.OnNightTime += delegate()
		{
			this.NightTime();
		};
		EnviroSkyMgr.instance.OnDayTime += delegate()
		{
			this.DayTime();
		};
		EnviroSkyMgr.instance.OnZoneChanged += delegate(EnviroZone zone)
		{
			this.ZoneChanged();
		};
	}

	// Token: 0x0600020D RID: 525 RVA: 0x000118A5 File Offset: 0x0000FAA5
	private void HourPassed()
	{
		this.onHourPassedActions.Invoke();
	}

	// Token: 0x0600020E RID: 526 RVA: 0x000118B2 File Offset: 0x0000FAB2
	private void DayPassed()
	{
		this.onDayPassedActions.Invoke();
	}

	// Token: 0x0600020F RID: 527 RVA: 0x000118BF File Offset: 0x0000FABF
	private void YearPassed()
	{
		this.onYearPassedActions.Invoke();
	}

	// Token: 0x06000210 RID: 528 RVA: 0x000118CC File Offset: 0x0000FACC
	private void WeatherChanged()
	{
		this.onWeatherChangedActions.Invoke();
	}

	// Token: 0x06000211 RID: 529 RVA: 0x000118D9 File Offset: 0x0000FAD9
	private void SeasonsChanged()
	{
		this.onSeasonChangedActions.Invoke();
	}

	// Token: 0x06000212 RID: 530 RVA: 0x000118E6 File Offset: 0x0000FAE6
	private void NightTime()
	{
		this.onNightActions.Invoke();
	}

	// Token: 0x06000213 RID: 531 RVA: 0x000118F3 File Offset: 0x0000FAF3
	private void DayTime()
	{
		this.onDayActions.Invoke();
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00011900 File Offset: 0x0000FB00
	private void ZoneChanged()
	{
		this.onZoneChangedActions.Invoke();
	}

	// Token: 0x040002ED RID: 749
	public EnviroEvents.EnviroActionEvent onHourPassedActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x040002EE RID: 750
	public EnviroEvents.EnviroActionEvent onDayPassedActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x040002EF RID: 751
	public EnviroEvents.EnviroActionEvent onYearPassedActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x040002F0 RID: 752
	public EnviroEvents.EnviroActionEvent onWeatherChangedActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x040002F1 RID: 753
	public EnviroEvents.EnviroActionEvent onSeasonChangedActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x040002F2 RID: 754
	public EnviroEvents.EnviroActionEvent onNightActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x040002F3 RID: 755
	public EnviroEvents.EnviroActionEvent onDayActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x040002F4 RID: 756
	public EnviroEvents.EnviroActionEvent onZoneChangedActions = new EnviroEvents.EnviroActionEvent();

	// Token: 0x02000076 RID: 118
	[Serializable]
	public class EnviroActionEvent : UnityEvent
	{
	}
}
