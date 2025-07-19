using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class GunShoot : MonoBehaviour
{
	// Token: 0x0600015F RID: 351 RVA: 0x0000B375 File Offset: 0x00009575
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.gunAim = base.GetComponentInParent<GunAim>();
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000B390 File Offset: 0x00009590
	private void Update()
	{
		if (Input.GetButtonDown("Fire1") && Time.time > this.nextFire && !this.gunAim.GetIsOutOfBounds())
		{
			this.nextFire = Time.time + this.fireRate;
			this.muzzleFlash.Play();
			this.cartridgeEjection.Play();
			this.anim.SetTrigger("Fire");
			RaycastHit hit;
			if (Physics.Raycast(this.gunEnd.position, this.gunEnd.forward, out hit, this.weaponRange))
			{
				this.HandleHit(hit);
			}
		}
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000B428 File Offset: 0x00009628
	private void HandleHit(RaycastHit hit)
	{
		if (hit.collider.sharedMaterial != null)
		{
			string name = hit.collider.sharedMaterial.name;
			if (name != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1044434307U)
				{
					if (num <= 329707512U)
					{
						if (num != 81868168U)
						{
							if (num != 329707512U)
							{
								return;
							}
							if (!(name == "WaterFilledExtinguish"))
							{
								return;
							}
							this.SpawnDecal(hit, this.waterLeakExtinguishEffect);
							this.SpawnDecal(hit, this.metalHitEffect);
						}
						else
						{
							if (!(name == "Wood"))
							{
								return;
							}
							this.SpawnDecal(hit, this.woodHitEffect);
							return;
						}
					}
					else if (num != 970575400U)
					{
						if (num != 1044434307U)
						{
							return;
						}
						if (!(name == "Sand"))
						{
							return;
						}
						this.SpawnDecal(hit, this.sandHitEffect);
						return;
					}
					else
					{
						if (!(name == "WaterFilled"))
						{
							return;
						}
						this.SpawnDecal(hit, this.waterLeakEffect);
						this.SpawnDecal(hit, this.metalHitEffect);
						return;
					}
				}
				else if (num <= 2840670588U)
				{
					if (num != 1842662042U)
					{
						if (num != 2840670588U)
						{
							return;
						}
						if (!(name == "Metal"))
						{
							return;
						}
						this.SpawnDecal(hit, this.metalHitEffect);
						return;
					}
					else
					{
						if (!(name == "Stone"))
						{
							return;
						}
						this.SpawnDecal(hit, this.stoneHitEffect);
						return;
					}
				}
				else if (num != 3966976176U)
				{
					if (num != 4022181330U)
					{
						return;
					}
					if (!(name == "Meat"))
					{
						return;
					}
					this.SpawnDecal(hit, this.fleshHitEffects[UnityEngine.Random.Range(0, this.fleshHitEffects.Length)]);
					return;
				}
				else
				{
					if (!(name == "Character"))
					{
						return;
					}
					this.SpawnDecal(hit, this.fleshHitEffects[UnityEngine.Random.Range(0, this.fleshHitEffects.Length)]);
					return;
				}
			}
		}
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000B5E7 File Offset: 0x000097E7
	private void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		UnityEngine.Object.Instantiate<GameObject>(prefab, hit.point, Quaternion.LookRotation(hit.normal)).transform.SetParent(hit.collider.transform);
	}

	// Token: 0x040001C8 RID: 456
	public float fireRate = 0.25f;

	// Token: 0x040001C9 RID: 457
	public float weaponRange = 20f;

	// Token: 0x040001CA RID: 458
	public Transform gunEnd;

	// Token: 0x040001CB RID: 459
	public ParticleSystem muzzleFlash;

	// Token: 0x040001CC RID: 460
	public ParticleSystem cartridgeEjection;

	// Token: 0x040001CD RID: 461
	public GameObject metalHitEffect;

	// Token: 0x040001CE RID: 462
	public GameObject sandHitEffect;

	// Token: 0x040001CF RID: 463
	public GameObject stoneHitEffect;

	// Token: 0x040001D0 RID: 464
	public GameObject waterLeakEffect;

	// Token: 0x040001D1 RID: 465
	public GameObject waterLeakExtinguishEffect;

	// Token: 0x040001D2 RID: 466
	public GameObject[] fleshHitEffects;

	// Token: 0x040001D3 RID: 467
	public GameObject woodHitEffect;

	// Token: 0x040001D4 RID: 468
	private float nextFire;

	// Token: 0x040001D5 RID: 469
	private Animator anim;

	// Token: 0x040001D6 RID: 470
	private GunAim gunAim;
}
