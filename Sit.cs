using System;
using System.Collections;
using System.Collections.Generic;
using NWH.Common.SceneManagement;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Modules.MotorcycleModule;
using Rewired;
using RVP;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class Sit : MonoBehaviour
{
	// Token: 0x060008BF RID: 2239 RVA: 0x00053ADD File Offset: 0x00051CDD
	private void Awake()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.RBList = new List<Rigidbody>();
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00053AFB File Offset: 0x00051CFB
	private void Start()
	{
		base.StartCoroutine(this.LateStart());
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00053B0A File Offset: 0x00051D0A
	private IEnumerator LateStart()
	{
		yield return new WaitForSeconds(2f);
		if (base.transform.root != null && base.transform.root.tag == "Vehicle")
		{
			this.exp = base.transform.root.GetComponent<VehicleController>();
			this.exp.Sleep();
		}
		yield break;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00053B19 File Offset: 0x00051D19
	private void FixedUpdate()
	{
		if (!this.sittingHere || !this.cansit)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00053B34 File Offset: 0x00051D34
	private void Update()
	{
		if (tools.tool == 24)
		{
			return;
		}
		if (!tools.sitting && !this.sittingHere)
		{
			if (this.cansit && !this.Player.GetComponent<tools>().Backpack.active)
			{
				if (this.player.GetButtonDown("Sit in car") && base.transform.root.GetComponent<MainCarProperties>().Owner != "Junkyard" && base.transform.root.GetComponent<MainCarProperties>().Owner != "Dealer")
				{
					if (this.Player.GetComponent<FloatingPoinMyfix>())
					{
						this.Player.GetComponent<FloatingPoinMyfix>().ResetPosition();
					}
					this.exp = base.transform.root.GetComponent<VehicleController>();
					if (!this.passanger)
					{
						if (base.gameObject.transform.root.GetComponent<MPobject>())
						{
							tools.NetworkPLayer.pickup(base.gameObject.transform.root.GetComponent<MPobject>().networkDummy);
						}
						this.Player.GetComponent<CharacterVehicleChanger>().EnterExitVehicle();
						base.gameObject.transform.root.GetComponent<VehicleDamage>().Start();
						if (tools.DirectSteer || base.transform.root.GetComponent<MainCarProperties>().Bike)
						{
							this.exp.steering.useDirectInput = true;
						}
						else
						{
							this.exp.steering.useDirectInput = false;
						}
						this.exp.Wake();
						if (PlayerPrefs.HasKey("SwappedInput") && PlayerPrefs.GetFloat("SwappedInput") == 1f)
						{
							this.exp.input.swapInputInReverse = true;
						}
						else
						{
							this.exp.input.swapInputInReverse = false;
						}
						if (PlayerPrefs.HasKey("InvSteering") && PlayerPrefs.GetFloat("InvSteering") == 1f)
						{
							this.exp.input.invertSteering = true;
						}
						else
						{
							this.exp.input.invertSteering = false;
						}
						if (PlayerPrefs.HasKey("InvBraking") && PlayerPrefs.GetFloat("InvBraking") == 1f)
						{
							this.exp.input.invertBrakes = true;
						}
						else
						{
							this.exp.input.invertBrakes = false;
						}
						if (PlayerPrefs.HasKey("InvThrottle") && PlayerPrefs.GetFloat("InvThrottle") == 1f)
						{
							this.exp.input.invertThrottle = true;
						}
						else
						{
							this.exp.input.invertThrottle = false;
						}
						if (PlayerPrefs.HasKey("InvClutch") && PlayerPrefs.GetFloat("InvClutch") == 1f)
						{
							this.exp.input.invertClutch = true;
						}
						else
						{
							this.exp.input.invertClutch = false;
						}
						if (tools.AutoClutch)
						{
							this.exp.powertrain.clutch.isAutomatic = true;
						}
						else
						{
							this.exp.powertrain.clutch.isAutomatic = false;
						}
						if (base.transform.root.GetComponent<MainCarProperties>().Gearbox && !base.transform.root.GetComponent<MainCarProperties>().Gearbox.Manual && base.transform.root.GetComponent<MainCarProperties>())
						{
							this.exp.powertrain.clutch.isAutomatic = true;
						}
						base.transform.root.GetComponent<MainCarProperties>().CheckChildVisConditions();
						base.transform.root.GetComponent<MainCarProperties>().CheckStates();
						base.transform.root.GetComponent<MainCarProperties>().SittingInCar = true;
						this.exp.ApplyInitialRigidbodyValues();
						this.AllCars = GameObject.FindGameObjectsWithTag("Vehicle");
						for (int i = 0; i < this.AllCars.Length; i++)
						{
							GameObject gameObject = this.AllCars[i];
							if (gameObject.transform.root != base.transform.root && gameObject.gameObject.GetComponent<MainCarProperties>())
							{
								foreach (Collider collider in Physics.OverlapSphere(gameObject.transform.position, 0.6f))
								{
									if (collider.gameObject.transform.root.GetComponent<MainTrailerProperties>() || (collider.gameObject.transform.root.GetComponent<MainCarProperties>() && collider.gameObject.transform.root.GetComponent<MainCarProperties>().CarTrailer && collider.gameObject.transform.root != gameObject.transform.root))
									{
										if (collider.gameObject.GetComponent<MPobject>() && collider.gameObject.GetComponent<MPobject>().networkDummy)
										{
											tools.NetworkPLayer.pickup(collider.gameObject.GetComponent<MPobject>().networkDummy);
										}
										gameObject.AddComponent<FixedJoint>();
										gameObject.GetComponent<FixedJoint>().connectedBody = collider.transform.root.gameObject.GetComponent<Rigidbody>();
										gameObject.GetComponent<FixedJoint>().massScale = 100f;
										break;
									}
								}
							}
						}
					}
					this.Player.GetComponent<tools>().DropAll();
					if (this.SitPosition)
					{
						this.Player.transform.position = this.SitPosition.position;
						this.Player.transform.rotation = this.SitPosition.rotation;
						base.transform.root.GetComponent<VehicleController>().moduleManager.GetModule<MotorcycleModule>().IsSitting = true;
						base.transform.root.GetComponent<VehicleController>().moduleManager.GetModule<MotorcycleModule>().UnFreeze();
					}
					else
					{
						this.Player.transform.rotation = base.transform.rotation;
					}
					if (base.gameObject.transform.root.GetComponent<MPobject>())
					{
						tools.NetworkPLayer.Sit(base.transform.parent.GetComponent<MPobject>().networkDummy);
					}
					this.Player.GetComponent<Rigidbody>().useGravity = false;
					this.Player.GetComponent<Rigidbody>().detectCollisions = false;
					this.Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
					this.Player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
					this.Player.transform.SetParent(base.transform.parent.parent);
					this.Player.AddComponent<FixedJoint>();
					this.Player.GetComponent<FixedJoint>().connectedBody = base.transform.root.GetComponent<Rigidbody>();
					this.Player.GetComponent<Rigidbody>().GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
					tools.sitting = true;
					this.sittingHere = true;
					tools.exp = this.exp;
					this.Player.GetComponent<FirstPersonAIO>().enabled = false;
					this.Head.transform.SetParent(base.transform.parent);
					this.Camera.GetComponent<carcam>().enabled = true;
					if (!this.passanger)
					{
						if (tools.UseMouseSteering)
						{
							this.Player.GetComponent<tools>().ActivateMouseSteering();
						}
						else
						{
							this.Player.GetComponent<tools>().DisableMouseSteering();
						}
					}
					if (!this.passanger && !base.gameObject.transform.root.GetComponent<MPobject>())
					{
						if (base.transform.root.GetComponent<MainCarProperties>().insideitems != null)
						{
							base.transform.root.GetComponent<MainCarProperties>().insideitems.Sit();
						}
						if (base.transform.root.GetComponent<MainCarProperties>().Traielerinsideitems != null)
						{
							base.transform.root.GetComponent<MainCarProperties>().Traielerinsideitems.Sit();
						}
					}
				}
				if (this.player.GetButtonDown("Sleep in car") && base.transform.root.GetComponent<MainCarProperties>().Owner != "Junkyard" && base.transform.root.GetComponent<MainCarProperties>().Owner != "Dealer")
				{
					this.Player.GetComponent<tools>().Advance4h();
					return;
				}
			}
		}
		else if (tools.sitting && this.sittingHere && base.transform.root.GetComponent<MainCarProperties>().exp.Speed < 5f)
		{
			if (this.Player.GetComponent<FloatingPoinMyfix>())
			{
				this.Player.GetComponent<FloatingPoinMyfix>().ResetPosition();
			}
			if (this.player.GetButtonDown("Sit in car") && this.Camera.GetComponent<Camera>().enabled)
			{
				this.Player.GetComponent<tools>().timer -= 60f;
				if (!this.passanger && base.transform.root.GetComponent<MainCarProperties>().insideitems != null)
				{
					base.transform.root.GetComponent<MainCarProperties>().insideitems.Stand();
				}
				if (!this.passanger && base.transform.root.GetComponent<MainCarProperties>().Traielerinsideitems != null)
				{
					base.transform.root.GetComponent<MainCarProperties>().Traielerinsideitems.Stand();
				}
				this.Player.GetComponent<Crouch>().inCar = true;
				if (!this.passanger)
				{
					this.Player.GetComponent<CharacterVehicleChanger>().EnterExitVehicle();
				}
				this.cansit = false;
				tools.cansit = false;
				if (base.gameObject.transform.root.GetComponent<MPobject>())
				{
					tools.NetworkPLayer.Stand(base.transform.parent.GetComponent<MPobject>().networkDummy);
				}
				this.Player.GetComponent<Rigidbody>().useGravity = true;
				this.Player.GetComponent<Rigidbody>().detectCollisions = true;
				if (!this.passanger)
				{
					base.gameObject.transform.root.GetComponent<VehicleDamage>().Start();
					for (int k = 0; k < this.AllCars.Length; k++)
					{
						GameObject gameObject2 = this.AllCars[k];
						if (gameObject2 && gameObject2 != base.transform.root && gameObject2.GetComponent<FixedJoint>())
						{
							UnityEngine.Object.Destroy(gameObject2.GetComponent<FixedJoint>());
						}
					}
					if (base.transform.root.GetComponent<MainCarProperties>())
					{
						base.transform.root.GetComponent<MainCarProperties>().SittingInCar = false;
						base.transform.root.GetComponent<MainCarProperties>().CheckChildVisConditions();
						base.transform.root.GetComponent<MainCarProperties>().CheckStates();
					}
				}
				this.Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
				this.Player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				this.Player.transform.SetParent(null);
				UnityEngine.Object.Destroy(this.Player.GetComponent<FixedJoint>());
				this.Player.GetComponent<tools>().SetPlayerCamera();
				tools.sitting = false;
				this.sittingHere = false;
				this.Camera.GetComponent<carcam>().enabled = false;
				this.Player.GetComponent<FirstPersonAIO>().targetAngles.x = this.Camera.transform.rotation.eulerAngles.x * -1f;
				this.Player.GetComponent<FirstPersonAIO>().targetAngles.y = base.transform.root.rotation.eulerAngles.y - this.Player.GetComponent<FirstPersonAIO>().originalRotation.y;
				this.Player.GetComponent<FirstPersonAIO>().enabled = true;
				this.Head.transform.SetParent(this.Player.transform);
				if (this.SitPosition)
				{
					base.transform.root.GetComponent<VehicleController>().moduleManager.GetModule<MotorcycleModule>().IsSitting = false;
					this.Player.GetComponent<Crouch>().WaitStart();
					this.Player.GetComponent<Crouch>().inCar = false;
					base.transform.root.GetComponent<VehicleController>().moduleManager.GetModule<MotorcycleModule>().Freeze();
					if (this.OutPosL.position.y + 0.2f > this.OutPosR.position.y)
					{
						this.Player.transform.position = this.OutPosL.position;
					}
					else
					{
						this.Player.transform.position = this.OutPosR.position;
					}
				}
				this.Player.GetComponent<tools>().DisableMouseSteering();
			}
		}
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00054853 File Offset: 0x00052A53
	private IEnumerator Wait()
	{
		yield return 100;
		yield break;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0005485C File Offset: 0x00052A5C
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.transform.name == "Player")
		{
			this.Player = other.gameObject;
			foreach (Transform transform in this.Player.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "Player Camera")
				{
					this.Camera = transform.gameObject;
				}
				if (transform.name == "HeadJoint")
				{
					this.Head = transform.gameObject;
				}
				if (transform.name == "joint2")
				{
					this.CameraParent = transform.gameObject;
				}
			}
			this.cansit = true;
			base.enabled = true;
		}
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0005491A File Offset: 0x00052B1A
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.transform.name == "Player")
		{
			this.cansit = true;
			base.enabled = true;
			tools.cansit = true;
		}
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0005494C File Offset: 0x00052B4C
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.transform.name == "Player")
		{
			this.cansit = false;
			tools.cansit = false;
		}
	}

	// Token: 0x04001063 RID: 4195
	private Player player;

	// Token: 0x04001064 RID: 4196
	private Vector3 objectPos;

	// Token: 0x04001065 RID: 4197
	private float distance;

	// Token: 0x04001066 RID: 4198
	public bool passanger;

	// Token: 0x04001067 RID: 4199
	public bool cansit;

	// Token: 0x04001068 RID: 4200
	public Transform SitPosition;

	// Token: 0x04001069 RID: 4201
	public Transform OutPosL;

	// Token: 0x0400106A RID: 4202
	public Transform OutPosR;

	// Token: 0x0400106B RID: 4203
	public GameObject Player;

	// Token: 0x0400106C RID: 4204
	public GameObject Camera;

	// Token: 0x0400106D RID: 4205
	public GameObject Head;

	// Token: 0x0400106E RID: 4206
	public GameObject CameraParent;

	// Token: 0x0400106F RID: 4207
	public bool sittingHere;

	// Token: 0x04001070 RID: 4208
	public VehicleController exp;

	// Token: 0x04001071 RID: 4209
	public List<Rigidbody> RBList;

	// Token: 0x04001072 RID: 4210
	public GameObject[] AllCars;
}
