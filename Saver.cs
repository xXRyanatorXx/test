using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.SimpleZip;
using Den.Tools;
using MapMagic.Core;
using MapMagic.Terrains;
using Newtonsoft.Json;
using PaintIn3D;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020001DC RID: 476
public class Saver : MonoBehaviour
{
	// Token: 0x06000B22 RID: 2850 RVA: 0x00075669 File Offset: 0x00073869
	private void Awake()
	{
		this.AwakeContinue(false);
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x00075674 File Offset: 0x00073874
	public void AwakeContinue(bool MultiPlayer)
	{
		this.MULTIPLAYERrunningMapMagic = MultiPlayer;
		if (this.GeneratingWorldText)
		{
			this.GeneratingWorldText.SetActive(true);
		}
		if (this.LoadingScreen)
		{
			this.LoadingScreen.SetActive(true);
		}
		if (this.mapMagic)
		{
			TerrainTile.OnAllComplete = (Action<MapMagicObject>)Delegate.Combine(TerrainTile.OnAllComplete, new Action<MapMagicObject>(this.WorldLoaded));
		}
		this.Player = GameObject.Find("Player");
		if (!File.Exists(Application.persistentDataPath + "/Radios_user.txt"))
		{
			this.xmlAsset = (TextAsset)Resources.Load("Radios_user");
			this.xmlContent = this.xmlAsset.text;
			File.WriteAllText(Application.persistentDataPath + "/Radios_user.txt", this.xmlContent);
		}
		else if (new FileInfo(Application.persistentDataPath + "/Radios_user.txt").Length < 550L)
		{
			this.xmlAsset = (TextAsset)Resources.Load("Radios_user");
			this.xmlContent = this.xmlAsset.text;
			File.WriteAllText(Application.persistentDataPath + "/Radios_user.txt", this.xmlContent);
		}
		this.Loaded = false;
		this.goList = new List<GameObject>();
		if (this.mapMagic)
		{
			this.SavePath = this.SaveFolder + "/save.dat";
			this.BackupPath = this.SaveFolder;
		}
		else
		{
			if (PlayerPrefs.HasKey("GameNumber") && PlayerPrefs.GetFloat("GameNumber") > 0f)
			{
				if (PlayerPrefs.GetFloat("GameNumber") == 1f)
				{
					this.SaveFolder = "\\save1";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 2f)
				{
					this.SaveFolder = "\\save3";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 3f)
				{
					this.SaveFolder = "\\save4";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 4f)
				{
					this.SaveFolder = "\\save5";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 5f)
				{
					this.SaveFolder = "\\save6";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 6f)
				{
					this.SaveFolder = "\\save7";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 7f)
				{
					this.SaveFolder = "\\save8";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 8f)
				{
					this.SaveFolder = "\\save9";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 9f)
				{
					this.SaveFolder = "\\save10";
				}
				if (PlayerPrefs.GetFloat("GameNumber") == 10f)
				{
					this.SaveFolder = "\\save11";
				}
			}
			this.BackupPath = this.SaveFolder;
			this.SavePath = this.SaveFolder + "/save.dat";
		}
		if (!Directory.Exists(Application.persistentDataPath + "/" + this.SaveFolder))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/" + this.SaveFolder);
		}
		P3dHelper.SaveFolder = this.SaveFolder;
		tools.Needs = 0;
		if (PlayerPrefs.HasKey("Needs") && PlayerPrefs.GetFloat("Needs") == 1f)
		{
			tools.Needs = 1;
			tools.Food = 1f;
			tools.Drink = 1f;
			tools.Sleep = 1f;
			tools.Health = 1f;
			this.Player.GetComponent<tools>().NeedsCanvas.SetActive(true);
		}
		ModLoader.OnLoad();
		if (this.mapMagic && !MultiPlayer)
		{
			if (File.Exists(Application.persistentDataPath + this.SavePath) && PlayerPrefs.GetFloat("LoadLevel") == 1f)
			{
				this.save = new SaveSystem(Application.persistentDataPath + this.SavePath);
				this.save.read();
				this.Seed = (int)this.save.get("Seed", 0);
				this.mapMagic.graph.random = new Noise(this.Seed, 16384);
				this.mapMagic.transform.position = (Vector3)this.save.get("MapMagicPosition", null);
				this.Player.transform.position = (Vector3)this.save.get("PlayerPosition", null);
				return;
			}
			this.Seed = PlayerPrefs.GetInt("MapSeed", 0);
			if (this.Seed == 0)
			{
				this.Seed = UnityEngine.Random.Range(0, 9999999);
			}
			this.mapMagic.graph.random = new Noise(this.Seed, 16384);
			return;
		}
		else
		{
			if (this.mapMagic && MultiPlayer)
			{
				this.save = new SaveSystem(Application.persistentDataPath + "/Multiplayer/save.dat");
				this.save.read();
				this.Seed = (int)this.save.get("Seed", 0);
				this.mapMagic.graph.random = new Noise(this.Seed, 16384);
				this.mapMagic.Refresh(true);
				this.mapMagic.transform.position = (Vector3)this.save.get("MapMagicPosition", null);
				this.Player.transform.position = (Vector3)this.save.get("PlayerPosition", null);
				base.StartCoroutine(this.WaitLoad5());
				return;
			}
			if (PlayerPrefs.GetFloat("LoadLevel") == 1f && !this.mapMagic && File.Exists(Application.persistentDataPath + this.SavePath))
			{
				this.save = new SaveSystem(Application.persistentDataPath + this.SavePath);
				this.save.read();
				this.Player.transform.position = (Vector3)this.save.get("PlayerPosition", this.Player.transform.localPosition);
				this.Player.GetComponent<FirstPersonAIO>().ControllerPause();
				base.StartCoroutine(this.WaitLoad(MultiPlayer));
				return;
			}
			base.StartCoroutine(this.WaitLoaded());
			return;
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x00075CEF File Offset: 0x00073EEF
	private IEnumerator WaitLoaded()
	{
		yield return 0;
		tools.GameLoaded = true;
		yield break;
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x00075CF7 File Offset: 0x00073EF7
	public void MenuQuit()
	{
		this.Save(true, false, false);
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00075D02 File Offset: 0x00073F02
	public void Quicksave()
	{
		this.Save(false, false, false);
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x00075D10 File Offset: 0x00073F10
	private void WorldLoaded(MapMagicObject obj)
	{
		Debug.Log("save - worldLoaded");
		tools.TerrainsLoaded = true;
		if (this.GeneratingWorldText)
		{
			this.GeneratingWorldText.SetActive(false);
		}
		if (this.LoadingPlayerText)
		{
			this.LoadingPlayerText.SetActive(true);
		}
		if (!this.Loaded)
		{
			if (PlayerPrefs.GetFloat("LoadLevel") == 1f || this.MULTIPLAYERrunningMapMagic)
			{
				this.Load(this.MULTIPLAYERrunningMapMagic);
			}
			else
			{
				this.Player.GetComponent<SetStart>().RestartPlayer();
				this.Loaded = true;
				if (this.LoadingScreen)
				{
					this.LoadingScreen.SetActive(false);
				}
				tools.GameLoaded = true;
			}
			if (this.DWall)
			{
				this.DWall.StartThis();
			}
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00075DDC File Offset: 0x00073FDC
	public void Save(bool quit, bool ClearBarn, bool MultiPlayer)
	{
		ModLoader.OnSave();
		if (this.Barn && this.Barn.Door.Open)
		{
			if (ClearBarn)
			{
				this.Barn.Save(true);
			}
			else
			{
				this.Barn.Save(false);
			}
		}
		this.ObjectNumber = 0;
		SaveSystem saveSystem;
		if (MultiPlayer)
		{
			saveSystem = new SaveSystem(Application.persistentDataPath + "/Multiplayer/save.dat");
		}
		else
		{
			saveSystem = new SaveSystem(Application.persistentDataPath + this.SavePath);
		}
		if (this.SaveAllInside)
		{
			Collider[] array = Physics.OverlapBox(this.SaveAllInside.transform.position, this.SaveAllInside.transform.localScale / 2f, Quaternion.identity);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name == "Collectible" && !array[i].gameObject.GetComponent<SaveItem>())
				{
					array[i].gameObject.AddComponent<SaveItem>();
					array[i].gameObject.GetComponent<SaveItem>().PrefabName = array[i].gameObject.GetComponent<MooveItem>().PrefabName;
				}
			}
		}
		if (this.BarnFind != null)
		{
			saveSystem.add("BarnfindPosition", this.BarnFind.transform.position - base.transform.root.transform.position);
			saveSystem.add("BarnfindRotation", this.BarnFind.transform.rotation);
			saveSystem.add("BarnFindSeed", this.BarnFind.GetComponent<SetStart>().barncarspawner.Seed);
			saveSystem.add("BarnFindspawned", this.BarnFind.GetComponent<SetStart>().spawned);
		}
		if (this.Garage1)
		{
			saveSystem.add("Garage1", this.Garage1.activeSelf);
		}
		if (this.Garage2)
		{
			saveSystem.add("Garage2", this.Garage2.activeSelf);
		}
		if (this.GarageNO)
		{
			saveSystem.add("GarageNO", this.GarageNO.activeSelf);
		}
		if (this.GarageYes)
		{
			saveSystem.add("GarageYes", this.GarageYes.activeSelf);
		}
		if (this.JM)
		{
			saveSystem.add("JOBSStartValue", this.JM.StartValue);
			saveSystem.add("JOBSPartcount", this.JM.Partcount);
			saveSystem.add("AllBolts", this.JM.AllBolts);
			saveSystem.add("AllWelds", this.JM.AllWelds);
			saveSystem.add("JOBSRealreward", this.JM.Realreward);
			if (this.JM.ActiveJob)
			{
				saveSystem.add("JOBSActiveJob", this.JM.ActiveJob.transform.name);
			}
		}
		saveSystem.add("NCL1", this.NC.L1);
		saveSystem.add("NCL2", this.NC.L2);
		saveSystem.add("NCL3", this.NC.L3);
		saveSystem.add("NCL4", this.NC.L4);
		saveSystem.add("NCL5", this.NC.L5);
		saveSystem.add("NCL6", this.NC.L6);
		saveSystem.add("NCL7", this.NC.L7);
		saveSystem.add("NCL8", this.NC.L8);
		saveSystem.add("NCL9", this.NC.L9);
		saveSystem.add("NCL10", this.NC.L10);
		saveSystem.add("NCL11", this.NC.L11);
		saveSystem.add("NCL12", this.NC.L12);
		saveSystem.add("NCL13", this.NC.L13);
		saveSystem.add("NCL14", this.NC.L14);
		saveSystem.add("NCL15", this.NC.L15);
		saveSystem.add("NCL16", this.NC.L16);
		saveSystem.add("NCL17", this.NC.L17);
		saveSystem.add("NCL18", this.NC.L18);
		saveSystem.add("NCL19", this.NC.L19);
		saveSystem.add("NCL20", this.NC.L20);
		if (PlayerPrefs.HasKey("DeathWall") && PlayerPrefs.GetFloat("DeathWall") == 1f)
		{
			saveSystem.add("DeathWall", 1);
		}
		else
		{
			saveSystem.add("DeathWall", 0);
		}
		saveSystem.add("Needs", tools.Needs);
		saveSystem.add("Food", tools.Food);
		saveSystem.add("Drink", tools.Drink);
		saveSystem.add("Sleep", tools.Sleep);
		saveSystem.add("Health", tools.Health);
		saveSystem.add("Money", tools.money);
		saveSystem.add("SavedVersion", Application.version.ToString());
		saveSystem.add("PizzaDeliveriesDay", tools.PizzaDeliveriesDay);
		foreach (GameObject gameObject in this.Player.GetComponent<tools>().BackpackParts)
		{
			gameObject.transform.position = new Vector3(0f, 45f, 0f);
		}
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform.name == "Player")
			{
				if (this.mapMagic)
				{
					saveSystem.add("Seed", this.Seed);
					saveSystem.add("MapMagicPosition", this.mapMagic.transform.position);
				}
				if (this.mapMagic)
				{
					saveSystem.add("PlayerPosition", transform.position);
				}
				else
				{
					saveSystem.add("PlayerPosition", transform.position - base.transform.root.transform.position);
				}
				saveSystem.add("PlayerRotation", transform.rotation);
				saveSystem.add("PlayerStarted", transform.GetComponent<tools>().Started);
				saveSystem.add("snowAmount", transform.GetComponent<tools>().snowAmount);
				saveSystem.add("day", EnviroSkyMgr.instance.Time.Days);
				saveSystem.add("Hours", EnviroSkyMgr.instance.Time.Hours);
				saveSystem.add("Minutes", EnviroSkyMgr.instance.Time.Minutes);
				saveSystem.add("StartPosition", transform.GetComponent<tools>().StartPosition);
			}
			if (!MultiPlayer || transform.gameObject.GetComponent<MPobject>())
			{
				if (transform.gameObject.GetComponent<SavePosition>() && transform.position.y > 45f)
				{
					if (transform.transform.parent || this.mapMagic)
					{
						saveSystem.add("PropPosition" + transform.gameObject.GetComponent<SavePosition>().SceneNumber.ToString(), transform.localPosition);
					}
					else
					{
						saveSystem.add("PropPosition" + transform.gameObject.GetComponent<SavePosition>().SceneNumber.ToString(), transform.position - base.transform.root.transform.position);
					}
					if (!transform.gameObject.GetComponent<SavePosition>().DontSaveRotation)
					{
						saveSystem.add("PropRotation" + transform.gameObject.GetComponent<SavePosition>().SceneNumber.ToString(), transform.localRotation);
					}
				}
				if (transform.gameObject.GetComponent<MainCarProperties>() && !transform.gameObject.GetComponent<MainCarProperties>().InBarn && (transform.gameObject.GetComponent<MainCarProperties>().Owner == "Player" || transform.gameObject.GetComponent<MainCarProperties>().Owner == "Client" || MultiPlayer))
				{
					this.ObjectNumber++;
					transform.gameObject.GetComponent<MainCarProperties>().ObjectNumber = this.ObjectNumber;
					if (transform.gameObject.GetComponent<MPobject>())
					{
						saveSystem.add("ObjectNr" + this.ObjectNumber.ToString() + "MPNumber", transform.gameObject.GetComponent<MPobject>().MPNumber);
					}
				}
				if (transform.gameObject.GetComponent<MainTrailerProperties>() && !transform.gameObject.GetComponent<MainTrailerProperties>().InBarn)
				{
					this.ObjectNumber++;
					transform.gameObject.GetComponent<MainTrailerProperties>().ObjectNumber = this.ObjectNumber;
					if (transform.gameObject.GetComponent<MPobject>())
					{
						saveSystem.add("ObjectNr" + this.ObjectNumber.ToString() + "MPNumber", transform.gameObject.GetComponent<MPobject>().MPNumber);
					}
				}
				if (transform.gameObject.GetComponent<CarProperties>() && !transform.gameObject.GetComponent<CarProperties>().InBarn && transform.gameObject.GetComponent<CarProperties>().PREFAB && (transform.gameObject.GetComponent<CarProperties>().Owner == "Player" || transform.gameObject.GetComponent<CarProperties>().Owner == "Client" || MultiPlayer) && (!transform.parent || !transform.parent.GetComponent<SaveItem>() || transform.parent.GetComponent<OpenableBox>()))
				{
					this.ObjectNumber++;
					transform.gameObject.GetComponent<CarProperties>().ObjectNumber = this.ObjectNumber;
					transform.gameObject.GetComponent<CarProperties>().SavingGame();
					if (transform.gameObject.GetComponent<MPobject>())
					{
						saveSystem.add("ObjectNr" + this.ObjectNumber.ToString() + "MPNumber", transform.gameObject.GetComponent<MPobject>().MPNumber);
					}
				}
				if (transform.gameObject.GetComponent<SaveItem>() && (!transform.parent || (transform.parent.name != "DiscBox" && transform.parent.name != "electrodebox")) && !transform.gameObject.GetComponent<SaveItem>().InBarn)
				{
					this.ObjectNumber++;
					transform.gameObject.GetComponent<SaveItem>().ObjectNumber = this.ObjectNumber;
					if (transform.gameObject.GetComponent<MPobject>())
					{
						saveSystem.add("ObjectNr" + this.ObjectNumber.ToString() + "MPNumber", transform.gameObject.GetComponent<MPobject>().MPNumber);
					}
					if (transform.gameObject.GetComponent<DeliveryTarget>())
					{
						saveSystem.add("ObjectNr" + this.ObjectNumber.ToString() + "Accepted", transform.gameObject.GetComponent<DeliveryTarget>().Accepted);
					}
					if (transform.gameObject.GetComponent<DeliveryTarget>())
					{
						saveSystem.add("ObjectNr" + this.ObjectNumber.ToString() + "BarnGiver", transform.gameObject.GetComponent<DeliveryTarget>().BarnGiver);
					}
					if (transform.gameObject.GetComponent<SaveItem>().pickuptool)
					{
						saveSystem.add("ObjectNr" + this.ObjectNumber.ToString() + "paintlife", transform.gameObject.GetComponent<SaveItem>().pickuptool.paintlife);
					}
				}
			}
		}
		foreach (Transform transform2 in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform2.position.y > 40f && transform2.position.x < 20000f && transform2.position.x > -20000f && transform2.position.z < 20000f && transform2.position.z > -20000f && (!MultiPlayer || transform2.gameObject.GetComponent<MPobject>()))
			{
				if (transform2.gameObject.GetComponent<MainCarProperties>() && !transform2.gameObject.GetComponent<MainCarProperties>().InBarn && (transform2.gameObject.GetComponent<MainCarProperties>().Owner == "Player" || transform2.gameObject.GetComponent<MainCarProperties>().Owner == "Client" || MultiPlayer))
				{
					MainCarProperties component = transform2.gameObject.GetComponent<MainCarProperties>();
					this.CurrentNumber = component.ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component.PREFAB.transform.name.ToString());
					if (this.mapMagic)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position - base.transform.root.transform.position);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", transform2.rotation);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Owner", component.Owner);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "OriginalColor", component.OriginalColor);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Color", component.Color);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Started", component.Started);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "CarPrice", component.CarPrice);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mileage", component.Mileage);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "OriginalInteriorColor", component.OriginalInteriorColor);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "HP", component.HP);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedEngineParts", component.RuinedEngineParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornEngineParts", component.WornEngineParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedSuspensionParts", component.RuinedSuspensionParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornSuspensionParts", component.WornSuspensionParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedBrakeParts", component.RuinedBrakeParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornBrakeParts", component.WornBrakeParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "RuinedOtherParts", component.RuinedOtherParts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "WornOtherparts", component.WornOtherparts);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "CantCrank", component.CantCrank);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "CantRun", component.CantRun);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "BrakeProblems", component.BrakeProblems);
					saveSystem.add("Parent" + this.CurrentNumber.ToString(), 0);
				}
				if (transform2.gameObject.GetComponent<MainTrailerProperties>() && !transform2.gameObject.GetComponent<MainTrailerProperties>().InBarn)
				{
					this.CurrentNumber = transform2.gameObject.GetComponent<MainTrailerProperties>().ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<MainTrailerProperties>().PrefabName);
					if (this.mapMagic)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position - base.transform.root.transform.position);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", transform2.rotation);
				}
				if (transform2.gameObject.GetComponent<CarProperties>() && !transform2.gameObject.GetComponent<CarProperties>().InBarn && transform2.gameObject.GetComponent<CarProperties>().PREFAB && (transform2.gameObject.GetComponent<CarProperties>().Owner == "Player" || transform2.gameObject.GetComponent<CarProperties>().Owner == "Client" || MultiPlayer))
				{
					CarProperties component2 = transform2.gameObject.GetComponent<CarProperties>();
					this.CurrentNumber = component2.ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component2.PrefabName);
					if (transform2.transform.parent)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.localPosition);
					}
					else if (this.mapMagic)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position - base.transform.root.transform.position);
					}
					if ((this.mapMagic && transform2.root.name == "EngineStand") || transform2.root.GetComponent<OpenableBox>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position);
					}
					else if (transform2.root.name == "EngineStand" || transform2.root.name == "BikeEngineStand" || transform2.root.GetComponent<OpenableBox>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position - base.transform.root.transform.position);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", transform2.localRotation);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Owner", component2.Owner);
					if (transform2.parent && transform2.parent.parent && transform2.parent.parent.GetComponent<CarProperties>() && transform2.parent.parent.GetComponent<CarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), transform2.parent.parent.GetComponent<CarProperties>().ObjectNumber);
					}
					else if (transform2.parent && transform2.parent.parent && transform2.parent.parent.GetComponent<MainCarProperties>() && transform2.parent.parent.GetComponent<MainCarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), transform2.parent.parent.GetComponent<MainCarProperties>().ObjectNumber);
					}
					else if (transform2.parent && transform2.parent.parent && transform2.parent.parent.parent && transform2.parent.parent.parent.GetComponent<CarProperties>() && transform2.parent.parent.parent.GetComponent<CarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), transform2.parent.parent.parent.GetComponent<CarProperties>().ObjectNumber);
					}
					else if (transform2.parent && transform2.parent.parent && transform2.parent.parent.parent && transform2.parent.parent.parent.GetComponent<MainCarProperties>() && transform2.parent.parent.parent.GetComponent<MainCarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), transform2.parent.parent.parent.GetComponent<MainCarProperties>().ObjectNumber);
					}
					else if (transform2.root.GetComponent<MainTrailerProperties>())
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), transform2.root.GetComponent<MainTrailerProperties>().ObjectNumber);
					}
					else
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), 0);
					}
					string str = "detala+ parents";
					string str2 = this.CurrentNumber.ToString();
					object obj = saveSystem.get("Parent" + this.CurrentNumber.ToString(), null);
					Debug.Log(str + str2 + ((obj != null) ? obj.ToString() : null));
					saveSystem.add("started" + this.CurrentNumber.ToString(), component2.started);
					if (component2.BikeStand)
					{
						saveSystem.add("BikeStandHeight" + this.CurrentNumber.ToString(), component2.BikeStand.steps);
					}
					if (component2.ClusterMileage > 0f)
					{
						saveSystem.add("ClusterMileage" + this.CurrentNumber.ToString(), component2.ClusterMileage);
					}
					saveSystem.add("FluidSize" + this.CurrentNumber.ToString(), component2.FluidSize);
					saveSystem.add("DieselPercent" + this.CurrentNumber.ToString(), component2.DieselPercent);
					saveSystem.add("FluidCondition" + this.CurrentNumber.ToString(), component2.FluidCondition);
					saveSystem.add("TirePressure" + this.CurrentNumber.ToString(), component2.TirePressure);
					saveSystem.add("Condition" + this.CurrentNumber.ToString(), component2.Condition);
					saveSystem.add("Chromed" + this.CurrentNumber.ToString(), component2.Chromed);
					saveSystem.add("SavePosition" + this.CurrentNumber.ToString(), component2.SavePosition);
					if (component2.coilnut)
					{
						saveSystem.add("coilnut" + this.CurrentNumber.ToString(), component2.coilnut.height);
					}
					if (this.compress)
					{
						if (component2.Texture1 != "")
						{
							string data = Zip.CompressToString(component2.Texture1);
							saveSystem.add("Texture1" + this.CurrentNumber.ToString(), data);
						}
						if (component2.Texture3 != "")
						{
							string data2 = Zip.CompressToString(component2.Texture3);
							saveSystem.add("Texture3" + this.CurrentNumber.ToString(), data2);
						}
						if (component2.Texture4 != "")
						{
							string data3 = Zip.CompressToString(component2.Texture4);
							saveSystem.add("Texture4" + this.CurrentNumber.ToString(), data3);
						}
					}
					else
					{
						saveSystem.add("Texture1" + this.CurrentNumber.ToString(), component2.Texture1);
						saveSystem.add("Texture3" + this.CurrentNumber.ToString(), component2.Texture3);
						saveSystem.add("Texture4" + this.CurrentNumber.ToString(), component2.Texture4);
					}
					saveSystem.add("BodyMatNumber" + this.CurrentNumber.ToString(), component2.BodyMatNumber);
					saveSystem.add("OriginalInterior" + this.CurrentNumber.ToString(), component2.OriginalInterior);
					saveSystem.add("TintLevel" + this.CurrentNumber.ToString(), component2.TintLevel);
					saveSystem.add("Ruined" + this.CurrentNumber.ToString(), component2.Ruined);
					saveSystem.add("Damaged" + this.CurrentNumber.ToString(), component2.Damaged);
					saveSystem.add("PartIsOld" + this.CurrentNumber.ToString(), component2.PartIsOld);
					saveSystem.add("MeshDamaged" + this.CurrentNumber.ToString(), component2.MeshDamaged);
					saveSystem.add("MeshLittleDamaged" + this.CurrentNumber.ToString(), component2.MeshLittleDamaged);
					saveSystem.add("Damagedvertices" + this.CurrentNumber.ToString(), component2.Damagedvertices);
					if (component2.ChildDamag && component2.ChildDamag.MeshLittleDamaged)
					{
						saveSystem.add("RuinedCH" + this.CurrentNumber.ToString(), component2.ChildDamag.Ruined);
						saveSystem.add("MeshDamagedCH" + this.CurrentNumber.ToString(), component2.ChildDamag.MeshDamaged);
						saveSystem.add("MeshLittleDamagedCH" + this.CurrentNumber.ToString(), component2.ChildDamag.MeshLittleDamaged);
						saveSystem.add("DamagedverticesCH" + this.CurrentNumber.ToString(), component2.ChildDamag.GetComponent<MeshFilter>().mesh.vertices);
					}
					saveSystem.add("BackPack" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<Partinfo>().InBackpack);
					saveSystem.add("Loose" + this.CurrentNumber.ToString(), component2.Loose);
					if (transform2.gameObject.transform.parent)
					{
						saveSystem.add("tightnuts" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<Partinfo>().tightnuts);
					}
					if (component2.NumberPlate)
					{
						saveSystem.add("One" + this.CurrentNumber.ToString(), component2.One.name.ToString());
						saveSystem.add("Two" + this.CurrentNumber.ToString(), component2.Two.name.ToString());
						saveSystem.add("Three" + this.CurrentNumber.ToString(), component2.Three.name.ToString());
						saveSystem.add("Four" + this.CurrentNumber.ToString(), component2.Four.name.ToString());
						saveSystem.add("Five" + this.CurrentNumber.ToString(), component2.Five.name.ToString());
						saveSystem.add("Six" + this.CurrentNumber.ToString(), component2.Six.name.ToString());
					}
				}
				if (transform2.gameObject.tag != "Building" && transform2.gameObject.GetComponent<SaveItem>() && (!transform2.parent || (transform2.parent && transform2.parent.name != "DiscBox" && transform2.parent.name != "electrodebox")) && !transform2.gameObject.GetComponent<SaveItem>().InBarn)
				{
					this.CurrentNumber = transform2.gameObject.GetComponent<SaveItem>().ObjectNumber;
					if (transform2.gameObject.GetComponent<SaveItem>().PrefabName != "" && transform2.gameObject.GetComponent<SaveItem>().PrefabName != null)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<SaveItem>().PrefabName);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.name);
					}
					if (this.mapMagic)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position - base.transform.root.transform.position);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", transform2.rotation);
					if (transform2.gameObject.GetComponent<PickupTool>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "childCount", transform2.gameObject.GetComponent<PickupTool>().InBox);
					}
					if (transform2.GetComponent<PickupTool>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "FluidSize", transform2.GetComponent<PickupTool>().FluidSize);
					}
					if (transform2.GetComponent<PickupTool>() && transform2.GetComponent<PickupTool>().NestedFluid)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "NestedFluid", transform2.GetComponent<PickupTool>().NestedFluid.FluidSize);
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "NestedFluidDieselPercent", transform2.GetComponent<PickupTool>().NestedFluid.DieselPercent);
					}
					if (transform2.GetComponent<PickupTool>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "paintlife", transform2.GetComponent<PickupTool>().paintlife);
					}
					if (transform2.GetComponent<PickupTool>())
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Color", transform2.GetComponent<PickupTool>().colorpaint);
					}
					if (transform2.GetComponent<PickupItems>() && transform2.GetComponent<PickupItems>().Condition > 0f)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Condition", transform2.GetComponent<PickupItems>().Condition);
					}
					if (transform2.GetComponent<SaveItem>().Handle != null)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "HandleHeight", transform2.GetComponent<SaveItem>().Handle.steps);
					}
					if (transform2.GetComponent<PickupTool>() && transform2.GetComponent<PickupTool>().material != null)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "material", transform2.GetComponent<PickupTool>().material.name);
					}
					saveSystem.add("BackPack" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<SaveItem>().InBackpack);
				}
				if (transform2.gameObject.tag == "Building" && transform2.gameObject.GetComponent<SaveItem>())
				{
					this.CurrentNumber = transform2.gameObject.GetComponent<SaveItem>().ObjectNumber;
					saveSystem.add("Parent" + this.CurrentNumber.ToString(), transform2.parent.GetComponent<SaveItem>().ObjectNumber);
					if (transform2.gameObject.GetComponent<SaveItem>().PrefabName != "" && transform2.gameObject.GetComponent<SaveItem>().PrefabName != null)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<SaveItem>().PrefabName);
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.name);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.localPosition);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", transform2.localRotation);
					if (transform2.gameObject.GetComponent<SaveItem>().ChildrenRend != null)
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "ChildrenRend", transform2.gameObject.GetComponent<SaveItem>().ChildrenRend.sharedMaterials[0].name);
					}
					if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials[0].name.EndsWith("nce)"))
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat0", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[0].name.Replace(" (Instance)", ""));
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat0", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[0].name);
					}
					if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials.Length > 1)
					{
						if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials[1].name.EndsWith("nce)"))
						{
							saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat1", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[1].name.Replace(" (Instance)", ""));
						}
						else
						{
							saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat1", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[1].name);
						}
					}
					if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials.Length > 2)
					{
						if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials[2].name.EndsWith("nce)"))
						{
							saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat2", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[2].name.Replace(" (Instance)", ""));
						}
						else
						{
							saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat2", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[2].name);
						}
					}
				}
			}
		}
		if (MultiPlayer)
		{
			SimplifiedMods simplifiedMods = new SimplifiedMods();
			foreach (Mod mod in ModLoader.mods)
			{
				SimplifiedModObject item = new SimplifiedModObject(mod.ID, mod.Name, mod.Version);
				simplifiedMods.Mods.Add(item);
			}
			saveSystem.add("MultiplayerModList", JsonConvert.SerializeObject(simplifiedMods));
		}
		saveSystem.add("Objects", this.ObjectNumber);
		ModLoader.OnSaveSystemSave(saveSystem, false);
		if (File.Exists(Application.persistentDataPath + this.SavePath) && !MultiPlayer)
		{
			if (File.Exists(Application.persistentDataPath + this.BackupPath + "/saveBackupOlder.dat"))
			{
				File.Delete(Application.persistentDataPath + this.BackupPath + "/saveBackupOlder.dat");
			}
			if (File.Exists(Application.persistentDataPath + this.BackupPath + "/saveBackup.dat"))
			{
				File.Move(Application.persistentDataPath + this.BackupPath + "/saveBackup.dat", Application.persistentDataPath + this.BackupPath + "/saveBackupOlder.dat");
			}
			if (File.Exists(Application.persistentDataPath + this.BackupPath + "/save.dat"))
			{
				File.Move(Application.persistentDataPath + this.BackupPath + "/save.dat", Application.persistentDataPath + this.BackupPath + "/saveBackup.dat");
			}
		}
		saveSystem.write();
		ModLoader.OnSaveFinish();
		if (quit)
		{
			this.Quit();
		}
		else
		{
			foreach (Transform transform3 in UnityEngine.Object.FindObjectsOfType<Transform>())
			{
				if (transform3.gameObject.GetComponent<MainCarProperties>())
				{
					transform3.gameObject.GetComponent<MainCarProperties>().InBarn = false;
				}
				if (transform3.gameObject.GetComponent<MainTrailerProperties>())
				{
					transform3.gameObject.GetComponent<MainTrailerProperties>().InBarn = false;
				}
				if (transform3.gameObject.GetComponent<CarProperties>())
				{
					transform3.gameObject.GetComponent<CarProperties>().InBarn = false;
				}
			}
		}
		if (GameObject.Find("Player").GetComponent<FloatingPoinMyfix>())
		{
			GameObject.Find("Player").GetComponent<FloatingPoinMyfix>().ResetPosition();
		}
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00078984 File Offset: 0x00076B84
	private IEnumerator WaitLoad(bool MultiPlayer)
	{
		yield return 0;
		yield return 0;
		this.Load(MultiPlayer);
		yield break;
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x0007899A File Offset: 0x00076B9A
	private IEnumerator WaitLoad5()
	{
		GameObject.Find("Player").GetComponent<Rigidbody>().isKinematic = true;
		yield return new WaitForSeconds(8f);
		tools.TerrainsLoaded = true;
		if (this.GeneratingWorldText)
		{
			this.GeneratingWorldText.SetActive(false);
		}
		if (this.LoadingPlayerText)
		{
			this.LoadingPlayerText.SetActive(true);
		}
		if (!this.Loaded)
		{
			if (PlayerPrefs.GetFloat("LoadLevel") == 1f || this.MULTIPLAYERrunningMapMagic)
			{
				this.Load(this.MULTIPLAYERrunningMapMagic);
			}
			else
			{
				this.Player.GetComponent<SetStart>().RestartPlayer();
				this.Loaded = true;
				if (this.LoadingScreen)
				{
					this.LoadingScreen.SetActive(false);
				}
				tools.GameLoaded = true;
			}
			if (this.DWall)
			{
				this.DWall.StartThis();
			}
		}
		yield break;
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x000789AC File Offset: 0x00076BAC
	public void SaveBuildings()
	{
		this.ObjectNumber = 0;
		SaveSystem saveSystem = new SaveSystem(Application.persistentDataPath + this.SaveFolder + "/Buildings.dat");
		Transform[] array = UnityEngine.Object.FindObjectsOfType<Transform>();
		foreach (Transform transform in array)
		{
			if (transform.gameObject.tag == "Building" || transform.name == "BuildingSite")
			{
				this.ObjectNumber++;
				transform.gameObject.GetComponent<SaveItem>().ObjectNumber = this.ObjectNumber;
			}
		}
		foreach (Transform transform2 in array)
		{
			if (transform2.name == "BuildingSite")
			{
				this.CurrentNumber = transform2.gameObject.GetComponent<SaveItem>().ObjectNumber;
				if (transform2.gameObject.GetComponent<SaveItem>().PrefabName != "" && transform2.gameObject.GetComponent<SaveItem>().PrefabName != null)
				{
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<SaveItem>().PrefabName);
				}
				else
				{
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.name);
				}
				saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.position - base.transform.root.transform.position);
				saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", transform2.rotation);
			}
			if (transform2.gameObject.tag == "Building" && transform2.gameObject.GetComponent<SaveItem>())
			{
				this.CurrentNumber = transform2.gameObject.GetComponent<SaveItem>().ObjectNumber;
				saveSystem.add("Parent" + this.CurrentNumber.ToString(), transform2.parent.GetComponent<SaveItem>().ObjectNumber);
				if (transform2.gameObject.GetComponent<SaveItem>().PrefabName != "" && transform2.gameObject.GetComponent<SaveItem>().PrefabName != null)
				{
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.gameObject.GetComponent<SaveItem>().PrefabName);
				}
				else
				{
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), transform2.name);
				}
				saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", transform2.localPosition);
				saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", transform2.localRotation);
				if (transform2.gameObject.GetComponent<SaveItem>().ChildrenRend != null)
				{
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "ChildrenRend", transform2.gameObject.GetComponent<SaveItem>().ChildrenRend.sharedMaterials[0].name);
				}
				if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials[0].name.EndsWith("nce)"))
				{
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat0", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[0].name.Replace(" (Instance)", ""));
				}
				else
				{
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat0", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[0].name);
				}
				if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials.Length > 1)
				{
					if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials[1].name.EndsWith("nce)"))
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat1", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[1].name.Replace(" (Instance)", ""));
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat1", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[1].name);
					}
				}
				if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials.Length > 2)
				{
					if (transform2.gameObject.GetComponent<Renderer>().sharedMaterials[2].name.EndsWith("nce)"))
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat2", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[2].name.Replace(" (Instance)", ""));
					}
					else
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Mat2", transform2.gameObject.GetComponent<Renderer>().sharedMaterials[2].name);
					}
				}
			}
		}
		saveSystem.add("Objects", this.ObjectNumber);
		saveSystem.write();
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00078F68 File Offset: 0x00077168
	public void Load(bool MultiPlayer)
	{
		Debug.Log("save - LoadStarted");
		this.Loaded = true;
		this.goList.Clear();
		SaveSystem saveSystem;
		string path;
		if (MultiPlayer)
		{
			saveSystem = new SaveSystem(Application.persistentDataPath + "/Multiplayer/save.dat");
			path = Application.persistentDataPath + "/Multiplayer/save.dat";
		}
		else
		{
			saveSystem = new SaveSystem(Application.persistentDataPath + this.SavePath);
			path = Application.persistentDataPath + this.SavePath;
		}
		if (File.Exists(path))
		{
			cachedResources.InitializeResources();
			saveSystem.read();
			GameObject[] array = new GameObject[(int)saveSystem.get("Objects", 0) + 10];
			if (!MultiPlayer || this.mapMagic)
			{
				this.Player.transform.position = (Vector3)saveSystem.get("PlayerPosition", this.Player.transform.localPosition);
				this.Player.transform.rotation = (Quaternion)saveSystem.get("PlayerRotation", this.Player.transform.localRotation);
			}
			this.Player.GetComponent<tools>().Started = (bool)saveSystem.get("PlayerStarted", false);
			this.Player.GetComponent<tools>().snowAmount = (float)saveSystem.get("snowAmount", 0f);
			this.Player.GetComponent<tools>().StartPosition = (Vector3)saveSystem.get("StartPosition", new Vector3(0f, 0f, 0f));
			this.Player.GetComponent<FirstPersonAIO>().ControllerUnPause();
			tools.money = (float)saveSystem.get("Money", 10000f);
			tools.SavedVersion = (string)saveSystem.get("SavedVersion", "");
			if ((int)saveSystem.get("BarnFindSeed", 0) > 0 || (bool)saveSystem.get("BarnFindspawned", false))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BarnFindPrefab);
				gameObject.transform.position = (Vector3)saveSystem.get("BarnfindPosition", gameObject.transform.localPosition);
				gameObject.transform.rotation = (Quaternion)saveSystem.get("BarnfindRotation", gameObject.transform.localRotation);
				gameObject.GetComponent<SetStart>().barncarspawner.Seed = (int)saveSystem.get("BarnFindSeed", 0);
				gameObject.GetComponent<SetStart>().spawned = (bool)saveSystem.get("BarnFindspawned", false);
				this.BarnFind = gameObject;
			}
			if (!this.mapMagic)
			{
				if (this.Garage1)
				{
					this.Garage1.SetActive((bool)saveSystem.get("Garage1", true));
				}
				if (this.Garage2)
				{
					this.Garage2.SetActive((bool)saveSystem.get("Garage2", false));
				}
				if (this.GarageYes)
				{
					this.GarageYes.SetActive((bool)saveSystem.get("GarageYes", true));
				}
				if (this.GarageNO)
				{
					this.GarageNO.SetActive((bool)saveSystem.get("GarageNO", false));
				}
				if (!this.Garage1.activeSelf && !this.Garage2.activeSelf)
				{
					this.Garage0.SetActive(true);
				}
			}
			if (this.JM)
			{
				this.JM.StartValue = (float)saveSystem.get("JOBSStartValue", 0f);
				this.JM.Partcount = (float)saveSystem.get("JOBSPartcount", 0f);
				this.JM.AllBolts = (float)saveSystem.get("AllBolts", 0f);
				this.JM.AllWelds = (float)saveSystem.get("AllWelds", 0f);
				this.JM.Realreward = (float)saveSystem.get("JOBSRealreward", 0f);
				if (this.JM.StartValue > 0f)
				{
					this.JM.ActiveJob = GameObject.Find((string)saveSystem.get("JOBSActiveJob", "ella"));
				}
			}
			this.NC.L1 = (string)saveSystem.get("NCL1", "");
			this.NC.L2 = (string)saveSystem.get("NCL2", "");
			this.NC.L3 = (string)saveSystem.get("NCL3", "");
			this.NC.L4 = (string)saveSystem.get("NCL4", "");
			this.NC.L5 = (string)saveSystem.get("NCL5", "");
			this.NC.L6 = (string)saveSystem.get("NCL6", "");
			this.NC.L7 = (string)saveSystem.get("NCL7", "");
			this.NC.L8 = (string)saveSystem.get("NCL8", "");
			this.NC.L9 = (string)saveSystem.get("NCL9", "");
			this.NC.L10 = (string)saveSystem.get("NCL10", "");
			this.NC.L11 = (string)saveSystem.get("NCL11", "");
			this.NC.L12 = (string)saveSystem.get("NCL12", "");
			this.NC.L13 = (string)saveSystem.get("NCL13", "");
			this.NC.L14 = (string)saveSystem.get("NCL14", "");
			this.NC.L15 = (string)saveSystem.get("NCL15", "");
			this.NC.L16 = (string)saveSystem.get("NCL16", "");
			this.NC.L17 = (string)saveSystem.get("NCL17", "");
			this.NC.L18 = (string)saveSystem.get("NCL18", "");
			this.NC.L19 = (string)saveSystem.get("NCL19", "");
			this.NC.L20 = (string)saveSystem.get("NCL20", "");
			PlayerPrefs.SetFloat("DeathWall", (float)((int)saveSystem.get("DeathWall", 0)));
			if (this.mapMagic)
			{
				tools.Needs = (int)saveSystem.get("Needs", 0);
			}
			tools.Food = (float)saveSystem.get("Food", 1f);
			tools.Drink = (float)saveSystem.get("Drink", 1f);
			tools.Sleep = (float)saveSystem.get("Sleep", 1f);
			tools.Health = (float)saveSystem.get("Health", 1f);
			tools.PizzaDeliveriesDay = (int)saveSystem.get("PizzaDeliveriesDay", 1);
			if (tools.Needs == 1)
			{
				this.Player.GetComponent<tools>().NeedsCanvas.SetActive(true);
			}
			ModLoader.OnSaveSystemLoad(saveSystem, false);
			if ((int)saveSystem.get("Objects", 0) > 0)
			{
				this.ObjectNumber = (int)saveSystem.get("Objects", 0);
				for (int i = 0; i < (int)saveSystem.get("Objects", 0); i++)
				{
					string text = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), null);
					if (text != "" && text != null)
					{
						GameObject gameObject2 = null;
						try
						{
							gameObject2 = UnityEngine.Object.Instantiate<GameObject>(cachedResources.Load(text) as GameObject);
						}
						catch (Exception)
						{
							if (Saver.modParts.ContainsKey(text))
							{
								gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Saver.modParts[text] as GameObject);
							}
						}
						if (gameObject2 == null)
						{
							Debug.Log("Object Not in resources" + text);
						}
						else
						{
							if (MultiPlayer && (int)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "MPNumber", 0) > 0)
							{
								gameObject2.AddComponent<MPobject>();
								gameObject2.GetComponent<MPobject>().MPNumber = (int)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "MPNumber", 0);
							}
							if (gameObject2.GetComponent<Partinfo>() && gameObject2.GetComponent<Partinfo>().RenamedPrefab != "")
							{
								gameObject2.transform.name = gameObject2.GetComponent<Partinfo>().RenamedPrefab;
							}
							else
							{
								gameObject2.transform.name = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), null);
							}
							if (gameObject2.GetComponent<CarProperties>())
							{
								CarProperties component = gameObject2.gameObject.GetComponent<CarProperties>();
								component.ObjectNumber = this.ObjectNumber;
								component.SavePosition = (int)saveSystem.get("SavePosition" + this.ObjectNumber.ToString(), 0);
								component.started = (bool)saveSystem.get("started" + this.ObjectNumber.ToString(), false);
								if (component.BikeStand)
								{
									component.BikeStand.steps = (int)saveSystem.get("BikeStandHeight" + this.ObjectNumber.ToString(), 0f);
									component.BikeStand.LiftObject.transform.position += component.BikeStand.transform.TransformDirection(0f, -0.01f * (float)component.BikeStand.steps, 0f);
								}
								component.Owner = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Owner", "");
								component.ClusterMileage = (float)saveSystem.get("ClusterMileage" + this.ObjectNumber.ToString(), 200000f);
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
								if (component.coilnut)
								{
									component.coilnut.height = (float)saveSystem.get("coilnut" + this.ObjectNumber.ToString(), 0f);
									component.coilnut.restart();
								}
								if (component.Tire && component.TirePressure > 0.3f && component.Condition < 0.5f)
								{
									component.TirePressure -= 0.2f;
								}
								component.Chromed = (bool)saveSystem.get("Chromed" + this.ObjectNumber.ToString(), false);
								if (this.compress)
								{
									component.Texture1 = Zip.Decompress((string)saveSystem.get("Texture1" + this.ObjectNumber.ToString(), ""));
									component.Texture3 = Zip.Decompress((string)saveSystem.get("Texture3" + this.ObjectNumber.ToString(), ""));
									component.Texture4 = Zip.Decompress((string)saveSystem.get("Texture3" + this.ObjectNumber.ToString(), ""));
								}
								else
								{
									component.Texture1 = (string)saveSystem.get("Texture1" + this.ObjectNumber.ToString(), "");
									component.Texture3 = (string)saveSystem.get("Texture3" + this.ObjectNumber.ToString(), "");
									component.Texture4 = (string)saveSystem.get("Texture4" + this.ObjectNumber.ToString(), "");
								}
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
								gameObject2.GetComponent<Partinfo>().InBackpack = (bool)saveSystem.get("BackPack" + this.ObjectNumber.ToString(), false);
								if (gameObject2.GetComponent<Partinfo>().InBackpack)
								{
									if (gameObject2.GetComponent<Rigidbody>())
									{
										UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
									}
									this.Player.GetComponent<tools>().BackpackParts.Add(gameObject2);
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
							if (!gameObject2.GetComponent<SaveItem>() || gameObject2.tag == "Building")
							{
								this.goList.Add(gameObject2);
							}
							array[this.ObjectNumber] = gameObject2;
							if (gameObject2.GetComponent<MainCarProperties>())
							{
								MainCarProperties component2 = gameObject2.GetComponent<MainCarProperties>();
								component2.ObjectNumber = this.ObjectNumber;
								component2.Owner = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Owner", "");
								component2.OriginalColor = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "OriginalColor", Color.white);
								component2.Color = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Color", Color.white);
								component2.Started = (bool)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Started", false);
								component2.CarPrice = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "CarPrice", component2.CarPrice);
								component2.Mileage = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mileage", 200000f);
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
								if (component2.Owner == "Client")
								{
									this.JM.Car = gameObject2;
								}
							}
							if (gameObject2.GetComponent<MainTrailerProperties>())
							{
								gameObject2.GetComponent<MainTrailerProperties>().ObjectNumber = this.ObjectNumber;
							}
							if (gameObject2.GetComponent<SaveItem>())
							{
								gameObject2.GetComponent<SaveItem>().ObjectNumber = this.ObjectNumber;
								if (gameObject2.GetComponent<PickupTool>())
								{
									gameObject2.GetComponent<PickupTool>().InBox = (int)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "childCount", 0);
								}
								if (gameObject2.GetComponent<PickupTool>())
								{
									gameObject2.GetComponent<PickupTool>().FluidSize = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "FluidSize", 0f);
								}
								if (gameObject2.GetComponent<PickupTool>() && gameObject2.GetComponent<PickupTool>().NestedFluid)
								{
									gameObject2.GetComponent<PickupTool>().NestedFluid.FluidSize = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "NestedFluid", 0f);
									gameObject2.GetComponent<PickupTool>().NestedFluid.DieselPercent = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "NestedFluidDieselPercent", 0f);
								}
								if (gameObject2.GetComponent<PickupTool>())
								{
									gameObject2.GetComponent<PickupTool>().paintlife = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "paintlife", 0f);
								}
								if (gameObject2.GetComponent<PickupTool>())
								{
									gameObject2.GetComponent<PickupTool>().colorpaint = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Color", Color.white);
								}
								if (gameObject2.GetComponent<PickupTool>() && gameObject2.GetComponent<PickupTool>().material != null)
								{
									gameObject2.GetComponent<PickupTool>().material = (cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "material", "")) as Material);
									gameObject2.GetComponent<PickupTool>().previewLabel.sharedMaterial = gameObject2.GetComponent<PickupTool>().material;
								}
								if (gameObject2.GetComponent<PickupItems>())
								{
									gameObject2.GetComponent<PickupItems>().Condition = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Condition", 8f);
								}
								if (gameObject2.GetComponent<DeliveryTarget>())
								{
									gameObject2.GetComponent<DeliveryTarget>().Accepted = (bool)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Accepted", false);
								}
								if (gameObject2.GetComponent<DeliveryTarget>())
								{
									gameObject2.GetComponent<DeliveryTarget>().BarnGiver = (bool)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "BarnGiver", false);
								}
								if (gameObject2.GetComponent<SaveItem>().pickuptool)
								{
									gameObject2.GetComponent<SaveItem>().pickuptool.paintlife = (float)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "paintlife", 0f);
									gameObject2.GetComponent<SaveItem>().pickuptool.VisualUpdate();
								}
								if (gameObject2.GetComponent<SaveItem>().Handle != null)
								{
									gameObject2.GetComponent<SaveItem>().Handle.steps = (int)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "HandleHeight", 0);
									gameObject2.GetComponent<SaveItem>().Handle.LiftObject.transform.position -= gameObject2.GetComponent<SaveItem>().Handle.transform.TransformDirection(0f, -0.01f * (float)gameObject2.GetComponent<SaveItem>().Handle.steps, 0f);
								}
								gameObject2.GetComponent<SaveItem>().InBackpack = (bool)saveSystem.get("BackPack" + this.ObjectNumber.ToString(), false);
								if (gameObject2.GetComponent<SaveItem>().InBackpack)
								{
									if (gameObject2.GetComponent<Rigidbody>())
									{
										UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
									}
									this.Player.GetComponent<tools>().BackpackParts.Add(gameObject2);
								}
								gameObject2.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null);
								gameObject2.transform.localRotation = (Quaternion)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "rotation", null);
								if ((gameObject2.transform.localPosition.y < 40f || gameObject2.transform.localPosition.y > 1000f) && gameObject2.tag != "Building")
								{
									UnityEngine.Object.Destroy(gameObject2.gameObject);
								}
								if (gameObject2.tag == "Building")
								{
									gameObject2.AddComponent<MeshCollider>();
									if (gameObject2.GetComponent<SaveItem>().ChildrenRend)
									{
										gameObject2.GetComponent<SaveItem>().ChildrenRend.gameObject.AddComponent<BoxCollider>();
									}
									if ((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "ChildrenRend", "") != "" && gameObject2.GetComponent<SaveItem>().ChildrenRend)
									{
										gameObject2.GetComponent<SaveItem>().ChildrenRend.sharedMaterial = (cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "ChildrenRend", null)) as Material);
									}
									Material[] materials = gameObject2.GetComponent<Renderer>().materials;
									if ((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat0", "") != "")
									{
										Material material = cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat0", null)) as Material;
										if (material != null)
										{
											materials[0] = material;
										}
									}
									if (gameObject2.GetComponent<Renderer>().sharedMaterials.Length > 1 && (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat1", "") != "")
									{
										Material material = cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat1", null)) as Material;
										if (material != null)
										{
											materials[1] = material;
										}
									}
									if (gameObject2.GetComponent<Renderer>().sharedMaterials.Length > 2 && (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat2", "") != "")
									{
										Material material = cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat2", null)) as Material;
										if (material != null)
										{
											materials[2] = material;
										}
									}
									gameObject2.GetComponent<Renderer>().sharedMaterials = materials;
								}
							}
						}
					}
					else
					{
						Debug.Log("Object Not in resources" + (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), ""));
					}
					this.ObjectNumber--;
				}
				foreach (GameObject gameObject3 in this.goList)
				{
					if (gameObject3.GetComponent<MainCarProperties>())
					{
						this.ObjectNumber = gameObject3.GetComponent<MainCarProperties>().ObjectNumber;
					}
					if (gameObject3.GetComponent<CarProperties>())
					{
						this.ObjectNumber = gameObject3.GetComponent<CarProperties>().ObjectNumber;
					}
					if (gameObject3.GetComponent<MainTrailerProperties>())
					{
						this.ObjectNumber = gameObject3.GetComponent<MainTrailerProperties>().ObjectNumber;
					}
					if (gameObject3.GetComponent<SaveItem>())
					{
						this.ObjectNumber = gameObject3.GetComponent<SaveItem>().ObjectNumber;
					}
					int num = (int)saveSystem.get("Parent" + this.ObjectNumber.ToString(), 0);
					if (num > 0)
					{
						GameObject gameObject4 = array[num];
						if (gameObject4 != null)
						{
							if ((gameObject4.GetComponent<MainCarProperties>() && gameObject4.GetComponent<MainCarProperties>().ObjectNumber == num) || (gameObject4.GetComponent<MainTrailerProperties>() && gameObject4.GetComponent<MainTrailerProperties>().ObjectNumber == num) || (gameObject4.GetComponent<CarProperties>() && gameObject4.GetComponent<CarProperties>().ObjectNumber == num))
							{
								foreach (transparents transparents in gameObject4.GetComponentsInChildren<transparents>())
								{
									if (transparents.transform.name == gameObject3.name && transparents.SavePosition == gameObject3.GetComponent<CarProperties>().SavePosition)
									{
										gameObject3.transform.SetParent(transparents.transform);
									}
									gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
								}
							}
							if (gameObject3.tag == "Building" && gameObject4.GetComponent<SaveItem>() && gameObject4.GetComponent<SaveItem>().ObjectNumber == num)
							{
								gameObject3.transform.SetParent(gameObject4.transform);
							}
						}
					}
					if (gameObject3.transform.parent && gameObject3.GetComponent<CarProperties>() && !gameObject3.GetComponent<CarProperties>().DMGdisplacepart)
					{
						gameObject3.transform.position = gameObject3.transform.parent.position;
						gameObject3.transform.rotation = gameObject3.transform.parent.rotation;
					}
					else
					{
						gameObject3.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null);
						gameObject3.transform.localRotation = (Quaternion)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "rotation", null);
					}
					if (gameObject3.transform.localPosition.y < -100f || gameObject3.transform.localPosition.y > 1000f)
					{
						UnityEngine.Object.Destroy(gameObject3.gameObject);
					}
				}
			}
			foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
			{
				if (transform.gameObject.GetComponent<SavePosition>())
				{
					transform.localPosition = (Vector3)saveSystem.get("PropPosition" + transform.gameObject.GetComponent<SavePosition>().SceneNumber.ToString(), transform.localPosition);
					transform.localRotation = (Quaternion)saveSystem.get("PropRotation" + transform.gameObject.GetComponent<SavePosition>().SceneNumber.ToString(), transform.localRotation);
				}
				if (transform.gameObject.GetComponent<AutoAttach>())
				{
					transform.GetComponent<AutoAttach>().Attach();
				}
			}
			Array.Clear(array, 0, array.Length);
		}
		tools.GameLoaded = true;
		EnviroSkyMgr.instance.SetTime(EnviroSkyMgr.instance.Time.Years, (int)saveSystem.get("day", 0), (int)saveSystem.get("Hours", 11), (int)saveSystem.get("Minutes", 0), 0);
		this.Player.GetComponent<tools>().HourPassed();
		if (this.LoadingPlayerText)
		{
			this.LoadingPlayerText.SetActive(false);
		}
		if (this.LoadingScreen)
		{
			this.LoadingScreen.SetActive(false);
		}
		ModLoader.Continue();
		if (MultiPlayer)
		{
			try
			{
				string text2 = (string)saveSystem.get("MultiplayerModList", "");
				SimplifiedMods simplifiedMods = null;
				SimplifiedMods simplifiedMods2 = new SimplifiedMods();
				foreach (Mod mod in ModLoader.mods)
				{
					SimplifiedModObject item = new SimplifiedModObject(mod.ID, mod.Name, mod.Version);
					simplifiedMods2.Mods.Add(item);
				}
				if (text2 != "")
				{
					simplifiedMods = JsonConvert.DeserializeObject<SimplifiedMods>(text2);
				}
				if (simplifiedMods == null)
				{
					simplifiedMods = new SimplifiedMods();
				}
				List<SimplifiedModObject> list = simplifiedMods2.Mods.Except(simplifiedMods.Mods, new SimplifiedModObjectComparer()).ToList<SimplifiedModObject>();
				List<SimplifiedModObject> list2 = simplifiedMods.Mods.Except(simplifiedMods2.Mods, new SimplifiedModObjectComparer()).ToList<SimplifiedModObject>();
				List<SimplifiedModObject> list3 = (from obj1 in simplifiedMods2.Mods
				join obj2 in simplifiedMods.Mods on obj1.ModId equals obj2.ModId
				where obj1.ModVersion != obj2.ModVersion
				select obj1).ToList<SimplifiedModObject>();
				if (Application.version != tools.SavedVersion)
				{
					this.MPcompatibilityInfo.SetActive(true);
					this.GameVersion.text = "Mismatched game versions \n YOUR version is " + Application.version + "\n HOST version is " + tools.SavedVersion;
				}
				string text3 = "";
				foreach (SimplifiedModObject simplifiedModObject in list)
				{
					text3 = string.Concat(new string[]
					{
						text3,
						simplifiedModObject.ModName,
						" - ",
						simplifiedModObject.ModVersion,
						"\n"
					});
				}
				string text4 = "";
				foreach (SimplifiedModObject simplifiedModObject2 in list2)
				{
					text4 = string.Concat(new string[]
					{
						text4,
						simplifiedModObject2.ModName,
						" - ",
						simplifiedModObject2.ModVersion,
						"\n"
					});
				}
				string text5 = "";
				foreach (SimplifiedModObject simplifiedModObject3 in list3)
				{
					text5 = string.Concat(new string[]
					{
						text5,
						simplifiedModObject3.ModName,
						" - ",
						simplifiedModObject3.ModVersion,
						"\n"
					});
				}
				if (text3 != "")
				{
					this.MPcompatibilityInfo.SetActive(true);
					this.MPmodsMissingInHost.text = "Mods missing in Host\n(Host dont have them but you have):\n" + text3;
				}
				if (text4 != "")
				{
					this.MPcompatibilityInfo.SetActive(true);
					this.MPmodsMissingInClient.text = "Mods missing in Client \n(You dont have them but host have):\n" + text4;
				}
				if (text5 != "")
				{
					this.MPcompatibilityInfo.SetActive(true);
					this.MPmodswrongVersiont.text = "Mod versions not matching:\n" + text5;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("An issue occured while trying to do the mod check! " + ex.Message);
			}
			tools.MPrunning = true;
			tools.NetworkPLayer.AddMPobjects();
			tools.NetworkPLayer.ClientLoaded();
		}
		this.goList.Clear();
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0007B934 File Offset: 0x00079B34
	public void LoadBuildings()
	{
		this.goList.Clear();
		SaveSystem saveSystem = new SaveSystem(Application.persistentDataPath + this.SaveFolder + "/Buildings.dat");
		if (File.Exists(Application.persistentDataPath + this.SaveFolder + "/Buildings.dat"))
		{
			cachedResources.InitializeResources();
			saveSystem.read();
			GameObject[] array = new GameObject[(int)saveSystem.get("Objects", 0) + 10];
			if ((int)saveSystem.get("Objects", 0) > 0)
			{
				this.ObjectNumber = (int)saveSystem.get("Objects", 0);
				for (int i = 0; i < (int)saveSystem.get("Objects", 0); i++)
				{
					string text = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), null);
					if (text != "" && text != null)
					{
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
							if (!gameObject.GetComponent<SaveItem>() || gameObject.tag == "Building")
							{
								this.goList.Add(gameObject);
							}
							array[this.ObjectNumber] = gameObject;
							gameObject.transform.name = (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), null);
							if (gameObject.GetComponent<SaveItem>())
							{
								gameObject.GetComponent<SaveItem>().ObjectNumber = this.ObjectNumber;
								gameObject.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null) + base.transform.parent.position;
								gameObject.transform.localRotation = (Quaternion)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "rotation", null);
								if ((gameObject.transform.localPosition.y < 40f || gameObject.transform.localPosition.y > 1000f) && gameObject.tag != "Building")
								{
									UnityEngine.Object.Destroy(gameObject.gameObject);
								}
								if (gameObject.tag == "Building")
								{
									gameObject.AddComponent<MeshCollider>();
									if (gameObject.GetComponent<SaveItem>().ChildrenRend)
									{
										gameObject.GetComponent<SaveItem>().ChildrenRend.gameObject.AddComponent<BoxCollider>();
									}
									if ((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "ChildrenRend", "") != "" && gameObject.GetComponent<SaveItem>().ChildrenRend)
									{
										gameObject.GetComponent<SaveItem>().ChildrenRend.sharedMaterial = (cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "ChildrenRend", null)) as Material);
									}
									Material[] materials = gameObject.GetComponent<Renderer>().materials;
									if ((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat0", "") != "")
									{
										Material material = cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat0", null)) as Material;
										if (material != null)
										{
											materials[0] = material;
										}
									}
									if (gameObject.GetComponent<Renderer>().sharedMaterials.Length > 1 && (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat1", "") != "")
									{
										Material material = cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat1", null)) as Material;
										if (material != null)
										{
											materials[1] = material;
										}
									}
									if (gameObject.GetComponent<Renderer>().sharedMaterials.Length > 2 && (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat2", "") != "")
									{
										Material material = cachedResources.Load((string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Mat2", null)) as Material;
										if (material != null)
										{
											materials[2] = material;
										}
									}
									gameObject.GetComponent<Renderer>().sharedMaterials = materials;
								}
							}
						}
					}
					else
					{
						Debug.Log("Object Not in resources" + (string)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString(), ""));
					}
					this.ObjectNumber--;
				}
				foreach (GameObject gameObject2 in this.goList)
				{
					if (gameObject2.GetComponent<SaveItem>() && gameObject2.tag == "Building")
					{
						this.ObjectNumber = gameObject2.GetComponent<SaveItem>().ObjectNumber;
					}
					int num = (int)saveSystem.get("Parent" + this.ObjectNumber.ToString(), 0);
					if (num > 0)
					{
						GameObject gameObject3 = array[num];
						if (gameObject3 != null && gameObject2.tag == "Building" && gameObject3.GetComponent<SaveItem>() && gameObject3.GetComponent<SaveItem>().ObjectNumber == num)
						{
							gameObject2.transform.SetParent(gameObject3.transform);
						}
					}
					if (gameObject2.transform.parent)
					{
						gameObject2.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null);
						gameObject2.transform.localRotation = (Quaternion)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "rotation", null);
					}
					if (gameObject2.transform.localPosition.y < -100f || gameObject2.transform.localPosition.y > 1000f)
					{
						UnityEngine.Object.Destroy(gameObject2.gameObject);
					}
				}
			}
			Array.Clear(array, 0, array.Length);
		}
		this.goList.Clear();
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0007C0A4 File Offset: 0x0007A2A4
	public void Quit()
	{
		this.Player.GetComponent<tools>().Loadingtext.SetActive(true);
		SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x0400138E RID: 5006
	public MapMagicObject mapMagic;

	// Token: 0x0400138F RID: 5007
	public int Seed;

	// Token: 0x04001390 RID: 5008
	public bool compress;

	// Token: 0x04001391 RID: 5009
	public GameObject LoadingScreen;

	// Token: 0x04001392 RID: 5010
	public GameObject GeneratingWorldText;

	// Token: 0x04001393 RID: 5011
	public GameObject LoadingPlayerText;

	// Token: 0x04001394 RID: 5012
	public string CurrentChildren;

	// Token: 0x04001395 RID: 5013
	public GameObject BarnFindPrefab;

	// Token: 0x04001396 RID: 5014
	public GameObject BarnFind;

	// Token: 0x04001397 RID: 5015
	public JobsManager JM;

	// Token: 0x04001398 RID: 5016
	public NotesCanvas NC;

	// Token: 0x04001399 RID: 5017
	public GameObject Player;

	// Token: 0x0400139A RID: 5018
	public GameObject Garage0;

	// Token: 0x0400139B RID: 5019
	public GameObject Garage1;

	// Token: 0x0400139C RID: 5020
	public GameObject Garage2;

	// Token: 0x0400139D RID: 5021
	public GameObject GarageNO;

	// Token: 0x0400139E RID: 5022
	public GameObject GarageYes;

	// Token: 0x0400139F RID: 5023
	public SaveInside Barn;

	// Token: 0x040013A0 RID: 5024
	public GameObject SaveAllInside;

	// Token: 0x040013A1 RID: 5025
	public int ObjectNumber;

	// Token: 0x040013A2 RID: 5026
	public int CurrentNumber;

	// Token: 0x040013A3 RID: 5027
	public DeathWall DWall;

	// Token: 0x040013A4 RID: 5028
	public static Hashtable modParts = new Hashtable();

	// Token: 0x040013A5 RID: 5029
	public string SaveFolder;

	// Token: 0x040013A6 RID: 5030
	public string SavePath;

	// Token: 0x040013A7 RID: 5031
	public string BackupPath;

	// Token: 0x040013A8 RID: 5032
	public bool MULTIPLAYERrunningMapMagic;

	// Token: 0x040013A9 RID: 5033
	public bool Loaded;

	// Token: 0x040013AA RID: 5034
	public GameObject MPcompatibilityInfo;

	// Token: 0x040013AB RID: 5035
	public Text GameVersion;

	// Token: 0x040013AC RID: 5036
	public Text MPmodsMissingInHost;

	// Token: 0x040013AD RID: 5037
	public Text MPmodsMissingInClient;

	// Token: 0x040013AE RID: 5038
	public Text MPmodswrongVersiont;

	// Token: 0x040013AF RID: 5039
	private List<GameObject> goList;

	// Token: 0x040013B0 RID: 5040
	private string xmlContent;

	// Token: 0x040013B1 RID: 5041
	private TextAsset xmlAsset;

	// Token: 0x040013B2 RID: 5042
	private SaveSystem save;
}
