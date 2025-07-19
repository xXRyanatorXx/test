using System;
using UnityEngine;

// Token: 0x02000286 RID: 646
public class UpdateMapPosition : MonoBehaviour
{
	// Token: 0x06000F2E RID: 3886 RVA: 0x0009E204 File Offset: 0x0009C404
	private void Update()
	{
		if (this.Target)
		{
			base.GetComponent<RectTransform>().anchoredPosition = new Vector2((this.Target.transform.root.position.x - base.transform.root.transform.position.x) / 3f, (this.Target.transform.root.position.z - base.transform.root.transform.position.z) / 3f);
		}
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x0009E2A8 File Offset: 0x0009C4A8
	public void SCALE()
	{
		base.transform.localScale = new Vector3(this.FixeScale / base.transform.parent.parent.transform.localScale.x, this.FixeScale / base.transform.parent.parent.transform.localScale.y, this.FixeScale / base.transform.parent.parent.transform.localScale.z);
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0009E338 File Offset: 0x0009C538
	public void AcceptDelivery()
	{
		this.DeliveryTarget.GetComponent<MeshRenderer>().enabled = true;
		this.DeliveryTarget.GetComponent<CapsuleCollider>().enabled = true;
		tools.PizzaDeliveriesCount++;
		this.DeliveryTarget.GetComponent<DeliveryTarget>().Accepted = true;
		base.transform.SetParent(this.DeliveryTarget.GetComponent<DeliveryTarget>().DeliveryBook.AcceptedPizzas.transform);
		this.DeliveryTarget.GetComponent<DeliveryTarget>().DeliveryBook.SpawnDeliveryObject();
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0009E3BE File Offset: 0x0009C5BE
	public void AbandonDelivery()
	{
		tools.PizzaDeliveriesCount--;
		tools.money -= 30f;
		UnityEngine.Object.Destroy(this.DeliveryTarget);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001854 RID: 6228
	public Transform Target;

	// Token: 0x04001855 RID: 6229
	public GameObject DeliveryTarget;

	// Token: 0x04001856 RID: 6230
	public float FixeScale = 1f;

	// Token: 0x04001857 RID: 6231
	public GameObject AcceptedButton;

	// Token: 0x04001858 RID: 6232
	public GameObject NotAcceptedButton;
}
