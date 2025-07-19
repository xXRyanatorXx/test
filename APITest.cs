using System;
using Gaia;
using UnityEngine;

// Token: 0x0200023D RID: 573
public class APITest : MonoBehaviour
{
	// Token: 0x06000DB1 RID: 3505 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x00092E2C File Offset: 0x0009102C
	public void CreateTerrainButton()
	{
		WorldCreationSettings worldCreationSettings = ScriptableObject.CreateInstance<WorldCreationSettings>();
		worldCreationSettings.m_xTiles = this.xTiles;
		worldCreationSettings.m_zTiles = this.zTiles;
		worldCreationSettings.m_tileSize = this.tileSize;
		worldCreationSettings.m_tileHeight = this.tileHeight;
		worldCreationSettings.m_seaLevel = this.seaLevel;
		worldCreationSettings.m_createInScene = this.createTerrainsInScenes;
		worldCreationSettings.m_autoUnloadScenes = this.autoUnloadScenes;
		worldCreationSettings.m_applyFloatingPointFix = this.applyFloatingPointFix;
		GaiaSessionManager.CreateWorld(worldCreationSettings, this.executeWorldCreation);
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x00092EAC File Offset: 0x000910AC
	public void StampButton()
	{
		StamperSettings stamperSettings = ScriptableObject.CreateInstance<StamperSettings>();
		stamperSettings.m_width = 150f;
		stamperSettings.m_imageMasks = new ImageMask[]
		{
			new ImageMask
			{
				m_operation = ImageMaskOperation.ImageMask,
				ImageMaskTexture = this.stamperTestTexture
			}
		};
		stamperSettings.m_y = 25.0;
		GaiaSessionManager.Stamp(stamperSettings, this.executeStamp, null, true);
	}

	// Token: 0x04001632 RID: 5682
	[Header("World Creation Settings")]
	public int xTiles = 1;

	// Token: 0x04001633 RID: 5683
	public int zTiles = 1;

	// Token: 0x04001634 RID: 5684
	public int tileSize = 1024;

	// Token: 0x04001635 RID: 5685
	public float tileHeight = 1000f;

	// Token: 0x04001636 RID: 5686
	public int seaLevel = 50;

	// Token: 0x04001637 RID: 5687
	public bool createTerrainsInScenes;

	// Token: 0x04001638 RID: 5688
	public bool autoUnloadScenes;

	// Token: 0x04001639 RID: 5689
	public bool applyFloatingPointFix;

	// Token: 0x0400163A RID: 5690
	public bool executeWorldCreation = true;

	// Token: 0x0400163B RID: 5691
	[Header("Stamper Settings")]
	public Texture2D stamperTestTexture;

	// Token: 0x0400163C RID: 5692
	public bool executeStamp = true;
}
