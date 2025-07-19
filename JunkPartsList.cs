using System;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class JunkPartsList : MonoBehaviour
{
	// Token: 0x0600071E RID: 1822 RVA: 0x0003A627 File Offset: 0x00038827
	private void Start()
	{
		if (this.catalogmanager)
		{
			this.ItemCardPrefab = this.catalogmanager.ItemCardPrefab;
			this.SetCatalog();
		}
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0003A650 File Offset: 0x00038850
	private void SetCatalog()
	{
		for (int i = 0; i < this.Parts.Length; i++)
		{
			GameObject gameObject = this.Parts[i];
			if (!gameObject.GetComponent<Partinfo>().DontShowInCatalog)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.ItemCardPrefab, Vector3.zero, Quaternion.identity);
				gameObject2.transform.SetParent(this.catalogmanager.transform);
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.GetComponent<SHOPitem>().ITEM = gameObject;
				gameObject2.SetActive(false);
			}
		}
	}

	// Token: 0x04000B20 RID: 2848
	public GameObject[] Parts;

	// Token: 0x04000B21 RID: 2849
	public CatalogueManager catalogmanager;

	// Token: 0x04000B22 RID: 2850
	public GameObject ItemCardPrefab;
}
