using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class ExtinguishableFire : MonoBehaviour
{
	// Token: 0x0600014A RID: 330 RVA: 0x0000B008 File Offset: 0x00009208
	private void Start()
	{
		this.m_isExtinguished = true;
		this.smokeParticleSystem.Stop();
		this.fireParticleSystem.Stop();
		base.StartCoroutine(this.StartingFire());
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000B034 File Offset: 0x00009234
	public void Extinguish()
	{
		if (this.m_isExtinguished)
		{
			return;
		}
		this.m_isExtinguished = true;
		base.StartCoroutine(this.Extinguishing());
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000B053 File Offset: 0x00009253
	private IEnumerator Extinguishing()
	{
		this.fireParticleSystem.Stop();
		this.smokeParticleSystem.time = 0f;
		this.smokeParticleSystem.Play();
		for (float elapsedTime = 0f; elapsedTime < 2f; elapsedTime += Time.deltaTime)
		{
			float d = Mathf.Max(0f, 1f - elapsedTime / 2f);
			this.fireParticleSystem.transform.localScale = Vector3.one * d;
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		this.smokeParticleSystem.Stop();
		this.fireParticleSystem.transform.localScale = Vector3.one;
		yield return new WaitForSeconds(4f);
		base.StartCoroutine(this.StartingFire());
		yield break;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000B062 File Offset: 0x00009262
	private IEnumerator StartingFire()
	{
		this.smokeParticleSystem.Stop();
		this.fireParticleSystem.time = 0f;
		this.fireParticleSystem.Play();
		for (float elapsedTime = 0f; elapsedTime < 2f; elapsedTime += Time.deltaTime)
		{
			float d = Mathf.Min(1f, elapsedTime / 2f);
			this.fireParticleSystem.transform.localScale = Vector3.one * d;
			yield return null;
		}
		this.fireParticleSystem.transform.localScale = Vector3.one;
		this.m_isExtinguished = false;
		yield break;
	}

	// Token: 0x040001B6 RID: 438
	public ParticleSystem fireParticleSystem;

	// Token: 0x040001B7 RID: 439
	public ParticleSystem smokeParticleSystem;

	// Token: 0x040001B8 RID: 440
	protected bool m_isExtinguished;

	// Token: 0x040001B9 RID: 441
	private const float m_FireStartingTime = 2f;
}
