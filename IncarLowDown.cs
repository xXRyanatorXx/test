using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class IncarLowDown : MonoBehaviour
{
	// Token: 0x060005F4 RID: 1524 RVA: 0x0002F07A File Offset: 0x0002D27A
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			FirstPersonAIO.isCrouching2 = true;
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0002F094 File Offset: 0x0002D294
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			FirstPersonAIO.isCrouching2 = false;
		}
	}
}
