using System;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class SelfDestroy : MonoBehaviour
{
	// Token: 0x06000B57 RID: 2903 RVA: 0x0007C3CC File Offset: 0x0007A5CC
	public void Start()
	{
		this.ps = base.GetComponent<ParticleSystem>();
		this.timer = 0f;
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0007C3E5 File Offset: 0x0007A5E5
	public void Update()
	{
		if (this.ps)
		{
			this.timer += Time.deltaTime;
			if (this.timer > 1f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x040013C6 RID: 5062
	private ParticleSystem ps;

	// Token: 0x040013C7 RID: 5063
	public float timer;
}
