using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class DisableRend : MonoBehaviour
{
	// Token: 0x06000917 RID: 2327 RVA: 0x00058A00 File Offset: 0x00056C00
	private void Start()
	{
		if (base.gameObject.transform.parent && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name)
		{
			base.GetComponent<MeshRenderer>().enabled = true;
			return;
		}
		base.GetComponent<MeshRenderer>().enabled = false;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00058A90 File Offset: 0x00056C90
	public void disableREND()
	{
		if (base.transform.parent.parent != null)
		{
			if (!(base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name))
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
				return;
			}
		}
		else
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x00058B10 File Offset: 0x00056D10
	public void enableREND()
	{
		if (base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
