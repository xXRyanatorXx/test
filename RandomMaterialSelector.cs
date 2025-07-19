using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200023C RID: 572
public class RandomMaterialSelector : MonoBehaviour
{
	// Token: 0x06000DAF RID: 3503 RVA: 0x00092DE0 File Offset: 0x00090FE0
	private void OnEnable()
	{
		int index = UnityEngine.Random.Range(1, this.materials.Count);
		base.GetComponent<ParticleSystemRenderer>().material = this.materials[index];
	}

	// Token: 0x04001631 RID: 5681
	public List<Material> materials = new List<Material>();
}
