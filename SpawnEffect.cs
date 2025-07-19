using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class SpawnEffect : MonoBehaviour
{
	// Token: 0x0600013A RID: 314 RVA: 0x0000AD38 File Offset: 0x00008F38
	private void Start()
	{
		this.shaderProperty = Shader.PropertyToID("_cutoff");
		this._renderer = base.GetComponent<Renderer>();
		this.ps = base.GetComponentInChildren<ParticleSystem>();
		this.ps.main.duration = this.spawnEffectTime;
		this.ps.Play();
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000AD94 File Offset: 0x00008F94
	private void Update()
	{
		if (this.timer < this.spawnEffectTime + this.pause)
		{
			this.timer += Time.deltaTime;
		}
		else
		{
			this.ps.Play();
			this.timer = 0f;
		}
		this._renderer.material.SetFloat(this.shaderProperty, this.fadeIn.Evaluate(Mathf.InverseLerp(0f, this.spawnEffectTime, this.timer)));
	}

	// Token: 0x040001A6 RID: 422
	public float spawnEffectTime = 2f;

	// Token: 0x040001A7 RID: 423
	public float pause = 1f;

	// Token: 0x040001A8 RID: 424
	public AnimationCurve fadeIn;

	// Token: 0x040001A9 RID: 425
	private ParticleSystem ps;

	// Token: 0x040001AA RID: 426
	private float timer;

	// Token: 0x040001AB RID: 427
	private Renderer _renderer;

	// Token: 0x040001AC RID: 428
	private int shaderProperty;
}
