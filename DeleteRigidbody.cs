using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class DeleteRigidbody : MonoBehaviour
{
	// Token: 0x060000F9 RID: 249 RVA: 0x00009990 File Offset: 0x00007B90
	private void OnCollisionEnter(Collision col)
	{
		if (tools.tool != 22 && base.gameObject.GetComponent<Rigidbody>() && col.gameObject.layer == LayerMask.NameToLayer("Default"))
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000099E0 File Offset: 0x00007BE0
	private void OnCollisionStay(Collision col)
	{
		if (tools.tool != 22 && base.gameObject.GetComponent<Rigidbody>() && col.gameObject.layer == LayerMask.NameToLayer("Default"))
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			UnityEngine.Object.Destroy(this);
		}
	}
}
