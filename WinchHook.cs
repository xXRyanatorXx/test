using System;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class WinchHook : MonoBehaviour
{
	// Token: 0x06000B96 RID: 2966 RVA: 0x00080DA0 File Offset: 0x0007EFA0
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		this.Hand = GameObject.Find("handStatic");
		this.ToolHand = GameObject.Find("ToolHand");
		base.GetComponent<LineRenderer>().enabled = false;
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00080DE0 File Offset: 0x0007EFE0
	private void Update()
	{
		if (!this.wait)
		{
			this.placetoput = null;
			this.seetoput = false;
		}
		this.wait = false;
		RaycastHit raycastHit;
		if (tools.LookingAtTransparent && tools.helditem == base.gameObject.name && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.4f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject.name == base.gameObject.name)
		{
			this.placetoput = raycastHit.collider.gameObject;
			this.seetoput = true;
			tools.canput = true;
			this.wait = true;
		}
		if (Input.GetMouseButtonUp(0) && this.ThisInHandL)
		{
			this.RemoveFromHand();
		}
		if (Input.GetMouseButtonUp(1) && this.ThisInHandL)
		{
			this.RemoveFromHand();
		}
		base.GetComponent<LineRenderer>().SetPosition(0, base.transform.position);
		base.GetComponent<LineRenderer>().SetPosition(1, this.OriginalPosition.transform.position);
		if (Vector3.Distance(this.OriginalPosition.transform.position, base.transform.position) > this.MaxLength)
		{
			this.RemoveFromHand();
		}
		if (this.reeling)
		{
			this.ReeliIn();
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.reeling = false;
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00080F5C File Offset: 0x0007F15C
	public void TakeInHand()
	{
		base.enabled = true;
		RaycastHit raycastHit;
		if (tools.helditem == "Nothing" && base.transform.root.tag == "Vehicle" && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.4f, 1 << LayerMask.NameToLayer("Windows")) && raycastHit.collider.gameObject == base.gameObject && !tools.Clicked)
		{
			if (this.trose.GetComponent<ConfigurableJoint>())
			{
				UnityEngine.Object.Destroy(this.trose.GetComponent<ConfigurableJoint>());
			}
			if (this.MainJoint)
			{
				UnityEngine.Object.Destroy(this.MainJoint);
			}
			if (this.trose.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(this.trose.GetComponent<Rigidbody>());
			}
			tools.Clicked = true;
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			this.AudioParent.transform.position = raycastHit.point;
			this.ThisInHandL = true;
			tools.helditem = base.gameObject.name;
			if (base.gameObject.GetComponent<Rigidbody>() == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			base.gameObject.GetComponent<Rigidbody>().useGravity = false;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.gameObject.transform.SetParent(this.AudioParent.transform);
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().Recheck();
				}
			}
			base.GetComponent<LineRenderer>().enabled = true;
			if (this.staticRopeObj)
			{
				this.staticRopeObj.SetActive(false);
			}
		}
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x000811AC File Offset: 0x0007F3AC
	public void RemoveFromHand()
	{
		if (this.seetoput)
		{
			if (base.gameObject.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			}
			base.gameObject.transform.position = this.placetoput.transform.position;
			base.gameObject.transform.rotation = this.placetoput.transform.rotation;
			base.gameObject.transform.SetParent(this.placetoput.transform);
			this.ThisInHandL = false;
			tools.helditem = "Nothing";
			this.trose.transform.position = this.trose.transform.parent.position;
			this.trose.AddComponent<Rigidbody>();
			this.trose.AddComponent<ConfigurableJoint>();
			this.trose.GetComponent<ConfigurableJoint>().connectedBody = this.placetoput.transform.root.GetComponent<Rigidbody>();
			this.trose.GetComponent<ConfigurableJoint>().anchor = base.transform.localPosition;
			this.trose.GetComponent<ConfigurableJoint>().xMotion = ConfigurableJointMotion.Locked;
			this.trose.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Locked;
			this.trose.GetComponent<ConfigurableJoint>().zMotion = ConfigurableJointMotion.Locked;
			this.trose.GetComponent<ConfigurableJoint>().enableCollision = true;
			this.MainJoint = this.OriginalPosition.transform.root.gameObject.AddComponent<ConfigurableJoint>();
			this.MainJoint.connectedBody = this.trose.GetComponent<Rigidbody>();
			this.MainJoint.autoConfigureConnectedAnchor = false;
			this.MainJoint.anchor = this.OriginalPosition.transform.parent.parent.localPosition;
			if (this.OriginalPosition.transform.root.name == "TrailerCar")
			{
				this.MainJoint.anchor = this.OriginalPosition.transform.root.InverseTransformPoint(this.OriginalPosition.transform.position);
			}
			if (this.OriginalPosition.transform.root.GetComponent<MainCarProperties>())
			{
				this.MainJoint.anchor = this.OriginalPosition.transform.root.InverseTransformPoint(this.OriginalPosition.transform.position);
			}
			this.MainJoint.connectedAnchor = base.transform.localPosition;
			this.MainJoint.xMotion = ConfigurableJointMotion.Limited;
			this.MainJoint.yMotion = ConfigurableJointMotion.Limited;
			this.MainJoint.zMotion = ConfigurableJointMotion.Limited;
			SoftJointLimit linearLimit = default(SoftJointLimit);
			linearLimit.limit = Vector3.Distance(this.OriginalPosition.transform.position, base.transform.position) + 1f;
			this.RopeLength = linearLimit.limit;
			this.MainJoint.linearLimit = linearLimit;
			this.MainJoint.enableCollision = true;
		}
		else
		{
			tools.helditem = "Nothing";
			this.ThisInHandL = false;
			if (this.trose.GetComponent<ConfigurableJoint>())
			{
				UnityEngine.Object.Destroy(this.trose.GetComponent<ConfigurableJoint>());
			}
			if (this.MainJoint)
			{
				UnityEngine.Object.Destroy(this.MainJoint);
			}
			if (this.trose.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(this.trose.GetComponent<Rigidbody>());
			}
			if (base.gameObject.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			}
			base.gameObject.transform.SetParent(this.OriginalPosition.transform);
			if (this.RestingPosition)
			{
				base.transform.position = this.RestingPosition.transform.position;
				base.transform.rotation = this.RestingPosition.transform.rotation;
			}
			else
			{
				base.transform.position = this.OriginalPosition.transform.position;
				base.transform.rotation = this.OriginalPosition.transform.rotation;
			}
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			base.GetComponent<LineRenderer>().enabled = false;
			if (this.staticRopeObj)
			{
				this.staticRopeObj.SetActive(true);
			}
			base.enabled = false;
		}
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
		{
			if (gameObject.name == base.gameObject.name)
			{
				gameObject.GetComponent<transparents>().Disablerend();
			}
		}
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00081658 File Offset: 0x0007F858
	public void ReeliIn()
	{
		if (this.MainJoint)
		{
			SoftJointLimit linearLimit = default(SoftJointLimit);
			linearLimit.limit = this.RopeLength - 0.03f;
			this.RopeLength = linearLimit.limit;
			this.MainJoint.linearLimit = linearLimit;
			this.WinchHandle.transform.Rotate(Vector3.right * 500f * Time.deltaTime);
			if (!this.AudioParent.GetComponent<AudioSource>().isPlaying)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Winching);
			}
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00081704 File Offset: 0x0007F904
	public void ReelOut()
	{
		if (this.MainJoint)
		{
			SoftJointLimit linearLimit = default(SoftJointLimit);
			linearLimit.limit = this.RopeLength + 0.03f;
			this.RopeLength = linearLimit.limit;
			this.MainJoint.linearLimit = linearLimit;
			this.WinchHandle.transform.Rotate(-Vector3.right * 500f * Time.deltaTime);
		}
	}

	// Token: 0x04001437 RID: 5175
	public GameObject Hand;

	// Token: 0x04001438 RID: 5176
	public GameObject ToolHand;

	// Token: 0x04001439 RID: 5177
	public GameObject OriginalPosition;

	// Token: 0x0400143A RID: 5178
	public GameObject RestingPosition;

	// Token: 0x0400143B RID: 5179
	public bool seetoput;

	// Token: 0x0400143C RID: 5180
	public GameObject placetoput;

	// Token: 0x0400143D RID: 5181
	public GameObject AudioParent;

	// Token: 0x0400143E RID: 5182
	public bool ThisInHandL;

	// Token: 0x0400143F RID: 5183
	public GameObject Attached;

	// Token: 0x04001440 RID: 5184
	public bool wait;

	// Token: 0x04001441 RID: 5185
	public float MaxLength;

	// Token: 0x04001442 RID: 5186
	public GameObject trose;

	// Token: 0x04001443 RID: 5187
	public GameObject WinchHandle;

	// Token: 0x04001444 RID: 5188
	public bool reeling;

	// Token: 0x04001445 RID: 5189
	public GameObject staticRopeObj;

	// Token: 0x04001446 RID: 5190
	public float RopeLength;

	// Token: 0x04001447 RID: 5191
	private ConfigurableJoint MainJoint;
}
