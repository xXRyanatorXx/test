using System;
using AwesomeTechnologies.VegetationStudio;
using AwesomeTechnologies.VegetationSystem;
using UnityEngine;

// Token: 0x02000061 RID: 97
[AddComponentMenu("Enviro/Integration/VS Pro Integration")]
public class EnviroVegetationStudioPro : MonoBehaviour
{
	// Token: 0x060001A9 RID: 425 RVA: 0x0000C578 File Offset: 0x0000A778
	private void Start()
	{
		if (VegetationStudioManager.Instance == null || EnviroSkyMgr.instance == null)
		{
			return;
		}
		if (this.setWindZone)
		{
			for (int i = 0; i < VegetationStudioManager.Instance.VegetationSystemList.Count; i++)
			{
				VegetationStudioManager.Instance.VegetationSystemList[i].SelectedWindZone = EnviroSkyMgr.instance.Components.windZone;
			}
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000C5E8 File Offset: 0x0000A7E8
	private void Update()
	{
		if (VegetationStudioManager.Instance == null || EnviroSkyMgr.instance == null)
		{
			return;
		}
		foreach (VegetationSystemPro vegetationSystemPro in VegetationStudioManager.Instance.VegetationSystemList)
		{
			if (vegetationSystemPro.enabled)
			{
				EnviroWeather weather = EnviroSkyMgr.instance.Weather;
				EnvironmentSettings environmentSettings = vegetationSystemPro.EnvironmentSettings;
				bool flag = false;
				if (this.syncRain && Mathf.Abs(environmentSettings.RainAmount - weather.wetness) >= 0.01f)
				{
					flag = true;
				}
				if (this.syncSnow && Mathf.Abs(environmentSettings.SnowAmount - weather.snowStrength) >= 0.01f)
				{
					flag = true;
				}
				if (this.syncRain && flag)
				{
					environmentSettings.RainAmount = weather.wetness;
				}
				if (this.syncSnow && flag)
				{
					environmentSettings.SnowAmount = weather.snowStrength;
				}
				if (flag)
				{
					vegetationSystemPro.RefreshMaterials();
				}
			}
		}
	}

	// Token: 0x04000214 RID: 532
	private const float updatePrecision = 0.01f;

	// Token: 0x04000215 RID: 533
	public bool setWindZone = true;

	// Token: 0x04000216 RID: 534
	public bool syncRain = true;

	// Token: 0x04000217 RID: 535
	public bool syncSnow = true;
}
