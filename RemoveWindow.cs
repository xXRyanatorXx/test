using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B5 RID: 437
public class RemoveWindow : MonoBehaviour
{
	// Token: 0x06000A28 RID: 2600 RVA: 0x00064714 File Offset: 0x00062914
	private void Start()
	{
		this.Controller = GameObject.Find("Player").GetComponent<FirstPersonAIO>();
		this.LoadingSlider = this.Controller.GetComponent<tools>().LoadingSlider;
		if (base.transform.parent && base.transform.parent.name == base.transform.name && !base.GetComponent<PickupHand>())
		{
			base.gameObject.GetComponent<Partinfo>().attachedbolts += 1f;
			base.gameObject.GetComponent<Partinfo>().tightnuts += 1f;
		}
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x000647C8 File Offset: 0x000629C8
	private void Update()
	{
		if (this.LoadingTime > 0f)
		{
			this.LoadingTime -= Time.deltaTime;
		}
		if (this.removalloading)
		{
			this.LoadingSlider.value = this.LoadingTime * 2f;
		}
		if (tools.tool == 6)
		{
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 2f, 1 << LayerMask.NameToLayer("Windows")) && raycastHit.collider.gameObject == base.gameObject)
			{
				if (base.transform.parent)
				{
					this.cc = base.StartCoroutine(this.RemovalLoading());
				}
				else
				{
					this.Removing();
				}
				tools.helditem = base.gameObject.name;
				tools.PartInHand = base.gameObject;
			}
			RaycastHit raycastHit2;
			if (Input.GetMouseButtonUp(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 2f, 1 << LayerMask.NameToLayer("Windows")) && raycastHit2.collider.gameObject == base.gameObject)
			{
				this.Release();
				return;
			}
		}
		else if (!this.removalloading)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00064936 File Offset: 0x00062B36
	private IEnumerator RemovalLoading()
	{
		this.LoadingTime = 0.5f;
		base.enabled = true;
		this.removalloading = true;
		this.Controller.ControllerPause();
		yield return new WaitForSeconds(0.5f);
		if (this.removalloading)
		{
			this.Removing();
		}
		this.Controller.ControllerUnPause();
		yield break;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00064948 File Offset: 0x00062B48
	public void Removing()
	{
		if (base.transform.parent != null)
		{
			if (this.PARENTHaveToBeREmoved)
			{
				if (base.transform.parent.parent.parent == null)
				{
					base.transform.position = Vector3.Lerp(base.transform.position, Camera.main.transform.position, 0.07f);
					base.gameObject.AddComponent<FixedJoint>();
					if (base.GetComponent<MPobject>())
					{
						base.GetComponent<MPobject>().networkDummy.RemoveWindow();
						return;
					}
					this.RemovingContinue();
					return;
				}
			}
			else
			{
				base.transform.position = Vector3.Lerp(base.transform.position, Camera.main.transform.position, 0.07f);
				base.gameObject.AddComponent<FixedJoint>();
				if (base.GetComponent<MPobject>())
				{
					base.GetComponent<MPobject>().networkDummy.RemoveWindow();
					return;
				}
				this.RemovingContinue();
			}
		}
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00064A58 File Offset: 0x00062C58
	public void RemovingContinue()
	{
		if (!base.GetComponent<Rigidbody>())
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
		if (!base.GetComponent<FixedJoint>())
		{
			base.transform.position = Vector3.Lerp(base.transform.position, Camera.main.transform.position, 0.07f);
		}
		if (base.transform.parent && base.transform.parent.gameObject.GetComponent<transparents>())
		{
			base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES();
			base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = false;
		}
		base.gameObject.transform.SetParent(null);
		base.gameObject.GetComponent<Partinfo>().remove(false);
		base.StartCoroutine(this.FallCoroutine());
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00064B64 File Offset: 0x00062D64
	private void Release()
	{
		if (this.removalloading)
		{
			this.removalloading = false;
			this.LoadingSlider.value = 0f;
			if (this.cc != null)
			{
				base.StopCoroutine(this.cc);
			}
			this.Controller.ControllerUnPause();
		}
		tools.helditem = "Nothing";
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00064BB9 File Offset: 0x00062DB9
	private IEnumerator FallCoroutine()
	{
		yield return new WaitForSeconds(3f);
		if (!(base.transform.parent != null))
		{
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			if (base.gameObject.GetComponent<Rigidbody>())
			{
				base.gameObject.GetComponent<Rigidbody>().useGravity = true;
				base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
				base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			}
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
		}
		yield break;
	}

	// Token: 0x04001205 RID: 4613
	public GameObject player;

	// Token: 0x04001206 RID: 4614
	public bool PARENTHaveToBeREmoved;

	// Token: 0x04001207 RID: 4615
	public FirstPersonAIO Controller;

	// Token: 0x04001208 RID: 4616
	public bool removalloading;

	// Token: 0x04001209 RID: 4617
	public Slider LoadingSlider;

	// Token: 0x0400120A RID: 4618
	public float LoadingTime;

	// Token: 0x0400120B RID: 4619
	public Coroutine cc;
}
