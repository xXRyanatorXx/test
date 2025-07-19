using System;
using CrazyMinnow.SALSA;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class SALSA_Template_SalsaEventSubscriber : MonoBehaviour
{
	// Token: 0x060000E2 RID: 226 RVA: 0x00009379 File Offset: 0x00007579
	private void OnEnable()
	{
		Salsa.StartedSalsaing += this.OnStartedSalsaing;
		Salsa.StoppedSalsaing += this.OnStoppedSalsaing;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000939D File Offset: 0x0000759D
	private void OnDisable()
	{
		Salsa.StartedSalsaing -= this.OnStartedSalsaing;
		Salsa.StoppedSalsaing -= this.OnStoppedSalsaing;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000093C1 File Offset: 0x000075C1
	private void OnStoppedSalsaing(object sender, Salsa.SalsaNotificationArgs e)
	{
		if (e.salsaInstance == this.salsa)
		{
			Debug.Log("SALSA fired OnStoppedSalsaing for: " + e.salsaInstance.name);
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000093F0 File Offset: 0x000075F0
	private void OnStartedSalsaing(object sender, Salsa.SalsaNotificationArgs e)
	{
		if (e.salsaInstance == this.salsa)
		{
			Debug.Log("SALSA fired OnStartedSalsaing for: " + e.salsaInstance.name);
		}
	}

	// Token: 0x0400015F RID: 351
	public Salsa salsa;
}
