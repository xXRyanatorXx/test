using System;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class DemoCube : MonoBehaviour
{
	// Token: 0x06000EE4 RID: 3812 RVA: 0x0009D6A8 File Offset: 0x0009B8A8
	private void Update()
	{
		base.transform.position = Vector3.up * Mathf.Sin(Time.time) * 2f;
		base.transform.Rotate(Vector3.one * 90f * Time.deltaTime);
	}
}
