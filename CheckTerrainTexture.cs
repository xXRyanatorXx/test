using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class CheckTerrainTexture : MonoBehaviour
{
	// Token: 0x060000BE RID: 190 RVA: 0x00008852 File Offset: 0x00006A52
	private void Start()
	{
		this.t = base.transform.parent.parent.Find("Main Terrain").GetComponent<Terrain>();
		this.playerTransform = base.gameObject.transform;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00008852 File Offset: 0x00006A52
	private void OnEnable()
	{
		this.t = base.transform.parent.parent.Find("Main Terrain").GetComponent<Terrain>();
		this.playerTransform = base.gameObject.transform;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000888A File Offset: 0x00006A8A
	private void Update()
	{
		this.GetTerrainTexture();
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00008892 File Offset: 0x00006A92
	public void GetTerrainTexture()
	{
		this.ConvertPosition(this.playerTransform.position);
		this.CheckTexture();
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000088AC File Offset: 0x00006AAC
	private void ConvertPosition(Vector3 playerPosition)
	{
		Vector3 vector = playerPosition - this.t.transform.position;
		Vector3 vector2 = new Vector3(vector.x / this.t.terrainData.size.x, 0f, vector.z / this.t.terrainData.size.z);
		float num = vector2.x * (float)this.t.terrainData.alphamapWidth;
		float num2 = vector2.z * (float)this.t.terrainData.alphamapHeight;
		this.posX = (int)num;
		this.posZ = (int)num2;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00008954 File Offset: 0x00006B54
	private void CheckTexture()
	{
		float[,,] alphamaps = this.t.terrainData.GetAlphamaps(this.posX, this.posZ, 1, 1);
		this.textureValues[0] = alphamaps[0, 0, 0];
		this.textureValues[1] = alphamaps[0, 0, 1];
		this.textureValues[2] = alphamaps[0, 0, 2];
		this.textureValues[3] = alphamaps[0, 0, 3];
		this.textureValues[4] = alphamaps[0, 0, 4];
		this.textureValues[5] = alphamaps[0, 0, 5];
		this.textureValues[6] = alphamaps[0, 0, 6];
		this.textureValues[7] = alphamaps[0, 0, 7];
		this.textureValues[8] = alphamaps[0, 0, 8];
		this.textureValues[9] = alphamaps[0, 0, 9];
	}

	// Token: 0x0400013F RID: 319
	public Transform playerTransform;

	// Token: 0x04000140 RID: 320
	public Terrain t;

	// Token: 0x04000141 RID: 321
	public int posX;

	// Token: 0x04000142 RID: 322
	public int posZ;

	// Token: 0x04000143 RID: 323
	public float[] textureValues;
}
