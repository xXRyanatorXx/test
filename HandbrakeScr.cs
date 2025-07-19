using System;
using System.Collections;
using NWH.VehiclePhysics2;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class HandbrakeScr : MonoBehaviour
{
	// Token: 0x060007D5 RID: 2005 RVA: 0x0004479B File Offset: 0x0004299B
	private void Start()
	{
		base.StartCoroutine(this.LateStart());
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x000447AA File Offset: 0x000429AA
	private IEnumerator LateStart()
	{
		yield return new WaitForSeconds(5f);
		this.AudioParent = GameObject.Find("hand");
		if (base.transform.root != null && base.transform.root.tag == "Vehicle")
		{
			this.exp = base.transform.root.GetComponent<VehicleController>();
			this.installed = true;
			base.transform.root.GetComponent<MainCarProperties>().SetHandbrake();
		}
		else
		{
			this.installed = false;
		}
		yield break;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x000447BC File Offset: 0x000429BC
	private void Update()
	{
		if (tools.DontAllowClick)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && this.installed && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.6f, 1 << LayerMask.NameToLayer("OpenableParts")) && raycastHit.collider.gameObject == base.gameObject)
		{
			if (!this.Engaged)
			{
				if (base.transform.parent.GetComponent<MPobject>())
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.setHandbrake();
				}
				else
				{
					this.set();
				}
			}
			else if (base.transform.parent.GetComponent<MPobject>())
			{
				base.transform.parent.GetComponent<MPobject>().networkDummy.releaseHandbrake();
			}
			else
			{
				this.release();
			}
		}
		if (this.installed)
		{
			if (this.exp.input.Handbrake == 1f || this.exp.brakes.HandbrakeDeployed)
			{
				this.Engaged = true;
				if (this.footbrake)
				{
					base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					return;
				}
				base.transform.localEulerAngles = new Vector3(30f, 0f, 0f);
				return;
			}
			else
			{
				this.Engaged = false;
				if (this.footbrake)
				{
					base.transform.localEulerAngles = new Vector3(30f, 0f, 0f);
					return;
				}
				base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
		}
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0004498C File Offset: 0x00042B8C
	public void set()
	{
		this.Engaged = true;
		if (base.transform.root != null && base.transform.root.tag == "Vehicle")
		{
			base.transform.root.GetComponent<MainCarProperties>().SetHandbrake();
		}
		if (this.AudioParent && Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 10f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().HandbrakeSet);
		}
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00044A38 File Offset: 0x00042C38
	public void release()
	{
		this.Engaged = false;
		if (base.transform.root != null && base.transform.root.tag == "Vehicle")
		{
			base.transform.root.GetComponent<MainCarProperties>().ReleaseHandbrake();
		}
		if (this.AudioParent && Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 10f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().HandbrakeRelease);
		}
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00044AE4 File Offset: 0x00042CE4
	public void remove()
	{
		this.installed = false;
		if (this.Engaged)
		{
			this.Engaged = false;
			if (this.footbrake)
			{
				base.transform.localEulerAngles = new Vector3(30f, 0f, 0f);
			}
			else
			{
				base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
			this.exp = null;
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00044B58 File Offset: 0x00042D58
	public void attach()
	{
		if (base.transform.root != null && base.transform.root.tag == "Vehicle")
		{
			this.exp = base.transform.root.GetComponent<VehicleController>();
			this.installed = true;
		}
	}

	// Token: 0x04000EC4 RID: 3780
	public VehicleController exp;

	// Token: 0x04000EC5 RID: 3781
	public bool footbrake;

	// Token: 0x04000EC6 RID: 3782
	public bool Engaged;

	// Token: 0x04000EC7 RID: 3783
	public bool installed;

	// Token: 0x04000EC8 RID: 3784
	public GameObject AudioParent;
}
