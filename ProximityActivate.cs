using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class ProximityActivate : MonoBehaviour
{
	// Token: 0x0600016B RID: 363 RVA: 0x0000B7EC File Offset: 0x000099EC
	private void Start()
	{
		this.originRotation = base.transform.rotation;
		this.alpha = (float)(this.activeState ? 1 : -1);
		if (this.activator == null)
		{
			this.activator = Camera.main.transform;
		}
		this.infoIcon.SetActive(this.infoPanel != null);
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000B854 File Offset: 0x00009A54
	private bool IsTargetNear()
	{
		if ((this.distanceActivator.position - this.activator.position).sqrMagnitude < this.distance * this.distance)
		{
			if (this.lookAtActivator != null)
			{
				Vector3 vector = this.lookAtActivator.position - this.activator.position;
				if (Vector3.Dot(this.activator.forward, vector.normalized) > 0.95f)
				{
					return true;
				}
			}
			Vector3 vector2 = this.target.transform.position - this.activator.position;
			if (Vector3.Dot(this.activator.forward, vector2.normalized) > 0.95f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000B924 File Offset: 0x00009B24
	private void Update()
	{
		if (!this.activeState)
		{
			if (this.IsTargetNear())
			{
				this.alpha = 1f;
				this.activeState = true;
			}
		}
		else if (!this.IsTargetNear())
		{
			this.alpha = -1f;
			this.activeState = false;
			this.enableInfoPanel = false;
		}
		this.target.alpha = Mathf.Clamp01(this.target.alpha + this.alpha * Time.deltaTime);
		if (this.infoPanel != null)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.enableInfoPanel = !this.enableInfoPanel;
			}
			this.infoPanel.alpha = Mathf.Lerp(this.infoPanel.alpha, Mathf.Clamp01(this.enableInfoPanel ? this.alpha : 0f), Time.deltaTime * 10f);
		}
		if (this.lookAtCamera)
		{
			if (this.activeState)
			{
				this.targetRotation = Quaternion.LookRotation(this.activator.position - base.transform.position);
			}
			else
			{
				this.targetRotation = this.originRotation;
			}
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.targetRotation, Time.deltaTime);
		}
	}

	// Token: 0x040001E7 RID: 487
	public Transform distanceActivator;

	// Token: 0x040001E8 RID: 488
	public Transform lookAtActivator;

	// Token: 0x040001E9 RID: 489
	public float distance;

	// Token: 0x040001EA RID: 490
	public Transform activator;

	// Token: 0x040001EB RID: 491
	public bool activeState;

	// Token: 0x040001EC RID: 492
	public CanvasGroup target;

	// Token: 0x040001ED RID: 493
	public bool lookAtCamera = true;

	// Token: 0x040001EE RID: 494
	public bool enableInfoPanel;

	// Token: 0x040001EF RID: 495
	public GameObject infoIcon;

	// Token: 0x040001F0 RID: 496
	private float alpha;

	// Token: 0x040001F1 RID: 497
	public CanvasGroup infoPanel;

	// Token: 0x040001F2 RID: 498
	private Quaternion originRotation;

	// Token: 0x040001F3 RID: 499
	private Quaternion targetRotation;
}
