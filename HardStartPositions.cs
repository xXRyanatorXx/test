using System;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class HardStartPositions : MonoBehaviour
{
	// Token: 0x0600050E RID: 1294 RVA: 0x00029DF8 File Offset: 0x00027FF8
	public void Awake()
	{
		if (PlayerPrefs.HasKey("HardStart") && PlayerPrefs.GetFloat("HardStart") == 1f)
		{
			this.player.position = this.playerPOS.position;
			this.sign.position = this.signPOS.position;
			this.table.position = this.tablePOS.position;
			this.toolSpawner.position = this.toolSpawnerPOS.position;
			this.player.rotation = this.playerPOS.rotation;
			this.sign.rotation = this.signPOS.rotation;
			this.table.rotation = this.tablePOS.rotation;
			this.toolSpawner.rotation = this.toolSpawnerPOS.rotation;
			this.GarageOn.SetActive(true);
			this.GarageOff.SetActive(false);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040006C5 RID: 1733
	public Transform player;

	// Token: 0x040006C6 RID: 1734
	public Transform sign;

	// Token: 0x040006C7 RID: 1735
	public Transform table;

	// Token: 0x040006C8 RID: 1736
	public Transform toolSpawner;

	// Token: 0x040006C9 RID: 1737
	public Transform playerPOS;

	// Token: 0x040006CA RID: 1738
	public Transform signPOS;

	// Token: 0x040006CB RID: 1739
	public Transform tablePOS;

	// Token: 0x040006CC RID: 1740
	public Transform toolSpawnerPOS;

	// Token: 0x040006CD RID: 1741
	public GameObject GarageOn;

	// Token: 0x040006CE RID: 1742
	public GameObject GarageOff;
}
