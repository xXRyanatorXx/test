using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class AutoAttach : MonoBehaviour
{
	// Token: 0x06000015 RID: 21 RVA: 0x00002400 File Offset: 0x00000600
	private void Start()
	{
		if (base.transform.name == "Disc")
		{
			base.StartCoroutine(this.WaitAttach());
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002426 File Offset: 0x00000626
	public void Attach()
	{
		base.StartCoroutine(this.WaitAttach());
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002435 File Offset: 0x00000635
	private IEnumerator WaitAttach()
	{
		yield return 0;
		foreach (Collider collider in Physics.OverlapSphere(this.CheckSpot.transform.position, this.CheckRadius))
		{
			if (collider.gameObject.transform.name == this.CheckSpot.transform.name && this.CheckSpot.transform.childCount == 0 && !collider.gameObject.transform.parent)
			{
				collider.gameObject.transform.SetParent(this.CheckSpot.transform);
				collider.gameObject.transform.position = this.CheckSpot.transform.position;
				collider.gameObject.transform.rotation = this.CheckSpot.transform.rotation;
				if (collider.gameObject.layer == LayerMask.NameToLayer("LooseParts"))
				{
					collider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
				}
				if (collider.gameObject.GetComponent<Rigidbody>())
				{
					UnityEngine.Object.Destroy(collider.GetComponent<Rigidbody>());
				}
				if (base.transform.name == "Disc")
				{
					base.transform.parent.GetComponent<PickupTool>().Attached = collider.gameObject;
				}
				if (base.transform.name == "Electrode")
				{
					base.transform.parent.GetComponent<PickupTool>().Attached = collider.gameObject;
				}
				if (collider.transform.name == "CrankCaseA00")
				{
					this.BikeEngine = collider.gameObject;
				}
			}
		}
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		if (this.BikeEngine)
		{
			this.BikeEngine.GetComponent<Partinfo>().tightnuts = 2f;
			foreach (HexNut hexNut in this.BikeEngine.GetComponentsInChildren<HexNut>())
			{
				hexNut.tight = true;
				hexNut.gameObject.transform.position += hexNut.transform.TransformDirection(0f, -0.007f, 0f);
			}
			this.BikeEngine = null;
		}
		yield break;
	}

	// Token: 0x04000007 RID: 7
	public GameObject CheckSpot;

	// Token: 0x04000008 RID: 8
	public float CheckRadius;

	// Token: 0x04000009 RID: 9
	public GameObject BikeEngine;
}
