using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D9 RID: 217
[ExecuteInEditMode]
public class FCP_SpriteMeshEditor : MonoBehaviour
{
	// Token: 0x0600049A RID: 1178 RVA: 0x00027388 File Offset: 0x00025588
	private void Update()
	{
		int settingHash = this.GetSettingHash();
		if (settingHash != 0 && settingHash != this.bufferedHash)
		{
			this.MakeMesh(this.sprite, this.x, this.y, this.meshType);
			Image component = base.GetComponent<Image>();
			if (component)
			{
				component.useSpriteMesh = false;
				component.useSpriteMesh = true;
			}
			this.bufferedHash = settingHash;
		}
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x000273EC File Offset: 0x000255EC
	private int GetSettingHash()
	{
		if (this.sprite == null || this.x <= 0 || this.y <= 0)
		{
			return 0;
		}
		return this.sprite.GetHashCode() * (this.x ^ 136) * (this.y ^ 1342) * (int)(this.meshType + 1 ^ (FCP_SpriteMeshEditor.MeshType)99999);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00027450 File Offset: 0x00025650
	private void MakeMesh(Sprite sprite, int x, int y, FCP_SpriteMeshEditor.MeshType meshtype)
	{
		bool flag = this.meshType == FCP_SpriteMeshEditor.MeshType.CenterPoint;
		int num = x + 1;
		int num2 = y + 1;
		int num3 = num * num2;
		Vector2[] array;
		ushort[] array2;
		if (flag)
		{
			array = new Vector2[num3 + x * y];
			array2 = new ushort[x * y * 12];
		}
		else
		{
			array = new Vector2[num3];
			array2 = new ushort[x * y * 6];
		}
		for (int i = 0; i < num; i++)
		{
			float num4 = (float)i / (float)x;
			for (int j = 0; j < num2; j++)
			{
				float num5 = (float)j / (float)y;
				array[num * j + i] = new Vector2(num4, num5);
			}
		}
		if (flag)
		{
			for (int k = 0; k < x; k++)
			{
				float num6 = ((float)k + 0.5f) / (float)x;
				for (int l = 0; l < y; l++)
				{
					float num7 = ((float)l + 0.5f) / (float)y;
					array[l * x + k + num3] = new Vector2(num6, num7);
				}
			}
			for (int m = 0; m < x; m++)
			{
				for (int n = 0; n < y; n++)
				{
					int num8 = 12 * (n * x + m);
					int num9 = n * num + m;
					ushort num10 = (ushort)(n * x + m + num3);
					array2[num8 + 11] = (array2[num8] = (ushort)num9);
					array2[num8 + 3] = (array2[num8 + 2] = (ushort)(num9 + 1));
					array2[num8 + 6] = (array2[num8 + 5] = (ushort)(num9 + num + 1));
					array2[num8 + 9] = (array2[num8 + 8] = (ushort)(num9 + num));
					array2[num8 + 1] = (array2[num8 + 4] = (array2[num8 + 7] = (array2[num8 + 10] = num10)));
				}
			}
		}
		else if (meshtype == FCP_SpriteMeshEditor.MeshType.forward)
		{
			for (int num11 = 0; num11 < x; num11++)
			{
				for (int num12 = 0; num12 < y; num12++)
				{
					int num13 = 6 * (num12 * x + num11);
					int num14 = num12 * num + num11;
					array2[num13 + 5] = (array2[num13 + 1] = (ushort)num14);
					array2[num13] = (ushort)(num14 + 1);
					array2[num13 + 4] = (array2[num13 + 2] = (ushort)(num14 + num + 1));
					array2[num13 + 3] = (ushort)(num14 + num);
				}
			}
		}
		else if (this.meshType == FCP_SpriteMeshEditor.MeshType.backward)
		{
			for (int num15 = 0; num15 < x; num15++)
			{
				for (int num16 = 0; num16 < y; num16++)
				{
					int num17 = 6 * (num16 * x + num15);
					int num18 = num16 * num + num15;
					array2[num17] = (ushort)num18;
					array2[num17 + 4] = (array2[num17 + 2] = (ushort)(num18 + 1));
					array2[num17 + 3] = (ushort)(num18 + num + 1);
					array2[num17 + 5] = (array2[num17 + 1] = (ushort)(num18 + num));
				}
			}
		}
		sprite.OverrideGeometry(array, array2);
	}

	// Token: 0x04000659 RID: 1625
	public int x;

	// Token: 0x0400065A RID: 1626
	public int y;

	// Token: 0x0400065B RID: 1627
	public FCP_SpriteMeshEditor.MeshType meshType;

	// Token: 0x0400065C RID: 1628
	public Sprite sprite;

	// Token: 0x0400065D RID: 1629
	private int bufferedHash;

	// Token: 0x020000DA RID: 218
	public enum MeshType
	{
		// Token: 0x0400065F RID: 1631
		CenterPoint,
		// Token: 0x04000660 RID: 1632
		forward,
		// Token: 0x04000661 RID: 1633
		backward
	}
}
