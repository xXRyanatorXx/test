using System;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class adjustPosition : MonoBehaviour
{
	// Token: 0x06000C98 RID: 3224 RVA: 0x0008C890 File Offset: 0x0008AA90
	private void Update()
	{
		this.H = this.mainbody.GetComponent<CapsuleCollider>().height / 2f;
		base.gameObject.transform.position = new Vector3(base.transform.parent.position.x, this.mainbody.transform.position.y - this.H - 0.1f, base.transform.parent.position.z);
	}

	// Token: 0x04001578 RID: 5496
	public GameObject mainbody;

	// Token: 0x04001579 RID: 5497
	public float H;
}
