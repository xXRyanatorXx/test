using System;
using System.Collections;
using NWH.VehiclePhysics2;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class DisablerCollider : MonoBehaviour
{
	// Token: 0x060005C6 RID: 1478 RVA: 0x0002C1BC File Offset: 0x0002A3BC
	private void Start()
	{
		Physics.OverlapSphere(base.transform.position, 10f);
		this.hand = GameObject.Find("ToolHand");
		this.RestartJump();
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0002C1EC File Offset: 0x0002A3EC
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<DISABLER>() != null)
		{
			other.GetComponent<DISABLER>().EnableOtherScripts();
		}
		if (other.GetComponent<MainCarProperties>() != null)
		{
			other.GetComponent<MainCarProperties>().SetAwake();
			other.GetComponent<VehicleController>().enabled = true;
		}
		if (other.GetComponent<MainTrailerProperties>() != null)
		{
			other.GetComponent<MainTrailerProperties>().SetAwake();
		}
		if (other.tag == "ElectricityZone")
		{
			tools.electricity = true;
		}
		if (other.tag == "JunkZone")
		{
			tools.JunkZone = true;
			tools.tool = 18;
		}
		if (other.tag == "FIrstEnableWithPlayer" && tools.GameLoaded)
		{
			other.GetComponent<ENABLER>().GO();
		}
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0002C2AC File Offset: 0x0002A4AC
	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<DISABLER>() != null)
		{
			other.GetComponent<DISABLER>().DisableOtherScripts();
		}
		if (other.GetComponent<MainCarProperties>() != null)
		{
			other.GetComponent<MainCarProperties>().SetSleep();
		}
		if (other.GetComponent<MainCarProperties>() != null && !tools.sitting)
		{
			other.GetComponent<VehicleController>().enabled = false;
		}
		if (other.GetComponent<MainTrailerProperties>() != null)
		{
			other.GetComponent<MainTrailerProperties>().SetSleep();
		}
		if (other.tag == "ElectricityZone")
		{
			tools.electricity = false;
		}
		if (other.tag == "JunkZone")
		{
			tools.JunkZone = false;
			tools.tool = 1;
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002C35C File Offset: 0x0002A55C
	public void RestartJump()
	{
		foreach (object obj in this.hand.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<PickupTool>())
			{
				tools.tool = transform.GetComponent<PickupTool>().tool;
			}
		}
		base.StartCoroutine(this.Restart());
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0002C3E0 File Offset: 0x0002A5E0
	private IEnumerator Restart()
	{
		base.gameObject.transform.localPosition = new Vector3(0f, 16f, 0f);
		yield return 1;
		base.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		tools.cooldown = false;
		using (IEnumerator enumerator = this.hand.transform.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.GetComponent<PickupTool>())
				{
					tools.tool = transform.GetComponent<PickupTool>().tool;
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x04000840 RID: 2112
	public GameObject hand;
}
