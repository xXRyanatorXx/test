using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class RoadNodeStart : MonoBehaviour
{
	// Token: 0x06000C6D RID: 3181 RVA: 0x0008B7C0 File Offset: 0x000899C0
	private void Start()
	{
		if (base.transform.position.y < 51f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (base.transform.position.y > 160f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!base.transform.parent.GetComponent<GenerateRoad>())
		{
			base.transform.parent.gameObject.AddComponent<GenerateRoad>();
			base.transform.parent.GetComponent<GenerateRoad>().listOfAllTransforms = new List<Transform>();
		}
		base.transform.parent.GetComponent<GenerateRoad>().listOfAllTransforms.Add(base.transform);
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x0008B87C File Offset: 0x00089A7C
	private void backup()
	{
		if (base.transform.position.y < 51f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (!base.transform.parent.GetComponent<GenerateRoad>())
		{
			base.transform.parent.gameObject.AddComponent<GenerateRoad>();
			base.transform.parent.GetComponent<GenerateRoad>().listOfTransforms = new List<Transform>();
			base.transform.parent.GetComponent<GenerateRoad>().listOfTransforms.Add(base.transform);
			base.transform.parent.GetComponent<GenerateRoad>().lastTransform = base.transform;
			return;
		}
		if (base.transform.parent.GetComponent<GenerateRoad>() && base.transform.parent.GetComponent<GenerateRoad>().lastTransform == null)
		{
			base.transform.parent.GetComponent<GenerateRoad>().listOfTransforms = new List<Transform>();
			base.transform.parent.GetComponent<GenerateRoad>().listOfTransforms.Add(base.transform);
			base.transform.parent.GetComponent<GenerateRoad>().lastTransform = base.transform;
			return;
		}
		if (Vector3.Distance(base.transform.parent.GetComponent<GenerateRoad>().lastTransform.localPosition, base.transform.localPosition) < 5f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (Vector3.Distance(base.transform.parent.GetComponent<GenerateRoad>().lastTransform.localPosition, base.transform.localPosition) < 60f)
		{
			base.transform.parent.GetComponent<GenerateRoad>().lastTransform = base.transform;
			base.transform.parent.GetComponent<GenerateRoad>().listOfTransforms.Add(base.transform);
			return;
		}
		base.transform.parent.GetComponent<GenerateRoad>().CreateRoad();
		base.transform.parent.GetComponent<GenerateRoad>().listOfTransforms = new List<Transform>();
		base.transform.parent.GetComponent<GenerateRoad>().listOfTransforms.Add(base.transform);
		base.transform.parent.GetComponent<GenerateRoad>().lastTransform = base.transform;
	}
}
