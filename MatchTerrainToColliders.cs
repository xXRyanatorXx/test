using System;
using System.IO;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class MatchTerrainToColliders : MonoBehaviour
{
	// Token: 0x06000737 RID: 1847 RVA: 0x0003B4E0 File Offset: 0x000396E0
	private void GeneratePerimeterHeightRampAndFlange(float[,] heightMap, float[,] blendStencil, int distance)
	{
		int length = blendStencil.GetLength(0);
		int length2 = blendStencil.GetLength(1);
		float[][,] array = new float[distance + 1][,];
		float[,] array2 = new float[length, length2];
		int[] array3 = new int[]
		{
			0,
			1,
			1,
			0,
			0,
			-1,
			-1,
			0,
			1,
			1,
			-1,
			1,
			1,
			-1,
			-1,
			-1
		};
		int num = 4;
		float[,] array4 = blendStencil;
		for (int i = 0; i <= distance; i++)
		{
			array[i] = array4;
			float[,] array5 = new float[length, length2];
			for (int j = 0; j < length2; j++)
			{
				for (int k = 0; k < length; k++)
				{
					array5[k, j] = array4[k, j];
				}
			}
			if (i == distance)
			{
				break;
			}
			for (int l = 0; l < length2; l++)
			{
				for (int m = 0; m < length; m++)
				{
					if (array4[m, l] == 0f)
					{
						int num2 = 0;
						float num3 = 0f;
						for (int n = 0; n < num; n++)
						{
							int num4 = m + array3[n * 2];
							int num5 = l + array3[n * 2 + 1];
							if (num4 >= 0 && num4 < length && num5 >= 0 && num5 < length2 && array4[num4, num5] != 0f)
							{
								num3 += heightMap[num4, num5];
								num2++;
							}
						}
						if (num2 > 0)
						{
							array5[m, l] = 1f;
							array2[m, l] = num3 / (float)num2;
						}
					}
				}
			}
			for (int num6 = 0; num6 < length2; num6++)
			{
				for (int num7 = 0; num7 < length; num7++)
				{
					float num8 = array2[num7, num6];
					if (num8 > 0f)
					{
						heightMap[num7, num6] = num8;
					}
					array2[num7, num6] = 0f;
				}
			}
			array4 = array5;
		}
		for (int num9 = 0; num9 < length2; num9++)
		{
			for (int num10 = 0; num10 < length; num10++)
			{
				float num11 = 0f;
				for (int num12 = 0; num12 <= distance; num12++)
				{
					num11 += array[num12][num10, num9];
				}
				num11 /= (float)(distance + 1);
				blendStencil[num10, num9] = num11;
			}
		}
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0003B714 File Offset: 0x00039914
	private void BringTerrainToUndersideOfCollider()
	{
		Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			Debug.LogError("We must have at least one collider on ourselves or below us in the hierarchy. We will cast to it and match terrain to that contour.");
			return;
		}
		if (this.terrain)
		{
			TerrainData terrainData = this.terrain.terrainData;
			int heightmapResolution = terrainData.heightmapResolution;
			int heightmapResolution2 = terrainData.heightmapResolution;
			float[,] heights = terrainData.GetHeights(0, 0, heightmapResolution, heightmapResolution2);
			float[,] array = new float[heights.GetLength(0), heights.GetLength(1)];
			float[,] array2 = new float[heights.GetLength(0), heights.GetLength(1)];
			for (int i = 0; i < heightmapResolution2; i++)
			{
				for (int j = 0; j < heightmapResolution; j++)
				{
					Vector3 origin = this.terrain.transform.position + new Vector3((float)j * terrainData.size.x / (float)(heightmapResolution - 1), -10f, (float)i * terrainData.size.z / (float)(heightmapResolution2 - 1));
					Ray ray = new Ray(origin, Vector3.up);
					if (this.CastFromAbove)
					{
						origin.y = base.transform.position.y + terrainData.size.y + 10f;
						ray = new Ray(origin, Vector3.down);
					}
					bool flag = false;
					float num = 0f;
					Collider[] array3 = componentsInChildren;
					for (int k = 0; k < array3.Length; k++)
					{
						RaycastHit raycastHit;
						if (array3[k].Raycast(ray, out raycastHit, 1000f))
						{
							if (!flag)
							{
								num = raycastHit.point.y;
							}
							flag = true;
							if (this.CastFromAbove)
							{
								if (raycastHit.point.y > num)
								{
									num = raycastHit.point.y;
								}
							}
							else if (raycastHit.point.y < num)
							{
								num = raycastHit.point.y;
							}
						}
						if (flag)
						{
							float num2 = num / terrainData.size.y;
							array[i, j] = num2;
							array2[i, j] = 1f;
						}
					}
				}
			}
			if (this.PerimeterRampDistance > 0)
			{
				this.GeneratePerimeterHeightRampAndFlange(array, array2, this.PerimeterRampDistance);
			}
			for (int l = 0; l < heightmapResolution2; l++)
			{
				for (int m = 0; m < heightmapResolution; m++)
				{
					float num3 = array2[l, m];
					if (this.ApplyPerimeterRampCurve)
					{
						num3 = this.PerimeterRampCurve.Evaluate(num3);
					}
					heights[l, m] = Mathf.Lerp(heights[l, m], array[l, m], num3);
				}
			}
			terrainData.SetHeights(0, 0, heights);
			return;
		}
		this.terrain = UnityEngine.Object.FindObjectOfType<Terrain>();
		if (!this.terrain)
		{
			Debug.LogError("couldn't find a terrain");
			return;
		}
		Debug.LogWarning("Terrain not supplied; finding it myself. I found and assigned " + this.terrain.name + ", but I didn't do anything yet... click again to actually DO the modification.");
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0003B9F8 File Offset: 0x00039BF8
	private void WritePNG(float[,] array, string filename, bool normalize = false)
	{
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		Texture2D texture2D = new Texture2D(length, length2);
		Color[] array2 = new Color[length * length2];
		float num = 0f;
		float num2 = 1f;
		if (normalize)
		{
			num = 1f;
			num2 = 0f;
			for (int i = 0; i < length2; i++)
			{
				for (int j = 0; j < length; j++)
				{
					float num3 = array[j, i];
					if (num3 < num)
					{
						num = num3;
					}
					if (num3 > num2)
					{
						num2 = num3;
					}
				}
			}
			if (num2 <= num)
			{
				num = 0f;
				num2 = 1f;
			}
		}
		int num4 = 0;
		for (int k = 0; k < length2; k++)
		{
			for (int l = 0; l < length; l++)
			{
				float num5 = array[l, k];
				num5 -= num;
				num5 /= num2 - num;
				array2[num4] = new Color(num5, num5, num5);
				num4++;
			}
		}
		texture2D.SetPixels(array2);
		texture2D.Apply();
		byte[] bytes = texture2D.EncodeToPNG();
		UnityEngine.Object.DestroyImmediate(texture2D);
		filename += ".png";
		File.WriteAllBytes(filename, bytes);
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0003BB24 File Offset: 0x00039D24
	private void Debug_Microtest()
	{
		float[,] array = new float[3, 3];
		array[1, 1] = 0.5f;
		float[,] array2 = array;
		float[,] array3 = new float[3, 3];
		array3[1, 1] = 1f;
		float[,] array4 = array3;
		this.WritePNG(array2, "height-0", true);
		this.WritePNG(array4, "alpha-0", true);
		this.GeneratePerimeterHeightRampAndFlange(array2, array4, this.PerimeterRampDistance);
		this.WritePNG(array2, "height-1", true);
		this.WritePNG(array4, "alpha-1", true);
	}

	// Token: 0x04000B4B RID: 2891
	[Tooltip("Assign Terrain here if you like, otherwise we search for one.")]
	public Terrain terrain;

	// Token: 0x04000B4C RID: 2892
	[Tooltip("Default is to cast from below. This will cast from above and bring the terrain to match the TOP of our collider.")]
	public bool CastFromAbove;

	// Token: 0x04000B4D RID: 2893
	[Header("Related to smoothing around the edges.")]
	[Tooltip("Size of gaussian filter applied to change array. Set to zero for none")]
	public int PerimeterRampDistance;

	// Token: 0x04000B4E RID: 2894
	[Tooltip("Use Perimeter Ramp Curve in lieu of direct gaussian smooth.")]
	public bool ApplyPerimeterRampCurve;

	// Token: 0x04000B4F RID: 2895
	[Tooltip("Optional shaped ramp around perimeter.")]
	public AnimationCurve PerimeterRampCurve;

	// Token: 0x04000B50 RID: 2896
	[Header("Misc/Editor")]
	[Tooltip("Enable this if you want undo. It is SUPER-dog slow though, so I would leave it OFF.")]
	public bool EnableEditorUndo;
}
