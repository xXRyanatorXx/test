using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200028C RID: 652
public class carwash : MonoBehaviour
{
	// Token: 0x06000F48 RID: 3912 RVA: 0x0009ECA7 File Offset: 0x0009CEA7
	private void OnTriggerEnter(Collider other)
	{
		if (!this.WashingInProgress && other.GetComponent<MainCarProperties>() && tools.money > 50f)
		{
			base.StartCoroutine(this.Washing());
		}
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0009ECD7 File Offset: 0x0009CED7
	private IEnumerator Washing()
	{
		tools.money -= 50f;
		tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Cash);
		this.ps1.Play();
		this.ps2.Play();
		this.ps3.Play();
		this.WashingInProgress = true;
		this.water.SetActive(true);
		yield return new WaitForSeconds(20f);
		this.WashingInProgress = false;
		this.water.SetActive(false);
		this.ps1.Stop();
		this.ps2.Stop();
		this.ps3.Stop();
		yield break;
	}

	// Token: 0x04001876 RID: 6262
	public bool WashingInProgress;

	// Token: 0x04001877 RID: 6263
	public GameObject water;

	// Token: 0x04001878 RID: 6264
	public ParticleSystem ps1;

	// Token: 0x04001879 RID: 6265
	public ParticleSystem ps2;

	// Token: 0x0400187A RID: 6266
	public ParticleSystem ps3;
}
