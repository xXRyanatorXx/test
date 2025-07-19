using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class SaveInside : MonoBehaviour
{
	// Token: 0x06000B02 RID: 2818 RVA: 0x000721B1 File Offset: 0x000703B1
	private void Awake()
	{
		this.goList = new List<GameObject>();
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x000721C0 File Offset: 0x000703C0
	public void Save(bool destroy)
	{
		ModLoader.OnBarnSave();
		if (this.OpenSaveSlot == 0)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn.dat";
		}
		if (this.OpenSaveSlot == 1)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn1.dat";
		}
		if (this.OpenSaveSlot == 2)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn2.dat";
		}
		if (this.OpenSaveSlot == 3)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn3.dat";
		}
		if (this.OpenSaveSlot == 4)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn4.dat";
		}
		if (this.OpenSaveSlot == 5)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn5.dat";
		}
		if (this.OpenSaveSlot == 6)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn6.dat";
		}
		if (this.OpenSaveSlot == 7)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn7.dat";
		}
		if (this.OpenSaveSlot == 8)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn8.dat";
		}
		if (this.OpenSaveSlot == 9)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn9.dat";
		}
		SaveSystem saveSystem = new SaveSystem(Application.persistentDataPath + this.SavePath);
		this.ObjectNumber = 0;
		this.goList.Clear();
		Collider[] array = Physics.OverlapBox(base.gameObject.transform.position, base.transform.localScale / 2f, Quaternion.identity);
		for (int i = 0; i < array.Length; i++)
		{
			Transform component = array[i].GetComponent<Transform>();
			if (component.gameObject.GetComponent<MainCarProperties>() && component.gameObject.GetComponent<MainCarProperties>().Owner == "Player")
			{
				this.ObjectNumber++;
				component.gameObject.GetComponent<MainCarProperties>().ObjectNumber = this.ObjectNumber;
				component.gameObject.GetComponent<MainCarProperties>().InBarn = true;
				this.goList.Add(component.gameObject);
				foreach (CarProperties carProperties in component.gameObject.GetComponentsInChildren<CarProperties>())
				{
					if (carProperties.PREFAB && carProperties.Owner == "Player")
					{
						this.goList.Add(carProperties.gameObject);
						this.ObjectNumber++;
						carProperties.ObjectNumber = this.ObjectNumber;
						carProperties.SavingGame();
						carProperties.InBarn = true;
					}
				}
			}
			if (component.gameObject.GetComponent<MainTrailerProperties>())
			{
				this.ObjectNumber++;
				component.gameObject.GetComponent<MainTrailerProperties>().ObjectNumber = this.ObjectNumber;
				component.gameObject.GetComponent<MainTrailerProperties>().InBarn = true;
				this.goList.Add(component.gameObject);
				foreach (CarProperties carProperties2 in component.gameObject.GetComponentsInChildren<CarProperties>())
				{
					if (carProperties2.PREFAB && carProperties2.Owner == "Player")
					{
						this.goList.Add(carProperties2.gameObject);
						this.ObjectNumber++;
						carProperties2.ObjectNumber = this.ObjectNumber;
						carProperties2.SavingGame();
						carProperties2.InBarn = true;
					}
				}
			}
			if (component.gameObject.GetComponent<CarProperties>() && component.gameObject.GetComponent<CarProperties>().PREFAB && component.gameObject.GetComponent<CarProperties>().Owner == "Player" && (!component.gameObject.transform.parent || component.parent.GetComponent<OpenableBox>()))
			{
				this.ObjectNumber++;
				component.gameObject.GetComponent<CarProperties>().ObjectNumber = this.ObjectNumber;
				component.gameObject.GetComponent<CarProperties>().InBarn = true;
				component.gameObject.GetComponent<CarProperties>().SavingGame();
				this.goList.Add(component.gameObject);
				foreach (CarProperties carProperties3 in component.gameObject.GetComponentsInChildren<CarProperties>())
				{
					if (carProperties3.PREFAB && carProperties3.Owner == "Player")
					{
						this.goList.Add(carProperties3.gameObject);
						this.ObjectNumber++;
						carProperties3.ObjectNumber = this.ObjectNumber;
						carProperties3.SavingGame();
						carProperties3.InBarn = true;
					}
				}
			}
			if (component.gameObject.GetComponent<SaveItem>() && component.gameObject.GetComponent<PickupTool>() && component.gameObject.GetComponent<PickupTool>().MONEY)
			{
				this.ObjectNumber++;
				component.gameObject.GetComponent<SaveItem>().ObjectNumber = this.ObjectNumber;
				component.gameObject.GetComponent<SaveItem>().InBarn = true;
				this.goList.Add(component.gameObject);
			}
		}
		foreach (GameObject gameObject in this.goList)
		{
			Transform component2 = gameObject.GetComponent<Transform>();
			if (component2.position.y > 45f)
			{
				if (component2.gameObject.GetComponent<MainCarProperties>() && component2.gameObject.GetComponent<MainCarProperties>().Owner == "Player")
				{
					MainCarProperties component3 = component2.gameObject.GetComponent<MainCarProperties>();
					this.CurrentNumber = component3.ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component3.PREFAB.transform.name.ToString());
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component2.position - this.MainSaver.transform.root.transform.position);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", component2.rotation);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Owner", component3.Owner);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "OriginalColor", component3.OriginalColor);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Color", component3.Color);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Started", component3.Started);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mileage", component3.Mileage);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "OriginalInteriorColor", component3.OriginalInteriorColor);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "HP", component3.HP);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedEngineParts", component3.RuinedEngineParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornEngineParts", component3.WornEngineParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedSuspensionParts", component3.RuinedSuspensionParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornSuspensionParts", component3.WornSuspensionParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedBrakeParts", component3.RuinedBrakeParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornBrakeParts", component3.WornBrakeParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedOtherParts", component3.RuinedOtherParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornOtherparts", component3.WornOtherparts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "CantCrank", component3.CantCrank);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "CantRun", component3.CantRun);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "BrakeProblems", component3.BrakeProblems);
					saveSystem.add("Parent" + this.CurrentNumber.ToString(), 0);
				}
				if (component2.gameObject.GetComponent<MainTrailerProperties>())
				{
					this.CurrentNumber = component2.gameObject.GetComponent<MainTrailerProperties>().ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component2.gameObject.GetComponent<MainTrailerProperties>().PrefabName);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component2.position - this.MainSaver.transform.root.transform.position);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", component2.rotation);
				}
				if (component2.gameObject.GetComponent<CarProperties>() && component2.gameObject.GetComponent<CarProperties>().PREFAB && component2.gameObject.GetComponent<CarProperties>().Owner == "Player")
				{
					CarProperties component4 = component2.gameObject.GetComponent<CarProperties>();
					this.CurrentNumber = component4.ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component4.PrefabName);
					if (component2.parent)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component2.localPosition);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component2.localPosition - this.MainSaver.transform.root.transform.position);
					}
					if (component2.root.name == "EngineStand" || component2.root.GetComponent<OpenableBox>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component2.position);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", component2.localRotation);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Owner", component4.Owner);
					if (component2.parent && component2.parent.parent && component2.parent.parent.GetComponent<CarProperties>() && component2.parent.parent.GetComponent<CarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component2.parent.parent.GetComponent<CarProperties>().ObjectNumber);
					}
					else if (component2.parent && component2.parent.parent && component2.parent.parent.GetComponent<MainCarProperties>() && component2.parent.parent.GetComponent<MainCarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component2.parent.parent.GetComponent<MainCarProperties>().ObjectNumber);
					}
					else if (component2.parent && component2.parent.parent && component2.parent.parent.parent && component2.parent.parent.parent.GetComponent<CarProperties>() && component2.parent.parent.parent.GetComponent<CarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component2.parent.parent.parent.GetComponent<CarProperties>().ObjectNumber);
					}
					else if (component2.parent && component2.parent.parent && component2.parent.parent.parent && component2.parent.parent.parent.GetComponent<MainCarProperties>() && component2.parent.parent.parent.GetComponent<MainCarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component2.parent.parent.parent.GetComponent<MainCarProperties>().ObjectNumber);
					}
					else if (component2.root.GetComponent<MainTrailerProperties>())
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component2.root.GetComponent<MainTrailerProperties>().ObjectNumber);
					}
					else
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), 0);
					}
					string str = "detala+ parents";
					string str2 = this.CurrentNumber.ToString();
					object obj = saveSystem.get("Parent" + this.CurrentNumber.ToString(), null);
					Debug.Log(str + str2 + ((obj != null) ? obj.ToString() : null));
					saveSystem.add("started" + this.CurrentNumber.ToString(), component4.started);
					if (component4.BikeStand)
					{
						saveSystem.add("BikeStandHeight" + this.CurrentNumber.ToString(), component4.BikeStand.steps);
					}
					if (component4.ClusterMileage > 0f)
					{
						saveSystem.add("ClusterMileage" + this.CurrentNumber.ToString(), component4.ClusterMileage);
					}
					saveSystem.add("FluidSize" + this.CurrentNumber.ToString(), component4.FluidSize);
					saveSystem.add("DieselPercent" + this.CurrentNumber.ToString(), component4.DieselPercent);
					saveSystem.add("FluidCondition" + this.CurrentNumber.ToString(), component4.FluidCondition);
					saveSystem.add("TirePressure" + this.CurrentNumber.ToString(), component4.TirePressure);
					saveSystem.add("Condition" + this.CurrentNumber.ToString(), component4.Condition);
					saveSystem.add("Chromed" + this.CurrentNumber.ToString(), component4.Chromed);
					saveSystem.add("SavePosition" + this.CurrentNumber.ToString(), component4.SavePosition);
					if (component4.coilnut)
					{
						saveSystem.add("coilnut" + this.CurrentNumber.ToString(), component4.coilnut.height);
					}
					saveSystem.add("Texture1" + this.CurrentNumber.ToString(), component4.Texture1);
					saveSystem.add("Texture3" + this.CurrentNumber.ToString(), component4.Texture3);
					saveSystem.add("Texture4" + this.CurrentNumber.ToString(), component4.Texture4);
					saveSystem.add("BodyMatNumber" + this.CurrentNumber.ToString(), component4.BodyMatNumber);
					saveSystem.add("OriginalInterior" + this.CurrentNumber.ToString(), component4.OriginalInterior);
					saveSystem.add("TintLevel" + this.CurrentNumber.ToString(), component4.TintLevel);
					saveSystem.add("Ruined" + this.CurrentNumber.ToString(), component4.Ruined);
					saveSystem.add("Damaged" + this.CurrentNumber.ToString(), component4.Damaged);
					saveSystem.add("PartIsOld" + this.CurrentNumber.ToString(), component4.PartIsOld);
					saveSystem.add("MeshDamaged" + this.CurrentNumber.ToString(), component4.MeshDamaged);
					saveSystem.add("MeshLittleDamaged" + this.CurrentNumber.ToString(), component4.MeshLittleDamaged);
					saveSystem.add("Damagedvertices" + this.CurrentNumber.ToString(), component4.Damagedvertices);
					if (component4.ChildDamag && component4.ChildDamag.MeshLittleDamaged)
					{
						saveSystem.add("RuinedCH" + this.CurrentNumber.ToString(), component4.ChildDamag.Ruined);
						saveSystem.add("MeshDamagedCH" + this.CurrentNumber.ToString(), component4.ChildDamag.MeshDamaged);
						saveSystem.add("MeshLittleDamagedCH" + this.CurrentNumber.ToString(), component4.ChildDamag.MeshLittleDamaged);
						saveSystem.add("DamagedverticesCH" + this.CurrentNumber.ToString(), component4.ChildDamag.GetComponent<MeshFilter>().mesh.vertices);
					}
					saveSystem.add("Loose" + this.CurrentNumber.ToString(), component4.Loose);
					if (component2.gameObject.transform.parent)
					{
						saveSystem.add("tightnuts" + this.CurrentNumber.ToString(), component2.gameObject.GetComponent<Partinfo>().tightnuts);
					}
					if (component4.NumberPlate)
					{
						saveSystem.add("One" + this.CurrentNumber.ToString(), component4.One.name.ToString());
						saveSystem.add("Two" + this.CurrentNumber.ToString(), component4.Two.name.ToString());
						saveSystem.add("Three" + this.CurrentNumber.ToString(), component4.Three.name.ToString());
						saveSystem.add("Four" + this.CurrentNumber.ToString(), component4.Four.name.ToString());
						saveSystem.add("Five" + this.CurrentNumber.ToString(), component4.Five.name.ToString());
						saveSystem.add("Six" + this.CurrentNumber.ToString(), component4.Six.name.ToString());
					}
				}
				if (component2.gameObject.GetComponent<SaveItem>())
				{
					this.CurrentNumber = component2.gameObject.GetComponent<SaveItem>().ObjectNumber;
					if (component2.gameObject.GetComponent<SaveItem>().PrefabName != "")
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component2.gameObject.GetComponent<SaveItem>().PrefabName);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component2.name);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component2.position - this.MainSaver.transform.root.transform.position);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", component2.rotation);
					if (component2.GetComponent<PickupTool>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "paintlife", component2.GetComponent<PickupTool>().paintlife);
					}
				}
			}
		}
		ModLoader.OnSaveSystemSave(saveSystem, true);
		foreach (GameObject gameObject2 in this.goList)
		{
			if (destroy && (gameObject2.GetComponent<CarProperties>() || gameObject2.GetComponent<MainCarProperties>() || gameObject2.GetComponent<MainTrailerProperties>() || (gameObject2.gameObject.GetComponent<PickupTool>() && gameObject2.gameObject.GetComponent<PickupTool>().MONEY)))
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		Debug.Log(this.ObjectNumber.ToString());
		saveSystem.add("Objects", this.ObjectNumber);
		saveSystem.write();
		ModLoader.OnBarnSaveFinish();
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x000739B8 File Offset: 0x00071BB8
	public void Load()
	{
		this.goList.Clear();
		if (this.SaveSlot == 0)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn.dat";
		}
		if (this.SaveSlot == 1)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn1.dat";
		}
		if (this.SaveSlot == 2)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn2.dat";
		}
		if (this.SaveSlot == 3)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn3.dat";
		}
		if (this.SaveSlot == 4)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn4.dat";
		}
		if (this.SaveSlot == 5)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn5.dat";
		}
		if (this.SaveSlot == 6)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn6.dat";
		}
		if (this.SaveSlot == 7)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn7.dat";
		}
		if (this.SaveSlot == 8)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn8.dat";
		}
		if (this.SaveSlot == 9)
		{
			this.SavePath = this.MainSaver.SaveFolder + "/Barn9.dat";
		}
		this.OpenSaveSlot = this.SaveSlot;
		SaveSystem saveSystem = new SaveSystem(Application.persistentDataPath + this.SavePath);
		saveSystem.read();
		GameObject[] array = new GameObject[(int)saveSystem.get("Objects", 0) + 10];
		if ((int)saveSystem.get("Objects", 0) > 0)
		{
			this.ObjectNumber = (int)saveSystem.get("Objects", 0);
			int i = 0;
			while (i < (int)saveSystem.get("Objects", 0))
			{
				string text = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), null);
				if (!(text != "") || text == null)
				{
					goto IL_112D;
				}
				GameObject gameObject = null;
				try
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(cachedResources.Load(text) as GameObject);
				}
				catch (Exception)
				{
					if (Saver.modParts.ContainsKey(text))
					{
						gameObject = UnityEngine.Object.Instantiate<GameObject>(Saver.modParts[text] as GameObject);
					}
				}
				if (gameObject == null)
				{
					Debug.Log("Object Not in resources" + text);
				}
				else
				{
					if (gameObject.GetComponent<Partinfo>() && gameObject.GetComponent<Partinfo>().RenamedPrefab != "")
					{
						gameObject.transform.name = gameObject.GetComponent<Partinfo>().RenamedPrefab;
					}
					else
					{
						gameObject.transform.name = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), null);
					}
					if (gameObject.GetComponent<CarProperties>())
					{
						CarProperties component = gameObject.gameObject.GetComponent<CarProperties>();
						component.InBarn = true;
						component.ObjectNumber = this.ObjectNumber;
						component.SavePosition = (int)saveSystem.get("SavePosition" + this.ObjectNumber.ToString(), 0);
						component.started = (bool)saveSystem.get("started" + this.ObjectNumber.ToString(), false);
						if (component.BikeStand)
						{
							component.BikeStand.steps = (int)saveSystem.get("BikeStandHeight" + this.ObjectNumber.ToString(), 0f);
							component.BikeStand.LiftObject.transform.position += component.BikeStand.transform.TransformDirection(0f, -0.01f * (float)component.BikeStand.steps, 0f);
						}
						component.Owner = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Owner", "");
						component.ClusterMileage = (float)saveSystem.get("ClusterMileage" + this.ObjectNumber.ToString(), 0f);
						if (component.MileageText)
						{
							component.MileageText.text = component.ClusterMileage.ToString("F0");
						}
						component.FluidSize = (float)saveSystem.get("FluidSize" + this.ObjectNumber.ToString(), 0f);
						component.DieselPercent = (float)saveSystem.get("DieselPercent" + this.ObjectNumber.ToString(), 0f);
						component.FluidCondition = (float)saveSystem.get("FluidCondition" + this.ObjectNumber.ToString(), 0f);
						if (component.Fuel)
						{
							component.Fuel.StartFromMain();
						}
						if (component.Coolant)
						{
							component.Coolant.StartFromMain();
						}
						if (component.BrakeFluid)
						{
							component.BrakeFluid.StartFromMain();
						}
						if (component.EngineOil)
						{
							component.EngineOil.StartFromMain();
						}
						component.TirePressure = (float)saveSystem.get("TirePressure" + this.ObjectNumber.ToString(), 2f);
						component.Condition = (float)saveSystem.get("Condition" + this.ObjectNumber.ToString(), 0f);
						component.Chromed = (bool)saveSystem.get("Chromed" + this.ObjectNumber.ToString(), false);
						if (component.coilnut)
						{
							component.coilnut.height = (float)saveSystem.get("coilnut" + this.ObjectNumber.ToString(), 0f);
							component.coilnut.restart();
						}
						component.Texture1 = (string)saveSystem.get("Texture1" + this.ObjectNumber.ToString(), "");
						component.Texture3 = (string)saveSystem.get("Texture3" + this.ObjectNumber.ToString(), "");
						component.Texture4 = (string)saveSystem.get("Texture4" + this.ObjectNumber.ToString(), "");
						component.BodyMatNumber = (int)saveSystem.get("BodyMatNumber" + this.ObjectNumber.ToString(), 0);
						component.OriginalInterior = (int)saveSystem.get("OriginalInterior" + this.ObjectNumber.ToString(), 0);
						component.TintLevel = (int)saveSystem.get("TintLevel" + this.ObjectNumber.ToString(), 0);
						component.Ruined = (bool)saveSystem.get("Ruined" + this.ObjectNumber.ToString(), false);
						component.Damaged = (bool)saveSystem.get("Damaged" + this.ObjectNumber.ToString(), false);
						component.PartIsOld = (bool)saveSystem.get("PartIsOld" + this.ObjectNumber.ToString(), false);
						component.MeshDamaged = (bool)saveSystem.get("MeshDamaged" + this.ObjectNumber.ToString(), false);
						component.MeshLittleDamaged = (bool)saveSystem.get("MeshLittleDamaged" + this.ObjectNumber.ToString(), false);
						Vector3[] array2 = (Vector3[])saveSystem.get("Damagedvertices" + this.ObjectNumber.ToString(), null);
						if (array2 != null)
						{
							component.Damagedvertices = array2;
						}
						if (component.ChildDamag)
						{
							component.ChildDamag.Ruined = (bool)saveSystem.get("RuinedCH" + this.ObjectNumber.ToString(), false);
							component.ChildDamag.MeshDamaged = (bool)saveSystem.get("MeshDamagedCH" + this.ObjectNumber.ToString(), false);
							component.ChildDamag.MeshLittleDamaged = (bool)saveSystem.get("MeshLittleDamagedCH" + this.ObjectNumber.ToString(), false);
							Vector3[] array3 = (Vector3[])saveSystem.get("DamagedverticesCH" + this.ObjectNumber.ToString(), null);
							if (array3 != null)
							{
								component.ChildDamag.Damagedvertices = array3;
							}
						}
						component.Loose = (List<string>)saveSystem.get("Loose" + this.ObjectNumber.ToString(), null);
						component.tightnuts = (float)saveSystem.get("tightnuts" + this.ObjectNumber.ToString(), 0f);
						if (component.NumberPlate)
						{
							component.One = (Resources.Load((string)saveSystem.get("One" + this.ObjectNumber.ToString(), "Empty"), typeof(Material)) as Material);
							component.Two = (Resources.Load((string)saveSystem.get("Two" + this.ObjectNumber.ToString(), "Empty"), typeof(Material)) as Material);
							component.Three = (Resources.Load((string)saveSystem.get("Three" + this.ObjectNumber.ToString(), "Empty"), typeof(Material)) as Material);
							component.Four = (Resources.Load((string)saveSystem.get("Four" + this.ObjectNumber.ToString(), "Empty"), typeof(Material)) as Material);
							component.Five = (Resources.Load((string)saveSystem.get("Five" + this.ObjectNumber.ToString(), "Empty"), typeof(Material)) as Material);
							component.Six = (Resources.Load((string)saveSystem.get("Six" + this.ObjectNumber.ToString(), "Empty"), typeof(Material)) as Material);
							component.LoadNumber();
						}
					}
					if (!gameObject.GetComponent<SaveItem>())
					{
						this.goList.Add(gameObject);
					}
					array[this.ObjectNumber] = gameObject;
					if (gameObject.GetComponent<MainCarProperties>())
					{
						MainCarProperties component2 = gameObject.GetComponent<MainCarProperties>();
						component2.InBarn = true;
						component2.ObjectNumber = this.ObjectNumber;
						component2.Owner = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Owner", "");
						component2.OriginalColor = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "OriginalColor", Color.white);
						component2.Color = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Color", Color.white);
						component2.Started = (bool)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Started", false);
						component2.Mileage = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mileage", 350000f);
						component2.OriginalInteriorColor = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "OriginalInteriorColor", "");
						component2.HP = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "HP", 0f);
						component2.RuinedEngineParts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "RuinedEngineParts", "");
						component2.WornEngineParts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "WornEngineParts", "");
						component2.RuinedSuspensionParts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "RuinedSuspensionParts", "");
						component2.WornSuspensionParts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "WornSuspensionParts", "");
						component2.RuinedBrakeParts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "RuinedBrakeParts", "");
						component2.WornBrakeParts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "WornBrakeParts", "");
						component2.RuinedOtherParts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "RuinedOtherParts", "");
						component2.WornOtherparts = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "WornOtherparts", "");
						component2.CantCrank = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "CantCrank", "");
						component2.CantRun = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "CantRun", "");
						component2.BrakeProblems = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "BrakeProblems", "");
					}
					if (gameObject.GetComponent<MainTrailerProperties>())
					{
						gameObject.GetComponent<MainTrailerProperties>().ObjectNumber = this.ObjectNumber;
						gameObject.GetComponent<MainTrailerProperties>().InBarn = true;
					}
					if (!gameObject.GetComponent<SaveItem>())
					{
						goto IL_112D;
					}
					if (gameObject.GetComponent<PickupTool>())
					{
						gameObject.GetComponent<PickupTool>().InBox = (int)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "childCount", 0);
					}
					if (gameObject.GetComponent<PickupTool>())
					{
						gameObject.GetComponent<PickupTool>().FluidSize = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "FluidSize", 0f);
					}
					if (gameObject.GetComponent<PickupTool>())
					{
						gameObject.GetComponent<PickupTool>().paintlife = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "paintlife", 0f);
					}
					if (gameObject.GetComponent<PickupTool>())
					{
						gameObject.GetComponent<PickupTool>().colorpaint = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Color", Color.white);
					}
					gameObject.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null);
					gameObject.transform.localRotation = (Quaternion)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "rotation", null);
					if (gameObject.transform.localPosition.y < 45f || gameObject.transform.localPosition.y > 1000f)
					{
						UnityEngine.Object.Destroy(gameObject.gameObject);
						goto IL_112D;
					}
					goto IL_112D;
				}
				IL_113B:
				i++;
				continue;
				IL_112D:
				this.ObjectNumber--;
				goto IL_113B;
			}
			foreach (GameObject gameObject2 in this.goList)
			{
				if (gameObject2.GetComponent<MainCarProperties>())
				{
					this.ObjectNumber = gameObject2.GetComponent<MainCarProperties>().ObjectNumber;
				}
				if (gameObject2.GetComponent<CarProperties>())
				{
					this.ObjectNumber = gameObject2.GetComponent<CarProperties>().ObjectNumber;
				}
				if (gameObject2.GetComponent<MainTrailerProperties>())
				{
					this.ObjectNumber = gameObject2.GetComponent<MainTrailerProperties>().ObjectNumber;
				}
				int num = (int)saveSystem.get("Parent" + this.ObjectNumber.ToString(), 0);
				if (num > 0)
				{
					GameObject gameObject3 = array[num];
					if ((gameObject3.GetComponent<MainCarProperties>() && gameObject3.GetComponent<MainCarProperties>().InBarn && gameObject3.GetComponent<MainCarProperties>().ObjectNumber == num) || (gameObject3.GetComponent<MainTrailerProperties>() && gameObject3.GetComponent<MainTrailerProperties>().InBarn && gameObject3.GetComponent<MainTrailerProperties>().ObjectNumber == num) || (gameObject3.GetComponent<CarProperties>() && gameObject3.GetComponent<CarProperties>().InBarn && gameObject3.GetComponent<CarProperties>().ObjectNumber == num))
					{
						foreach (transparents transparents in gameObject3.GetComponentsInChildren<transparents>())
						{
							if (transparents.transform.name == gameObject2.name && transparents.SavePosition == gameObject2.GetComponent<CarProperties>().SavePosition)
							{
								gameObject2.transform.SetParent(transparents.transform);
							}
							gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
						}
					}
				}
				if (gameObject2.transform.parent && gameObject2.GetComponent<CarProperties>() && !gameObject2.GetComponent<CarProperties>().DMGdisplacepart)
				{
					gameObject2.transform.position = gameObject2.transform.parent.position;
					gameObject2.transform.rotation = gameObject2.transform.parent.rotation;
				}
				else if (gameObject2.transform.parent)
				{
					gameObject2.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null);
					gameObject2.transform.localRotation = (Quaternion)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "rotation", null);
				}
				else
				{
					gameObject2.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null) + this.MainSaver.transform.root.transform.position;
					gameObject2.transform.localRotation = (Quaternion)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "rotation", null);
				}
				if (gameObject2.transform.localPosition.y < -100f || gameObject2.transform.localPosition.y > 1000f)
				{
					UnityEngine.Object.Destroy(gameObject2.gameObject);
				}
			}
			foreach (GameObject gameObject4 in this.goList)
			{
				if (gameObject4.GetComponent<MainCarProperties>())
				{
					gameObject4.GetComponent<MainCarProperties>().InBarn = false;
				}
				if (gameObject4.GetComponent<CarProperties>())
				{
					gameObject4.GetComponent<CarProperties>().InBarn = false;
				}
				if (gameObject4.GetComponent<MainTrailerProperties>())
				{
					gameObject4.GetComponent<MainTrailerProperties>().InBarn = false;
				}
			}
		}
		ModLoader.OnSaveSystemLoad(saveSystem, true);
		ModLoader.OnBarnLoad();
	}

	// Token: 0x04001380 RID: 4992
	public Saver MainSaver;

	// Token: 0x04001381 RID: 4993
	public int ObjectNumber;

	// Token: 0x04001382 RID: 4994
	public int CurrentNumber;

	// Token: 0x04001383 RID: 4995
	public OpenGarage Door;

	// Token: 0x04001384 RID: 4996
	public string SavePath;

	// Token: 0x04001385 RID: 4997
	public int SaveSlot;

	// Token: 0x04001386 RID: 4998
	public int OpenSaveSlot;

	// Token: 0x04001387 RID: 4999
	public List<GameObject> goList;

	// Token: 0x04001388 RID: 5000
	private SaveSystem save;
}
