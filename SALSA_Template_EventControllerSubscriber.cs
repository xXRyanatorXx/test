using System;
using CrazyMinnow.SALSA;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class SALSA_Template_EventControllerSubscriber : MonoBehaviour
{
	// Token: 0x060000DB RID: 219 RVA: 0x000091EC File Offset: 0x000073EC
	private void OnEnable()
	{
		EventController.AnimationStarting += this.OnAnimationStarting;
		EventController.AnimationON += this.OnAnimationON;
		EventController.AnimationEnding += this.OnAnimationEnding;
		EventController.AnimationOFF += this.OnAnimationOFF;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00009240 File Offset: 0x00007440
	private void OnDisable()
	{
		EventController.AnimationStarting -= this.OnAnimationStarting;
		EventController.AnimationON -= this.OnAnimationON;
		EventController.AnimationEnding -= this.OnAnimationEnding;
		EventController.AnimationOFF -= this.OnAnimationOFF;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00009291 File Offset: 0x00007491
	private void OnAnimationStarting(object sender, EventController.EventControllerNotificationArgs e)
	{
		if (e.eventName == this.componentEventName)
		{
			Debug.Log("EventController fired OnAnimationStarting for: " + this.componentEventName + " from sender: " + e.sender.name);
		}
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000092CB File Offset: 0x000074CB
	private void OnAnimationON(object sender, EventController.EventControllerNotificationArgs e)
	{
		if (e.eventName == this.componentEventName)
		{
			Debug.Log("EventController fired OnAnimationON for: " + this.componentEventName + " from sender: " + e.sender.name);
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00009305 File Offset: 0x00007505
	private void OnAnimationEnding(object sender, EventController.EventControllerNotificationArgs e)
	{
		if (e.eventName == this.componentEventName)
		{
			Debug.Log("EventController fired OnAnimationEnding for: " + this.componentEventName + " from sender: " + e.sender.name);
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000933F File Offset: 0x0000753F
	private void OnAnimationOFF(object sender, EventController.EventControllerNotificationArgs e)
	{
		if (e.eventName == this.componentEventName)
		{
			Debug.Log("EventController fired OnAnimationOFF for: " + this.componentEventName + " from sender: " + e.sender.name);
		}
	}

	// Token: 0x0400015E RID: 350
	public string componentEventName;
}
