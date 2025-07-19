using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
[ExecuteInEditMode]
public class EnviroCloudsAnimateWithTime : MonoBehaviour
{
	// Token: 0x06000495 RID: 1173 RVA: 0x00027254 File Offset: 0x00025454
	private void Update()
	{
		if (EnviroSky.instance == null)
		{
			return;
		}
		float num = EnviroSky.instance.Remap(EnviroSky.instance.GetTimeOfDay(), 0f, 24f, -1f, 1f);
		float num2 = EnviroSky.instance.GetTimeOfDay() + (float)EnviroSky.instance.GameTime.Days * 24f;
		EnviroSky.instance.cloudAnim = new Vector3(num * EnviroSky.instance.cloudsSettings.cloudsWindDirectionX, num * -1f, num * EnviroSky.instance.cloudsSettings.cloudsWindDirectionY);
		EnviroSky.instance.cloudAnimNonScaled = new Vector2(num2 * EnviroSky.instance.cloudsSettings.cloudsWindDirectionX, num2 * EnviroSky.instance.cloudsSettings.cloudsWindDirectionY);
	}
}
