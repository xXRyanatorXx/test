using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200023A RID: 570
[ExecuteInEditMode]
public class PW_Terrain_Grass_Manager : MonoBehaviour
{
	// Token: 0x06000DA4 RID: 3492 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x00092CB5 File Offset: 0x00090EB5
	private IEnumerator wait()
	{
		yield return new WaitForSeconds(1f);
		this.TerrainDataRecheck();
		base.StartCoroutine(this.wait());
		yield break;
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x00092CC4 File Offset: 0x00090EC4
	private void Update()
	{
		this.TerrainDataRecheck();
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00092CCC File Offset: 0x00090ECC
	private void TerrainDataRecheck()
	{
		Shader.SetGlobalColor("PW_Grass_Color", this.TerrainGrassColorOverride);
		if (this.CurrentTerrain != null)
		{
			this.TerrainPosition = this.CurrentTerrain.transform.position;
		}
		Shader.SetGlobalFloat("TerrainTexResolutionX", this.TerrainTexResolutionX);
		Shader.SetGlobalFloat("TerrainTexResolutionY", this.TerrainTexResolutionY);
		Shader.SetGlobalTexture("PW_Splat_Grass", this.PW_Splat_Grass);
	}

	// Token: 0x04001628 RID: 5672
	public Texture2D PW_Splat_Grass;

	// Token: 0x04001629 RID: 5673
	public Terrain CurrentTerrain;

	// Token: 0x0400162A RID: 5674
	public Color TerrainGrassColorOverride = Color.white;

	// Token: 0x0400162B RID: 5675
	public Vector3 TerrainPosition;

	// Token: 0x0400162C RID: 5676
	public float TerrainTexResolutionX = 2048f;

	// Token: 0x0400162D RID: 5677
	public float TerrainTexResolutionY = 2048f;
}
