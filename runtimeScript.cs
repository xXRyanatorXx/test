using System;
using EasyRoads3Dv3;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class runtimeScript : MonoBehaviour
{
	// Token: 0x06000135 RID: 309 RVA: 0x0000AB00 File Offset: 0x00008D00
	private void Start()
	{
		Debug.Log("Please read the comments at the top of the runtime script (/Assets/EasyRoads3D/Scripts/runtimeScript) before using the runtime API!");
		this.roadNetwork = new ERRoadNetwork();
		ERRoadType erroadType = new ERRoadType();
		erroadType.roadWidth = 6f;
		erroadType.roadMaterial = (Resources.Load("Materials/roads/road material") as Material);
		erroadType.layer = 1;
		erroadType.tag = "Untagged";
		Vector3[] markers = new Vector3[]
		{
			new Vector3(200f, 5f, 200f),
			new Vector3(250f, 5f, 200f),
			new Vector3(250f, 5f, 250f),
			new Vector3(300f, 5f, 250f)
		};
		this.road = this.roadNetwork.CreateRoad("road 1", erroadType, markers);
		this.road.AddMarker(new Vector3(300f, 5f, 300f));
		this.road.InsertMarker(new Vector3(275f, 5f, 235f));
		this.road.DeleteMarker(2);
		this.roadNetwork.BuildRoadNetwork();
		this.go = GameObject.CreatePrimitive(PrimitiveType.Cube);
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000AC48 File Offset: 0x00008E48
	private void Update()
	{
		if (this.roadNetwork != null)
		{
			float num = Time.deltaTime * this.speed;
			this.distance += num;
			Vector3 position = this.road.GetPosition(this.distance, ref this.currentElement);
			position.y += 1f;
			this.go.transform.position = position;
			this.go.transform.forward = this.road.GetLookatSmooth(this.distance, this.currentElement);
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000ACD8 File Offset: 0x00008ED8
	private void OnDestroy()
	{
		if (this.roadNetwork != null && this.roadNetwork.isInBuildMode)
		{
			this.roadNetwork.RestoreRoadNetwork();
			Debug.Log("Restore Road Network");
		}
	}

	// Token: 0x0400019C RID: 412
	public ERRoadNetwork roadNetwork;

	// Token: 0x0400019D RID: 413
	public ERRoad road;

	// Token: 0x0400019E RID: 414
	public GameObject go;

	// Token: 0x0400019F RID: 415
	public int currentElement;

	// Token: 0x040001A0 RID: 416
	public float distance;

	// Token: 0x040001A1 RID: 417
	public float speed = 5f;
}
