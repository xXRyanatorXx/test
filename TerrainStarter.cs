using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class TerrainStarter : MonoBehaviour
{
	// Token: 0x06000515 RID: 1301 RVA: 0x0002A038 File Offset: 0x00028238
	private void Start()
	{
		this.index = UnityEngine.Random.Range(0, this.Roads.Length);
		this.SelectedRoad = this.Roads[this.index];
		UnityEngine.Object.Instantiate<GameObject>(this.SelectedRoad, new Vector3(base.transform.position.x, 0f, base.transform.position.z), Quaternion.identity).transform.SetParent(base.transform);
	}

	// Token: 0x040006D1 RID: 1745
	public GameObject[] Roads;

	// Token: 0x040006D2 RID: 1746
	public GameObject SelectedRoad;

	// Token: 0x040006D3 RID: 1747
	private int index;
}
