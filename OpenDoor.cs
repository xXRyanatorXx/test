using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FA RID: 506
public class OpenDoor : MonoBehaviour
{
	// Token: 0x06000BD3 RID: 3027 RVA: 0x00083198 File Offset: 0x00081398
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		if (base.transform.root.tag == "Vehicle")
		{
			this.doorOpened = false;
			base.transform.position = base.transform.parent.position;
			base.transform.rotation = base.transform.parent.rotation;
		}
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x0008320E File Offset: 0x0008140E
	private IEnumerator waitsec()
	{
		if (base.GetComponent<HingeJoint>())
		{
			yield return new WaitForSeconds(1f);
			HingeJoint component = base.GetComponent<HingeJoint>();
			component.useSpring = false;
			if (component.angle <= 1f)
			{
				this.doorOpened = false;
				UnityEngine.Object.Destroy(base.GetComponent<HingeJoint>());
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
				base.transform.position = base.transform.parent.position;
				base.transform.rotation = base.transform.parent.rotation;
			}
			if (component.angle >= 80f || (base.gameObject.GetComponent<Partinfo>().HoodHalf && component.angle >= 40f))
			{
				this.doorOpened = true;
				if (!base.GetComponent<FixedJoint>())
				{
					base.gameObject.AddComponent<FixedJoint>();
				}
				base.gameObject.GetComponent<FixedJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
				base.gameObject.GetComponent<FixedJoint>().breakForce = 25000f;
				base.gameObject.GetComponent<HingeJoint>().breakForce = 60000f;
				if (base.gameObject.GetComponent<Partinfo>().Trunk)
				{
					base.gameObject.GetComponent<FixedJoint>().breakForce = 10000f;
					base.gameObject.GetComponent<HingeJoint>().breakForce = 30000f;
				}
			}
		}
		this.isRunning = false;
		yield break;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0008321D File Offset: 0x0008141D
	private IEnumerator waitSound()
	{
		yield return 2;
		if (base.GetComponent<HingeJoint>().angle <= 1f)
		{
			if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().CloseCarDoor);
			}
		}
		else if (this.isRunning)
		{
			base.StartCoroutine(this.waitSound());
		}
		yield break;
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x0008322C File Offset: 0x0008142C
	public void OnMouseDown1(bool open)
	{
		base.StartCoroutine(this.LateClick(open));
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x0008323C File Offset: 0x0008143C
	public void MPdoorOperation(bool open)
	{
		if (!base.transform.parent.GetComponent<transparents>())
		{
			return;
		}
		tools.Clicked = true;
		if (!open)
		{
			if (base.transform.parent)
			{
				base.transform.rotation = base.transform.parent.rotation;
				if (base.gameObject.GetComponent<Partinfo>().Rdoor)
				{
					base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y - 90f, base.transform.localEulerAngles.z);
				}
				if (base.gameObject.GetComponent<Partinfo>().Ldoor)
				{
					base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y + 90f, base.transform.localEulerAngles.z);
				}
				if (base.gameObject.GetComponent<Partinfo>().Trunk)
				{
					base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x + 90f, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z);
				}
				if (base.gameObject.GetComponent<Partinfo>().Hood)
				{
					base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x - 90f, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z);
				}
				if (base.gameObject.GetComponent<Partinfo>().HoodHalf)
				{
					base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x - 50f, base.transform.localEulerAngles.y, base.transform.localEulerAngles.z);
				}
			}
			if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().OpenCarDoor);
			}
			this.doorOpened = true;
			return;
		}
		if (base.gameObject.GetComponent<FixedJoint>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
		}
		if (base.gameObject.GetComponent<HingeJoint>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<HingeJoint>());
		}
		if (base.gameObject.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		}
		base.transform.position = base.transform.parent.position;
		base.transform.rotation = base.transform.parent.rotation;
		if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().CloseCarDoor);
		}
		this.doorOpened = false;
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0008357F File Offset: 0x0008177F
	private IEnumerator LateClick(bool open)
	{
		yield return 0;
		if (!tools.Clicked && !tools.cooldown && base.gameObject.GetComponent<Partinfo>().tightnuts > 0f && tools.tool != 7 && tools.tool != 4 && tools.tool != 5 && tools.tool != 12 && tools.tool != 11 && tools.tool != 14 && tools.tool != 16 && tools.tool != 19 && tools.tool != 25)
		{
			tools.Clicked = true;
			if (!open)
			{
				if (base.transform.parent)
				{
					base.transform.rotation = base.transform.parent.rotation;
					if (!base.gameObject.GetComponent<Rigidbody>())
					{
						base.gameObject.AddComponent<Rigidbody>();
					}
					if (!base.gameObject.GetComponent<HingeJoint>())
					{
						base.gameObject.AddComponent<HingeJoint>();
					}
					base.gameObject.GetComponent<HingeJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
					base.gameObject.GetComponent<HingeJoint>().anchor = new Vector3(0f, 0f, 0f);
					if (base.gameObject.GetComponent<Partinfo>().Rdoor)
					{
						base.gameObject.GetComponent<HingeJoint>().axis = Vector3.down;
						HingeJoint component = base.GetComponent<HingeJoint>();
						JointLimits limits = component.limits;
						limits.min = 0f;
						limits.bounciness = 0f;
						limits.bounceMinVelocity = 0f;
						limits.max = 90f;
						component.limits = limits;
						component.useLimits = true;
					}
					if (base.gameObject.GetComponent<Partinfo>().Ldoor)
					{
						base.gameObject.GetComponent<HingeJoint>().axis = Vector3.up;
						HingeJoint component2 = base.GetComponent<HingeJoint>();
						JointLimits limits2 = component2.limits;
						limits2.min = 0f;
						limits2.bounciness = 0f;
						limits2.bounceMinVelocity = 0f;
						limits2.max = 90f;
						component2.limits = limits2;
						component2.useLimits = true;
					}
					if (base.gameObject.GetComponent<Partinfo>().Trunk)
					{
						base.gameObject.GetComponent<HingeJoint>().axis = Vector3.right;
						HingeJoint component3 = base.GetComponent<HingeJoint>();
						JointLimits limits3 = component3.limits;
						limits3.min = 0f;
						limits3.bounciness = 0f;
						limits3.bounceMinVelocity = 0f;
						limits3.max = 90f;
						component3.limits = limits3;
						component3.useLimits = true;
					}
					if (base.gameObject.GetComponent<Partinfo>().Hood)
					{
						base.gameObject.GetComponent<HingeJoint>().axis = Vector3.left;
						HingeJoint component4 = base.GetComponent<HingeJoint>();
						JointLimits limits4 = component4.limits;
						limits4.min = 0f;
						limits4.bounciness = 0f;
						limits4.bounceMinVelocity = 0f;
						limits4.max = 90f;
						component4.limits = limits4;
						component4.useLimits = true;
					}
					if (base.gameObject.GetComponent<Partinfo>().HoodHalf)
					{
						base.gameObject.GetComponent<HingeJoint>().axis = Vector3.left;
						HingeJoint component5 = base.GetComponent<HingeJoint>();
						JointLimits limits5 = component5.limits;
						limits5.min = 0f;
						limits5.bounciness = 0f;
						limits5.bounceMinVelocity = 0f;
						limits5.max = 50f;
						component5.limits = limits5;
						component5.useLimits = true;
					}
				}
				HingeJoint component6 = base.GetComponent<HingeJoint>();
				if (!base.transform.GetComponent<Partinfo>().HingePivot)
				{
					base.transform.GetComponent<Partinfo>().FindHinge();
				}
				base.transform.GetComponent<HingeJoint>().anchor = base.transform.GetComponent<Partinfo>().HingePivot.transform.localPosition;
				base.gameObject.GetComponent<HingeJoint>().breakForce = 25000f;
				JointSpring spring = component6.spring;
				spring.spring = 100f;
				spring.damper = 3f;
				spring.targetPosition = 90f;
				component6.spring = spring;
				component6.useSpring = true;
				this.doorOpened = true;
				if (component6.angle <= 1f && Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
				{
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().OpenCarDoor);
				}
				if (!this.isRunning)
				{
					base.StartCoroutine(this.waitsec());
				}
				this.isRunning = true;
			}
			else
			{
				if (base.gameObject.GetComponent<FixedJoint>())
				{
					UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
				}
				base.gameObject.GetComponent<HingeJoint>().breakForce = 25000f;
				HingeJoint component7 = base.GetComponent<HingeJoint>();
				JointSpring spring2 = component7.spring;
				spring2.spring = 102f;
				spring2.damper = 0f;
				spring2.targetPosition = -20f;
				component7.spring = spring2;
				component7.useSpring = true;
				this.doorOpened = false;
				if (!this.isRunning)
				{
					base.StartCoroutine(this.waitsec());
					base.StartCoroutine(this.waitSound());
				}
				this.isRunning = true;
			}
		}
		yield break;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00083598 File Offset: 0x00081798
	public void installed()
	{
		HingeJoint component = base.GetComponent<HingeJoint>();
		UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
		JointSpring spring = component.spring;
		spring.spring = 18f;
		spring.damper = 2f;
		spring.targetPosition = 90f;
		component.spring = spring;
		component.useSpring = true;
		base.gameObject.GetComponent<HingeJoint>().breakForce = 60000f;
		if (base.gameObject.GetComponent<Partinfo>().Trunk || base.gameObject.GetComponent<Partinfo>().HoodHalf)
		{
			base.gameObject.GetComponent<HingeJoint>().breakForce = 30000f;
		}
		this.doorOpened = true;
		if (!this.isRunning)
		{
			base.StartCoroutine(this.waitsec());
		}
		this.isRunning = true;
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00083660 File Offset: 0x00081860
	public void BrakeOpen()
	{
		if (!base.gameObject.GetComponent<Rigidbody>())
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		if (!base.gameObject.GetComponent<HingeJoint>())
		{
			base.gameObject.AddComponent<HingeJoint>();
		}
		base.gameObject.GetComponent<HingeJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
		base.gameObject.GetComponent<HingeJoint>().anchor = new Vector3(0f, 0f, 0f);
		if (base.gameObject.GetComponent<Partinfo>().Rdoor)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.down;
			HingeJoint component = base.GetComponent<HingeJoint>();
			JointLimits limits = component.limits;
			limits.min = 0f;
			limits.bounciness = 0f;
			limits.bounceMinVelocity = 0f;
			limits.max = 90f;
			component.limits = limits;
			component.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().Ldoor)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.up;
			HingeJoint component2 = base.GetComponent<HingeJoint>();
			JointLimits limits2 = component2.limits;
			limits2.min = 0f;
			limits2.bounciness = 0f;
			limits2.bounceMinVelocity = 0f;
			limits2.max = 90f;
			component2.limits = limits2;
			component2.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().Trunk)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.right;
			HingeJoint component3 = base.GetComponent<HingeJoint>();
			JointLimits limits3 = component3.limits;
			limits3.min = 0f;
			limits3.bounciness = 0f;
			limits3.bounceMinVelocity = 0f;
			limits3.max = 90f;
			component3.limits = limits3;
			component3.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().Hood)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.left;
			HingeJoint component4 = base.GetComponent<HingeJoint>();
			JointLimits limits4 = component4.limits;
			limits4.min = 0f;
			limits4.bounciness = 0f;
			limits4.bounceMinVelocity = 0f;
			limits4.max = 90f;
			component4.limits = limits4;
			component4.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().HoodHalf)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.left;
			HingeJoint component5 = base.GetComponent<HingeJoint>();
			JointLimits limits5 = component5.limits;
			limits5.min = 0f;
			limits5.bounciness = 0f;
			limits5.bounceMinVelocity = 0f;
			limits5.max = 50f;
			component5.limits = limits5;
			component5.useLimits = true;
		}
		base.transform.GetComponent<HingeJoint>().anchor = base.transform.GetComponent<Partinfo>().HingePivot.transform.localPosition;
		base.GetComponent<HingeJoint>();
		base.gameObject.GetComponent<HingeJoint>().breakForce = 25000f;
		this.doorOpened = true;
	}

	// Token: 0x0400147C RID: 5244
	public GameObject AudioParent;

	// Token: 0x0400147D RID: 5245
	public bool isRunning;

	// Token: 0x0400147E RID: 5246
	public bool doorOpened;

	// Token: 0x0400147F RID: 5247
	public bool DontOpenSitting;

	// Token: 0x04001480 RID: 5248
	private bool coroutineAllowed;

	// Token: 0x04001481 RID: 5249
	private Rigidbody m_Rigidbody;
}
