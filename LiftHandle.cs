using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class LiftHandle : MonoBehaviour
{
	// Token: 0x06000BFC RID: 3068 RVA: 0x0008494C File Offset: 0x00082B4C
	private void Start()
	{
		if (!this.ThisIsCarJack && this.LiftObject && this.LiftObject.transform.name == "RISER")
		{
			this.LiftObject.transform.localPosition = new Vector3(-0.02978098f, 0.03220512f, -0.0007836223f);
		}
		this.AudioParent = GameObject.Find("hand");
		if (!this.ThisIsBikeStand && !this.ThisIsCarJack)
		{
			this.steps = 0;
		}
		this.CoroutineAllowed = true;
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x000849DC File Offset: 0x00082BDC
	private void Update()
	{
		if (this.ThisIsCarJack)
		{
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.8f, 1 << LayerMask.NameToLayer("Default")) && raycastHit.collider.gameObject == base.gameObject)
			{
				if (this.steps < this.limit)
				{
					this.LiftObject.transform.position += base.transform.TransformDirection(0f, 0.01f, 0f);
					this.steps++;
					if (this.RotatingObj)
					{
						this.RotatingObj.transform.LookAt(this.RotatingToObj.transform);
						this.RotatingToObj.transform.position += base.transform.TransformDirection(0.003f, 0f, 0f);
					}
				}
				if (base.transform.parent.GetComponent<MPobject>() && base.transform.parent.GetComponent<MPobject>().networkDummy)
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.LiftObject(this.steps, this.LiftObject.transform.localPosition);
				}
			}
			RaycastHit raycastHit2;
			if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 1.8f, 1 << LayerMask.NameToLayer("Default")) && raycastHit2.collider.gameObject == base.gameObject)
			{
				if (this.steps > 0)
				{
					this.LiftObject.transform.position -= base.transform.TransformDirection(0f, 0.01f, 0f);
					this.steps--;
					if (this.RotatingObj)
					{
						this.RotatingObj.transform.LookAt(this.RotatingToObj.transform);
						this.RotatingToObj.transform.position -= base.transform.TransformDirection(0.003f, 0f, 0f);
					}
				}
				if (base.transform.parent.GetComponent<MPobject>() && base.transform.parent.GetComponent<MPobject>().networkDummy)
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.LiftObject(this.steps, this.LiftObject.transform.localPosition);
				}
			}
		}
		if (this.ThisIsBikeStand)
		{
			RaycastHit raycastHit3;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit3, 1.8f, 1 << LayerMask.NameToLayer("Default")) && raycastHit3.collider.gameObject == base.gameObject)
			{
				if (this.steps < this.limit)
				{
					this.LiftObject.transform.position -= base.transform.TransformDirection(0f, 0.01f, 0f);
					this.steps++;
					if (this.RotatingObj)
					{
						this.RotatingObj.transform.LookAt(this.RotatingToObj.transform);
						this.RotatingToObj.transform.position += base.transform.TransformDirection(0.003f, 0f, 0f);
					}
				}
				if (base.transform.parent.GetComponent<MPobject>() && base.transform.parent.GetComponent<MPobject>().networkDummy)
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.LiftObject(this.steps, this.LiftObject.transform.localPosition);
				}
			}
			RaycastHit raycastHit4;
			if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit4, 1.8f, 1 << LayerMask.NameToLayer("Default")) && raycastHit4.collider.gameObject == base.gameObject)
			{
				if (this.steps > 0)
				{
					this.LiftObject.transform.position += base.transform.TransformDirection(0f, 0.01f, 0f);
					this.steps--;
					if (this.RotatingObj)
					{
						this.RotatingObj.transform.LookAt(this.RotatingToObj.transform);
						this.RotatingToObj.transform.position -= base.transform.TransformDirection(0.003f, 0f, 0f);
					}
				}
				if (base.transform.parent.GetComponent<MPobject>() && base.transform.parent.GetComponent<MPobject>().networkDummy)
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.LiftObject(this.steps, this.LiftObject.transform.localPosition);
				}
			}
		}
		RaycastHit raycastHit5;
		if (this.Light && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit5, 1f, 1 << LayerMask.NameToLayer("Default")) && raycastHit5.collider.gameObject == base.gameObject)
		{
			if (this.LightOn)
			{
				this.LightOn = false;
				GameObject[] lights = this.Lights;
				for (int i = 0; i < lights.Length; i++)
				{
					lights[i].SetActive(false);
				}
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BlinkerOff);
				if (this.BuildingLights)
				{
					foreach (Transform transform in base.transform.root.transform.GetComponentsInChildren<Transform>())
					{
						if (transform.GetComponent<Light>())
						{
							transform.GetComponent<Light>().enabled = false;
						}
					}
				}
			}
			else
			{
				this.LightOn = true;
				GameObject[] lights = this.Lights;
				for (int i = 0; i < lights.Length; i++)
				{
					lights[i].SetActive(true);
				}
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BlinkerOn);
				if (this.BuildingLights)
				{
					foreach (Transform transform2 in base.transform.root.transform.GetComponentsInChildren<Transform>())
					{
						if (transform2.GetComponent<Light>())
						{
							transform2.GetComponent<Light>().enabled = true;
						}
					}
				}
			}
		}
		RaycastHit raycastHit6;
		if (this.GateButton && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit6, 1f, 1 << LayerMask.NameToLayer("Default")) && raycastHit6.collider.gameObject == base.gameObject)
		{
			if (this.Gate.GetComponent<MPobject>() && this.Gate.GetComponent<MPobject>().networkDummy)
			{
				tools.NetworkPLayer.pickup(this.Gate.GetComponent<MPobject>().networkDummy);
			}
			this.Gate.GetComponent<OpenGarage>().Interact();
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BlinkerOff);
		}
		if (this.ThisIsCarLift)
		{
			RaycastHit raycastHit7;
			if (this.Up && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit7, 1f, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Weld")) && raycastHit7.collider.gameObject == base.gameObject)
			{
				this.Rising = true;
				tools.cooldown = true;
				if (this.LiftObject.GetComponent<MPobject>() && this.LiftObject.GetComponent<MPobject>().networkDummy)
				{
					tools.NetworkPLayer.pickup(this.LiftObject.GetComponent<MPobject>().networkDummy);
				}
				this.AudioParent.GetComponent<AudioSource>().clip = this.AudioParent.GetComponent<AudioManager>().CarLift;
				this.AudioParent.GetComponent<AudioSource>().Play();
			}
			RaycastHit raycastHit8;
			if (this.Down && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit8, 1f, 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Weld")) && raycastHit8.collider.gameObject == base.gameObject)
			{
				this.Lowering = true;
				tools.cooldown = true;
				if (this.LiftObject.GetComponent<MPobject>() && this.LiftObject.GetComponent<MPobject>().networkDummy)
				{
					tools.NetworkPLayer.pickup(this.LiftObject.GetComponent<MPobject>().networkDummy);
				}
				this.AudioParent.GetComponent<AudioSource>().clip = this.AudioParent.GetComponent<AudioManager>().CarLift;
				this.AudioParent.GetComponent<AudioSource>().Play();
			}
			if (Input.GetMouseButtonUp(0) && (this.Rising || this.Lowering))
			{
				this.Rising = false;
				this.Lowering = false;
				this.AudioParent.GetComponent<AudioSource>().Stop();
			}
			if (this.Rising)
			{
				if (this.LiftObject.transform.localPosition.y < 1f)
				{
					this.LiftObject.transform.position += base.transform.up * 0.3f * Time.deltaTime;
					this.steps++;
					this.OtherBUtton.GetComponent<LiftHandle>().steps++;
				}
				else
				{
					this.Rising = false;
					this.AudioParent.GetComponent<AudioSource>().Stop();
				}
			}
			if (this.Lowering)
			{
				if (this.LiftObject.transform.localPosition.y > -1f)
				{
					this.LiftObject.transform.position -= base.transform.up * 0.3f * Time.deltaTime;
					this.steps--;
					this.OtherBUtton.GetComponent<LiftHandle>().steps--;
				}
				else
				{
					this.Lowering = false;
					this.AudioParent.GetComponent<AudioSource>().Stop();
				}
			}
		}
		RaycastHit raycastHit9;
		if (this.ThisIsTireMounter && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit9, 1.6f, 1 << LayerMask.NameToLayer("Weld")) && raycastHit9.collider.gameObject == base.gameObject && this.CoroutineAllowed)
		{
			if (base.transform.parent.GetComponent<MPobject>())
			{
				base.transform.parent.GetComponent<MPobject>().networkDummy.StartMounting();
				return;
			}
			this.StartMounting();
		}
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x000856AC File Offset: 0x000838AC
	public void StartMounting()
	{
		foreach (Transform transform in this.RotatingMount.GetComponentsInChildren<Transform>())
		{
			if (transform.GetComponent<CarProperties>() && transform.GetComponent<CarProperties>().RealWheel && transform.GetComponent<Rigidbody>())
			{
				this.disc = transform;
			}
			if (transform.GetComponent<CarProperties>() && transform.GetComponent<CarProperties>().Tire)
			{
				this.tire = transform;
			}
		}
		if (this.tire && this.tire.GetComponent<Rigidbody>())
		{
			base.StartCoroutine(this.MountTires());
			this.CoroutineAllowed = false;
		}
		if (this.tire && !this.tire.GetComponent<Rigidbody>())
		{
			base.StartCoroutine(this.UnMountTires());
			this.CoroutineAllowed = false;
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x0008578C File Offset: 0x0008398C
	private IEnumerator MountTires()
	{
		this.disc.GetComponent<Rigidbody>().isKinematic = true;
		this.disc.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.tire.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		for (float i = 0f; i <= 270f; i += 5f)
		{
			this.RotatingMount.transform.rotation = Quaternion.Euler(0f, i, 0f);
			foreach (Transform transform in this.RotatingMount.GetComponentsInChildren<Transform>())
			{
				if (transform.GetComponent<HexNut>())
				{
					transform.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			UnityEngine.Object.Destroy(this.tire.GetComponent<FixedJoint>());
			UnityEngine.Object.Destroy(this.tire.GetComponent<Rigidbody>());
			this.tire.transform.position = this.tire.transform.position + new Vector3(0f, -0.001f, 0f);
			yield return 0;
		}
		this.CoroutineAllowed = true;
		this.disc.GetComponent<Rigidbody>().isKinematic = false;
		this.disc.gameObject.layer = LayerMask.NameToLayer("LooseParts");
		this.tire.transform.GetComponent<Partinfo>().attach();
		this.tire.transform.parent.parent.GetComponent<CarProperties>().tireObject = this.tire.transform.GetComponent<CarProperties>();
		this.tire.GetComponent<CarProperties>().TirePressure = 0f;
		this.disc = null;
		this.tire = null;
		yield break;
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x0008579B File Offset: 0x0008399B
	private IEnumerator UnMountTires()
	{
		this.disc.GetComponent<Rigidbody>().isKinematic = true;
		this.disc.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		this.tire.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		for (float i = 0f; i <= 270f; i += 5f)
		{
			this.RotatingMount.transform.rotation = Quaternion.Euler(0f, i, 0f);
			foreach (Transform transform in this.RotatingMount.GetComponentsInChildren<Transform>())
			{
				if (transform.GetComponent<HexNut>())
				{
					transform.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			this.tire.transform.position = this.tire.transform.position + new Vector3(0f, 0.001f, 0f);
			yield return 0;
		}
		this.CoroutineAllowed = true;
		this.disc.GetComponent<Rigidbody>().isKinematic = false;
		this.disc.gameObject.layer = LayerMask.NameToLayer("LooseParts");
		this.tire.transform.GetComponent<Partinfo>().remove(false);
		this.tire.transform.parent.parent.GetComponent<CarProperties>().tireObject = null;
		this.tire.GetComponent<CarProperties>().TirePressure = 0f;
		this.disc = null;
		this.tire = null;
		yield break;
	}

	// Token: 0x040014B8 RID: 5304
	public GameObject AudioParent;

	// Token: 0x040014B9 RID: 5305
	public GameObject LiftObject;

	// Token: 0x040014BA RID: 5306
	public GameObject RotatingObj;

	// Token: 0x040014BB RID: 5307
	public GameObject RotatingToObj;

	// Token: 0x040014BC RID: 5308
	public bool Up;

	// Token: 0x040014BD RID: 5309
	public bool Down;

	// Token: 0x040014BE RID: 5310
	public bool Rising;

	// Token: 0x040014BF RID: 5311
	public bool Lowering;

	// Token: 0x040014C0 RID: 5312
	public bool ThisIsCarJack;

	// Token: 0x040014C1 RID: 5313
	public bool ThisIsBikeStand;

	// Token: 0x040014C2 RID: 5314
	public bool ThisIsCarLift;

	// Token: 0x040014C3 RID: 5315
	public bool ThisIsTireMounter;

	// Token: 0x040014C4 RID: 5316
	public GameObject RotatingMount;

	// Token: 0x040014C5 RID: 5317
	public GameObject DiscParent;

	// Token: 0x040014C6 RID: 5318
	public GameObject TireParent;

	// Token: 0x040014C7 RID: 5319
	public int limit;

	// Token: 0x040014C8 RID: 5320
	public int steps;

	// Token: 0x040014C9 RID: 5321
	public GameObject OtherBUtton;

	// Token: 0x040014CA RID: 5322
	public bool CoroutineAllowed;

	// Token: 0x040014CB RID: 5323
	public Transform tire;

	// Token: 0x040014CC RID: 5324
	public Transform disc;

	// Token: 0x040014CD RID: 5325
	public bool BuildingLights;

	// Token: 0x040014CE RID: 5326
	public bool Light;

	// Token: 0x040014CF RID: 5327
	public bool LightOn;

	// Token: 0x040014D0 RID: 5328
	public GameObject[] Lights;

	// Token: 0x040014D1 RID: 5329
	public bool GateButton;

	// Token: 0x040014D2 RID: 5330
	public GameObject Gate;
}
