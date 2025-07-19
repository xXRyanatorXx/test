using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class SaveCar : MonoBehaviour
{
	// Token: 0x06000AF6 RID: 2806 RVA: 0x00070649 File Offset: 0x0006E849
	private void Awake()
	{
		this.goList = new List<GameObject>();
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00070656 File Offset: 0x0006E856
	public void MultiLoad()
	{
		base.StartCoroutine(this.WaitSpawn());
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00070665 File Offset: 0x0006E865
	private IEnumerator WaitSpawn()
	{
		yield return new WaitForSeconds(3f);
		if (File.Exists(Application.persistentDataPath + "/Multiplayer/" + this.SaveName))
		{
			this.Load();
		}
		else
		{
			base.StartCoroutine(this.WaitSpawn());
		}
		yield break;
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00070674 File Offset: 0x0006E874
	public void Save(bool destroy)
	{
		this.SavePath = "\\save1 /Car.dat";
		SaveSystem saveSystem = new SaveSystem(Application.persistentDataPath + this.SavePath);
		this.ObjectNumber = 0;
		this.Car = base.transform.parent.parent.GetComponent<CarInformation>().Car;
		this.goList.Clear();
		this.ObjectNumber++;
		this.Car.gameObject.GetComponent<MainCarProperties>().ObjectNumber = this.ObjectNumber;
		this.goList.Add(this.Car.gameObject);
		saveSystem.add("MPCar", this.Car.gameObject.GetComponent<MainCarProperties>().PREFAB.transform.name);
		foreach (CarProperties carProperties in this.Car.gameObject.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.PREFAB && carProperties.Owner == "Player")
			{
				this.goList.Add(carProperties.gameObject);
				this.ObjectNumber++;
				carProperties.ObjectNumber = this.ObjectNumber;
				carProperties.SavingGame();
			}
		}
		foreach (GameObject gameObject in this.goList)
		{
			Transform component = gameObject.GetComponent<Transform>();
			if (component.position.y > 45f)
			{
				if (component.gameObject.GetComponent<MainCarProperties>() && component.gameObject.GetComponent<MainCarProperties>().Owner == "Player")
				{
					MainCarProperties component2 = component.gameObject.GetComponent<MainCarProperties>();
					this.CurrentNumber = component2.ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component2.PREFAB.transform.name.ToString());
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component.position);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", component.rotation);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Owner", component2.Owner);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "OriginalColor", component2.OriginalColor);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Color", component2.Color);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Started", component2.Started);
					saveSystem.add("Parent" + this.CurrentNumber.ToString(), 0);
				}
				if (component.gameObject.GetComponent<CarProperties>() && component.gameObject.GetComponent<CarProperties>().PREFAB && component.gameObject.GetComponent<CarProperties>().Owner == "Player")
				{
					CarProperties component3 = component.gameObject.GetComponent<CarProperties>();
					this.CurrentNumber = component3.ObjectNumber;
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString(), component3.PrefabName);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component.localPosition);
					if (component.root.name == "EngineStand")
					{
						saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "position", component.position);
					}
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "rotation", component.localRotation);
					saveSystem.add("ObjectNr" + this.CurrentNumber.ToString() + "Owner", component3.Owner);
					if (component.parent && component.parent.parent && component.parent.parent.GetComponent<CarProperties>() && component.parent.parent.GetComponent<CarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component.parent.parent.GetComponent<CarProperties>().ObjectNumber);
					}
					else if (component.parent && component.parent.parent && component.parent.parent.GetComponent<MainCarProperties>() && component.parent.parent.GetComponent<MainCarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component.parent.parent.GetComponent<MainCarProperties>().ObjectNumber);
					}
					else if (component.parent && component.parent.parent && component.parent.parent.parent && component.parent.parent.parent.GetComponent<CarProperties>() && component.parent.parent.parent.GetComponent<CarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component.parent.parent.parent.GetComponent<CarProperties>().ObjectNumber);
					}
					else if (component.parent && component.parent.parent && component.parent.parent.parent && component.parent.parent.parent.GetComponent<MainCarProperties>() && component.parent.parent.parent.GetComponent<MainCarProperties>().PREFAB)
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component.parent.parent.parent.GetComponent<MainCarProperties>().ObjectNumber);
					}
					else if (component.root.GetComponent<MainTrailerProperties>())
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), component.root.GetComponent<MainTrailerProperties>().ObjectNumber);
					}
					else
					{
						saveSystem.add("Parent" + this.CurrentNumber.ToString(), 0);
					}
					string str = "detala+ parents";
					string str2 = this.CurrentNumber.ToString();
					object obj = saveSystem.get("Parent" + this.CurrentNumber.ToString(), null);
					Debug.Log(str + str2 + ((obj != null) ? obj.ToString() : null));
					if (component3.FluidSize > 0f)
					{
						saveSystem.add("FluidSize" + this.CurrentNumber.ToString(), component3.FluidSize);
					}
					if (component3.FluidCondition > 0f)
					{
						saveSystem.add("FluidCondition" + this.CurrentNumber.ToString(), component3.FluidCondition);
					}
					if (component3.TirePressure > 0f)
					{
						saveSystem.add("TirePressure" + this.CurrentNumber.ToString(), component3.TirePressure);
					}
					saveSystem.add("Condition" + this.CurrentNumber.ToString(), component3.Condition);
					saveSystem.add("Chromed" + this.CurrentNumber.ToString(), component3.Chromed);
					saveSystem.add("SavePosition" + this.CurrentNumber.ToString(), component3.SavePosition);
					if (component3.BodyMatNumber > 0 && component3.Condition < 1f)
					{
						saveSystem.add("BodyMatNumber" + this.CurrentNumber.ToString(), component3.BodyMatNumber);
					}
					if (component3.OriginalInterior > 0)
					{
						saveSystem.add("OriginalInterior" + this.CurrentNumber.ToString(), component3.OriginalInterior);
					}
					if (component3.TintLevel > 0)
					{
						saveSystem.add("TintLevel" + this.CurrentNumber.ToString(), component3.TintLevel);
					}
					if (component3.Ruined)
					{
						saveSystem.add("Ruined" + this.CurrentNumber.ToString(), component3.Ruined);
					}
					if (component3.Damaged)
					{
						saveSystem.add("Damaged" + this.CurrentNumber.ToString(), component3.Damaged);
					}
					if (component3.PartIsOld)
					{
						saveSystem.add("PartIsOld" + this.CurrentNumber.ToString(), component3.PartIsOld);
					}
					if (component3.MeshDamaged)
					{
						saveSystem.add("MeshDamaged" + this.CurrentNumber.ToString(), component3.MeshDamaged);
					}
					if (component3.MeshLittleDamaged)
					{
						saveSystem.add("MeshLittleDamaged" + this.CurrentNumber.ToString(), component3.MeshLittleDamaged);
					}
					if (component3.MeshDamaged || component3.MeshLittleDamaged)
					{
						saveSystem.add("Damagedvertices" + this.CurrentNumber.ToString(), component3.Damagedvertices);
					}
					if (component3.ChildDamag && component3.ChildDamag.MeshLittleDamaged)
					{
						saveSystem.add("RuinedCH" + this.CurrentNumber.ToString(), component3.ChildDamag.Ruined);
						saveSystem.add("MeshDamagedCH" + this.CurrentNumber.ToString(), component3.ChildDamag.MeshDamaged);
						saveSystem.add("MeshLittleDamagedCH" + this.CurrentNumber.ToString(), component3.ChildDamag.MeshLittleDamaged);
						saveSystem.add("DamagedverticesCH" + this.CurrentNumber.ToString(), component3.ChildDamag.GetComponent<MeshFilter>().mesh.vertices);
					}
					saveSystem.add("Loose" + this.CurrentNumber.ToString(), component3.Loose);
					if (component.gameObject.transform.parent && component.gameObject.GetComponent<Partinfo>().tightnuts > 0f)
					{
						saveSystem.add("tightnuts" + this.CurrentNumber.ToString(), component.gameObject.GetComponent<Partinfo>().tightnuts);
					}
					if (component3.NumberPlate)
					{
						saveSystem.add("One" + this.CurrentNumber.ToString(), component3.One.name.ToString());
						saveSystem.add("Two" + this.CurrentNumber.ToString(), component3.Two.name.ToString());
						saveSystem.add("Three" + this.CurrentNumber.ToString(), component3.Three.name.ToString());
						saveSystem.add("Four" + this.CurrentNumber.ToString(), component3.Four.name.ToString());
						saveSystem.add("Five" + this.CurrentNumber.ToString(), component3.Five.name.ToString());
						saveSystem.add("Six" + this.CurrentNumber.ToString(), component3.Six.name.ToString());
					}
				}
			}
		}
		foreach (GameObject gameObject2 in this.goList)
		{
			if (destroy && (gameObject2.GetComponent<CarProperties>() || gameObject2.GetComponent<MainCarProperties>() || gameObject2.GetComponent<MainTrailerProperties>()))
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		Debug.Log(this.ObjectNumber.ToString());
		saveSystem.add("Objects", this.ObjectNumber);
		saveSystem.write();
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00071450 File Offset: 0x0006F650
	public void Load()
	{
		this.goList.Clear();
		this.Car = base.transform.root.gameObject;
		SaveSystem saveSystem = new SaveSystem(Application.persistentDataPath + "/Multiplayer/" + this.SaveName);
		saveSystem.read();
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
							component.started = true;
							component.Owner = "Multiplayer";
							component.FluidSize = (float)saveSystem.get("FluidSize" + this.ObjectNumber.ToString(), 0f);
							component.FluidCondition = (float)saveSystem.get("FluidCondition" + this.ObjectNumber.ToString(), 0f);
							component.TirePressure = (float)saveSystem.get("TirePressure" + this.ObjectNumber.ToString(), 2f);
							component.Condition = (float)saveSystem.get("Condition" + this.ObjectNumber.ToString(), 0f);
							component.Chromed = (bool)saveSystem.get("Chromed" + this.ObjectNumber.ToString(), false);
							component.BodyMatNumber = (int)saveSystem.get("BodyMatNumber" + this.ObjectNumber.ToString(), 0);
							component.OriginalInterior = (int)saveSystem.get("OriginalInterior" + this.ObjectNumber.ToString(), 0);
							component.TintLevel = (int)saveSystem.get("TintLevel" + this.ObjectNumber.ToString(), 0);
							component.Ruined = (bool)saveSystem.get("Ruined" + this.ObjectNumber.ToString(), false);
							component.Damaged = (bool)saveSystem.get("Damaged" + this.ObjectNumber.ToString(), false);
							component.PartIsOld = (bool)saveSystem.get("PartIsOld" + this.ObjectNumber.ToString(), false);
							component.MeshDamaged = (bool)saveSystem.get("MeshDamaged" + this.ObjectNumber.ToString(), false);
							component.MeshLittleDamaged = (bool)saveSystem.get("MeshLittleDamaged" + this.ObjectNumber.ToString(), false);
							Vector3[] array = (Vector3[])saveSystem.get("Damagedvertices" + this.ObjectNumber.ToString(), null);
							if (array != null)
							{
								component.Damagedvertices = array;
							}
							if (component.ChildDamag)
							{
								component.ChildDamag.Ruined = (bool)saveSystem.get("RuinedCH" + this.ObjectNumber.ToString(), false);
								component.ChildDamag.MeshDamaged = (bool)saveSystem.get("MeshDamagedCH" + this.ObjectNumber.ToString(), false);
								component.ChildDamag.MeshLittleDamaged = (bool)saveSystem.get("MeshLittleDamagedCH" + this.ObjectNumber.ToString(), false);
								Vector3[] array2 = (Vector3[])saveSystem.get("DamagedverticesCH" + this.ObjectNumber.ToString(), null);
								if (array2 != null)
								{
									component.ChildDamag.Damagedvertices = array2;
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
						if (!gameObject.GetComponent<SaveItem>() && !gameObject.GetComponent<MainCarProperties>())
						{
							this.goList.Add(gameObject);
						}
						if (gameObject.GetComponent<MainCarProperties>())
						{
							MainCarProperties component2 = this.Car.GetComponent<MainCarProperties>();
							component2.InBarn = true;
							component2.ObjectNumber = this.ObjectNumber;
							component2.Owner = "Multiplayer";
							component2.OriginalColor = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "OriginalColor", Color.white);
							component2.Color = (Color)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Color", Color.white);
							component2.Started = (bool)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "Started", false);
							UnityEngine.Object.Destroy(gameObject);
							this.goList.Add(this.Car);
						}
					}
				}
				this.ObjectNumber--;
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
				if ((int)saveSystem.get("Parent" + this.ObjectNumber.ToString(), 0) > 0)
				{
					foreach (GameObject gameObject3 in this.goList)
					{
						if ((gameObject3.GetComponent<MainCarProperties>() && gameObject3.GetComponent<MainCarProperties>().InBarn && gameObject3.GetComponent<MainCarProperties>().ObjectNumber == (int)saveSystem.get("Parent" + this.ObjectNumber.ToString(), 0)) || (gameObject3.GetComponent<MainTrailerProperties>() && gameObject3.GetComponent<MainTrailerProperties>().InBarn && gameObject3.GetComponent<MainTrailerProperties>().ObjectNumber == (int)saveSystem.get("Parent" + this.ObjectNumber.ToString(), 0)) || (gameObject3.GetComponent<CarProperties>() && gameObject3.GetComponent<CarProperties>().InBarn && gameObject3.GetComponent<CarProperties>().ObjectNumber == (int)saveSystem.get("Parent" + this.ObjectNumber.ToString(), 0)))
						{
							foreach (transparents transparents in gameObject3.GetComponentsInChildren<transparents>())
							{
								if (transparents.transform.name == gameObject2.name && transparents.SavePosition == gameObject2.GetComponent<CarProperties>().SavePosition)
								{
									gameObject2.transform.SetParent(transparents.transform);
								}
							}
						}
					}
				}
				if (gameObject2.transform.parent && gameObject2.GetComponent<CarProperties>() && !gameObject2.GetComponent<CarProperties>().DMGdisplacepart)
				{
					gameObject2.transform.position = gameObject2.transform.parent.position;
					gameObject2.transform.rotation = gameObject2.transform.parent.rotation;
				}
				else if (!gameObject2.GetComponent<MainCarProperties>())
				{
					gameObject2.transform.localPosition = (Vector3)saveSystem.get("ObjectNr" + this.ObjectNumber.ToString() + "position", null);
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
			base.GetComponent<MainCarProperties>().MPstart();
		}
	}

	// Token: 0x04001375 RID: 4981
	public Saver MainSaver;

	// Token: 0x04001376 RID: 4982
	public int ObjectNumber;

	// Token: 0x04001377 RID: 4983
	public int CurrentNumber;

	// Token: 0x04001378 RID: 4984
	public string SavePath;

	// Token: 0x04001379 RID: 4985
	public List<GameObject> goList;

	// Token: 0x0400137A RID: 4986
	private SaveSystem save;

	// Token: 0x0400137B RID: 4987
	public GameObject Car;

	// Token: 0x0400137C RID: 4988
	public string SaveName;
}
