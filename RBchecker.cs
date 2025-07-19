using System;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class RBchecker : MonoBehaviour
{
	// Token: 0x06000DC2 RID: 3522 RVA: 0x0009342C File Offset: 0x0009162C
	private void OnTriggerEnter(Collider other)
	{
		if (tools.MPrunning)
		{
			return;
		}
		if (!other.transform.parent && other.GetComponent<Rigidbody>() && !other.GetComponent<MainCarProperties>())
		{
			other.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x0009347C File Offset: 0x0009167C
	private void OnTriggerExit(Collider other)
	{
		if (tools.MPrunning)
		{
			return;
		}
		if (!other.transform.parent && other.GetComponent<Rigidbody>() && !other.GetComponent<MainCarProperties>() && !other.transform.parent)
		{
			other.GetComponent<Rigidbody>().isKinematic = true;
		}
	}
}
