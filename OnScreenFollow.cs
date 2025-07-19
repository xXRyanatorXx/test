using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class OnScreenFollow : MonoBehaviour
{
	// Token: 0x060005F7 RID: 1527 RVA: 0x0002F0B0 File Offset: 0x0002D2B0
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 0.5f;
		base.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
		base.transform.LookAt(base.transform.position - (this.target.transform.position - base.transform.position));
		base.transform.localRotation = Quaternion.Euler(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, 0f);
	}

	// Token: 0x040008E4 RID: 2276
	public Transform target;
}
