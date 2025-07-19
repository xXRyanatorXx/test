using System;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class UnmountLoose : MonoBehaviour
{
	// Token: 0x06000C91 RID: 3217 RVA: 0x0008C6C4 File Offset: 0x0008A8C4
	public void CheckChildren()
	{
		foreach (Transform transform in base.GetComponentsInChildren<Transform>())
		{
			if (transform.GetComponent<Partinfo>() != null)
			{
				transform.GetComponent<Partinfo>().remove(true);
			}
		}
	}
}
