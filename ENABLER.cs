using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class ENABLER : MonoBehaviour
{
	// Token: 0x060006AC RID: 1708 RVA: 0x00035A87 File Offset: 0x00033C87
	private void OnTriggerEnter(Collider other)
	{
		if ((other.tag == "MpPlayer" || other.tag == "Player") && tools.GameLoaded)
		{
			this.GO();
		}
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00035ABC File Offset: 0x00033CBC
	public void GO()
	{
		if (this.obj && !this.obj.activeSelf && (!tools.MPrunning || (tools.MPrunning && tools.NetworkPLayer.isServer)))
		{
			GameObject.Find("Player").GetComponent<tools>().Loadingtext.SetActive(true);
			if (this.Gate)
			{
				base.StartCoroutine(this.LerpPosition());
			}
			base.StartCoroutine(this.WaitStart());
		}
		if (this.SetStart && (!tools.MPrunning || (tools.MPrunning && tools.NetworkPLayer.isServer)))
		{
			this.SetStart.GO();
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00035B70 File Offset: 0x00033D70
	private IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(0.1f);
		if (this.obj)
		{
			this.obj.SetActive(true);
		}
		yield return new WaitForSeconds(2f);
		if (this.obj)
		{
			this.obj.SetActive(true);
		}
		yield return new WaitForSeconds(5f);
		yield break;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00035B7F File Offset: 0x00033D7F
	private IEnumerator LerpPosition()
	{
		float time = 0f;
		Vector3 startPosition = this.Gate.transform.position;
		while (time < 15f)
		{
			this.Gate.transform.position = Vector3.Lerp(startPosition, this.GateOpen.transform.position, time / 15f);
			time += Time.deltaTime;
			yield return null;
		}
		this.Gate.transform.position = this.GateOpen.transform.position;
		yield break;
	}

	// Token: 0x04000A2C RID: 2604
	public GameObject Gate;

	// Token: 0x04000A2D RID: 2605
	public GameObject GateOpen;

	// Token: 0x04000A2E RID: 2606
	public GameObject GateClosed;

	// Token: 0x04000A2F RID: 2607
	public GameObject obj;

	// Token: 0x04000A30 RID: 2608
	public SetStart SetStart;
}
