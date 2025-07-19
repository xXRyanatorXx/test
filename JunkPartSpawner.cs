using System;
using PaintIn3D;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class JunkPartSpawner : MonoBehaviour
{
	// Token: 0x0600071C RID: 1820 RVA: 0x0003A390 File Offset: 0x00038590
	private void OnEnable()
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.7f)
		{
			if (!this.PartList)
			{
				this.PartList = GameObject.Find("PartsParent");
			}
			this.Part = this.PartList.GetComponent<JunkPartsList>().Parts[UnityEngine.Random.Range(0, this.PartList.GetComponent<JunkPartsList>().Parts.Length)];
			if (tools.MPrunning && (!this.Part.GetComponent<Partinfo>().DontSpawnInJunyard || !this.Junkyard))
			{
				int num = (int)(base.transform.position.x - base.transform.root.position.x);
				int seed = DateTime.Now.Millisecond + num;
				tools.NetworkPLayer.ITEM = this.Part;
				tools.NetworkPLayer.Spawnposition = base.transform.position;
				tools.NetworkPLayer.SpawnObject(seed, false);
				return;
			}
			if (!this.Part.GetComponent<Partinfo>().DontSpawnInJunyard || !this.Junkyard)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Part, base.transform.position, Quaternion.identity);
				gameObject.transform.name = this.Part.transform.name;
				if (gameObject.GetComponent<CarProperties>())
				{
					this.CarProp = gameObject.GetComponent<CarProperties>();
					this.Color = UnityEngine.Random.ColorHSV();
					this.CarProp.Condition = UnityEngine.Random.Range(0.003f, 1f);
					if (this.CarProp.Paintable)
					{
						this.CarProp.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
					}
					if (this.CarProp.Tire)
					{
						if (this.CarProp.Condition < 0.1f)
						{
							this.CarProp.PartIsOld = true;
							this.CarProp.gameObject.GetComponent<Renderer>().sharedMaterial = this.CarProp.OldMaterial;
						}
						this.CarProp.TirePressure = 0f;
					}
					if (this.CarProp.Interior)
					{
						this.CarProp.OriginalInterior = UnityEngine.Random.Range(1, 7);
					}
					if (this.Junkyard)
					{
						this.CarProp.Owner = "Junkyard";
					}
					else
					{
						this.CarProp.Owner = "Player";
					}
					this.CarProp.ReStart();
				}
				if (gameObject.GetComponent<Partinfo>())
				{
					gameObject.GetComponent<Partinfo>().CreatingJunk();
				}
			}
		}
	}

	// Token: 0x04000B1A RID: 2842
	public GameObject PartList;

	// Token: 0x04000B1B RID: 2843
	public GameObject Part;

	// Token: 0x04000B1C RID: 2844
	public CarProperties CarProp;

	// Token: 0x04000B1D RID: 2845
	public Color Color;

	// Token: 0x04000B1E RID: 2846
	public bool Junkyard;

	// Token: 0x04000B1F RID: 2847
	public float ChanceToSpawn = 1f;
}
