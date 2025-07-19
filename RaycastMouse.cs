using System;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class RaycastMouse : MonoBehaviour
{
	// Token: 0x06000A07 RID: 2567 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x00063F90 File Offset: 0x00062190
	private void Update()
	{
		Debug.Log("Text: ");
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.2f, 1 << LayerMask.NameToLayer("OpenableParts")))
		{
			if (raycastHit.rigidbody != null)
			{
				raycastHit.rigidbody.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
				return;
			}
			raycastHit.collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
		}
	}
}
