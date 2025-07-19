using System;
using System.Collections;
using PaintIn3D;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class CheckCondition : MonoBehaviour
{
	// Token: 0x060000B5 RID: 181 RVA: 0x00008206 File Offset: 0x00006406
	public void Check(GameObject Car)
	{
		base.StartCoroutine(this.FirstCheckCarCondition(Car));
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00008216 File Offset: 0x00006416
	private IEnumerator FirstCheckCarCondition(GameObject Car)
	{
		P3dChangeCounter[] targetLis = Car.GetComponentsInChildren<P3dChangeCounter>();
		foreach (P3dChangeCounter p3dChangeCounter in targetLis)
		{
			p3dChangeCounter.enabled = true;
			if (p3dChangeCounter.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter.Threshold == 0.1f)
			{
				p3dChangeCounter.Color = Car.GetComponent<MainCarProperties>().Color;
			}
		}
		yield return 0;
		yield return 0;
		yield return 0;
		if (Car != null)
		{
			if (Car.transform.position.y < 49f)
			{
				this.FirstCheckCarCondition(Car);
			}
			else
			{
				this.JobsManager.CleanRatio = 0.1f;
				this.JobsManager.NoRustRatio = 0.1f;
				this.JobsManager.PaintRatio = 0.1f;
				this.JobsManager.CleanRatioParts = 0f;
				this.JobsManager.NoRustRatioParts = 0f;
				this.JobsManager.PaintRatioParts = 0f;
				this.JobsManager.PaintGoodParts = 0f;
				this.JobsManager.RustGoodParts = 0f;
				this.JobsManager.StartValue = 0f;
				foreach (P3dChangeCounter p3dChangeCounter2 in targetLis)
				{
					float num = 1f - p3dChangeCounter2.Ratio;
					if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Washable && p3dChangeCounter2.Threshold == 0.7f)
					{
						if ((double)num > 0.6)
						{
							this.JobsManager.CleanRatio += 1f;
						}
						this.JobsManager.CleanRatioParts += 1f;
						p3dChangeCounter2.gameObject.GetComponent<CarProperties>().CleanRatio = num;
					}
					if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter2.Threshold == 0.5f)
					{
						this.JobsManager.NoRustRatio += num;
						this.JobsManager.NoRustRatioParts += 1f;
						p3dChangeCounter2.gameObject.GetComponent<CarProperties>().NoRustRatio = num;
						if (num > 0.95f)
						{
							this.JobsManager.RustGoodParts += 1f;
						}
					}
					if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter2.Threshold == 0.1f)
					{
						this.JobsManager.PaintRatio += num;
						this.JobsManager.PaintRatioParts += 1f;
						p3dChangeCounter2.gameObject.GetComponent<CarProperties>().PaintRatio = num;
						if (num > 0.9f)
						{
							this.JobsManager.PaintGoodParts += 1f;
						}
					}
					p3dChangeCounter2.enabled = false;
				}
				this.JobsManager.CleanRatio = this.JobsManager.CleanRatio / this.JobsManager.CleanRatioParts;
				this.JobsManager.NoRustRatio = this.JobsManager.RustGoodParts / this.JobsManager.NoRustRatioParts;
				this.JobsManager.PaintRatio = this.JobsManager.PaintGoodParts / this.JobsManager.PaintRatioParts;
				this.JobsManager.NoRuinedParts = true;
				this.JobsManager.NoWornParts = true;
				this.JobsManager.NoMeshDamagedParts = true;
				this.JobsManager.Realreward = 0f;
				foreach (CarProperties carProperties in Car.GetComponentsInChildren<CarProperties>())
				{
					if (carProperties.SinglePart)
					{
						if (carProperties.Condition < 0.4f)
						{
							this.JobsManager.NoWornParts = false;
							this.JobsManager.Realreward += carProperties.gameObject.GetComponent<Partinfo>().price;
						}
						if (carProperties.Condition < 0.1f)
						{
							this.JobsManager.NoWornParts = false;
							this.JobsManager.NoRuinedParts = false;
						}
						if (carProperties.MeshDamaged && !this.JobsManager.JobScript.CrashedCar)
						{
							this.JobsManager.NoMeshDamagedParts = false;
							this.JobsManager.Realreward += carProperties.gameObject.GetComponent<Partinfo>().price;
						}
						if (this.JobsManager.JobScript.CrashedCar && (carProperties.MeshLittleDamaged || carProperties.MeshDamaged))
						{
							this.JobsManager.Realreward += carProperties.gameObject.GetComponent<Partinfo>().price;
							carProperties.Condition = 0.05f;
						}
						if (carProperties.Condition > 0.4f)
						{
							this.JobsManager.StartValue += carProperties.gameObject.GetComponent<Partinfo>().price;
						}
					}
				}
				this.JobsManager.CurrentValue = this.JobsManager.StartValue;
				if (this.JobsManager.JobScript.RewardModifier > 0f)
				{
					this.JobsManager.Realreward = this.JobsManager.Realreward * this.JobsManager.JobScript.RewardModifier;
				}
				this.JobsManager.Realreward = this.JobsManager.Realreward * UnityEngine.Random.Range(0.9f, 1.1f);
				this.JobsManager.Realreward = Mathf.Round(this.JobsManager.Realreward);
				this.JobsManager.CheckJobProgress(true);
			}
		}
		yield break;
	}

	// Token: 0x04000139 RID: 313
	public JobsManager JobsManager;
}
