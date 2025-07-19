using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class GunAim : MonoBehaviour
{
	// Token: 0x0600015B RID: 347 RVA: 0x0000B2CB File Offset: 0x000094CB
	private void Start()
	{
		this.parentCamera = base.GetComponentInParent<Camera>();
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000B2DC File Offset: 0x000094DC
	private void Update()
	{
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		if (x <= (float)this.borderLeft || x >= (float)(Screen.width - this.borderRight) || y <= (float)this.borderBottom || y >= (float)(Screen.height - this.borderTop))
		{
			this.isOutOfBounds = true;
		}
		else
		{
			this.isOutOfBounds = false;
		}
		if (!this.isOutOfBounds)
		{
			base.transform.LookAt(this.parentCamera.ScreenToWorldPoint(new Vector3(x, y, 5f)));
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000B36D File Offset: 0x0000956D
	public bool GetIsOutOfBounds()
	{
		return this.isOutOfBounds;
	}

	// Token: 0x040001C2 RID: 450
	public int borderLeft;

	// Token: 0x040001C3 RID: 451
	public int borderRight;

	// Token: 0x040001C4 RID: 452
	public int borderTop;

	// Token: 0x040001C5 RID: 453
	public int borderBottom;

	// Token: 0x040001C6 RID: 454
	private Camera parentCamera;

	// Token: 0x040001C7 RID: 455
	private bool isOutOfBounds;
}
