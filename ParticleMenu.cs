using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004F RID: 79
public class ParticleMenu : MonoBehaviour
{
	// Token: 0x06000168 RID: 360 RVA: 0x0000B6B4 File Offset: 0x000098B4
	private void Start()
	{
		this.Navigate(0);
		this.currentIndex = 0;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000B6C4 File Offset: 0x000098C4
	public void Navigate(int i)
	{
		this.currentIndex = (this.particleSystems.Length + this.currentIndex + i) % this.particleSystems.Length;
		if (this.currentGO != null)
		{
			UnityEngine.Object.Destroy(this.currentGO);
		}
		this.currentGO = UnityEngine.Object.Instantiate<GameObject>(this.particleSystems[this.currentIndex].particleSystemGO, this.spawnLocation.position + this.particleSystems[this.currentIndex].particlePosition, Quaternion.Euler(this.particleSystems[this.currentIndex].particleRotation));
		this.gunGameObject.SetActive(this.particleSystems[this.currentIndex].isWeaponEffect);
		this.title.text = this.particleSystems[this.currentIndex].title;
		this.description.text = this.particleSystems[this.currentIndex].description;
		this.navigationDetails.text = (this.currentIndex + 1).ToString() + " out of " + this.particleSystems.Length.ToString();
	}

	// Token: 0x040001DF RID: 479
	public ParticleExamples[] particleSystems;

	// Token: 0x040001E0 RID: 480
	public GameObject gunGameObject;

	// Token: 0x040001E1 RID: 481
	private int currentIndex;

	// Token: 0x040001E2 RID: 482
	private GameObject currentGO;

	// Token: 0x040001E3 RID: 483
	public Transform spawnLocation;

	// Token: 0x040001E4 RID: 484
	public Text title;

	// Token: 0x040001E5 RID: 485
	public Text description;

	// Token: 0x040001E6 RID: 486
	public Text navigationDetails;
}
