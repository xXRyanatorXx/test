using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200028A RID: 650
public class ZoomImage : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0009E967 File Offset: 0x0009CB67
	private RectTransform rectTransform
	{
		get
		{
			return base.transform as RectTransform;
		}
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x0009E974 File Offset: 0x0009CB74
	private void Awake()
	{
		this.initialScale = base.transform.localScale;
		UpdateMapPosition[] componentsInChildren = base.GetComponentsInChildren<UpdateMapPosition>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SCALE();
		}
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x0009E9B0 File Offset: 0x0009CBB0
	public void OnScroll(PointerEventData eventData)
	{
		Vector3 b = Vector3.one * (eventData.scrollDelta.y * this.zoomSpeed);
		Vector3 vector = base.transform.localScale + b;
		Vector3 lhs = vector;
		vector = this.ClampDesiredScale(vector);
		if (lhs == vector)
		{
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, Input.mousePosition, base.GetComponentInParent<Canvas>().worldCamera, out point);
			Vector2 vector2 = Rect.PointToNormalized(this.rectTransform.rect, point);
			Vector3 vector3 = this.rectTransform.pivot - vector2;
			vector3.Scale(this.rectTransform.rect.size);
			vector3.Scale(this.rectTransform.localScale);
			vector3 = this.rectTransform.rotation * vector3;
			this.rectTransform.pivot = vector2;
			this.rectTransform.localPosition -= vector3;
		}
		vector = this.ClampDesiredScale(vector);
		base.transform.localScale = vector;
		UpdateMapPosition[] componentsInChildren = base.GetComponentsInChildren<UpdateMapPosition>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SCALE();
		}
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x0009EAF0 File Offset: 0x0009CCF0
	private Vector3 ClampDesiredScale(Vector3 desiredScale)
	{
		desiredScale = Vector3.Max(this.initialScale, desiredScale);
		desiredScale = Vector3.Min(this.initialScale * this.maxZoom, desiredScale);
		return desiredScale;
	}

	// Token: 0x0400186C RID: 6252
	private Vector3 initialScale;

	// Token: 0x0400186D RID: 6253
	[SerializeField]
	private float zoomSpeed = 0.1f;

	// Token: 0x0400186E RID: 6254
	[SerializeField]
	private float maxZoom = 10f;
}
