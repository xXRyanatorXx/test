using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class InsideItems : MonoBehaviour
{
	// Token: 0x06000521 RID: 1313 RVA: 0x0002A20C File Offset: 0x0002840C
	private void Start()
	{
		this.ObjectList = new List<GameObject>();
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.transform.root.GetComponent<MainCarProperties>().insideitems = this;
		}
		if (base.transform.root.GetComponent<MainTrailerProperties>())
		{
			base.transform.root.GetComponent<MainTrailerProperties>().insideitems = this;
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0002A27E File Offset: 0x0002847E
	public void Sit()
	{
		if (!tools.MPrunning)
		{
			this.ObjectList.Clear();
			this.InsideCollider.SetActive(true);
		}
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0002A2A0 File Offset: 0x000284A0
	public void Stand()
	{
		this.ObjectList.RemoveAll((GameObject item) => item == null);
		foreach (GameObject gameObject in this.ObjectList)
		{
			if (!gameObject.GetComponent<Rigidbody>())
			{
				gameObject.AddComponent<Rigidbody>();
			}
			gameObject.transform.SetParent(null);
		}
	}

	// Token: 0x040006D8 RID: 1752
	public GameObject InsideCollider;

	// Token: 0x040006D9 RID: 1753
	public List<GameObject> ObjectList;
}
