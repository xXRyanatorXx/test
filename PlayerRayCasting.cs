using System;
using PaintIn3D;
using Parabox.CSG;
using RVP;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class PlayerRayCasting : MonoBehaviour
{
	// Token: 0x06000ADC RID: 2780 RVA: 0x0006D0E0 File Offset: 0x0006B2E0
	private void Start()
	{
		this.matint = -3;
		this.distanceToSee = 1.7f;
		this.highlightColor = Color.green;
		this.unmountedColor = Color.yellow;
		this.AudioParent = GameObject.Find("hand");
		Camera component = base.GetComponent<Camera>();
		float[] array = new float[32];
		array[2] = 300f;
		array[9] = 20f;
		array[10] = 200f;
		array[15] = 100f;
		array[16] = 20f;
		array[17] = 20f;
		array[18] = 200f;
		array[19] = 100f;
		array[25] = 100f;
		array[26] = 100f;
		component.layerCullDistances = array;
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0006D194 File Offset: 0x0006B394
	private void Update()
	{
		this.timer += Time.deltaTime;
		if (tools.tool != 19)
		{
			this.Preview.enabled = false;
		}
		RaycastHit raycastHit;
		if (tools.sitting && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows")) && raycastHit.collider.tag == "Window")
		{
			tools.LookingWindow = true;
		}
		RaycastHit raycastHit2;
		if (tools.tool == 24 && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit2, 2.5f, 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("Items") | 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("LooseParts")) && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife > 0f && Input.GetMouseButtonDown(0))
		{
			if (raycastHit2.collider.transform.root.GetComponent<MainCarProperties>())
			{
				return;
			}
			if (raycastHit2.collider.GetComponent<PickupTool>() && !raycastHit2.collider.GetComponent<PickupTool>().CanPutInBox)
			{
				return;
			}
			if (raycastHit2.collider.GetComponent<MooveItem>() || raycastHit2.collider.GetComponent<Dragable>())
			{
				return;
			}
			tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife -= 1f;
			if (Input.GetMouseButtonDown(0))
			{
				if (raycastHit2.collider.transform.GetComponent<Partinfo>() && raycastHit2.collider.transform.GetComponent<CarProperties>() && raycastHit2.collider.transform.GetComponent<CarProperties>().Owner != "Client" && raycastHit2.collider.transform.GetComponent<CarProperties>().Owner != "Junkyard" && raycastHit2.collider.transform.GetComponent<CarProperties>().Owner != "Multiplayer")
				{
					if (!tools.MPrunning || !raycastHit2.collider.GetComponent<MPobject>() || (!(raycastHit2.collider.GetComponent<MPobject>().networkDummy == null) && raycastHit2.collider.GetComponent<MPobject>().networkDummy.hasAuthority))
					{
						tools.money += raycastHit2.collider.transform.GetComponent<Partinfo>().price * 0.1f;
						this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
						if (tools.MPrunning)
						{
							foreach (MPobject mpobject in raycastHit2.collider.transform.root.GetComponentsInChildren<MPobject>())
							{
								if (mpobject.networkDummy)
								{
									mpobject.networkDummy.DestroyMe();
								}
							}
						}
					}
					UnityEngine.Object.Destroy(raycastHit2.collider.gameObject);
				}
				if (raycastHit2.collider.gameObject.layer == LayerMask.NameToLayer("Items"))
				{
					if (raycastHit2.collider.gameObject.GetComponent<networkDummy>())
					{
						raycastHit2.collider.gameObject.GetComponent<networkDummy>().DestroyMe();
					}
					UnityEngine.Object.Destroy(raycastHit2.collider.gameObject);
				}
			}
		}
		RaycastHit raycastHit3;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit3, this.distanceToSee, 1 << LayerMask.NameToLayer("OpenableParts")))
		{
			if (Input.GetMouseButtonDown(0) && raycastHit3.collider.GetComponent<OpenDoor>() && !tools.Clicked && tools.tool != 21 && tools.tool != 4)
			{
				if ((tools.sitting && raycastHit3.collider.GetComponent<OpenDoor>().DontOpenSitting) || tools.DontAllowClick)
				{
					return;
				}
				if (!raycastHit3.collider.transform.parent || !raycastHit3.collider.transform.parent.GetComponent<transparents>() || tools.cooldown)
				{
					return;
				}
				if (raycastHit3.collider.GetComponent<MPobject>())
				{
					raycastHit3.collider.GetComponent<MPobject>().networkDummy.opendoor(raycastHit3.collider.GetComponent<OpenDoor>().doorOpened);
				}
				else
				{
					raycastHit3.collider.GetComponent<OpenDoor>().OnMouseDown1(raycastHit3.collider.GetComponent<OpenDoor>().doorOpened);
				}
			}
			if (raycastHit3.collider.GetComponent<WindowLift>())
			{
				raycastHit3.collider.GetComponent<WindowLift>().enabled = true;
			}
			if (Input.GetMouseButtonDown(0) && raycastHit3.collider.GetComponent<Switch>())
			{
				raycastHit3.collider.GetComponent<Switch>().Clicked();
				tools.Clicked = true;
			}
			if (Input.GetMouseButtonDown(1) && raycastHit3.collider.GetComponent<Switch>())
			{
				raycastHit3.collider.GetComponent<Switch>().ClickedR();
			}
			if (raycastHit3.collider.GetComponent<Switch>() && raycastHit3.collider.GetComponent<Switch>().Throttle)
			{
				raycastHit3.collider.GetComponent<Switch>().enabled = true;
			}
			if (raycastHit3.collider.GetComponent<Switch>() && Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				raycastHit3.collider.GetComponent<Switch>().ScrollUP();
			}
			if (raycastHit3.collider.GetComponent<Switch>() && Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				raycastHit3.collider.GetComponent<Switch>().ScrollDown();
			}
		}
		if (tools.tool == 24 || tools.UIisOpen)
		{
			return;
		}
		RaycastHit raycastHit4;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit4, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("LooseParts")))
		{
			Transform transform = base.transform;
			if (Input.GetMouseButtonDown(0) && raycastHit4.collider.GetComponent<Pickup>())
			{
				if (raycastHit4.collider.GetComponent<Pickup>())
				{
					transform = raycastHit4.transform;
				}
				foreach (RaycastHit raycastHit5 in Physics.RaycastAll(base.transform.position, base.transform.forward, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("LooseParts")))
				{
					if (raycastHit5.transform.GetComponent<CarProperties>().SparkPlug || raycastHit5.transform.GetComponent<CarProperties>().GlowPlug || raycastHit5.transform.GetComponent<CarProperties>().Injector || raycastHit5.transform.GetComponent<PickupCup>())
					{
						transform = raycastHit5.transform;
					}
				}
				if (transform != base.transform)
				{
					if (transform.GetComponent<PickupCup>())
					{
						transform.GetComponent<PickupCup>().TakeInHand();
					}
					else if (transform.GetComponent<Pickup>())
					{
						transform.GetComponent<Pickup>().OnMouseDowns();
					}
				}
			}
		}
		RaycastHit raycastHit6;
		if (!tools.sitting && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit6, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")))
		{
			if (tools.tool != 4 && tools.tool != 7 && tools.tool != 9)
			{
				if ((raycastHit6.collider.GetComponent<PickupHand>() || raycastHit6.collider.GetComponent<PickupCup>() || raycastHit6.collider.GetComponent<WinchHook>() || raycastHit6.collider.GetComponent<Pickup>()) && tools.tool == 1)
				{
					tools.cantake = true;
				}
				if (raycastHit6.collider.GetComponent<Rigidbody>() && raycastHit6.collider.GetComponent<Partinfo>().tightnuts == 0f && !tools.cooldown)
				{
					tools.cantake = true;
				}
			}
			if (tools.tool == 9 && raycastHit6.collider.GetComponent<RemoveSpring>())
			{
				tools.cantake = true;
			}
			if (raycastHit6.collider.GetComponent<RemoveWindow>())
			{
				raycastHit6.collider.GetComponent<RemoveWindow>().enabled = true;
			}
		}
		RaycastHit raycastHit7;
		if ((tools.tool == 1 || tools.tool == 30) && Input.GetMouseButtonDown(0) && !tools.Clicked && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit7, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows")))
		{
			if (raycastHit7.collider.GetComponent<PickupCup>())
			{
				raycastHit7.collider.GetComponent<PickupCup>().TakeInHand();
			}
			if (raycastHit7.collider.GetComponent<PickupHand>())
			{
				raycastHit7.collider.GetComponent<PickupHand>().TakeInHand();
			}
			if (raycastHit7.collider.GetComponent<WinchHook>())
			{
				raycastHit7.collider.GetComponent<WinchHook>().TakeInHand();
			}
		}
		RaycastHit raycastHit8;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit8, this.distanceToSee, 1 << LayerMask.NameToLayer("Items")) && raycastHit8.collider.GetComponent<PickupItems>())
		{
			raycastHit8.collider.GetComponent<PickupItems>().TakeInHand();
		}
		RaycastHit raycastHit9;
		if (tools.tool == 15 && Input.GetMouseButtonDown(0) && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit9, this.distanceToSee, 1 << LayerMask.NameToLayer("FlatBolts")) && raycastHit9.collider.transform.name == "TireValve" && raycastHit9.collider.transform.parent.GetComponent<CarProperties>().tireObject && !raycastHit9.collider.transform.parent.GetComponent<CarProperties>().Ruined && !raycastHit9.collider.transform.parent.GetComponent<CarProperties>().tireObject.Ruined && raycastHit9.collider.transform.parent.GetComponent<CarProperties>().tireObject.TirePressure <= 3f)
		{
			if (raycastHit9.collider.transform.parent.GetComponent<MPobject>())
			{
				raycastHit9.collider.transform.parent.GetComponent<MPobject>().networkDummy.PumpIn();
			}
			else
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().TirePump);
				raycastHit9.collider.transform.parent.GetComponent<CarProperties>().tireObject.TirePressure += 0.025f;
				raycastHit9.collider.transform.parent.GetComponent<CarProperties>().ReStart();
			}
		}
		RaycastHit raycastHit10;
		if ((tools.tool == 1 || tools.tool == 3) && Input.GetMouseButtonDown(0) && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit10, this.distanceToSee, 1 << LayerMask.NameToLayer("FlatBolts")) && raycastHit10.collider.transform.name == "TireValve" && raycastHit10.collider.transform.parent.GetComponent<CarProperties>().tireObject && raycastHit10.collider.transform.parent.GetComponent<CarProperties>().tireObject.TirePressure >= 0.1f)
		{
			if (raycastHit10.collider.transform.parent.GetComponent<MPobject>())
			{
				raycastHit10.collider.transform.parent.GetComponent<MPobject>().networkDummy.PumpOut();
			}
			else
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().TirePump);
				raycastHit10.collider.transform.parent.GetComponent<CarProperties>().tireObject.TirePressure -= 0.025f;
				raycastHit10.collider.transform.parent.GetComponent<CarProperties>().ReStart();
			}
		}
		if (tools.tool == 1 || tools.tool == 3 || tools.tool == 15)
		{
			if (this.matint > -1)
			{
				Material[] materials = this.rend.materials;
				materials[this.matint] = this.originalMaterial;
				this.rend.materials = materials;
				this.originalMaterial = null;
				this.rend = null;
				this.matint = -2;
			}
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			RaycastHit raycastHit11;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit11, this.distanceToSee, 1 << LayerMask.NameToLayer("FlatBolts")) && raycastHit11.collider.transform.name == "TireValve")
			{
				Renderer component = raycastHit11.collider.gameObject.GetComponent<Renderer>();
				if (component == this.rend)
				{
					return;
				}
				if (component && component != this.rend && this.rend)
				{
					this.rend.sharedMaterial = this.originalMaterial;
				}
				if (!component)
				{
					return;
				}
				this.rend = component;
				this.originalMaterial = this.rend.sharedMaterial;
				this.tempMaterial = new Material(this.originalMaterial);
				this.rend.material = this.tempMaterial;
				this.rend.material.color = this.highlightColor;
			}
			else if (this.rend)
			{
				this.rend.sharedMaterial = this.originalMaterial;
				this.rend = null;
			}
		}
		if (tools.tool == 42)
		{
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			int num = 0;
			RaycastHit raycastHit12;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit12, this.distanceToSee, 1 << LayerMask.NameToLayer("Default")) && raycastHit12.collider.transform.parent.name == "BuildingSite")
			{
				this.mat = null;
				MeshCollider meshCollider = raycastHit12.collider as MeshCollider;
				if (meshCollider != null || meshCollider.sharedMesh != null)
				{
					Mesh sharedMesh = meshCollider.sharedMesh;
					Renderer component2 = raycastHit12.collider.GetComponent<MeshRenderer>();
					int[] array2 = new int[]
					{
						sharedMesh.triangles[raycastHit12.triangleIndex * 3],
						sharedMesh.triangles[raycastHit12.triangleIndex * 3 + 1],
						sharedMesh.triangles[raycastHit12.triangleIndex * 3 + 2]
					};
					for (int k = 0; k < sharedMesh.subMeshCount; k++)
					{
						int[] triangles = sharedMesh.GetTriangles(k);
						for (int l = 0; l < triangles.Length; l += 3)
						{
							if (triangles[l] == array2[0] && triangles[l + 1] == array2[1] && triangles[l + 2] == array2[2])
							{
								this.mat = component2.sharedMaterials[k];
								num = k;
							}
						}
					}
				}
				if (Input.GetMouseButtonDown(0))
				{
					if (raycastHit12.collider.gameObject.transform.parent.childCount < 2)
					{
						UnityEngine.Object.Destroy(raycastHit12.collider.gameObject.transform.parent.gameObject);
					}
					else
					{
						UnityEngine.Object.Destroy(raycastHit12.collider.gameObject);
					}
					this.rend = null;
					this.matint = -2;
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().SledgeHammer);
					return;
				}
				if (this.mat && (!(this.rend == raycastHit12.collider.gameObject.GetComponent<Renderer>()) || num != this.matint))
				{
					if (this.rend)
					{
						Material[] materials2 = this.rend.materials;
						materials2[this.matint] = this.originalMaterial;
						this.rend.materials = materials2;
					}
					this.originalMaterial = this.mat;
					this.rend = raycastHit12.collider.gameObject.GetComponent<Renderer>();
					Material[] materials3 = this.rend.materials;
					materials3[num] = this.testmat;
					this.rend.materials = materials3;
					this.matint = num;
				}
			}
			else if (this.rend)
			{
				Material[] materials4 = this.rend.materials;
				materials4[this.matint] = this.originalMaterial;
				this.rend.materials = materials4;
				this.originalMaterial = null;
				this.rend = null;
				this.matint = -2;
			}
		}
		if (tools.tool == 45)
		{
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			int num2 = 0;
			RaycastHit raycastHit13;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit13, this.distanceToSee, 1 << LayerMask.NameToLayer("Default")) && raycastHit13.collider.transform.root.name == "BuildingSite")
			{
				this.mat = null;
				if (raycastHit13.collider.GetComponent<MeshCollider>())
				{
					MeshCollider meshCollider2 = raycastHit13.collider as MeshCollider;
					if (meshCollider2 != null || meshCollider2.sharedMesh != null)
					{
						Mesh sharedMesh2 = meshCollider2.sharedMesh;
						Renderer component3 = raycastHit13.collider.GetComponent<MeshRenderer>();
						int[] array3 = new int[]
						{
							sharedMesh2.triangles[raycastHit13.triangleIndex * 3],
							sharedMesh2.triangles[raycastHit13.triangleIndex * 3 + 1],
							sharedMesh2.triangles[raycastHit13.triangleIndex * 3 + 2]
						};
						for (int m = 0; m < sharedMesh2.subMeshCount; m++)
						{
							int[] triangles2 = sharedMesh2.GetTriangles(m);
							for (int n = 0; n < triangles2.Length; n += 3)
							{
								if (triangles2[n] == array3[0] && triangles2[n + 1] == array3[1] && triangles2[n + 2] == array3[2])
								{
									this.mat = component3.sharedMaterials[m];
									num2 = m;
								}
							}
						}
					}
				}
				else if (raycastHit13.collider.GetComponent<BoxCollider>())
				{
					Mesh sharedMesh3 = raycastHit13.collider.GetComponent<MeshFilter>().sharedMesh;
					Renderer component4 = raycastHit13.collider.GetComponent<MeshRenderer>();
					this.mat = component4.sharedMaterials[0];
				}
				if (Input.GetMouseButtonDown(0) && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife > 0f && Input.GetMouseButtonDown(0))
				{
					tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife -= 1f;
					this.originalMaterial = null;
					this.rend = null;
					this.matint = -2;
					this.mat = null;
					if (tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife == 0f)
					{
						UnityEngine.Object.Destroy(tools.ToolHand.transform.GetChild(0).gameObject);
						tools.tool = 1;
					}
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().PlaceWall);
				}
				if (this.mat && (!(this.rend == raycastHit13.collider.gameObject.GetComponent<Renderer>()) || num2 != this.matint))
				{
					if (this.rend)
					{
						Material[] sharedMaterials = this.rend.sharedMaterials;
						sharedMaterials[this.matint] = this.originalMaterial;
						this.rend.sharedMaterials = sharedMaterials;
					}
					this.originalMaterial = this.mat;
					this.rend = raycastHit13.collider.gameObject.GetComponent<Renderer>();
					Material[] sharedMaterials2 = this.rend.sharedMaterials;
					sharedMaterials2[num2] = tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().material;
					this.rend.sharedMaterials = sharedMaterials2;
					this.matint = num2;
				}
			}
			else if (this.rend)
			{
				Material[] sharedMaterials3 = this.rend.sharedMaterials;
				sharedMaterials3[this.matint] = this.originalMaterial;
				this.rend.sharedMaterials = sharedMaterials3;
				this.originalMaterial = null;
				this.rend = null;
				this.matint = -2;
			}
		}
		if (Input.GetMouseButtonUp(0) && tools.PartInHand)
		{
			if (tools.PartInHand.GetComponent<PickupCup>())
			{
				tools.PartInHand.GetComponent<PickupCup>().RemoveFromHand();
			}
			else if (tools.PartInHand.GetComponent<PickupItems>())
			{
				tools.PartInHand.GetComponent<PickupItems>().RemoveFromHand();
			}
		}
		RaycastHit raycastHit14;
		if (tools.tool == 11 && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit14, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")) && (raycastHit14.collider.GetComponent<Pickup>() || raycastHit14.collider.GetComponent<PickupDoor>() || raycastHit14.collider.GetComponent<ShatterPart>()))
		{
			tools.canremove = true;
			if (Input.GetMouseButtonDown(0))
			{
				if (raycastHit14.collider.transform.root.gameObject.GetComponent<VehicleDamage>())
				{
					raycastHit14.collider.transform.root.gameObject.GetComponent<VehicleDamage>().Start();
				}
				if (raycastHit14.collider.gameObject.GetComponent<DetachablePart>())
				{
					if (raycastHit14.collider.gameObject.GetComponent<DetachablePart>().breakForce > 17f)
					{
						raycastHit14.collider.gameObject.GetComponent<DetachablePart>().breakForce -= 10f;
						this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BreakOff);
					}
					else
					{
						Pickup component5 = raycastHit14.collider.GetComponent<Pickup>();
						if (component5 != null)
						{
							if (!raycastHit14.collider.gameObject.GetComponent<Rigidbody>())
							{
								raycastHit14.collider.gameObject.AddComponent<Rigidbody>();
								raycastHit14.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
								raycastHit14.collider.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
							}
							component5.BRAKE();
							this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BreakOff2);
						}
						PickupDoor component6 = raycastHit14.collider.GetComponent<PickupDoor>();
						if (component6 != null)
						{
							if (raycastHit14.collider.GetComponent<FixedJoint>() != null)
							{
								UnityEngine.Object.Destroy(raycastHit14.collider.GetComponent<FixedJoint>());
							}
							if (raycastHit14.collider.GetComponent<HingeJoint>() != null)
							{
								UnityEngine.Object.Destroy(raycastHit14.collider.GetComponent<HingeJoint>());
							}
							component6.BRAKE();
							this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BreakOff2);
						}
						if (raycastHit14.collider.transform.root.gameObject.GetComponent<VehicleDamage>())
						{
							raycastHit14.collider.transform.root.gameObject.GetComponent<VehicleDamage>().Start();
						}
					}
				}
				else
				{
					Pickup component7 = raycastHit14.collider.GetComponent<Pickup>();
					if (component7 != null)
					{
						if (!raycastHit14.collider.gameObject.GetComponent<Rigidbody>())
						{
							raycastHit14.collider.gameObject.AddComponent<Rigidbody>();
							raycastHit14.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
							raycastHit14.collider.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
						}
						component7.BRAKE();
					}
					PickupDoor component8 = raycastHit14.collider.GetComponent<PickupDoor>();
					if (component8 != null)
					{
						if (raycastHit14.collider.GetComponent<FixedJoint>() != null)
						{
							UnityEngine.Object.Destroy(raycastHit14.collider.GetComponent<FixedJoint>());
						}
						if (raycastHit14.collider.GetComponent<HingeJoint>() != null)
						{
							UnityEngine.Object.Destroy(raycastHit14.collider.GetComponent<HingeJoint>());
						}
						component8.BRAKE();
					}
				}
				if (raycastHit14.collider.GetComponent<ShatterPart>())
				{
					raycastHit14.collider.GetComponent<ShatterPart>().Shatter();
				}
				CarProperties component9 = raycastHit14.collider.GetComponent<CarProperties>();
				if (component9 != null)
				{
					component9.Ruined = true;
				}
				Mesh mesh = raycastHit14.collider.GetComponent<MeshFilter>().mesh;
				Vector3[] vertices = mesh.vertices;
				Vector3 b = Vector3.zero;
				b = raycastHit14.collider.transform.InverseTransformPoint(raycastHit14.point);
				for (int num3 = 0; num3 < vertices.Length; num3++)
				{
					if ((vertices[num3] - b).sqrMagnitude <= 0.15f)
					{
						vertices[num3] = Vector3.Lerp(vertices[num3], b, 0.1f);
					}
					mesh.vertices = vertices;
				}
			}
		}
		RaycastHit raycastHit15;
		if (tools.tool == 12 && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit15, this.distanceToSee, 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")) && raycastHit15.collider.GetComponent<CarProperties>() && raycastHit15.collider.GetComponent<CarProperties>().MeshDamaged && !raycastHit15.collider.GetComponent<CarProperties>().Ruined && raycastHit15.collider.GetComponent<CarProperties>().MeshRepairable)
		{
			tools.canrepair = true;
			if (Input.GetMouseButtonDown(0))
			{
				CarProperties component10 = raycastHit15.collider.GetComponent<CarProperties>();
				if (component10 != null && !component10.Ruined && component10.Fairable)
				{
					component10.Repair(raycastHit15.point);
				}
				if (component10 != null && !component10.Ruined && !component10.Fairable)
				{
					component10.Repair2(raycastHit15.point);
				}
			}
		}
		RaycastHit raycastHit16;
		if (tools.tool == 21 && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit16, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")) && raycastHit16.collider.GetComponent<CarProperties>().Tintable && Input.GetMouseButtonDown(0))
		{
			raycastHit16.collider.GetComponent<CarProperties>().Tint();
		}
		if (tools.tool == 19)
		{
			RaycastHit raycastHit17;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit17, 2.5f, 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("LooseParts")))
			{
				if (tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife > 0f)
				{
					if (!this.TexturList.enabled)
					{
						this.TexturList.enabled = true;
					}
					this.Preview.enabled = true;
					if (Input.GetMouseButtonDown(0))
					{
						tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife -= 1f;
						tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().VisualUpdate();
					}
				}
				else
				{
					this.Preview.enabled = false;
				}
			}
			else
			{
				this.Preview.enabled = false;
			}
		}
		RaycastHit raycastHit18;
		if (tools.tool == 23 && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit18, 2.5f, 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")) && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife > 0f && Input.GetMouseButtonDown(0))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(raycastHit18.transform.gameObject);
			gameObject.transform.position = raycastHit18.transform.position;
			gameObject.transform.rotation = raycastHit18.transform.rotation;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.CutOutShape);
			gameObject2.transform.position = this.CutOutShape.transform.position;
			gameObject2.transform.rotation = this.CutOutShape.transform.rotation;
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject.transform.position = new Vector3(0f, 0f, 0f);
			gameObject.transform.rotation = Quaternion.identity;
			Model model = CSG.Subtract(gameObject, gameObject2);
			UnityEngine.Object.Destroy(gameObject2);
			UnityEngine.Object.Destroy(gameObject);
			raycastHit18.transform.gameObject.GetComponent<MeshFilter>().sharedMesh = model.mesh;
			string str = "Cutting";
			GameObject gameObject3 = raycastHit18.transform.gameObject;
			Debug.Log(str + ((gameObject3 != null) ? gameObject3.ToString() : null));
			tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife -= 1f;
			tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().VisualUpdate();
		}
		RaycastHit raycastHit19;
		if (tools.tool == 7 && tools.ToolHand.transform.childCount > 0 && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife > 0f && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit19, this.distanceToSee, 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")) && !raycastHit19.collider.GetComponent<CarProperties>().Ruined && raycastHit19.collider.GetComponent<CarProperties>().MeshRepairable && (raycastHit19.collider.GetComponent<CarProperties>().MeshDamaged || raycastHit19.collider.GetComponent<CarProperties>().MeshLittleDamaged))
		{
			tools.canfair = true;
			if (Input.GetMouseButtonDown(0))
			{
				CarProperties component11 = raycastHit19.collider.GetComponent<CarProperties>();
				if (component11 != null && !component11.Ruined && component11.Fairable)
				{
					component11.Fair(raycastHit19.point);
				}
			}
		}
		RaycastHit raycastHit20;
		if (tools.tool == 4 && tools.ToolHand.transform.childCount > 0 && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().Attached && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().Attached.GetComponent<PickupItems>().Grinder && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit20, this.distanceToSee, 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")) && !raycastHit20.collider.GetComponent<CarProperties>().Ruined && raycastHit20.collider.GetComponent<CarProperties>().MeshRepairable && Input.GetMouseButton(0) && this.timer > 0.2f)
		{
			this.timer = 0f;
			CarProperties component12 = raycastHit20.collider.GetComponent<CarProperties>();
			if (component12 != null && !component12.Ruined)
			{
				component12.RustRemove(new Vector3?(raycastHit20.point));
			}
		}
		if (tools.tool == 4 || tools.tool == 5)
		{
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			RaycastHit raycastHit21;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit21, this.distanceToSee, 1 << LayerMask.NameToLayer("Weld")))
			{
				Renderer component13 = raycastHit21.collider.gameObject.GetComponent<Renderer>();
				if (component13 == this.rend)
				{
					return;
				}
				if (component13 && component13 != this.rend && this.rend)
				{
					this.rend.sharedMaterial = this.originalMaterial;
				}
				if (!component13)
				{
					return;
				}
				this.rend = component13;
				this.originalMaterial = this.rend.sharedMaterial;
				this.tempMaterial = new Material(this.originalMaterial);
				this.rend.material = this.tempMaterial;
				this.rend.material.color = this.highlightColor;
			}
			else if (this.rend)
			{
				this.rend.sharedMaterial = this.originalMaterial;
				this.rend = null;
			}
		}
		if (tools.tool == 3)
		{
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			RaycastHit raycastHit22;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit22, this.distanceToSee, 1 << LayerMask.NameToLayer("FlatBolts")))
			{
				Renderer component14 = raycastHit22.collider.gameObject.GetComponent<Renderer>();
				if (component14 == this.rend)
				{
					return;
				}
				if (component14 && component14 != this.rend && this.rend)
				{
					this.rend.sharedMaterial = this.originalMaterial;
				}
				if (!component14)
				{
					return;
				}
				this.rend = component14;
				this.originalMaterial = this.rend.sharedMaterial;
				this.tempMaterial = new Material(this.originalMaterial);
				this.rend.material = this.tempMaterial;
				this.rend.material.color = this.highlightColor;
			}
			else if (this.rend)
			{
				this.rend.sharedMaterial = this.originalMaterial;
				this.rend = null;
			}
		}
		if (tools.tool == 2)
		{
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			RaycastHit raycastHit23;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit23, this.distanceToSee, 1 << LayerMask.NameToLayer("Bolts")))
			{
				if (Input.GetMouseButtonDown(0) && raycastHit23.collider.gameObject.GetComponent<CoilNut>())
				{
					raycastHit23.collider.gameObject.GetComponent<CoilNut>().tighten();
				}
				Renderer component15 = raycastHit23.collider.gameObject.GetComponent<Renderer>();
				if (component15 == this.rend && this.highlightColor == Color.red)
				{
					this.rend.material.color = this.highlightColor;
				}
				if (component15 == this.rend)
				{
					return;
				}
				if (component15 && component15 != this.rend && this.rend)
				{
					this.rend.sharedMaterial = this.originalMaterial;
				}
				if (!component15)
				{
					return;
				}
				this.rend = component15;
				this.originalMaterial = this.rend.sharedMaterial;
				this.tempMaterial = new Material(this.originalMaterial);
				this.rend.material = this.tempMaterial;
				this.rend.material.color = this.highlightColor;
			}
			else if (this.rend)
			{
				this.rend.sharedMaterial = this.originalMaterial;
				this.rend = null;
			}
		}
		if (tools.tool == 6)
		{
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			RaycastHit raycastHit24;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit24, this.distanceToSee, 1 << LayerMask.NameToLayer("Windows")))
			{
				if (raycastHit24.collider.gameObject.GetComponent<RemoveWindow>())
				{
					raycastHit24.collider.gameObject.GetComponent<RemoveWindow>().enabled = true;
					Renderer component16 = raycastHit24.collider.gameObject.GetComponent<Renderer>();
					if (component16 == this.rend)
					{
						return;
					}
					if (component16 && component16 != this.rend && this.rend)
					{
						this.rend.sharedMaterial = this.originalMaterial;
					}
					if (!component16)
					{
						return;
					}
					this.rend = component16;
					this.originalMaterial = this.rend.sharedMaterial;
					this.tempMaterial = new Material(this.originalMaterial);
					this.rend.material = this.tempMaterial;
					this.rend.material.color = this.highlightColor;
				}
			}
			else if (this.rend)
			{
				this.rend.sharedMaterial = this.originalMaterial;
				this.rend = null;
			}
		}
		if (tools.tool == 8)
		{
			Debug.DrawRay(base.transform.position, base.transform.forward * this.distanceToSee, Color.magenta);
			RaycastHit raycastHit25;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit25, this.distanceToSee, 1 << LayerMask.NameToLayer("Bolts")) && raycastHit25.collider.gameObject.GetComponent<Sparkplug>())
			{
				raycastHit25.collider.gameObject.GetComponent<Sparkplug>().enabled = true;
				raycastHit25.collider.gameObject.GetComponent<Sparkplug>().EnabledThisFrame = true;
				raycastHit25.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
				if (!tools.cooldown3)
				{
					base.StartCoroutine(base.transform.parent.parent.parent.GetComponent<tools>().Cooldown3());
				}
			}
		}
	}

	// Token: 0x0400133C RID: 4924
	public float distanceToSee;

	// Token: 0x0400133D RID: 4925
	public string ObjectName;

	// Token: 0x0400133E RID: 4926
	public Color highlightColor;

	// Token: 0x0400133F RID: 4927
	public Color unmountedColor;

	// Token: 0x04001340 RID: 4928
	private Material originalMaterial;

	// Token: 0x04001341 RID: 4929
	private Material tempMaterial;

	// Token: 0x04001342 RID: 4930
	public Renderer rend;

	// Token: 0x04001343 RID: 4931
	public GameObject AudioParent;

	// Token: 0x04001344 RID: 4932
	public Material testmat;

	// Token: 0x04001345 RID: 4933
	public int matint;

	// Token: 0x04001346 RID: 4934
	public P3dHitScreen Preview;

	// Token: 0x04001347 RID: 4935
	public TextureList TexturList;

	// Token: 0x04001348 RID: 4936
	public float timer;

	// Token: 0x04001349 RID: 4937
	public GameObject CutOutShape;

	// Token: 0x0400134A RID: 4938
	private Material mat;
}
