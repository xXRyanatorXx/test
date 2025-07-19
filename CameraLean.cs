using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class CameraLean : MonoBehaviour
{
	// Token: 0x06000620 RID: 1568 RVA: 0x00030DCD File Offset: 0x0002EFCD
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.leaned = false;
		this.ParentMain = base.transform.root.gameObject;
		this.coroutineAllowed = true;
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00030E04 File Offset: 0x0002F004
	private void Update()
	{
		if (!tools.sitting)
		{
			if (this.player.GetButtonDown("Lean") && this.coroutineAllowed)
			{
				base.Invoke("RunCoroutine", 0f);
			}
		}
		else
		{
			this.leaned = false;
		}
		if (this.player.GetButtonDown("Sprint") && this.leaned)
		{
			base.Invoke("RunCoroutine", 0f);
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00030E75 File Offset: 0x0002F075
	private void RunCoroutine()
	{
		base.StartCoroutine("leanfront");
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00030E83 File Offset: 0x0002F083
	private IEnumerator leanfront()
	{
		this.coroutineAllowed = false;
		if (!this.leaned)
		{
			for (float i = 0f; i >= -0.9f; i -= 0.05f)
			{
				base.transform.localPosition = new Vector3(0f, i * 0.7f, -i);
				yield return new WaitForSeconds(0f);
			}
			this.leaned = true;
			this.coroutineAllowed = true;
		}
		else
		{
			for (float i = -0.9f; i <= 0f; i += 0.05f)
			{
				base.transform.localPosition = new Vector3(0f, i * 0.7f, -i);
				yield return new WaitForSeconds(0f);
			}
			this.leaned = false;
			this.coroutineAllowed = true;
		}
		yield break;
	}

	// Token: 0x04000934 RID: 2356
	private bool leaned;

	// Token: 0x04000935 RID: 2357
	private bool coroutineAllowed;

	// Token: 0x04000936 RID: 2358
	public GameObject ParentMain;

	// Token: 0x04000937 RID: 2359
	private Player player;
}
