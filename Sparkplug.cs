using System;
using Mirror;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class Sparkplug : MonoBehaviour
{
	// Token: 0x06000A3E RID: 2622 RVA: 0x00064E00 File Offset: 0x00063000
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		this.tight = false;
		base.gameObject.transform.parent.GetComponent<Partinfo>().attachedbolts += 1f;
		base.gameObject.transform.parent.position += base.transform.TransformDirection(0f, 0.007f, 0f);
		if (base.transform.parent.parent && base.transform.parent.parent.name == base.transform.parent.name)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts += 1f;
			this.tight = true;
			base.gameObject.transform.parent.position -= base.transform.TransformDirection(0f, 0.007f, 0f);
		}
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x00064F34 File Offset: 0x00063134
	public void disableREND()
	{
		if (base.transform.parent.parent != null)
		{
			if (!(base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name))
			{
				base.gameObject.GetComponent<BoxCollider>().enabled = false;
				if (this.tight)
				{
					this.tight = false;
					base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts -= 1f;
					base.gameObject.transform.parent.position += base.transform.TransformDirection(0f, 0.007f, 0f);
					return;
				}
			}
		}
		else
		{
			base.gameObject.GetComponent<BoxCollider>().enabled = false;
			if (this.tight)
			{
				this.tight = false;
				base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts -= 1f;
				base.gameObject.transform.parent.position += base.transform.TransformDirection(0f, 0.007f, 0f);
			}
		}
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0006509C File Offset: 0x0006329C
	public void enableREND()
	{
		if (base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name)
		{
			base.gameObject.GetComponent<BoxCollider>().enabled = true;
		}
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x000650F0 File Offset: 0x000632F0
	private void Update()
	{
		if (tools.tool == 8 && base.transform.parent.parent && base.transform.parent.parent.GetComponent<transparents>())
		{
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("Bolts")) && raycastHit.collider.gameObject == base.gameObject)
			{
				this.tighten();
			}
		}
		else
		{
			base.gameObject.GetComponent<BoxCollider>().enabled = false;
			base.enabled = false;
		}
		if (!this.EnabledThisFrame)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
			base.enabled = false;
		}
		this.EnabledThisFrame = false;
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x000651E4 File Offset: 0x000633E4
	public void tighten()
	{
		if (!this.tight || tools.Tighten)
		{
			if (!this.tight && tools.Tighten)
			{
				if (base.transform.parent.GetComponent<MPobject>())
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.tighten(base.transform.GetSiblingIndex());
					return;
				}
				this.tighten2();
			}
			return;
		}
		if (base.transform.parent.GetComponent<MPobject>())
		{
			base.transform.parent.GetComponent<MPobject>().networkDummy.Loosen(base.transform.GetSiblingIndex());
			return;
		}
		this.Loosen();
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00065298 File Offset: 0x00063498
	public void tighten2()
	{
		if (!base.transform.parent.parent)
		{
			return;
		}
		if (base.transform.parent.GetComponent<MPobject>())
		{
			base.transform.parent.GetComponent<MPobject>().networkDummy.GetComponent<NetworkTransform>().enabled = false;
			base.transform.parent.GetComponent<MPobject>().networkDummy.enabled = false;
		}
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting);
		this.tight = true;
		base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts += 1f;
		base.gameObject.transform.parent.position -= base.transform.TransformDirection(0f, 0.007f, 0f);
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().attach();
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x000653D4 File Offset: 0x000635D4
	public void Loosen()
	{
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting2);
		this.tight = false;
		base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts -= 1f;
		base.gameObject.transform.parent.position += base.transform.TransformDirection(0f, 0.007f, 0f);
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts == 0f && base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts == 0f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().remove(false);
		}
	}

	// Token: 0x04001214 RID: 4628
	private bool tight;

	// Token: 0x04001215 RID: 4629
	public bool canfix;

	// Token: 0x04001216 RID: 4630
	public bool EnabledThisFrame;

	// Token: 0x04001217 RID: 4631
	public GameObject AudioParent;
}
