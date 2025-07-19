using System;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class MPdisablecoll : MonoBehaviour
{
	// Token: 0x060005BF RID: 1471 RVA: 0x0002BDE9 File Offset: 0x00029FE9
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "FIrstEnableWithPlayer")
		{
			other.GetComponent<ENABLER>().GO();
		}
	}
}
