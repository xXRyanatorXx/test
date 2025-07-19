using System;
using System.Collections;
using Mirror;
using PaintIn3D;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020E RID: 526
public class PickupTool : MonoBehaviour
{
	// Token: 0x06000C3F RID: 3135 RVA: 0x00086988 File Offset: 0x00084B88
	private void Start()
	{
		if (this.crowbar)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.AudioParent = GameObject.Find("hand");
		this.Hand = GameObject.Find("handStatic");
		this.ToolHand = GameObject.Find("ToolHand");
		if (this.fuelpump)
		{
			this.FuelPrice.text = "Price  " + this.price.ToString();
			this.FuelPrice2.text = "Price  " + this.price.ToString();
		}
		if (this.paint)
		{
			if (!this.marker)
			{
				this.ToolHand = GameObject.Find("PaintHand");
			}
			base.GetComponentInChildren<P3dPaintSphere>().Color = this.colorpaint;
			if (!this.marker)
			{
				this.ps = base.GetComponentInChildren<ParticleSystem>();
				this.ps.main.startColor = new ParticleSystem.MinMaxGradient(this.colorpaint);
			}
			this.removableCup.GetComponent<Renderer>().material.SetColor("_Color", this.colorpaint);
			if (this.FluidContainer && !this.FluidContainerFillable && this.FluidSize <= 0f)
			{
				this.removableCup.GetComponent<Renderer>().enabled = false;
			}
			if (this.paint && this.paintlife <= 0f)
			{
				this.removableCup.GetComponent<Renderer>().enabled = false;
			}
			if (this.filler && this.paintlife <= 0f)
			{
				this.removableCup.GetComponent<Renderer>().enabled = false;
			}
		}
		if (this.waterhose || this.sandblaster)
		{
			this.ToolHand = GameObject.Find("PaintHand");
			this.ps = base.GetComponentInChildren<ParticleSystem>();
			ParticleSystem.MainModule main = this.ps.main;
		}
		if (this.FlashLight)
		{
			this.ToolHand = GameObject.Find("LightHand");
		}
		if (this.FluidContainer)
		{
			this.ToolHand = GameObject.Find("PourHand");
		}
		if (this.fuelpump)
		{
			this.ToolHand = GameObject.Find("handFuel");
			this.ps = base.GetComponentInChildren<ParticleSystem>();
			ParticleSystem.MainModule main2 = this.ps.main;
		}
		this.VisualUpdate();
		if (tools.MPrunning && (base.transform.name == "WelderHandle" || base.transform.name == "WaterHose" || base.transform.name == "Nozzle") && !base.GetComponent<MPobject>())
		{
			base.gameObject.AddComponent<MPobject>();
		}
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00086C23 File Offset: 0x00084E23
	public void UpdateCondition(int paintlif, bool updatevisual)
	{
		this.paintlife += (float)paintlif;
		if (updatevisual)
		{
			this.VisualUpdate();
		}
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00086C40 File Offset: 0x00084E40
	public void VisualUpdate()
	{
		if (this.Filling && !this.Tint && !this.Decal)
		{
			this.Filling.transform.localScale = new Vector3(1f, 1f, this.paintlife / 100f);
		}
		if (this.Filling && this.Tint && !this.Decal)
		{
			this.Filling.transform.localScale = new Vector3(this.paintlife / 10f + 0.4f, 1f, this.paintlife / 10f + 0.4f);
		}
		if (this.Filling && !this.Tint && this.Decal)
		{
			this.Filling.transform.localScale = new Vector3(1f, this.paintlife / 100f, 1f);
		}
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00086D38 File Offset: 0x00084F38
	public void RestartTool()
	{
		if (this.wrench)
		{
			tools.tool = 2;
		}
		if (this.screwdriver)
		{
			tools.tool = 3;
		}
		if (this.cutter)
		{
			tools.tool = 4;
		}
		if (this.welder)
		{
			tools.tool = 5;
		}
		if (this.prytool)
		{
			tools.tool = 6;
		}
		if (this.filler)
		{
			tools.tool = 7;
		}
		if (this.SparkplugSocket)
		{
			tools.tool = 8;
		}
		if (this.plyer)
		{
			tools.tool = 0;
		}
		if (this.SpringCompressor)
		{
			tools.tool = 9;
		}
		if (this.crowbar)
		{
			tools.tool = 11;
		}
		if (this.hammer)
		{
			tools.tool = 12;
		}
		if (this.Garbagebag)
		{
			tools.tool = 24;
		}
		if (this.FramingMaterial)
		{
			tools.tool = 41;
			if (!this.PrefabReady)
			{
				this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.WallParts[tools._buildparent.currentIndex], base.transform.position, Quaternion.identity);
				this.PrefabReady.name = tools._buildparent.WallParts[tools._buildparent.currentIndex].name;
			}
		}
		if (this.RoofMaterial)
		{
			tools.tool = 43;
			if (!this.PrefabReady)
			{
				this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.RoofParts[tools._buildparent.currentIndex1], base.transform.position, Quaternion.identity);
				this.PrefabReady.name = tools._buildparent.RoofParts[tools._buildparent.currentIndex1].name;
			}
		}
		if (this.DoorMaterial)
		{
			tools.tool = 44;
			if (!this.PrefabReady)
			{
				this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.DoorParts[tools._buildparent.currentIndex2], base.transform.position, Quaternion.identity);
				this.PrefabReady.name = tools._buildparent.DoorParts[tools._buildparent.currentIndex2].name;
			}
		}
		if (this.MaterialChanger)
		{
			tools.tool = 45;
		}
		if (this.Sledgehammer)
		{
			tools.tool = 42;
		}
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00086F64 File Offset: 0x00085164
	private void Update()
	{
		if (tools.tool == 24)
		{
			return;
		}
		this.placetoput = null;
		this.seetoput = false;
		if (this.ConnectedObject)
		{
			RaycastHit raycastHit;
			if (base.transform.parent && (base.transform.parent.gameObject == this.Hand || base.transform.parent.gameObject == this.ToolHand) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject == this.ConnectedObject)
			{
				tools.canput = true;
			}
			if (this.fuelpump && (tools.money <= this.money || tools.canput))
			{
				this.StopParticles();
				this.AudioParent.GetComponent<AudioManager>().canplay = false;
			}
			if (Vector3.Distance(this.ConnectedObject.transform.position, base.transform.position) > this.HoseLength)
			{
				tools.holdingitem = false;
				tools.tool = 1;
				this.ThisInHandR = false;
				this.ThisInHandR = false;
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
				base.transform.position = this.ConnectedObject.transform.position;
				base.transform.rotation = this.ConnectedObject.transform.rotation;
				base.gameObject.transform.SetParent(this.ConnectedObject.transform);
				if (base.GetComponent<MPobject>())
				{
					tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
				}
				this.AudioParent.GetComponent<AudioSource>().Stop();
				if (this.waterhose || this.sandblaster)
				{
					this.StopParticles();
					this.AudioParent.GetComponent<AudioManager>().canplay = false;
				}
				if (this.fuelpump)
				{
					this.StopParticles();
					this.AudioParent.GetComponent<AudioManager>().canplay = false;
					this.Cashregister.BUY();
				}
				GameObject.Find("Player").GetComponent<tools>().SphereJump();
			}
		}
		if (Input.GetMouseButtonDown(0) && tools.tool != 18 && (this.paint || this.waterhose || this.fuelpump || this.sandblaster) && this.ThisInHandR && this.paintlife > 0f)
		{
			this.PlayParticles();
			this.AudioParent.GetComponent<AudioManager>().canplay = true;
		}
		if (Input.GetMouseButtonUp(0) && this.ThisInHandR)
		{
			this.StopParticles();
		}
		if (Input.GetMouseButton(0) && tools.tool != 18)
		{
			if (this.paint && this.ThisInHandR && this.paintlife > 0f)
			{
				this.paintlife -= Time.deltaTime;
			}
			if (this.sandblaster && this.ThisInHandR && this.paintlife > 0f)
			{
				this.paintlife -= Time.deltaTime;
				this.VisualUpdate();
			}
			if (this.fuelpump && this.Cashregister && tools.money > this.Cashregister.AllMoney && this.ThisInHandR && !tools.canput)
			{
				this.liters += 2.5f * Time.deltaTime;
				this.money = this.price * this.liters;
				this.Cashregister.AllMoney = this.Cashregister.SpawnSpot.GetComponent<PickupTool>().money + this.Cashregister.SpawnSpot2.GetComponent<PickupTool>().money;
				this.FuelLiters.text = "Liters  " + (Mathf.Round(this.liters * 100f) / 100f).ToString();
				this.FuelLiters2.text = "Liters  " + (Mathf.Round(this.liters * 100f) / 100f).ToString();
				this.FuelMoney.text = "Cost  " + (Mathf.Round(this.money * 100f) / 100f).ToString();
				this.FuelMoney2.text = "Cost  " + (Mathf.Round(this.money * 100f) / 100f).ToString();
				this.CashReg.text = (Mathf.Round(this.Cashregister.AllMoney * 100f) / 100f).ToString();
				if (!tools.canput)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
			}
			if (this.Consumable && this.ThisInHandR)
			{
				tools.Food += this.Food;
				tools.Drink += this.Drink;
				if (tools.Food > 1f)
				{
					tools.Food = 1f;
				}
				if (tools.Food < 0f)
				{
					tools.Food = 0f;
				}
				if (tools.Drink > 1f)
				{
					tools.Drink = 1f;
				}
				if (tools.Drink < 0f)
				{
					tools.Drink = 0f;
				}
				if (this.MONEY)
				{
					tools.money += this.paintlife;
				}
				tools.tool = 1;
				tools.holdingitem = false;
				tools.helditem = "Nothing";
				this.ThisInHandL = false;
				this.ThisInHandR = false;
				if (base.GetComponent<MPobject>())
				{
					base.GetComponent<MPobject>().networkDummy.DestroyMe();
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
		if (Input.GetMouseButtonDown(0) && this.FlashLight && this.ThisInHandR)
		{
			if (base.GetComponent<MPobject>())
			{
				if (!this.Light.active)
				{
					base.GetComponent<MPobject>().networkDummy.FlashLight(true);
				}
				else
				{
					base.GetComponent<MPobject>().networkDummy.FlashLight(false);
				}
			}
			else if (!this.Light.active)
			{
				this.FlashLightTurn(true);
			}
			else
			{
				this.FlashLightTurn(false);
			}
		}
		if (this.PrefabReady && this.FramingMaterial)
		{
			this.BuildSite = null;
			foreach (Collider collider in Physics.OverlapSphere(tools._buildPos.transform.position, 3f))
			{
				if (collider.gameObject.tag == "Building" && collider.gameObject != this.PrefabReady)
				{
					this.BuildSite = collider.gameObject.transform.parent.gameObject;
					this.PrefabReady.transform.SetParent(this.BuildSite.transform);
					break;
				}
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				tools.BuildingRotation += 90;
			}
			if (this.ThisInHandR)
			{
				if (!tools.cooldown3)
				{
					Vector3 position = this.PrefabReady.transform.position;
					if (Input.GetAxis("Mouse ScrollWheel") < 0f)
					{
						tools._buildparent.NextWall();
						base.StartCoroutine(base.transform.root.GetComponent<tools>().Cooldown3());
						UnityEngine.Object.Destroy(this.PrefabReady);
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.WallParts[tools._buildparent.currentIndex], position, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.WallParts[tools._buildparent.currentIndex].name;
					}
					else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
					{
						tools._buildparent.PreviousWall();
						base.StartCoroutine(base.transform.root.GetComponent<tools>().Cooldown3());
						UnityEngine.Object.Destroy(this.PrefabReady);
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.WallParts[tools._buildparent.currentIndex], position, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.WallParts[tools._buildparent.currentIndex].name;
					}
				}
				this.PrefabReady.transform.position = tools._buildPos.transform.position;
				if (this.BuildSite)
				{
					float num = tools._buildPos.transform.position.y - this.BuildSite.transform.position.y;
					if (tools.HorGrid)
					{
						num = Mathf.Round(num * 10f) / 10f;
					}
					else
					{
						num = Mathf.Round(num);
					}
					if (tools.Snapping)
					{
						if (tools.VertGrid)
						{
							this.PrefabReady.transform.localPosition = new Vector3(Mathf.Round(this.PrefabReady.transform.localPosition.x * 10f) / 10f, num, Mathf.Round(this.PrefabReady.transform.localPosition.z * 10f) / 10f);
						}
						else
						{
							this.PrefabReady.transform.localPosition = new Vector3(Mathf.Round(this.PrefabReady.transform.localPosition.x), num, Mathf.Round(this.PrefabReady.transform.localPosition.z));
						}
						this.PrefabReady.transform.rotation = Quaternion.Euler(this.BuildSite.transform.localEulerAngles.x, this.BuildSite.transform.localEulerAngles.y + (float)tools.BuildingRotation, this.BuildSite.transform.localEulerAngles.z);
					}
					else
					{
						this.PrefabReady.transform.rotation = Quaternion.Euler(base.transform.root.localEulerAngles.x, base.transform.root.localEulerAngles.y + (float)tools.BuildingRotation, base.transform.root.localEulerAngles.z);
					}
				}
				else
				{
					this.PrefabReady.transform.rotation = Quaternion.Euler(base.transform.root.localEulerAngles.x, base.transform.root.localEulerAngles.y + (float)tools.BuildingRotation, base.transform.root.localEulerAngles.z);
				}
				if (Input.GetMouseButtonDown(0))
				{
					if (!this.BuildSite)
					{
						GameObject gameObject = new GameObject();
						gameObject.transform.position = this.PrefabReady.transform.position;
						gameObject.transform.rotation = this.PrefabReady.transform.rotation;
						gameObject.name = "BuildingSite";
						gameObject.AddComponent<SaveItem>();
						this.BuildSite = gameObject;
						this.PrefabReady.transform.SetParent(this.BuildSite.transform);
					}
					this.PrefabReady.AddComponent<MeshCollider>();
					if (this.PrefabReady.GetComponent<SaveItem>().ChildrenRend)
					{
						this.PrefabReady.GetComponent<SaveItem>().ChildrenRend.gameObject.AddComponent<BoxCollider>();
					}
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().PlaceWall);
					this.paintlife -= 1f;
					if (this.paintlife < 1f)
					{
						tools.tool = 1;
						tools.holdingitem = false;
						tools.helditem = "Nothing";
						this.ThisInHandL = false;
						this.ThisInHandR = false;
						if (base.GetComponent<MPobject>())
						{
							base.GetComponent<MPobject>().networkDummy.DestroyMe();
						}
						else
						{
							UnityEngine.Object.Destroy(base.gameObject);
						}
					}
					else
					{
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.WallParts[tools._buildparent.currentIndex], base.transform.position, Quaternion.identity);
						this.PrefabReady.transform.SetParent(this.BuildSite.transform);
						this.PrefabReady.name = tools._buildparent.WallParts[tools._buildparent.currentIndex].name;
					}
				}
			}
			else
			{
				UnityEngine.Object.Destroy(this.PrefabReady);
			}
		}
		if (this.PrefabReady && this.RoofMaterial)
		{
			this.BuildSite = null;
			foreach (Collider collider2 in Physics.OverlapSphere(tools._buildPos.transform.position, 3f))
			{
				if (collider2.gameObject.tag == "Building" && collider2.gameObject != this.PrefabReady)
				{
					this.BuildSite = collider2.gameObject.transform.parent.gameObject;
					this.PrefabReady.transform.SetParent(this.BuildSite.transform);
					break;
				}
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				tools.BuildingRotation += 90;
			}
			if (this.ThisInHandR)
			{
				if (!tools.cooldown3)
				{
					Vector3 position2 = this.PrefabReady.transform.position;
					if (Input.GetAxis("Mouse ScrollWheel") < 0f)
					{
						tools._buildparent.NextRoof();
						base.StartCoroutine(base.transform.root.GetComponent<tools>().Cooldown3());
						UnityEngine.Object.Destroy(this.PrefabReady);
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.RoofParts[tools._buildparent.currentIndex1], position2, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.RoofParts[tools._buildparent.currentIndex1].name;
					}
					else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
					{
						tools._buildparent.PreviousRoof();
						base.StartCoroutine(base.transform.root.GetComponent<tools>().Cooldown3());
						UnityEngine.Object.Destroy(this.PrefabReady);
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.RoofParts[tools._buildparent.currentIndex1], position2, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.RoofParts[tools._buildparent.currentIndex1].name;
					}
				}
				this.PrefabReady.transform.position = tools._buildPos.transform.position;
				if (this.BuildSite)
				{
					float num2 = tools._buildPos.transform.position.y - this.BuildSite.transform.position.y;
					if (tools.HorGrid)
					{
						num2 = Mathf.Round(num2 * 10f) / 10f;
					}
					else
					{
						num2 = Mathf.Round(num2);
					}
					if (tools.Snapping)
					{
						if (tools.VertGrid)
						{
							this.PrefabReady.transform.localPosition = new Vector3(Mathf.Round(this.PrefabReady.transform.localPosition.x * 10f) / 10f, num2, Mathf.Round(this.PrefabReady.transform.localPosition.z * 10f) / 10f);
						}
						else
						{
							this.PrefabReady.transform.localPosition = new Vector3(Mathf.Round(this.PrefabReady.transform.localPosition.x), num2, Mathf.Round(this.PrefabReady.transform.localPosition.z));
						}
						this.PrefabReady.transform.rotation = Quaternion.Euler(this.BuildSite.transform.localEulerAngles.x, this.BuildSite.transform.localEulerAngles.y + (float)tools.BuildingRotation, this.BuildSite.transform.localEulerAngles.z);
					}
					else
					{
						this.PrefabReady.transform.rotation = Quaternion.Euler(base.transform.root.localEulerAngles.x, base.transform.root.localEulerAngles.y + (float)tools.BuildingRotation, base.transform.root.localEulerAngles.z);
					}
				}
				else
				{
					this.PrefabReady.transform.rotation = Quaternion.Euler(base.transform.root.localEulerAngles.x, base.transform.root.localEulerAngles.y + (float)tools.BuildingRotation, base.transform.root.localEulerAngles.z);
				}
				if (Input.GetMouseButtonDown(0))
				{
					if (!this.BuildSite)
					{
						GameObject gameObject2 = new GameObject();
						gameObject2.transform.position = this.PrefabReady.transform.position;
						gameObject2.transform.rotation = this.PrefabReady.transform.rotation;
						gameObject2.name = "BuildingSite";
						gameObject2.AddComponent<SaveItem>();
						this.BuildSite = gameObject2;
						this.PrefabReady.transform.SetParent(this.BuildSite.transform);
					}
					this.PrefabReady.AddComponent<MeshCollider>();
					if (this.PrefabReady.GetComponent<SaveItem>().ChildrenRend)
					{
						this.PrefabReady.GetComponent<SaveItem>().ChildrenRend.gameObject.AddComponent<BoxCollider>();
					}
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().PlaceWall);
					this.paintlife -= 1f;
					if (this.paintlife < 1f)
					{
						tools.tool = 1;
						tools.holdingitem = false;
						tools.helditem = "Nothing";
						this.ThisInHandL = false;
						this.ThisInHandR = false;
						if (base.GetComponent<MPobject>())
						{
							base.GetComponent<MPobject>().networkDummy.DestroyMe();
						}
						else
						{
							UnityEngine.Object.Destroy(base.gameObject);
						}
					}
					else
					{
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.RoofParts[tools._buildparent.currentIndex1], base.transform.position, Quaternion.identity);
						this.PrefabReady.transform.SetParent(this.BuildSite.transform);
						this.PrefabReady.name = tools._buildparent.RoofParts[tools._buildparent.currentIndex1].name;
					}
				}
			}
			else
			{
				UnityEngine.Object.Destroy(this.PrefabReady);
			}
		}
		if (this.PrefabReady && this.DoorMaterial)
		{
			this.BuildSite = null;
			foreach (Collider collider3 in Physics.OverlapSphere(tools._buildPos.transform.position, 3f))
			{
				if (collider3.gameObject.tag == "Building" && collider3.gameObject != this.PrefabReady)
				{
					this.BuildSite = collider3.gameObject.transform.parent.gameObject;
					this.PrefabReady.transform.SetParent(this.BuildSite.transform);
					break;
				}
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				tools.BuildingRotation += 90;
			}
			if (this.ThisInHandR)
			{
				if (!tools.cooldown3)
				{
					Vector3 position3 = this.PrefabReady.transform.position;
					if (Input.GetAxis("Mouse ScrollWheel") < 0f)
					{
						tools._buildparent.NextDoor();
						base.StartCoroutine(base.transform.root.GetComponent<tools>().Cooldown3());
						UnityEngine.Object.Destroy(this.PrefabReady);
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.DoorParts[tools._buildparent.currentIndex2], position3, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.DoorParts[tools._buildparent.currentIndex2].name;
					}
					else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
					{
						tools._buildparent.PreviousDoor();
						base.StartCoroutine(base.transform.root.GetComponent<tools>().Cooldown3());
						UnityEngine.Object.Destroy(this.PrefabReady);
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.DoorParts[tools._buildparent.currentIndex2], position3, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.DoorParts[tools._buildparent.currentIndex2].name;
					}
				}
				this.PrefabReady.transform.position = tools._buildPos.transform.position;
				if (this.BuildSite)
				{
					float num3 = tools._buildPos.transform.position.y - this.BuildSite.transform.position.y;
					if (tools.HorGrid)
					{
						num3 = Mathf.Round(num3 * 10f) / 10f;
					}
					else
					{
						num3 = Mathf.Round(num3);
					}
					if (tools.Snapping)
					{
						if (tools.VertGrid)
						{
							this.PrefabReady.transform.localPosition = new Vector3(Mathf.Round(this.PrefabReady.transform.localPosition.x * 10f) / 10f, num3, Mathf.Round(this.PrefabReady.transform.localPosition.z * 10f) / 10f);
						}
						else
						{
							this.PrefabReady.transform.localPosition = new Vector3(Mathf.Round(this.PrefabReady.transform.localPosition.x), num3, Mathf.Round(this.PrefabReady.transform.localPosition.z));
						}
						this.PrefabReady.transform.rotation = Quaternion.Euler(this.BuildSite.transform.localEulerAngles.x, this.BuildSite.transform.localEulerAngles.y + (float)tools.BuildingRotation, this.BuildSite.transform.localEulerAngles.z);
					}
					else
					{
						this.PrefabReady.transform.rotation = Quaternion.Euler(base.transform.root.localEulerAngles.x, base.transform.root.localEulerAngles.y + (float)tools.BuildingRotation, base.transform.root.localEulerAngles.z);
					}
				}
				else
				{
					this.PrefabReady.transform.rotation = Quaternion.Euler(base.transform.root.localEulerAngles.x, base.transform.root.localEulerAngles.y + (float)tools.BuildingRotation, base.transform.root.localEulerAngles.z);
				}
				if (Input.GetMouseButtonDown(0))
				{
					if (!this.BuildSite)
					{
						GameObject gameObject3 = new GameObject();
						gameObject3.transform.position = this.PrefabReady.transform.position;
						gameObject3.transform.rotation = this.PrefabReady.transform.rotation;
						gameObject3.name = "BuildingSite";
						gameObject3.AddComponent<SaveItem>();
						this.BuildSite = gameObject3;
						this.PrefabReady.transform.SetParent(this.BuildSite.transform);
					}
					this.PrefabReady.AddComponent<MeshCollider>();
					if (this.PrefabReady.GetComponent<SaveItem>().ChildrenRend)
					{
						this.PrefabReady.GetComponent<SaveItem>().ChildrenRend.gameObject.AddComponent<BoxCollider>();
					}
					this.paintlife -= 1f;
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().PlaceWall);
					if (this.paintlife < 1f)
					{
						tools.tool = 1;
						tools.holdingitem = false;
						tools.helditem = "Nothing";
						this.ThisInHandL = false;
						this.ThisInHandR = false;
						if (base.GetComponent<MPobject>())
						{
							base.GetComponent<MPobject>().networkDummy.DestroyMe();
						}
						else
						{
							UnityEngine.Object.Destroy(base.gameObject);
						}
					}
					else
					{
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.DoorParts[tools._buildparent.currentIndex2], base.transform.position, Quaternion.identity);
						this.PrefabReady.transform.SetParent(this.BuildSite.transform);
						this.PrefabReady.name = tools._buildparent.DoorParts[tools._buildparent.currentIndex2].name;
					}
				}
			}
			else
			{
				UnityEngine.Object.Destroy(this.PrefabReady);
			}
		}
		if (Input.GetMouseButtonDown(0) && this.Box && this.ThisInHandR && this.InBox > 0)
		{
			if (tools.MPrunning)
			{
				tools.NetworkPLayer.ITEM = this.InBoxprefab;
				tools.NetworkPLayer.Itemname = this.InBoxprefab.name;
				tools.NetworkPLayer.Spawnposition = this.Hand.transform.position;
				tools.NetworkPLayer.Spawnrotation = this.Hand.transform.rotation;
				tools.NetworkPLayer.SpawnObject(0, true);
				base.GetComponent<MPobject>().networkDummy.ReducePickupToolInBox();
			}
			else
			{
				GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.InBoxprefab, this.Hand.transform.position, this.Hand.transform.rotation);
				gameObject4.transform.name = this.InBoxprefab.transform.name;
				if (gameObject4.GetComponent<Partinfo>())
				{
					gameObject4.GetComponent<Partinfo>().Creating();
				}
				this.InBox--;
			}
		}
		if (Input.GetMouseButtonDown(1) && this.ThisInHandR)
		{
			if (base.gameObject.GetComponent<Rigidbody>() == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			this.ThisInHandR = false;
			base.gameObject.transform.position = this.Hand.transform.position;
			base.gameObject.transform.SetParent(this.Hand.transform);
			tools.tool = 1;
			if (this.FluidContainer)
			{
				base.transform.rotation = Quaternion.identity;
			}
			this.AudioParent.GetComponent<AudioSource>().Stop();
			if (this.paint && !this.marker)
			{
				this.StopParticles();
				this.AudioParent.GetComponent<AudioManager>().canplay = false;
				if (this.paintlife > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
			}
			if (this.paint && this.marker)
			{
				base.gameObject.GetComponent<P3dHitScreen>().enabled = false;
				this.AudioParent.GetComponent<AudioManager>().canplay = false;
				if (this.paintlife > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
			}
			if (this.waterhose || this.sandblaster)
			{
				this.StopParticles();
				this.AudioParent.GetComponent<AudioManager>().canplay = true;
			}
			if (this.fuelpump)
			{
				this.StopParticles();
				if (this.Cashregister.AllMoney == 0f)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
				this.Cashregister.BUY();
			}
			if (this.filler)
			{
				this.thistool.GetComponent<Renderer>().enabled = false;
				if (this.paintlife > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
			}
			if (this.FluidContainer && !this.FluidContainerFillable)
			{
				if (this.FluidSize > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
				this.StopParticles();
			}
		}
		if (tools.tool == 1 && !tools.sitting && !tools.holdingitem)
		{
			RaycastHit raycastHit2;
			if (Input.GetMouseButtonDown(1) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 1.7f, 1 << LayerMask.NameToLayer("Items")) && raycastHit2.collider.gameObject == base.gameObject)
			{
				if (base.GetComponent<FixedJoint>())
				{
					UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
				}
				this.ThisInHandR = true;
				tools.holdingitem = true;
				base.StartCoroutine(this.Hand.transform.root.GetComponent<tools>().Cooldown2());
				if (this.wrench)
				{
					tools.tool = 2;
				}
				if (this.screwdriver)
				{
					tools.tool = 3;
				}
				if (this.cutter)
				{
					tools.tool = 4;
				}
				if (this.welder)
				{
					tools.tool = 5;
				}
				if (this.prytool)
				{
					tools.tool = 6;
				}
				if (this.filler)
				{
					tools.tool = 7;
				}
				if (this.SparkplugSocket)
				{
					tools.tool = 8;
				}
				if (this.plyer)
				{
					tools.tool = 0;
				}
				if (this.SpringCompressor)
				{
					tools.tool = 9;
				}
				if (this.crowbar)
				{
					tools.tool = 11;
				}
				if (this.hammer)
				{
					tools.tool = 12;
				}
				if (this.paint)
				{
					tools.tool = 14;
				}
				if (this.airpump)
				{
					tools.tool = 15;
				}
				if (this.waterhose)
				{
					tools.tool = 16;
				}
				if (this.fuelpump)
				{
					tools.tool = 17;
				}
				if (this.sandblaster)
				{
					tools.tool = 25;
				}
				if (this.Decal)
				{
					tools.tool = 19;
				}
				if (this.CutHole)
				{
					tools.tool = 23;
				}
				if (this.FlashLight)
				{
					tools.tool = 20;
				}
				if (this.Tint)
				{
					tools.tool = 21;
				}
				if (this.MoveTool)
				{
					tools.tool = 22;
				}
				if (this.Consumable)
				{
					tools.tool = 29;
				}
				if (this.FluidContainer)
				{
					tools.tool = 30;
				}
				if (this.Garbagebag)
				{
					tools.tool = 24;
				}
				if (this.FramingMaterial)
				{
					tools.tool = 41;
					if (!this.PrefabReady)
					{
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.WallParts[tools._buildparent.currentIndex], base.transform.position, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.WallParts[tools._buildparent.currentIndex].name;
					}
				}
				if (this.RoofMaterial)
				{
					tools.tool = 43;
					if (!this.PrefabReady)
					{
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.RoofParts[tools._buildparent.currentIndex1], base.transform.position, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.RoofParts[tools._buildparent.currentIndex1].name;
					}
				}
				if (this.DoorMaterial)
				{
					tools.tool = 44;
					if (!this.PrefabReady)
					{
						this.PrefabReady = UnityEngine.Object.Instantiate<GameObject>(tools._buildparent.DoorParts[tools._buildparent.currentIndex2], base.transform.position, Quaternion.identity);
						this.PrefabReady.name = tools._buildparent.DoorParts[tools._buildparent.currentIndex2].name;
					}
				}
				if (this.MaterialChanger)
				{
					tools.tool = 45;
				}
				if (this.Sledgehammer)
				{
					tools.tool = 42;
				}
				this.tool = tools.tool;
				if (this.removableCup != null && !this.FluidContainerFillable)
				{
					this.removableCup.GetComponent<Renderer>().enabled = false;
				}
				if (this.tray != null)
				{
					this.tray.GetComponent<Renderer>().enabled = false;
				}
				if (this.thistool != null)
				{
					this.thistool.GetComponent<Renderer>().enabled = true;
				}
				if (base.gameObject.GetComponent<Rigidbody>() == null)
				{
					base.gameObject.AddComponent<Rigidbody>();
				}
				base.gameObject.GetComponent<Rigidbody>().useGravity = false;
				base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
				base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
				base.gameObject.transform.SetParent(this.ToolHand.transform);
				base.gameObject.transform.position = this.ToolHand.transform.position;
				if (base.GetComponent<MPobject>())
				{
					tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
				}
				if (this.FluidContainer || this.paint)
				{
					base.transform.rotation = base.transform.parent.parent.parent.rotation;
				}
				else
				{
					base.transform.rotation = base.transform.parent.parent.rotation;
				}
				GameObject.Find("Player").GetComponent<tools>().SphereJump();
				if (this.paint && this.paintlife > 0f && !this.marker)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
				if (this.paint && this.paintlife > 0f && this.marker)
				{
					base.gameObject.GetComponent<P3dHitScreen>().enabled = true;
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
				if (this.waterhose)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
				if (this.sandblaster && this.paintlife > 0f)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
				if (!this.ConnectedObject)
				{
					foreach (GameObject gameObject5 in GameObject.FindGameObjectsWithTag("transparentpart"))
					{
						if (gameObject5.name == base.gameObject.name && gameObject5.transform.childCount == 0)
						{
							gameObject5.GetComponent<transparents>().ENABLE();
						}
					}
				}
			}
			RaycastHit raycastHit3;
			if (Input.GetMouseButtonDown(0) && this.fuelpump && !tools.canput && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit3, 1.7f, 1 << LayerMask.NameToLayer("Items")) && raycastHit3.collider.gameObject == base.gameObject)
			{
				if (base.GetComponent<FixedJoint>())
				{
					UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
				}
				this.ThisInHandR = true;
				tools.holdingitem = true;
				tools.Clicked = true;
				if (this.fuelpump)
				{
					tools.tool = 17;
				}
				if (this.removableCup != null && !this.FluidContainerFillable)
				{
					this.removableCup.GetComponent<Renderer>().enabled = false;
				}
				if (this.tray != null)
				{
					this.tray.GetComponent<Renderer>().enabled = false;
				}
				if (this.thistool != null)
				{
					this.thistool.GetComponent<Renderer>().enabled = true;
				}
				if (this.ConnectedObject && base.gameObject.GetComponent<Rigidbody>() == null)
				{
					base.gameObject.AddComponent<Rigidbody>();
				}
				base.gameObject.GetComponent<Rigidbody>().useGravity = false;
				base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
				base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
				base.gameObject.transform.SetParent(this.ToolHand.transform);
				base.gameObject.transform.position = this.ToolHand.transform.position;
				if (base.GetComponent<MPobject>())
				{
					tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
				}
				base.transform.rotation = base.transform.parent.parent.rotation;
				GameObject.Find("Player").GetComponent<tools>().SphereJump();
				if (this.fuelpump)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
			}
		}
		if (Input.GetMouseButtonDown(0) && this.ConnectedObject && tools.canput)
		{
			tools.holdingitem = false;
			tools.tool = 1;
			this.ThisInHandL = false;
			this.ThisInHandR = false;
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			base.transform.position = this.ConnectedObject.transform.position;
			base.transform.rotation = this.ConnectedObject.transform.rotation;
			base.gameObject.transform.SetParent(this.ConnectedObject.transform);
			if (base.GetComponent<MPobject>())
			{
				tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
			}
			this.AudioParent.GetComponent<AudioSource>().Stop();
			if (this.waterhose || this.sandblaster)
			{
				this.StopParticles();
				this.AudioParent.GetComponent<AudioManager>().canplay = true;
			}
			if (this.fuelpump)
			{
				this.StopParticles();
				if (this.Cashregister.AllMoney == 0f)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = true;
				}
				this.Cashregister.BUY();
			}
			GameObject.Find("Player").GetComponent<tools>().SphereJump();
		}
		if (Input.GetMouseButtonDown(0) && this.seetoput)
		{
			this.AttachPickup();
		}
		if (this.ThisInHandR && this.paintlife <= 0f && !this.filler && !this.FlashLight && !this.MoveTool && !this.Consumable && !this.Box)
		{
			this.AudioParent.GetComponent<AudioSource>().Stop();
			if (!this.marker && !this.Box)
			{
				this.StopParticles();
			}
			if (this.marker)
			{
				base.gameObject.GetComponent<P3dHitScreen>().enabled = false;
			}
		}
		if (this.ThisInHandR && this.marker)
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				base.gameObject.GetComponent<P3dPaintSphere>().Radius = 0.02f;
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				base.gameObject.GetComponent<P3dPaintSphere>().Radius = 0.01f;
			}
		}
		RaycastHit raycastHit4;
		if (tools.LookingAtTransparent && base.transform.parent && (base.transform.parent.gameObject == this.Hand || base.transform.parent.gameObject == this.ToolHand || tools.helditem == base.gameObject.name) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit4, 1.7f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit4.collider.gameObject.name == base.gameObject.name && (this.ConnectedObject == null || raycastHit4.collider.gameObject == this.ConnectedObject))
		{
			this.placetoput = raycastHit4.collider.gameObject;
			this.seetoput = true;
			tools.canput = true;
		}
		if (Input.GetMouseButtonUp(1) && !this.ThisInHandR && base.transform.parent && (base.transform.parent.gameObject == this.Hand || base.transform.parent.gameObject == this.ToolHand))
		{
			if (this.seetoput)
			{
				this.AttachPickup();
			}
			else
			{
				this.Release();
			}
		}
		if (this.FluidContainer && this.ThisInHandR && !this.FluidContainerFillable)
		{
			if (base.gameObject.transform.rotation.eulerAngles.z > 120f && base.gameObject.transform.rotation.eulerAngles.z < 240f && this.FluidSize > 0f)
			{
				this.PlayParticles();
				if (this.VegOil)
				{
					this.FluidSize -= 2.5f * Time.deltaTime;
					return;
				}
				this.FluidSize -= 0.1f * Time.deltaTime;
				return;
			}
			else
			{
				this.StopParticles();
			}
		}
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x000898B8 File Offset: 0x00087AB8
	public void AttachPickup()
	{
		if (base.GetComponent<MPobject>())
		{
			MPobject component = base.GetComponent<MPobject>();
			int childnumber = -1;
			int childnumber2 = -1;
			if (this.placetoput.transform.parent.GetComponent<MPobject>())
			{
				component = this.placetoput.transform.parent.GetComponent<MPobject>();
				childnumber = this.placetoput.transform.GetSiblingIndex();
			}
			if (this.placetoput.transform.parent.parent && this.placetoput.transform.parent.parent.GetComponent<MPobject>())
			{
				component = this.placetoput.transform.parent.parent.GetComponent<MPobject>();
				childnumber = this.placetoput.transform.GetSiblingIndex();
				childnumber2 = this.placetoput.transform.parent.GetSiblingIndex();
			}
			base.GetComponent<MPobject>().networkDummy.GetComponent<NetworkTransform>().enabled = false;
			base.GetComponent<MPobject>().networkDummy.enabled = false;
			base.GetComponent<MPobject>().networkDummy.AttachPickup(component, childnumber, childnumber2);
			base.gameObject.transform.position = this.placetoput.transform.position;
			base.gameObject.transform.rotation = this.placetoput.transform.rotation;
			return;
		}
		this.AttachPickup2(this.placetoput);
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00089A2C File Offset: 0x00087C2C
	public void AttachPickup2(GameObject place)
	{
		if (base.gameObject.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		}
		base.gameObject.transform.position = place.transform.position;
		base.gameObject.transform.rotation = place.transform.rotation;
		base.gameObject.transform.SetParent(place.transform);
		if (base.GetComponent<MPobject>())
		{
			tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
		}
		tools.holdingitem = false;
		tools.tool = 1;
		this.ThisInHandL = false;
		if (!this.ConnectedObject)
		{
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().DISABLE();
				}
			}
		}
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00089B24 File Offset: 0x00087D24
	public void FlashLightTurn(bool on)
	{
		this.Light.SetActive(on);
		if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 10f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BlinkerOn);
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00089B84 File Offset: 0x00087D84
	private void OnMouseDown()
	{
		if (tools.tool == 24 || tools.UIisOpen)
		{
			return;
		}
		RaycastHit raycastHit;
		if (!tools.sitting && tools.helditem == "Nothing" && tools.tool != 19 && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("Items")) && raycastHit.collider.gameObject == base.gameObject && !tools.Clicked)
		{
			tools.Clicked = true;
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			if (base.GetComponent<MeshCollider>())
			{
				base.GetComponent<MeshCollider>().isTrigger = false;
			}
			if (base.GetComponent<BoxCollider>())
			{
				base.GetComponent<BoxCollider>().isTrigger = false;
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
			if (base.GetComponent<MPobject>())
			{
				tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
			}
			if (Vector3.Distance(this.AudioParent.transform.position, Camera.main.transform.position) > 1.3f)
			{
				this.AudioParent.transform.position = Vector3.MoveTowards(this.AudioParent.transform.position, Camera.main.transform.position, Vector3.Distance(this.AudioParent.transform.position, Camera.main.transform.position) - 1.3f);
			}
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			if (!this.ConnectedObject)
			{
				foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
				{
					if (gameObject.name == base.gameObject.name && gameObject.transform.childCount == 0)
					{
						gameObject.GetComponent<transparents>().ENABLE();
					}
				}
			}
		}
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00089E5C File Offset: 0x0008805C
	private void OnMouseUp()
	{
		if (this.seetoput)
		{
			this.AttachPickup();
		}
		else if (this.ThisInHandL)
		{
			if (base.GetComponent<MeshCollider>())
			{
				base.GetComponent<MeshCollider>().isTrigger = false;
			}
			if (base.GetComponent<BoxCollider>())
			{
				base.GetComponent<BoxCollider>().isTrigger = false;
			}
			tools.helditem = "Nothing";
			this.ThisInHandL = false;
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.transform.SetParent(null);
			if (base.GetComponent<MPobject>())
			{
				tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
			}
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			base.StartCoroutine(this.GroundCHeck());
		}
		if (!this.ConnectedObject)
		{
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().DISABLE();
				}
			}
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00089FAC File Offset: 0x000881AC
	public void Release()
	{
		if (!this.ThisInHandL && !this.ThisInHandR && !tools.canput)
		{
			if (base.GetComponent<MeshCollider>())
			{
				base.GetComponent<MeshCollider>().isTrigger = false;
			}
			if (base.GetComponent<BoxCollider>())
			{
				base.GetComponent<BoxCollider>().isTrigger = false;
			}
			if (this.FluidContainer)
			{
				base.transform.rotation = Quaternion.identity;
			}
			if (!this.ConnectedObject)
			{
				foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
				{
					if (gameObject.name == base.gameObject.name)
					{
						gameObject.GetComponent<transparents>().DISABLE();
					}
				}
			}
			if (this.paint && !this.marker)
			{
				this.StopParticles();
				this.AudioParent.GetComponent<AudioManager>().canplay = false;
				if (this.paintlife > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
			}
			if (this.paint && this.marker)
			{
				base.gameObject.GetComponent<P3dHitScreen>().enabled = false;
				this.AudioParent.GetComponent<AudioManager>().canplay = false;
				if (this.paintlife > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
			}
			if (this.waterhose || this.sandblaster)
			{
				this.StopParticles();
				this.AudioParent.GetComponent<AudioManager>().canplay = false;
			}
			if (this.fuelpump)
			{
				this.StopParticles();
				if (this.Cashregister.AllMoney == 0f)
				{
					this.AudioParent.GetComponent<AudioManager>().canplay = false;
				}
				this.Cashregister.BUY();
			}
			if (this.filler)
			{
				this.thistool.GetComponent<Renderer>().enabled = false;
				if (this.paintlife > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
			}
			if (this.FluidContainer && !this.FluidContainerFillable)
			{
				if (this.FluidSize > 0f)
				{
					this.removableCup.GetComponent<Renderer>().enabled = true;
				}
				this.StopParticles();
			}
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.PickupToolSync(this.paintlife);
			}
			if (this.ConnectedObject && tools.canput)
			{
				tools.holdingitem = false;
				tools.tool = 1;
				this.ThisInHandL = false;
				this.ThisInHandR = false;
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
				base.transform.position = this.ConnectedObject.transform.position;
				base.transform.rotation = this.ConnectedObject.transform.rotation;
				base.gameObject.transform.SetParent(this.ConnectedObject.transform);
				if (base.GetComponent<MPobject>())
				{
					tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
				}
				this.AudioParent.GetComponent<AudioSource>().Stop();
				if (this.waterhose || this.sandblaster)
				{
					this.StopParticles();
					this.AudioParent.GetComponent<AudioManager>().canplay = false;
				}
				if (this.fuelpump)
				{
					this.StopParticles();
					if (this.Cashregister.AllMoney == 0f)
					{
						this.AudioParent.GetComponent<AudioManager>().canplay = false;
					}
					this.Cashregister.BUY();
				}
				GameObject.Find("Player").GetComponent<tools>().SphereJump();
			}
			else
			{
				if (!this.rollin)
				{
					tools.holdingitem = false;
					tools.tool = 1;
					if (base.gameObject.GetComponent<Rigidbody>())
					{
						base.gameObject.GetComponent<Rigidbody>().useGravity = true;
						base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
						base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
					}
					base.gameObject.transform.SetParent(null);
					if (base.GetComponent<MPobject>())
					{
						tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
					}
					base.transform.localScale = new Vector3(1f, 1f, 1f);
					GameObject.Find("Player").GetComponent<tools>().SphereJump();
				}
				if (this.rollin)
				{
					tools.holdingitem = false;
					tools.tool = 1;
					this.ThisInHandR = false;
					this.ThisInHandL = false;
					UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
					base.transform.position = this.ConnectedObject.transform.position;
					base.transform.rotation = this.ConnectedObject.transform.rotation;
					base.gameObject.transform.SetParent(this.ConnectedObject.transform);
					if (base.GetComponent<MPobject>())
					{
						tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
					}
					this.AudioParent.GetComponent<AudioSource>().Stop();
					if (this.waterhose || this.sandblaster)
					{
						this.StopParticles();
						this.AudioParent.GetComponent<AudioManager>().canplay = false;
					}
					if (this.fuelpump)
					{
						this.StopParticles();
						if (this.Cashregister.AllMoney == 0f)
						{
							this.AudioParent.GetComponent<AudioManager>().canplay = false;
						}
						this.Cashregister.BUY();
					}
					GameObject.Find("Player").GetComponent<tools>().SphereJump();
				}
			}
			base.StartCoroutine(this.GroundCHeck());
		}
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x0008A530 File Offset: 0x00088730
	public void ForceRelease()
	{
		if (this.FluidContainer)
		{
			base.transform.rotation = Quaternion.identity;
		}
		if (this.paint && !this.marker)
		{
			this.StopParticles();
			this.AudioParent.GetComponent<AudioManager>().canplay = false;
			if (this.paintlife > 0f)
			{
				this.removableCup.GetComponent<Renderer>().enabled = true;
			}
		}
		if (this.paint && this.marker)
		{
			base.gameObject.GetComponent<P3dHitScreen>().enabled = false;
			this.AudioParent.GetComponent<AudioManager>().canplay = false;
			if (this.paintlife > 0f)
			{
				this.removableCup.GetComponent<Renderer>().enabled = true;
			}
		}
		if (this.waterhose || this.sandblaster)
		{
			this.StopParticles();
			this.AudioParent.GetComponent<AudioManager>().canplay = false;
		}
		if (this.fuelpump)
		{
			this.StopParticles();
			if (this.Cashregister.AllMoney == 0f)
			{
				this.AudioParent.GetComponent<AudioManager>().canplay = false;
			}
			this.Cashregister.BUY();
		}
		if (this.filler)
		{
			this.thistool.GetComponent<Renderer>().enabled = false;
			if (this.paintlife > 0f)
			{
				this.removableCup.GetComponent<Renderer>().enabled = true;
			}
		}
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.PickupToolSync(this.paintlife);
		}
		if (this.FluidContainer && !this.FluidContainerFillable)
		{
			if (this.FluidSize > 0f)
			{
				this.removableCup.GetComponent<Renderer>().enabled = true;
			}
			this.StopParticles();
		}
		tools.tool = 1;
		tools.holdingitem = false;
		tools.helditem = "Nothing";
		this.ThisInHandL = false;
		this.ThisInHandR = false;
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		base.gameObject.transform.SetParent(null);
		if (base.GetComponent<MPobject>())
		{
			tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		base.StartCoroutine(this.GroundCHeck());
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0008A791 File Offset: 0x00088991
	private IEnumerator GroundCHeck()
	{
		yield return new WaitForSeconds(2f);
		Vector3 position = base.transform.position;
		Terrain[] activeTerrains = Terrain.activeTerrains;
		Terrain terrain = Terrain.activeTerrain;
		float num = (new Vector3(activeTerrains[0].transform.position.x + activeTerrains[0].terrainData.size.x / 2f, base.transform.position.y, activeTerrains[0].transform.position.z + activeTerrains[0].terrainData.size.z / 2f) - base.transform.position).sqrMagnitude;
		for (int i = 0; i < activeTerrains.Length; i++)
		{
			float sqrMagnitude = (new Vector3(activeTerrains[i].transform.position.x + activeTerrains[i].terrainData.size.x / 2f, base.transform.position.y, activeTerrains[i].transform.position.z + activeTerrains[i].terrainData.size.z / 2f) - base.transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				terrain = activeTerrains[i];
			}
		}
		if (terrain && base.GetComponent<SaveItem>() && !base.GetComponent<SaveItem>().InBackpack && position.y < terrain.SampleHeight(base.transform.position))
		{
			position.y = terrain.SampleHeight(base.transform.position) + 0.3f;
			base.transform.position = position;
			if (base.GetComponent<Rigidbody>())
			{
				base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.GetComponent<Rigidbody>().Sleep();
			base.gameObject.GetComponent<Rigidbody>().drag = 10f;
			base.StartCoroutine(this.SetDrag());
		}
		yield break;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0008A7A0 File Offset: 0x000889A0
	private IEnumerator SetDrag()
	{
		yield return new WaitForSeconds(2f);
		if (base.GetComponent<Rigidbody>())
		{
			base.gameObject.GetComponent<Rigidbody>().drag = 0f;
		}
		yield break;
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0008A7AF File Offset: 0x000889AF
	public void PlayParticles()
	{
		if (base.transform.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.PlayParticles();
			return;
		}
		this.PlayParticles2();
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x0008A7EC File Offset: 0x000889EC
	public void PlayParticles2()
	{
		if (base.GetComponentInChildren<ParticleSystem>())
		{
			base.GetComponentInChildren<ParticleSystem>().Play();
		}
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0008A806 File Offset: 0x00088A06
	public void StopParticles()
	{
		if (base.transform.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.StopParticles();
			return;
		}
		this.StopParticles2();
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0008A843 File Offset: 0x00088A43
	public void StopParticles2()
	{
		if (base.GetComponentInChildren<ParticleSystem>())
		{
			base.GetComponentInChildren<ParticleSystem>().Stop();
		}
	}

	// Token: 0x040014FA RID: 5370
	public string DescriptionText;

	// Token: 0x040014FB RID: 5371
	public bool CanPutInBox;

	// Token: 0x040014FC RID: 5372
	public GameObject Hand;

	// Token: 0x040014FD RID: 5373
	public GameObject ToolHand;

	// Token: 0x040014FE RID: 5374
	public GameObject ConnectedObject;

	// Token: 0x040014FF RID: 5375
	public float HoseLength;

	// Token: 0x04001500 RID: 5376
	public bool rollin;

	// Token: 0x04001501 RID: 5377
	public GameObject removableCup;

	// Token: 0x04001502 RID: 5378
	public GameObject tray;

	// Token: 0x04001503 RID: 5379
	public GameObject thistool;

	// Token: 0x04001504 RID: 5380
	public GameObject AudioParent;

	// Token: 0x04001505 RID: 5381
	public GameObject Filling;

	// Token: 0x04001506 RID: 5382
	public bool ThisInHandL;

	// Token: 0x04001507 RID: 5383
	public bool ThisInHandR;

	// Token: 0x04001508 RID: 5384
	public GameObject Attached;

	// Token: 0x04001509 RID: 5385
	public GameObject Light;

	// Token: 0x0400150A RID: 5386
	public int tool;

	// Token: 0x0400150B RID: 5387
	public bool wrench;

	// Token: 0x0400150C RID: 5388
	public bool screwdriver;

	// Token: 0x0400150D RID: 5389
	public bool cutter;

	// Token: 0x0400150E RID: 5390
	public bool welder;

	// Token: 0x0400150F RID: 5391
	public bool prytool;

	// Token: 0x04001510 RID: 5392
	public bool filler;

	// Token: 0x04001511 RID: 5393
	public bool plyer;

	// Token: 0x04001512 RID: 5394
	public bool SparkplugSocket;

	// Token: 0x04001513 RID: 5395
	public bool SpringCompressor;

	// Token: 0x04001514 RID: 5396
	public bool hammer;

	// Token: 0x04001515 RID: 5397
	public bool crowbar;

	// Token: 0x04001516 RID: 5398
	public bool paint;

	// Token: 0x04001517 RID: 5399
	public bool marker;

	// Token: 0x04001518 RID: 5400
	public bool airpump;

	// Token: 0x04001519 RID: 5401
	public bool waterhose;

	// Token: 0x0400151A RID: 5402
	public bool sandblaster;

	// Token: 0x0400151B RID: 5403
	public bool fuelpump;

	// Token: 0x0400151C RID: 5404
	public bool Decal;

	// Token: 0x0400151D RID: 5405
	public bool FlashLight;

	// Token: 0x0400151E RID: 5406
	public bool Tint;

	// Token: 0x0400151F RID: 5407
	public bool MoveTool;

	// Token: 0x04001520 RID: 5408
	public bool CutHole;

	// Token: 0x04001521 RID: 5409
	public bool Consumable;

	// Token: 0x04001522 RID: 5410
	public bool FramingMaterial;

	// Token: 0x04001523 RID: 5411
	public bool RoofMaterial;

	// Token: 0x04001524 RID: 5412
	public bool DoorMaterial;

	// Token: 0x04001525 RID: 5413
	public bool Sledgehammer;

	// Token: 0x04001526 RID: 5414
	public bool MaterialChanger;

	// Token: 0x04001527 RID: 5415
	public Renderer previewLabel;

	// Token: 0x04001528 RID: 5416
	public Material material;

	// Token: 0x04001529 RID: 5417
	public GameObject Prefab;

	// Token: 0x0400152A RID: 5418
	public GameObject PrefabReady;

	// Token: 0x0400152B RID: 5419
	public GameObject BuildSite;

	// Token: 0x0400152C RID: 5420
	public float Food;

	// Token: 0x0400152D RID: 5421
	public float Drink;

	// Token: 0x0400152E RID: 5422
	public bool Garbagebag;

	// Token: 0x0400152F RID: 5423
	public bool MONEY;

	// Token: 0x04001530 RID: 5424
	public bool VegOil;

	// Token: 0x04001531 RID: 5425
	public bool Box;

	// Token: 0x04001532 RID: 5426
	public int InBox;

	// Token: 0x04001533 RID: 5427
	public GameObject InBoxprefab;

	// Token: 0x04001534 RID: 5428
	public int TintLevel;

	// Token: 0x04001535 RID: 5429
	public P3dPaintDecal CustomDecal;

	// Token: 0x04001536 RID: 5430
	public SaleItem Cashregister;

	// Token: 0x04001537 RID: 5431
	public Text FuelPrice;

	// Token: 0x04001538 RID: 5432
	public Text FuelLiters;

	// Token: 0x04001539 RID: 5433
	public Text FuelMoney;

	// Token: 0x0400153A RID: 5434
	public Text FuelPrice2;

	// Token: 0x0400153B RID: 5435
	public Text FuelLiters2;

	// Token: 0x0400153C RID: 5436
	public Text FuelMoney2;

	// Token: 0x0400153D RID: 5437
	public Text CashReg;

	// Token: 0x0400153E RID: 5438
	public float price;

	// Token: 0x0400153F RID: 5439
	public float liters;

	// Token: 0x04001540 RID: 5440
	public float money;

	// Token: 0x04001541 RID: 5441
	public bool FluidContainer;

	// Token: 0x04001542 RID: 5442
	public bool FluidContainerFillable;

	// Token: 0x04001543 RID: 5443
	public FLUID NestedFluid;

	// Token: 0x04001544 RID: 5444
	public float ContainerSize;

	// Token: 0x04001545 RID: 5445
	public float FluidSize;

	// Token: 0x04001546 RID: 5446
	public float paintlife;

	// Token: 0x04001547 RID: 5447
	public Color colorpaint;

	// Token: 0x04001548 RID: 5448
	private ParticleSystem ps;

	// Token: 0x04001549 RID: 5449
	public GameObject placetoput;

	// Token: 0x0400154A RID: 5450
	public bool seetoput;
}
