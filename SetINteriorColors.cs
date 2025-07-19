using System;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class SetINteriorColors : MonoBehaviour
{
	// Token: 0x06000EB1 RID: 3761 RVA: 0x0009BBD5 File Offset: 0x00099DD5
	private void Start()
	{
		this.MaterialParent = GameObject.Find("MaterialParent");
		this.SphereCOl = GameObject.Find("SphereCollider");
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0009BBF8 File Offset: 0x00099DF8
	private void OnMouseDown()
	{
		if (Vector3.Distance(base.transform.position, this.SphereCOl.transform.position) < 3f)
		{
			for (int i = 0; i < this.Saleitems.Length; i++)
			{
				GameObject gameObject = this.Saleitems[i];
				if (this.interior == 1)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior1;
				}
				else if (this.interior == 2)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior2;
				}
				else if (this.interior == 3)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior3;
				}
				else if (this.interior == 4)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior4;
				}
				else if (this.interior == 5)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior5;
				}
				else if (this.interior == 6)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior6;
				}
				else if (this.interior == 7)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior7;
				}
				else if (this.interior == 8)
				{
					gameObject.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior8;
				}
				gameObject.GetComponent<SaleItem>().interior = this.interior;
			}
		}
	}

	// Token: 0x040017EA RID: 6122
	public GameObject SphereCOl;

	// Token: 0x040017EB RID: 6123
	public int interior;

	// Token: 0x040017EC RID: 6124
	public GameObject[] Saleitems;

	// Token: 0x040017ED RID: 6125
	public GameObject MaterialParent;
}
