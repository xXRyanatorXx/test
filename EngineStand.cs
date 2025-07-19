using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class EngineStand : MonoBehaviour
{
	// Token: 0x060006BD RID: 1725 RVA: 0x00035D6B File Offset: 0x00033F6B
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		if (this.ThisFixes)
		{
			base.StartCoroutine(this.WaitAttach());
		}
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00035D92 File Offset: 0x00033F92
	private IEnumerator WaitAttach()
	{
		yield return 0;
		yield return 0;
		if (this.engineparent.transform.childCount > 0)
		{
			base.transform.position = this.Closeposition.position;
			if (this.engineparent.transform.GetChild(0).GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(this.engineparent.transform.GetChild(0).GetComponent<FixedJoint>());
			}
			if (this.engineparent.transform.GetChild(0).GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(this.engineparent.transform.GetChild(0).GetComponent<Rigidbody>());
			}
			this.engineparent.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		yield break;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00035DA4 File Offset: 0x00033FA4
	private void Update()
	{
		RaycastHit raycastHit;
		if (this.ThisFixes && Input.GetMouseButtonDown(0) && tools.tool == 2 && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("Bolts")) && raycastHit.collider.gameObject == base.gameObject && this.engineparent.transform.childCount > 0)
		{
			if (this.engineparent.transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("LooseParts"))
			{
				if (tools.Tighten)
				{
					base.transform.position = this.Closeposition.position;
					UnityEngine.Object.Destroy(this.engineparent.transform.GetChild(0).GetComponent<FixedJoint>());
					UnityEngine.Object.Destroy(this.engineparent.transform.GetChild(0).GetComponent<Rigidbody>());
					this.engineparent.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting);
				}
			}
			else if (!tools.Tighten)
			{
				base.transform.position = this.Openposition.position;
				this.engineparent.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
				this.engineparent.transform.GetChild(0).gameObject.AddComponent<FixedJoint>();
				this.engineparent.transform.GetChild(0).gameObject.transform.GetComponent<FixedJoint>().breakForce = 5000f;
				this.engineparent.transform.GetChild(0).gameObject.transform.GetComponent<FixedJoint>().connectedBody = base.transform.root.GetComponent<Rigidbody>();
				this.engineparent.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("LooseParts");
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting);
			}
		}
		RaycastHit raycastHit2;
		if (this.ThisRotates && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 1f, 1 << LayerMask.NameToLayer("Items")) && raycastHit2.collider.gameObject == base.gameObject)
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				this.RotatingHead.transform.Rotate(Vector3.right * 500f * Time.deltaTime);
				foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
				{
					if (carProperties.gameObject.layer == LayerMask.NameToLayer("LooseParts") && carProperties.transform.name != "CylinderBlock")
					{
						carProperties.transform.position = carProperties.transform.parent.position;
						carProperties.transform.rotation = carProperties.transform.parent.rotation;
						UnityEngine.Object.Destroy(carProperties.GetComponent<FixedJoint>());
						carProperties.gameObject.AddComponent<FixedJoint>();
					}
				}
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				this.RotatingHead.transform.Rotate(-Vector3.right * 500f * Time.deltaTime);
				foreach (CarProperties carProperties2 in base.GetComponentsInChildren<CarProperties>())
				{
					if (carProperties2.gameObject.layer == LayerMask.NameToLayer("LooseParts"))
					{
						carProperties2.transform.position = carProperties2.transform.parent.position;
						carProperties2.transform.rotation = carProperties2.transform.parent.rotation;
						UnityEngine.Object.Destroy(carProperties2.GetComponent<FixedJoint>());
						carProperties2.gameObject.AddComponent<FixedJoint>();
					}
				}
			}
		}
	}

	// Token: 0x04000A39 RID: 2617
	public GameObject engineparent;

	// Token: 0x04000A3A RID: 2618
	public GameObject RotatingHead;

	// Token: 0x04000A3B RID: 2619
	public bool ThisFixes;

	// Token: 0x04000A3C RID: 2620
	public bool ThisRotates;

	// Token: 0x04000A3D RID: 2621
	public GameObject AudioParent;

	// Token: 0x04000A3E RID: 2622
	public Transform Closeposition;

	// Token: 0x04000A3F RID: 2623
	public Transform Openposition;
}
