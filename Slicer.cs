using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class Slicer : MonoBehaviour
{
	// Token: 0x06000B60 RID: 2912 RVA: 0x0007CAF0 File Offset: 0x0007ACF0
	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
		}
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0007CB11 File Offset: 0x0007AD11
	private void OnCollisionEnter(Collision other)
	{
		if (this.timer <= 0f && other.gameObject.tag == "Slicable")
		{
			this.timer += 1f;
			this.slice(other);
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0007CB50 File Offset: 0x0007AD50
	private void slice(Collision other)
	{
		Collider component = base.GetComponent<Collider>();
		Vector3 a = component.bounds.center;
		a += base.transform.up * component.bounds.extents.y;
		Vector3 vector = component.bounds.center;
		vector += base.transform.up * component.bounds.extents.y;
		vector += base.transform.right * component.bounds.extents.x;
		Vector3 vector2 = component.bounds.center;
		vector2 += base.transform.up * -component.bounds.extents.y;
		vector2 += base.transform.right * component.bounds.extents.x;
		Plane plane = new Plane(a, vector, vector2);
		Transform transform = other.transform;
		Mesh mesh = other.gameObject.GetComponent<MeshFilter>().mesh;
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		List<Vector3> list = new List<Vector3>();
		List<Slicer.Triangle> list2 = new List<Slicer.Triangle>();
		List<Slicer.Triangle> list3 = new List<Slicer.Triangle>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			List<Vector3> list4 = new List<Vector3>();
			int num = triangles[i];
			int num2 = triangles[i + 1];
			int num3 = triangles[i + 2];
			Vector3 vector3 = transform.TransformPoint(vertices[num]);
			Vector3 vector4 = transform.TransformPoint(vertices[num2]);
			Vector3 vector5 = transform.TransformPoint(vertices[num3]);
			Vector3 dir = Vector3.Cross(vector3 - vector4, vector3 - vector5);
			Vector3 direction = vector4 - vector3;
			float num4;
			if (plane.Raycast(new Ray(vector3, direction), out num4) && num4 <= direction.magnitude)
			{
				Vector3 item = vector3 + num4 * direction.normalized;
				list.Add(item);
				list4.Add(item);
			}
			direction = vector5 - vector4;
			if (plane.Raycast(new Ray(vector4, direction), out num4) && num4 <= direction.magnitude)
			{
				Vector3 item2 = vector4 + num4 * direction.normalized;
				list.Add(item2);
				list4.Add(item2);
			}
			direction = vector5 - vector3;
			if (plane.Raycast(new Ray(vector3, direction), out num4) && num4 <= direction.magnitude)
			{
				Vector3 item3 = vector3 + num4 * direction.normalized;
				list.Add(item3);
				list4.Add(item3);
			}
			if (list4.Count > 0)
			{
				List<Vector3> list5 = new List<Vector3>();
				List<Vector3> list6 = new List<Vector3>();
				list5.AddRange(list4);
				list6.AddRange(list4);
				if (plane.GetSide(vector3))
				{
					list5.Add(vector3);
				}
				else
				{
					list6.Add(vector3);
				}
				if (plane.GetSide(vector4))
				{
					list5.Add(vector4);
				}
				else
				{
					list6.Add(vector4);
				}
				if (plane.GetSide(vector5))
				{
					list5.Add(vector5);
				}
				else
				{
					list6.Add(vector5);
				}
				if (list5.Count == 3)
				{
					Slicer.Triangle triangle = new Slicer.Triangle
					{
						v1 = list5[1],
						v2 = list5[0],
						v3 = list5[2]
					};
					Slicer.Triangle item4 = triangle;
					item4.matchDirection(dir);
					list2.Add(item4);
				}
				else if (Vector3.Dot(list5[0] - list5[1], list5[2] - list5[3]) >= 0f)
				{
					Slicer.Triangle triangle = new Slicer.Triangle
					{
						v1 = list5[0],
						v2 = list5[2],
						v3 = list5[3]
					};
					Slicer.Triangle item5 = triangle;
					item5.matchDirection(dir);
					list2.Add(item5);
					triangle = new Slicer.Triangle
					{
						v1 = list5[0],
						v2 = list5[3],
						v3 = list5[1]
					};
					item5 = triangle;
					item5.matchDirection(dir);
					list2.Add(item5);
				}
				else
				{
					Slicer.Triangle triangle = new Slicer.Triangle
					{
						v1 = list5[0],
						v2 = list5[3],
						v3 = list5[2]
					};
					Slicer.Triangle item6 = triangle;
					item6.matchDirection(dir);
					list2.Add(item6);
					triangle = new Slicer.Triangle
					{
						v1 = list5[0],
						v2 = list5[2],
						v3 = list5[1]
					};
					item6 = triangle;
					item6.matchDirection(dir);
					list2.Add(item6);
				}
				if (list6.Count == 3)
				{
					Slicer.Triangle triangle = new Slicer.Triangle
					{
						v1 = list6[1],
						v2 = list6[0],
						v3 = list6[2]
					};
					Slicer.Triangle item7 = triangle;
					item7.matchDirection(dir);
					list3.Add(item7);
				}
				else if (Vector3.Dot(list6[0] - list6[1], list6[2] - list6[3]) >= 0f)
				{
					Slicer.Triangle triangle = new Slicer.Triangle
					{
						v1 = list6[0],
						v2 = list6[2],
						v3 = list6[3]
					};
					Slicer.Triangle item8 = triangle;
					item8.matchDirection(dir);
					list3.Add(item8);
					triangle = new Slicer.Triangle
					{
						v1 = list6[0],
						v2 = list6[3],
						v3 = list6[1]
					};
					item8 = triangle;
					item8.matchDirection(dir);
					list3.Add(item8);
				}
				else
				{
					Slicer.Triangle triangle = new Slicer.Triangle
					{
						v1 = list6[0],
						v2 = list6[3],
						v3 = list6[2]
					};
					Slicer.Triangle item9 = triangle;
					item9.matchDirection(dir);
					list3.Add(item9);
					triangle = new Slicer.Triangle
					{
						v1 = list6[0],
						v2 = list6[2],
						v3 = list6[1]
					};
					item9 = triangle;
					item9.matchDirection(dir);
					list3.Add(item9);
				}
			}
			else if (plane.GetSide(vector3))
			{
				List<Slicer.Triangle> list7 = list2;
				Slicer.Triangle triangle = new Slicer.Triangle
				{
					v1 = vector3,
					v2 = vector4,
					v3 = vector5
				};
				list7.Add(triangle);
			}
			else
			{
				List<Slicer.Triangle> list8 = list3;
				Slicer.Triangle triangle = new Slicer.Triangle
				{
					v1 = vector3,
					v2 = vector4,
					v3 = vector5
				};
				list8.Add(triangle);
			}
		}
		if (list.Count > 1)
		{
			Vector3 vector6 = Vector3.zero;
			foreach (Vector3 b in list)
			{
				vector6 += b;
			}
			vector6 /= (float)list.Count;
			for (int j = 0; j < list.Count; j++)
			{
				Slicer.Triangle triangle = new Slicer.Triangle
				{
					v1 = list[j],
					v2 = vector6,
					v3 = ((j + 1 == list.Count) ? list[j] : list[j + 1])
				};
				Slicer.Triangle item10 = triangle;
				item10.matchDirection(-plane.normal);
				list2.Add(item10);
			}
			for (int k = 0; k < list.Count; k++)
			{
				Slicer.Triangle triangle = new Slicer.Triangle
				{
					v1 = list[k],
					v2 = vector6,
					v3 = ((k + 1 == list.Count) ? list[k] : list[k + 1])
				};
				Slicer.Triangle item11 = triangle;
				item11.matchDirection(plane.normal);
				list3.Add(item11);
			}
		}
		if (list.Count > 0)
		{
			Material material = other.gameObject.GetComponent<MeshRenderer>().material;
			UnityEngine.Object.Destroy(other.gameObject);
			Mesh mesh2 = new Mesh();
			Mesh mesh3 = new Mesh();
			List<Vector3> list9 = new List<Vector3>();
			List<int> list10 = new List<int>();
			int num5 = 0;
			foreach (Slicer.Triangle triangle2 in list2)
			{
				list9.Add(triangle2.v1);
				list9.Add(triangle2.v2);
				list9.Add(triangle2.v3);
				list10.Add(num5++);
				list10.Add(num5++);
				list10.Add(num5++);
			}
			mesh2.vertices = list9.ToArray();
			mesh2.triangles = list10.ToArray();
			num5 = 0;
			list9.Clear();
			list10.Clear();
			foreach (Slicer.Triangle triangle3 in list3)
			{
				list9.Add(triangle3.v1);
				list9.Add(triangle3.v2);
				list9.Add(triangle3.v3);
				list10.Add(num5++);
				list10.Add(num5++);
				list10.Add(num5++);
			}
			mesh3.vertices = list9.ToArray();
			mesh3.triangles = list10.ToArray();
			mesh2.RecalculateNormals();
			mesh2.RecalculateBounds();
			mesh3.RecalculateNormals();
			mesh3.RecalculateBounds();
			GameObject gameObject = new GameObject();
			GameObject gameObject2 = new GameObject();
			gameObject.AddComponent<MeshFilter>().mesh = mesh2;
			gameObject.AddComponent<MeshRenderer>().material = material;
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.convex = true;
			gameObject.AddComponent<Rigidbody>();
			meshCollider.sharedMesh = mesh2;
			gameObject.tag = "Slicable";
			gameObject2.AddComponent<MeshFilter>().mesh = mesh3;
			gameObject2.AddComponent<MeshRenderer>().material = material;
			MeshCollider meshCollider2 = gameObject2.AddComponent<MeshCollider>();
			meshCollider2.convex = true;
			gameObject2.AddComponent<Rigidbody>();
			meshCollider2.sharedMesh = mesh3;
			gameObject2.tag = "Slicable";
		}
	}

	// Token: 0x040013D1 RID: 5073
	private float timer = 1f;

	// Token: 0x020001E8 RID: 488
	private struct Triangle
	{
		// Token: 0x06000B64 RID: 2916 RVA: 0x0007D68C File Offset: 0x0007B88C
		public Vector3 getNormal()
		{
			return Vector3.Cross(this.v1 - this.v2, this.v1 - this.v3).normalized;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0007D6C8 File Offset: 0x0007B8C8
		public void matchDirection(Vector3 dir)
		{
			if (Vector3.Dot(this.getNormal(), dir) > 0f)
			{
				return;
			}
			Vector3 vector = this.v1;
			this.v1 = this.v3;
			this.v3 = vector;
		}

		// Token: 0x040013D2 RID: 5074
		public Vector3 v1;

		// Token: 0x040013D3 RID: 5075
		public Vector3 v2;

		// Token: 0x040013D4 RID: 5076
		public Vector3 v3;
	}
}
