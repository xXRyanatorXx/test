using System;
using AwesomeTechnologies;
using AwesomeTechnologies.VegetationStudio;
using AwesomeTechnologies.VegetationSystem;
using AwesomeTechnologies.VegetationSystem.Biomes;
using EasyRoads3Dv3;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class ERVegetationStudio : ScriptableObject
{
	// Token: 0x0600012B RID: 299 RVA: 0x0000A87F File Offset: 0x00008A7F
	public static bool VegetationStudio()
	{
		return false;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000A882 File Offset: 0x00008A82
	public static bool VegetationStudioPro()
	{
		return true;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000A888 File Offset: 0x00008A88
	public static void CreateVegetationMaskLine(GameObject go, float grassPerimeter, float plantPerimeter, float treePerimeter, float objectPerimeter, float largeObjectPerimeter)
	{
		VegetationMaskLine vegetationMaskLine = go.GetComponent<VegetationMaskLine>();
		if (vegetationMaskLine == null)
		{
			vegetationMaskLine = go.AddComponent<VegetationMaskLine>();
		}
		VegetationMask vegetationMask = vegetationMaskLine;
		vegetationMaskLine.AdditionalGrassPerimiterMax = grassPerimeter;
		vegetationMask.AdditionalGrassPerimiter = grassPerimeter;
		VegetationMask vegetationMask2 = vegetationMaskLine;
		vegetationMaskLine.AdditionalPlantPerimiterMax = plantPerimeter;
		vegetationMask2.AdditionalPlantPerimiter = plantPerimeter;
		VegetationMask vegetationMask3 = vegetationMaskLine;
		vegetationMaskLine.AdditionalTreePerimiterMax = treePerimeter;
		vegetationMask3.AdditionalTreePerimiter = treePerimeter;
		VegetationMask vegetationMask4 = vegetationMaskLine;
		vegetationMaskLine.AdditionalObjectPerimiterMax = objectPerimeter;
		vegetationMask4.AdditionalObjectPerimiter = objectPerimeter;
		VegetationMask vegetationMask5 = vegetationMaskLine;
		vegetationMaskLine.AdditionalLargeObjectPerimiterMax = largeObjectPerimeter;
		vegetationMask5.AdditionalLargeObjectPerimiter = largeObjectPerimeter;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000A900 File Offset: 0x00008B00
	public static void UpdateVegetationMaskLine(GameObject go, ERVSData[] vsData, float grassPerimeter, float plantPerimeter, float treePerimeter, float objectPerimeter, float largeObjectPerimeter)
	{
		VegetationMaskLine vegetationMaskLine = go.GetComponent<VegetationMaskLine>();
		if (vegetationMaskLine == null)
		{
			vegetationMaskLine = go.AddComponent<VegetationMaskLine>();
		}
		VegetationMask vegetationMask = vegetationMaskLine;
		vegetationMaskLine.AdditionalGrassPerimiterMax = grassPerimeter;
		vegetationMask.AdditionalGrassPerimiter = grassPerimeter;
		VegetationMask vegetationMask2 = vegetationMaskLine;
		vegetationMaskLine.AdditionalPlantPerimiterMax = plantPerimeter;
		vegetationMask2.AdditionalPlantPerimiter = plantPerimeter;
		VegetationMask vegetationMask3 = vegetationMaskLine;
		vegetationMaskLine.AdditionalTreePerimiterMax = treePerimeter;
		vegetationMask3.AdditionalTreePerimiter = treePerimeter;
		VegetationMask vegetationMask4 = vegetationMaskLine;
		vegetationMaskLine.AdditionalObjectPerimiterMax = objectPerimeter;
		vegetationMask4.AdditionalObjectPerimiter = objectPerimeter;
		VegetationMask vegetationMask5 = vegetationMaskLine;
		vegetationMaskLine.AdditionalLargeObjectPerimiterMax = largeObjectPerimeter;
		vegetationMask5.AdditionalLargeObjectPerimiter = largeObjectPerimeter;
		vegetationMaskLine.ClearNodes();
		foreach (ERVSData ervsdata in vsData)
		{
			vegetationMaskLine.AddNodeToEnd(ervsdata.position, ervsdata.width, ervsdata.active);
		}
		vegetationMaskLine.UpdateVegetationMask();
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000A9B7 File Offset: 0x00008BB7
	public static void UpdateHeightmap(Bounds bounds)
	{
		VegetationStudioManager.RefreshTerrainHeightMap();
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000A9BE File Offset: 0x00008BBE
	public static void RemoveVegetationMaskLine(GameObject go)
	{
		if (go.GetComponent<VegetationMaskLine>() != null)
		{
			UnityEngine.Object.DestroyImmediate(go.GetComponent<VegetationMaskLine>());
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000A9DC File Offset: 0x00008BDC
	public static void CreateBiomeArea(GameObject go, float distance, float blendDistance, float noise)
	{
		BiomeMaskArea biomeMaskArea = go.GetComponent<BiomeMaskArea>();
		if (biomeMaskArea == null)
		{
			biomeMaskArea = go.AddComponent<BiomeMaskArea>();
			biomeMaskArea.BiomeType = BiomeType.Road;
		}
		biomeMaskArea.BlendDistance = blendDistance;
		if (noise > 0f)
		{
			biomeMaskArea.UseNoise = true;
			biomeMaskArea.NoiseScale = noise;
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000AA28 File Offset: 0x00008C28
	public static void UpdateBiomeArea(GameObject go, ERVSData[] vsData, float distance, float blendDistance, float noise)
	{
		BiomeMaskArea biomeMaskArea = go.GetComponent<BiomeMaskArea>();
		if (biomeMaskArea == null)
		{
			biomeMaskArea = go.AddComponent<BiomeMaskArea>();
			biomeMaskArea.BiomeType = BiomeType.Road;
		}
		biomeMaskArea.BlendDistance = blendDistance;
		if (noise > 0f)
		{
			biomeMaskArea.UseNoise = true;
			biomeMaskArea.NoiseScale = noise;
		}
		biomeMaskArea.ClearNodes();
		distance += blendDistance;
		foreach (ERVSData ervsdata in vsData)
		{
			biomeMaskArea.AddNode(ervsdata.leftPosition + distance * -ervsdata.dir);
			biomeMaskArea.AddNode(ervsdata.rightPosition + distance * ervsdata.dir);
		}
		biomeMaskArea.UpdateBiomeMask();
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000AADB File Offset: 0x00008CDB
	public static void RemoveBiomeArea(GameObject go)
	{
		if (go.GetComponent<BiomeMaskArea>() != null)
		{
			UnityEngine.Object.DestroyImmediate(go.GetComponent<BiomeMaskArea>());
		}
	}
}
