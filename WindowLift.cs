using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F0 RID: 496
public class WindowLift : MonoBehaviour
{
	// Token: 0x06000B9D RID: 2973 RVA: 0x00081780 File Offset: 0x0007F980
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		if (this.rotationspeed == 0f)
		{
			this.rotationspeed = 500f;
		}
		if (this.LowestPosition == 0f)
		{
			this.rotationspeed = -0.3f;
		}
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x000817D0 File Offset: 0x0007F9D0
	private void LateUpdate()
	{
		if (Input.GetMouseButtonDown(0) && this.Winch)
		{
			if (base.transform.parent.GetComponent<MPobject>())
			{
				base.transform.parent.GetComponent<MPobject>().networkDummy.WinnchReeling();
			}
			else
			{
				this.WinnchReeling();
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				if (base.transform.parent.GetComponent<MPobject>())
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.WindowUp();
				}
				else
				{
					this.WindowUp();
				}
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				if (base.transform.parent.GetComponent<MPobject>())
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.WindowDown();
					return;
				}
				this.WindowDown();
				return;
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x000818EB File Offset: 0x0007FAEB
	public void WinnchReeling()
	{
		this.Winch.reeling = true;
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x000818FC File Offset: 0x0007FAFC
	public void WindowUp()
	{
		if (!this.AudioParent)
		{
			this.AudioParent = GameObject.Find("hand");
		}
		if (!this.Winch && base.transform.parent.GetComponent<CarProperties>().Condition > 0.4f && this.Window.transform.localPosition.z < this.HighestPosition)
		{
			if (this.WindowClosed == null)
			{
				this.Window.transform.position += this.Window.transform.up * Time.deltaTime * 0.3f;
			}
			else
			{
				Vector3 position = this.Window.transform.position;
				Quaternion rotation = this.Window.transform.rotation;
				this.Window.transform.rotation = Quaternion.Lerp(rotation, this.WindowClosed.transform.rotation, Time.deltaTime * 3f);
				this.Window.transform.position = Vector3.Lerp(position, this.WindowClosed.transform.position, Time.deltaTime * 3f);
			}
			base.transform.Rotate(-Vector3.right * this.rotationspeed * Time.deltaTime);
			if (!this.AudioParent.GetComponent<AudioSource>().isPlaying && Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 5f)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().WindowManualLift);
			}
		}
		if (this.Winch)
		{
			this.Winch.ReeliIn();
		}
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00081AE8 File Offset: 0x0007FCE8
	public void WindowDown()
	{
		if (!this.AudioParent)
		{
			this.AudioParent = GameObject.Find("hand");
		}
		if (!this.Winch && base.transform.parent.GetComponent<CarProperties>().Condition > 0.4f && this.Window.transform.localPosition.z > this.LowestPosition)
		{
			if (this.WindowClosed == null)
			{
				this.Window.transform.position -= this.Window.transform.up * Time.deltaTime * 0.3f;
			}
			else
			{
				Vector3 position = this.Window.transform.position;
				Quaternion rotation = this.Window.transform.rotation;
				this.Window.transform.rotation = Quaternion.Lerp(rotation, this.WindowOpen.transform.rotation, Time.deltaTime * 3f);
				this.Window.transform.position = Vector3.Lerp(position, this.WindowOpen.transform.position, Time.deltaTime * 3f);
			}
			base.transform.Rotate(Vector3.right * this.rotationspeed * Time.deltaTime);
			if (!this.AudioParent.GetComponent<AudioSource>().isPlaying && Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 5f)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().WindowManualLift);
			}
		}
		if (this.Winch)
		{
			this.Winch.ReelOut();
		}
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00081CD0 File Offset: 0x0007FED0
	public void ElWindowUp()
	{
		if (!this.AudioParent)
		{
			this.AudioParent = GameObject.Find("hand");
		}
		if (base.GetComponent<CarProperties>().Condition > 0.4f && this.Window.transform.localPosition.z < this.HighestPosition && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().Electricity)
		{
			if (!this.AudioParent.GetComponent<AudioSource>().isPlaying)
			{
				this.AudioParent.GetComponent<AudioSource>().clip = this.AudioParent.GetComponent<AudioManager>().WindowElectriclift;
				if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 5f)
				{
					this.AudioParent.GetComponent<AudioSource>().Play();
				}
			}
			base.StartCoroutine(this.GoUp());
		}
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00081DD8 File Offset: 0x0007FFD8
	public void ElWindowDown()
	{
		if (!this.AudioParent)
		{
			this.AudioParent = GameObject.Find("hand");
		}
		if (base.GetComponent<CarProperties>().Condition > 0.4f && this.Window.transform.localPosition.z > this.LowestPosition && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().Electricity)
		{
			if (!this.AudioParent.GetComponent<AudioSource>().isPlaying)
			{
				this.AudioParent.GetComponent<AudioSource>().clip = this.AudioParent.GetComponent<AudioManager>().WindowElectriclift;
				if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 5f)
				{
					this.AudioParent.GetComponent<AudioSource>().Play();
				}
			}
			base.StartCoroutine(this.GoDown());
		}
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00081EDE File Offset: 0x000800DE
	private IEnumerator GoUp()
	{
		yield return 0;
		if (base.GetComponent<CarProperties>().Condition > 0.4f && this.Window.transform.localPosition.z < this.HighestPosition)
		{
			Vector3 position = this.Window.transform.position;
			Quaternion rotation = this.Window.transform.rotation;
			this.Window.transform.rotation = Quaternion.Lerp(rotation, this.WindowClosed.transform.rotation, Time.deltaTime);
			this.Window.transform.position = Vector3.Lerp(position, this.WindowClosed.transform.position, Time.deltaTime);
			if (Input.GetMouseButton(1))
			{
				base.StartCoroutine(this.GoUp());
			}
			else
			{
				this.AudioParent.GetComponent<AudioSource>().Stop();
			}
		}
		yield break;
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00081EED File Offset: 0x000800ED
	private IEnumerator GoDown()
	{
		yield return 0;
		if (base.GetComponent<CarProperties>().Condition > 0.4f && this.Window.transform.localPosition.z > this.LowestPosition)
		{
			Vector3 position = this.Window.transform.position;
			Quaternion rotation = this.Window.transform.rotation;
			this.Window.transform.rotation = Quaternion.Lerp(rotation, this.WindowOpen.transform.rotation, Time.deltaTime);
			this.Window.transform.position = Vector3.Lerp(position, this.WindowOpen.transform.position, Time.deltaTime);
			if (Input.GetMouseButton(0))
			{
				base.StartCoroutine(this.GoDown());
			}
			else
			{
				this.AudioParent.GetComponent<AudioSource>().Stop();
			}
		}
		yield break;
	}

	// Token: 0x04001448 RID: 5192
	public WinchHook Winch;

	// Token: 0x04001449 RID: 5193
	public float HighestPosition;

	// Token: 0x0400144A RID: 5194
	public float LowestPosition;

	// Token: 0x0400144B RID: 5195
	public GameObject Window;

	// Token: 0x0400144C RID: 5196
	public GameObject WindowOpen;

	// Token: 0x0400144D RID: 5197
	public GameObject WindowClosed;

	// Token: 0x0400144E RID: 5198
	public float rotationspeed;

	// Token: 0x0400144F RID: 5199
	public GameObject AudioParent;
}
