using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B2 RID: 434
public class RemoveSpring : MonoBehaviour
{
	// Token: 0x06000A14 RID: 2580 RVA: 0x00064272 File Offset: 0x00062472
	private void Start()
	{
		this.Controller = GameObject.Find("Player").GetComponent<FirstPersonAIO>();
		this.LoadingSlider = this.Controller.GetComponent<tools>().LoadingSlider;
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x000642A0 File Offset: 0x000624A0
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
		if (tools.tool == 9)
		{
			base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 2f, 1 << LayerMask.NameToLayer("LooseParts")) && raycastHit.collider.gameObject == base.gameObject)
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
			if (Input.GetMouseButtonUp(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 2f, 1 << LayerMask.NameToLayer("LooseParts")) && raycastHit2.collider.gameObject == base.gameObject)
			{
				this.Release();
				return;
			}
		}
		else
		{
			if (base.gameObject.GetComponent<Rigidbody>() != null)
			{
				base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
				return;
			}
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00064456 File Offset: 0x00062656
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

	// Token: 0x06000A17 RID: 2583 RVA: 0x00064465 File Offset: 0x00062665
	public void Removing()
	{
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.RemoveWindow();
			return;
		}
		this.RemovingContinue();
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0006448C File Offset: 0x0006268C
	public void RemovingContinue()
	{
		if (base.transform.parent != null)
		{
			base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES3();
			base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
			base.transform.position = Vector3.Lerp(base.transform.position, Camera.main.transform.position, 0.07f);
			base.gameObject.AddComponent<FixedJoint>();
			base.StartCoroutine(this.FallCoroutine());
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x00064524 File Offset: 0x00062724
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

	// Token: 0x06000A1A RID: 2586 RVA: 0x00064579 File Offset: 0x00062779
	private IEnumerator FallCoroutine()
	{
		yield return new WaitForSeconds(3f);
		if (!(base.transform.parent != null))
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
			base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
		}
		yield break;
	}

	// Token: 0x040011FA RID: 4602
	public FirstPersonAIO Controller;

	// Token: 0x040011FB RID: 4603
	public bool removalloading;

	// Token: 0x040011FC RID: 4604
	public Slider LoadingSlider;

	// Token: 0x040011FD RID: 4605
	public float LoadingTime;

	// Token: 0x040011FE RID: 4606
	public Coroutine cc;
}
