using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class EnviroLightning : MonoBehaviour
{
	// Token: 0x060003A5 RID: 933 RVA: 0x000193A8 File Offset: 0x000175A8
	public void Lightning()
	{
		base.StartCoroutine(this.LightningBolt());
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x000193B7 File Offset: 0x000175B7
	public void StopLightning()
	{
		base.StopAllCoroutines();
		base.GetComponent<Light>().enabled = false;
		EnviroSkyMgr.instance.SetLightningFlashTrigger(0f);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x000193DA File Offset: 0x000175DA
	public IEnumerator LightningBolt()
	{
		base.GetComponent<Light>().enabled = true;
		float defaultIntensity = base.GetComponent<Light>().intensity;
		int flashCount = UnityEngine.Random.Range(2, 5);
		int num;
		for (int thisFlash = 0; thisFlash < flashCount; thisFlash = num + 1)
		{
			base.GetComponent<Light>().intensity = defaultIntensity * UnityEngine.Random.Range(1f, 1.5f);
			EnviroSkyMgr.instance.SetLightningFlashTrigger(UnityEngine.Random.Range(5f, 10f));
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.1f));
			base.GetComponent<Light>().intensity = defaultIntensity;
			EnviroSkyMgr.instance.SetLightningFlashTrigger(1f);
			num = thisFlash;
		}
		base.GetComponent<Light>().enabled = false;
		yield break;
	}
}
