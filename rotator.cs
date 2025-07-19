using System;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class rotator : MonoBehaviour
{
	// Token: 0x0600114B RID: 4427 RVA: 0x000A5BDC File Offset: 0x000A3DDC
	private void Update()
	{
		this.brush1.Rotate(Vector3.forward, this.speed * Time.deltaTime);
		this.brush2.Rotate(Vector3.forward, this.speed * Time.deltaTime);
		this.brush3.Rotate(Vector3.forward, this.speed * Time.deltaTime);
	}

	// Token: 0x040018A8 RID: 6312
	public Transform brush1;

	// Token: 0x040018A9 RID: 6313
	public Transform brush2;

	// Token: 0x040018AA RID: 6314
	public Transform brush3;

	// Token: 0x040018AB RID: 6315
	public float speed;
}
