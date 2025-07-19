using System;
using CrazyMinnow.SALSA;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class SALSA_Template_SalsaVisemeTriggerEventSubscriber : MonoBehaviour
{
	// Token: 0x060000E7 RID: 231 RVA: 0x0000941F File Offset: 0x0000761F
	private void OnEnable()
	{
		Salsa.VisemeTriggered += this.SalsaOnVisemeTriggered;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00009432 File Offset: 0x00007632
	private void OnDisable()
	{
		Salsa.VisemeTriggered -= this.SalsaOnVisemeTriggered;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00009445 File Offset: 0x00007645
	private void SalsaOnVisemeTriggered(object sender, Salsa.SalsaNotificationArgs e)
	{
		if (e.salsaInstance == this.salsaInstance)
		{
			Debug.Log("Viseme triggered: " + e.visemeTrigger.ToString());
		}
	}

	// Token: 0x04000160 RID: 352
	[SerializeField]
	private Salsa salsaInstance;
}
