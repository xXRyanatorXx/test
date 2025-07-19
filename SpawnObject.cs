using System;
using UnityEngine;

// Token: 0x02000275 RID: 629
public class SpawnObject : MonoBehaviour
{
	// Token: 0x06000EEF RID: 3823 RVA: 0x0009D828 File Offset: 0x0009BA28
	public void spawn()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.obj).transform.SetParent(base.transform);
	}

	// Token: 0x04001827 RID: 6183
	public GameObject obj;
}
