using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class Deform : MonoBehaviour
{
	// Token: 0x06000633 RID: 1587 RVA: 0x00031447 File Offset: 0x0002F647
	private void Start()
	{
		this.startingVerticies = this.filter.mesh.vertices;
		this.meshVerticies = this.filter.mesh.vertices;
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00031478 File Offset: 0x0002F678
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 5f && collision.impulse.magnitude > this.minDamage)
		{
			if (this.collisionSounds.Length != 0)
			{
				AudioSource.PlayClipAtPoint(this.collisionSounds[UnityEngine.Random.Range(0, this.collisionSounds.Length)], base.transform.position, 0.5f);
			}
			foreach (ContactPoint contactPoint in collision.contacts)
			{
				for (int j = 0; j < this.meshVerticies.Length; j++)
				{
					Vector3 vector = this.meshVerticies[j];
					Vector3 vector2 = base.transform.InverseTransformPoint(contactPoint.point);
					float num = Vector3.Distance(vector, vector2);
					float num2 = Vector3.Distance(this.startingVerticies[j], vector);
					if (num < this.deformRadius && num2 < this.maxDeform)
					{
						float num3 = 1f - num / this.deformRadius * this.damageFalloff;
						float num4 = vector2.x * num3;
						float num5 = vector2.y * num3;
						float num6 = vector2.z * num3;
						num4 = Mathf.Clamp(num4, 0f, this.maxDeform);
						num5 = Mathf.Clamp(num5, 0f, this.maxDeform);
						num6 = Mathf.Clamp(num6, 0f, this.maxDeform);
						Vector3 a = new Vector3(num4, num5, num6);
						this.meshVerticies[j] -= a * this.damageMultiplier;
					}
				}
			}
			this.UpdateMeshVerticies();
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0003163C File Offset: 0x0002F83C
	private void UpdateMeshVerticies()
	{
		this.filter.mesh.vertices = this.meshVerticies;
		this.coll.sharedMesh = this.filter.mesh;
	}

	// Token: 0x04000950 RID: 2384
	[Range(0f, 10f)]
	public float deformRadius = 0.2f;

	// Token: 0x04000951 RID: 2385
	[Range(0f, 10f)]
	public float maxDeform = 0.1f;

	// Token: 0x04000952 RID: 2386
	[Range(0f, 1f)]
	public float damageFalloff = 1f;

	// Token: 0x04000953 RID: 2387
	[Range(0f, 10f)]
	public float damageMultiplier = 1f;

	// Token: 0x04000954 RID: 2388
	[Range(0f, 100000f)]
	public float minDamage = 1f;

	// Token: 0x04000955 RID: 2389
	public AudioClip[] collisionSounds;

	// Token: 0x04000956 RID: 2390
	public Rigidbody physics;

	// Token: 0x04000957 RID: 2391
	private Vector3[] startingVerticies;

	// Token: 0x04000958 RID: 2392
	private Vector3[] meshVerticies;

	// Token: 0x04000959 RID: 2393
	public MeshCollider coll;

	// Token: 0x0400095A RID: 2394
	public MeshFilter filter;
}
