using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class MeshCollisionScriptAI : MonoBehaviour
{
	// Token: 0x0600006F RID: 111 RVA: 0x00006DE7 File Offset: 0x00004FE7
	private void Start()
	{
		this.sqrDemRange = this.demolutionRange * this.demolutionRange;
		if (this.loseAftCollisions == 0)
		{
			this.fixedMesh = true;
		}
		else
		{
			this.fixedMesh = false;
		}
		this.collisionParticlesPrefab = Resources.Load<Transform>("CollisionParticles");
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00006E24 File Offset: 0x00005024
	private void FixedUpdate()
	{
		if (this.collisionHappened)
		{
			this.CollisionCalculator();
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00006E34 File Offset: 0x00005034
	public void OnTriggerEnter(Collider objCollided)
	{
		if (objCollided)
		{
			this.loseAftCollisions--;
			if (!this.fixedMesh && this.loseAftCollisions <= 0)
			{
				base.transform.parent = null;
				base.StartCoroutine(this.LoseObjectCoroutine());
			}
			this.collisionHappened = true;
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00006E88 File Offset: 0x00005088
	private void CollisionCalculator()
	{
		if (this.hitPoint != null)
		{
			if (this.collisionParticlesON && this.collisionParticles == null)
			{
				this.collisionParticles = UnityEngine.Object.Instantiate<Transform>(this.collisionParticlesPrefab);
				this.collisionParticles.GetComponent<ParticleSystem>().Emit(10);
				this.collisionParticles.transform.position = this.hitPoint.contacts[0].point;
				UnityEngine.Object.Destroy(this.collisionParticles.gameObject, 5f);
			}
			Vector3 a = this.hitPoint.relativeVelocity;
			a *= this.yForceDamp;
			Vector3 vector = base.transform.position - this.hitPoint.contacts[0].point;
			float num = a.magnitude * Vector3.Dot(this.hitPoint.contacts[0].normal, vector.normalized);
			this.OnMeshForce(this.hitPoint.contacts[0].point, Mathf.Clamp01(num / this.maxCollisionStrength));
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00006FA8 File Offset: 0x000051A8
	public IEnumerator LoseObjectCoroutine()
	{
		yield return new WaitForSeconds(0.1f);
		base.transform.gameObject.GetComponent<MeshCollider>().isTrigger = false;
		if (base.transform.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.transform.gameObject.AddComponent<Rigidbody>();
		}
		base.transform.gameObject.GetComponent<Rigidbody>().mass = 50f;
		yield break;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00006FB8 File Offset: 0x000051B8
	public void OnMeshForce(Vector3 originPos, float force)
	{
		force = Mathf.Clamp01(force);
		Vector3[] vertices = this.meshFilter.mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 point = Vector3.Scale(vertices[i], base.transform.localScale);
			Vector3 vector = this.meshFilter.transform.position + this.meshFilter.transform.rotation * point;
			Vector3 a = vector - originPos;
			Vector3 b = base.transform.position - vector;
			b.y = 0f;
			if (a.sqrMagnitude < this.sqrDemRange)
			{
				float num = Mathf.Clamp01(a.sqrMagnitude / this.sqrDemRange);
				float d = force * (1f - num) * this.maxMoveDelta;
				Vector3 point2 = Vector3.Slerp(a, b, this.impactDirManipulator).normalized * d;
				vertices[i] += Quaternion.Inverse(base.transform.rotation) * point2;
			}
		}
		this.meshFilter.mesh.vertices = vertices;
		this.meshFilter.mesh.RecalculateBounds();
		this.hitPoint = null;
		this.collisionHappened = false;
	}

	// Token: 0x040000F0 RID: 240
	[HideInInspector]
	public float maxCollisionStrength = 50f;

	// Token: 0x040000F1 RID: 241
	[HideInInspector]
	public float demolutionRange = 100f;

	// Token: 0x040000F2 RID: 242
	[HideInInspector]
	public MeshFilter meshFilter;

	// Token: 0x040000F3 RID: 243
	private float maxMoveDelta = 1.5f;

	// Token: 0x040000F4 RID: 244
	private float yForceDamp = 1f;

	// Token: 0x040000F5 RID: 245
	private float impactDirManipulator = 0.5f;

	// Token: 0x040000F6 RID: 246
	private float sqrDemRange;

	// Token: 0x040000F7 RID: 247
	[HideInInspector]
	public Collision hitPoint;

	// Token: 0x040000F8 RID: 248
	[HideInInspector]
	public bool collisionHappened;

	// Token: 0x040000F9 RID: 249
	[HideInInspector]
	public int loseAftCollisions;

	// Token: 0x040000FA RID: 250
	private bool fixedMesh = true;

	// Token: 0x040000FB RID: 251
	[HideInInspector]
	public bool collisionParticlesON;

	// Token: 0x040000FC RID: 252
	private Transform collisionParticles;

	// Token: 0x040000FD RID: 253
	private Transform collisionParticlesPrefab;
}
