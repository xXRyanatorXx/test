using System;
using UnityEngine;

// Token: 0x0200027F RID: 639
public class TouchController : MonoBehaviour
{
	// Token: 0x06000F10 RID: 3856 RVA: 0x0009DEB3 File Offset: 0x0009C0B3
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Switch>())
		{
			Debug.Log(other.GetComponent<Switch>().SwitchName);
		}
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0009DEB3 File Offset: 0x0009C0B3
	private void OnTriggerStay(Collider other)
	{
		if (other.GetComponent<Switch>())
		{
			Debug.Log(other.GetComponent<Switch>().SwitchName);
		}
	}
}
