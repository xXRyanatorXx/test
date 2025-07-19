using System;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class PanZoom : MonoBehaviour
{
	// Token: 0x06000D9B RID: 3483 RVA: 0x000929A8 File Offset: 0x00090BA8
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		if (Input.touchCount == 2)
		{
			Touch touch = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			Vector2 a = touch.position - touch.deltaPosition;
			Vector2 b = touch2.position - touch2.deltaPosition;
			float magnitude = (a - b).magnitude;
			float num = (touch.position - touch2.position).magnitude - magnitude;
			this.zoom(num * 0.01f);
		}
		else if (Input.GetMouseButton(0))
		{
			Vector3 b2 = this.touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			base.gameObject.transform.position -= b2;
		}
		this.zoom(Input.GetAxis("Mouse ScrollWheel"));
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x00092AA0 File Offset: 0x00090CA0
	private void zoom(float increment)
	{
		float num = Mathf.Clamp(base.gameObject.transform.localScale.x + increment, this.zoomOutMin, this.zoomOutMax);
		base.gameObject.transform.localScale = new Vector3(num, num, 0f);
	}

	// Token: 0x0400161E RID: 5662
	private Vector3 touchStart;

	// Token: 0x0400161F RID: 5663
	public float zoomOutMin = 1f;

	// Token: 0x04001620 RID: 5664
	public float zoomOutMax = 8f;
}
