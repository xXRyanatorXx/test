using System;
using Rewired;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class CloseEsckey : MonoBehaviour
{
	// Token: 0x060000CB RID: 203 RVA: 0x00008D9C File Offset: 0x00006F9C
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00008DAF File Offset: 0x00006FAF
	private void Update()
	{
		if (!this.JustSetUIisOpen && (Input.GetKeyDown(KeyCode.Escape) || this.player.GetButtonDown("Escape")))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00008DE0 File Offset: 0x00006FE0
	private void OnDisable()
	{
		tools.UIisOpen = false;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00008DE8 File Offset: 0x00006FE8
	private void OnEnable()
	{
		tools.UIisOpen = true;
	}

	// Token: 0x0400014F RID: 335
	private Player player;

	// Token: 0x04000150 RID: 336
	public bool JustSetUIisOpen;
}
