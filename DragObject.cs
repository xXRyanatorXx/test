using System;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class DragObject : MonoBehaviour
{
	// Token: 0x06000BC8 RID: 3016 RVA: 0x00082D30 File Offset: 0x00080F30
	private void OnMouseDown()
	{
		this.mZCoord = Camera.main.WorldToScreenPoint(base.gameObject.transform.position).z;
		this.mOffset = base.gameObject.transform.position - this.GetMouseAsWorldPoint();
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00082D84 File Offset: 0x00080F84
	private Vector3 GetMouseAsWorldPoint()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = this.mZCoord;
		return Camera.main.ScreenToWorldPoint(mousePosition);
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00082DAF File Offset: 0x00080FAF
	private void OnMouseDrag()
	{
		base.transform.position = this.GetMouseAsWorldPoint() + this.mOffset;
	}

	// Token: 0x04001471 RID: 5233
	private Vector3 mOffset;

	// Token: 0x04001472 RID: 5234
	private float mZCoord;
}
