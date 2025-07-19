using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000034 RID: 52
public class DeathWall : MonoBehaviour
{
	// Token: 0x060000F5 RID: 245 RVA: 0x00009836 File Offset: 0x00007A36
	public void StartThis()
	{
		if (PlayerPrefs.HasKey("DeathWall"))
		{
			if (PlayerPrefs.GetFloat("DeathWall") == 0f)
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.GameEnded = false;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00009878 File Offset: 0x00007A78
	private void Update()
	{
		base.transform.position = new Vector3(this.Player.transform.position.x, base.transform.position.y, base.transform.position.z - Time.deltaTime * this.speed);
		if (base.transform.position.z <= this.Player.transform.position.z)
		{
			this.GameOverCanvas.SetActive(true);
			this.DistanceFromStartText.text = Vector3.Distance(this.Player.transform.position, this.MapMagic.transform.position + this.Player.GetComponent<tools>().StartPosition).ToString("0") + "m";
			this.GameEnded = true;
		}
		if (Input.GetKeyDown(KeyCode.Escape) && this.GameEnded)
		{
			this.Quit();
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00009984 File Offset: 0x00007B84
	public void Quit()
	{
		SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x04000170 RID: 368
	public GameObject Player;

	// Token: 0x04000171 RID: 369
	public GameObject MapMagic;

	// Token: 0x04000172 RID: 370
	public GameObject GameOverCanvas;

	// Token: 0x04000173 RID: 371
	public Text DistanceFromStartText;

	// Token: 0x04000174 RID: 372
	public float speed;

	// Token: 0x04000175 RID: 373
	public bool GameEnded;
}
