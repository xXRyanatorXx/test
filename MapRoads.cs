using System;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class MapRoads : MonoBehaviour
{
	// Token: 0x06000CB7 RID: 3255 RVA: 0x0008CF10 File Offset: 0x0008B110
	private void Start()
	{
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform.tag == "RoadNode")
			{
				this.Target = UnityEngine.Object.Instantiate<GameObject>(this.roadimage, Vector3.zero, Quaternion.identity);
				this.Target.transform.SetParent(base.transform);
				this.Target.GetComponent<RectTransform>().anchoredPosition = new Vector2((transform.position.x - this.Target.transform.root.transform.position.x) / 3f, (transform.position.z - this.Target.transform.root.transform.position.z) / 3f);
			}
		}
	}

	// Token: 0x04001593 RID: 5523
	public GameObject Target;

	// Token: 0x04001594 RID: 5524
	public GameObject roadimage;
}
