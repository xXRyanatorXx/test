using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class OpenGarage : MonoBehaviour
{
	// Token: 0x060008D7 RID: 2263 RVA: 0x00054B38 File Offset: 0x00052D38
	public void Start()
	{
		this.tempParent = GameObject.Find("hand");
		this.AudioParent = this.tempParent;
		if (base.transform.localPosition.y > 0.2f && this.GarageDoor)
		{
			this.Open = true;
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00054B88 File Offset: 0x00052D88
	public void OnMouseDown()
	{
		if (tools.tool != 44 && tools.tool != 45 && this.OpenOnOThisClick && !this.tempParent.transform.root.GetComponent<tools>().EscMenu.active && Vector3.Distance(base.transform.position, this.tempParent.transform.position) < 5f)
		{
			this.Interact();
		}
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00054BFD File Offset: 0x00052DFD
	public void Interact()
	{
		if (!this.moving)
		{
			base.StartCoroutine(this.LerpPosition());
			this.moving = true;
		}
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00054C1C File Offset: 0x00052E1C
	public void JustOpen()
	{
		if (Vector3.Distance(base.transform.position, this.tempParent.transform.position) < 8f)
		{
			if (this.Opening)
			{
				return;
			}
			this.moving = true;
			this.Opening = true;
			this.Closing = false;
			base.StartCoroutine(this.LerpOpen());
			if (this.GarageDoor)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().GarageDoor);
			}
			if (this.SlidingDoor)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().SlidingDoor);
			}
		}
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00054CCC File Offset: 0x00052ECC
	public void JusClose()
	{
		if (this.Closing)
		{
			return;
		}
		this.moving = true;
		this.Closing = true;
		this.Opening = false;
		base.StartCoroutine(this.LerpClose());
		if (this.GarageDoor)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().GarageDoor);
		}
		if (this.SlidingDoor)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().SlidingDoor);
		}
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00054D54 File Offset: 0x00052F54
	private IEnumerator LerpPosition()
	{
		float time = 0f;
		Vector3 startPosition = this.Gate.transform.position;
		Quaternion startRotation = this.Gate.transform.rotation;
		if (!this.Open)
		{
			if (this.InSaver)
			{
				GameObject.Find("Player").GetComponent<tools>().Loadingtext.SetActive(true);
			}
			yield return 0;
			if (this.InSaver)
			{
				this.InSaver.Load();
			}
			if (this.GarageDoor)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().GarageDoor);
			}
			else if (!this.GarageDoor)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().OpenDoor);
			}
			while (time < this.speed)
			{
				this.Gate.transform.rotation = Quaternion.Lerp(startRotation, this.GateOpen.transform.rotation, time / this.speed);
				this.Gate.transform.position = Vector3.Lerp(startPosition, this.GateOpen.transform.position, time / this.speed);
				time += Time.deltaTime;
				yield return null;
			}
			this.Open = true;
			this.moving = false;
			this.Gate.transform.position = this.GateOpen.transform.position;
			this.Gate.transform.rotation = this.GateOpen.transform.rotation;
		}
		else
		{
			if (this.GarageDoor)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().GarageDoor);
			}
			else if (!this.GarageDoor)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().CloseDoor);
			}
			while (time < this.speed)
			{
				this.Gate.transform.rotation = Quaternion.Lerp(startRotation, this.GateClosed.transform.rotation, time / this.speed);
				this.Gate.transform.position = Vector3.Lerp(startPosition, this.GateClosed.transform.position, time / this.speed);
				time += Time.deltaTime;
				yield return null;
			}
			if (this.MainSaver)
			{
				GameObject.Find("Player").GetComponent<tools>().Savingtext.SetActive(true);
			}
			yield return 0;
			if (this.MainSaver)
			{
				this.MainSaver.Save(false, true, false);
			}
			this.Open = false;
			this.moving = false;
			this.Gate.transform.position = this.GateClosed.transform.position;
			this.Gate.transform.rotation = this.GateClosed.transform.rotation;
		}
		yield break;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00054D63 File Offset: 0x00052F63
	private IEnumerator LerpOpen()
	{
		float time = 0f;
		Vector3 startPosition = this.Gate.transform.position;
		Vector3 startPosition2 = Vector3.zero;
		if (this.Gate2)
		{
			startPosition2 = this.Gate2.transform.position;
		}
		Quaternion startRotation = this.Gate.transform.rotation;
		while (time < this.speed && !this.Closing)
		{
			this.Gate.transform.rotation = Quaternion.Lerp(startRotation, this.GateOpen.transform.rotation, time / this.speed);
			this.Gate.transform.position = Vector3.Lerp(startPosition, this.GateOpen.transform.position, time / this.speed);
			if (this.Gate2)
			{
				this.Gate2.transform.position = Vector3.Lerp(startPosition2, this.Gate2Open.transform.position, time / this.speed);
			}
			time += Time.deltaTime;
			yield return null;
		}
		this.Opening = false;
		this.moving = false;
		yield break;
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00054D72 File Offset: 0x00052F72
	private IEnumerator LerpClose()
	{
		float time = 0f;
		Vector3 startPosition = this.Gate.transform.position;
		Vector3 startPosition2 = Vector3.zero;
		if (this.Gate2)
		{
			startPosition2 = this.Gate2.transform.position;
		}
		Quaternion startRotation = this.Gate.transform.rotation;
		while (time < this.speed && !this.Opening)
		{
			this.Gate.transform.rotation = Quaternion.Lerp(startRotation, this.GateClosed.transform.rotation, time / this.speed);
			this.Gate.transform.position = Vector3.Lerp(startPosition, this.GateClosed.transform.position, time / this.speed);
			if (this.Gate2)
			{
				this.Gate2.transform.position = Vector3.Lerp(startPosition2, this.Gate2Closed.transform.position, time / this.speed);
			}
			time += Time.deltaTime;
			yield return null;
		}
		this.Closing = false;
		this.moving = false;
		yield break;
	}

	// Token: 0x040010A5 RID: 4261
	public bool GarageDoor;

	// Token: 0x040010A6 RID: 4262
	public bool SlidingDoor;

	// Token: 0x040010A7 RID: 4263
	public GameObject Gate;

	// Token: 0x040010A8 RID: 4264
	public GameObject GateOpen;

	// Token: 0x040010A9 RID: 4265
	public GameObject GateClosed;

	// Token: 0x040010AA RID: 4266
	public GameObject Gate2;

	// Token: 0x040010AB RID: 4267
	public GameObject Gate2Open;

	// Token: 0x040010AC RID: 4268
	public GameObject Gate2Closed;

	// Token: 0x040010AD RID: 4269
	public bool Open;

	// Token: 0x040010AE RID: 4270
	public bool Opening;

	// Token: 0x040010AF RID: 4271
	public bool Closing;

	// Token: 0x040010B0 RID: 4272
	public bool moving;

	// Token: 0x040010B1 RID: 4273
	public float speed;

	// Token: 0x040010B2 RID: 4274
	public GameObject tempParent;

	// Token: 0x040010B3 RID: 4275
	public GameObject AudioParent;

	// Token: 0x040010B4 RID: 4276
	public bool OpenOnOThisClick;

	// Token: 0x040010B5 RID: 4277
	public SaveInside InSaver;

	// Token: 0x040010B6 RID: 4278
	public Saver MainSaver;
}
