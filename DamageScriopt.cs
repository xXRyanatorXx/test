using System;
using UnityEngine;

// Token: 0x02000134 RID: 308
public class DamageScriopt : MonoBehaviour
{
	// Token: 0x06000690 RID: 1680 RVA: 0x0003522C File Offset: 0x0003342C
	private void Start()
	{
		this.originalVertices = new Vector3[this.meshFilters.Length][];
		for (int i = 0; i < this.meshFilters.Length; i++)
		{
			this.originalVertices[i] = this.meshFilters[i].mesh.vertices;
			this.meshFilters[i].mesh.MarkDynamic();
		}
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0003528C File Offset: 0x0003348C
	private void DeformationMesh(Mesh mesh, Transform localTransform, Vector3 contactPoint, Vector3 contactVelocity, int i)
	{
		bool flag = false;
		Vector3 a = localTransform.InverseTransformPoint(contactPoint);
		Vector3 a2 = localTransform.InverseTransformDirection(contactVelocity);
		Vector3[] vertices = mesh.vertices;
		for (int j = 0; j < vertices.Length; j++)
		{
			float magnitude = (a - vertices[j]).magnitude;
			if (magnitude <= this.deformationRadius)
			{
				vertices[j] += a2 * (this.deformationRadius - magnitude) * this.impactDamage;
				Vector3 vector = vertices[j] - this.originalVertices[i][j];
				if (vector.magnitude > this.maxDeformation)
				{
					vertices[j] = this.originalVertices[i][j] + vector.normalized * this.maxDeformation;
				}
				flag = true;
			}
		}
		if (flag)
		{
			mesh.vertices = vertices;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00035398 File Offset: 0x00033598
	private void OnCollisionEnter(Collision collision)
	{
		if (Time.time > this.nextTimeDeform && collision.relativeVelocity.magnitude > this.minVelocity && collision.gameObject.tag == "Vehicle")
		{
			this.isRepaired = false;
			this.damaged = true;
			Vector3 point = collision.contacts[0].point;
			Vector3 contactVelocity = collision.relativeVelocity * 0.02f;
			for (int i = 0; i < this.meshFilters.Length; i++)
			{
				if (this.meshFilters[i] != null)
				{
					this.DeformationMesh(this.meshFilters[i].mesh, this.meshFilters[i].transform, point, contactVelocity, i);
				}
			}
			if (this.collisionSounds.Length != 0)
			{
				AudioSource.PlayClipAtPoint(this.collisionSounds[UnityEngine.Random.Range(0, this.collisionSounds.Length)], base.transform.position, 0.5f);
			}
			this.nextTimeDeform = Time.time + this.delayTimeDeform;
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x000354A4 File Offset: 0x000336A4
	public void RestoreMesh()
	{
		if (this.damaged)
		{
			for (int i = 0; i < this.meshFilters.Length; i++)
			{
				Mesh mesh = this.meshFilters[i].mesh;
				Vector3[] vertices = mesh.vertices;
				Vector3[] array = this.originalVertices[i];
				for (int j = 0; j < vertices.Length; j++)
				{
					vertices[j] = array[j];
				}
				mesh.vertices = vertices;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
			}
			this.damaged = false;
		}
	}

	// Token: 0x04000A04 RID: 2564
	[SerializeField]
	private MeshFilter[] meshFilters;

	// Token: 0x04000A05 RID: 2565
	[SerializeField]
	private MeshCollider[] colliders;

	// Token: 0x04000A06 RID: 2566
	[SerializeField]
	private float impactDamage = 1f;

	// Token: 0x04000A07 RID: 2567
	[SerializeField]
	private float deformationRadius = 0.5f;

	// Token: 0x04000A08 RID: 2568
	[SerializeField]
	private float maxDeformation = 0.5f;

	// Token: 0x04000A09 RID: 2569
	[SerializeField]
	private float minVelocity = 2f;

	// Token: 0x04000A0A RID: 2570
	private float delayTimeDeform = 0.1f;

	// Token: 0x04000A0B RID: 2571
	private float minVertsDistanceToRestore = 0.002f;

	// Token: 0x04000A0C RID: 2572
	private float vertsRestoreSpeed = 2f;

	// Token: 0x04000A0D RID: 2573
	private Vector3[][] originalVertices;

	// Token: 0x04000A0E RID: 2574
	private float nextTimeDeform;

	// Token: 0x04000A0F RID: 2575
	private bool isRepairing;

	// Token: 0x04000A10 RID: 2576
	private bool isRepaired;

	// Token: 0x04000A11 RID: 2577
	public bool damaged;

	// Token: 0x04000A12 RID: 2578
	public AudioClip[] collisionSounds;
}
