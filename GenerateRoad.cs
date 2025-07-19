using System;
using System.Collections;
using System.Collections.Generic;
using FluffyUnderware.Curvy;
using MapMagic.Terrains;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class GenerateRoad : MonoBehaviour
{
	// Token: 0x06000C5E RID: 3166 RVA: 0x0008AB66 File Offset: 0x00088D66
	public void Start()
	{
		this.tile = base.transform.parent.GetComponent<TerrainTile>();
		base.StartCoroutine(this.WaitStart());
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0008AB8B File Offset: 0x00088D8B
	private IEnumerator WaitStart()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		if (Vector3.Distance(GameObject.Find("Player").transform.position, new Vector3(base.transform.position.x + 1000f, base.transform.position.y, base.transform.position.z + 1000f)) < 1500f)
		{
			this.playerNear = true;
		}
		if (this.tile.main.applyReady)
		{
			foreach (Transform transform in this.listOfAllTransforms)
			{
				if (this.lastTransform == null)
				{
					this.listOfTransforms = new List<Transform>();
					this.lastTransform = transform;
					this.listOfTransforms.Add(transform);
				}
				else if (Vector3.Distance(this.lastTransform.localPosition, transform.localPosition) >= 5f)
				{
					if (Vector3.Distance(this.lastTransform.localPosition, transform.localPosition) < 60f)
					{
						this.lastTransform = transform;
						this.listOfTransforms.Add(transform);
					}
					else
					{
						this.CreateRoad();
						this.listOfTransforms = new List<Transform>();
						this.listOfTransforms.Add(transform);
						this.lastTransform = transform;
						if (!this.playerNear)
						{
							yield return new WaitForSeconds(3f);
						}
					}
				}
			}
			List<Transform>.Enumerator enumerator = default(List<Transform>.Enumerator);
			this.CreateRoad();
		}
		else
		{
			base.StartCoroutine(this.WaitStart());
		}
		yield break;
		yield break;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0008AB9A File Offset: 0x00088D9A
	private void OnDisable()
	{
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x0008ABA4 File Offset: 0x00088DA4
	public void CreateRoad()
	{
		foreach (Transform transform in this.listOfTransforms)
		{
			UnityEngine.Object.Destroy(transform.gameObject.GetComponent<RoadNodeStart>());
		}
		this.waypoints = this.listOfTransforms.ToArray();
		if (this.waypoints.Length > 1)
		{
			GameObject gameObject = new GameObject("RoadParent");
			gameObject.transform.position = this.lastTransform.position;
			gameObject.transform.SetParent(base.transform.root.transform);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("CurvySplinePrefab") as GameObject);
			gameObject2.transform.SetParent(gameObject.transform);
			CurvySpline component = gameObject2.GetComponent<CurvySpline>();
			foreach (Transform transform2 in this.listOfTransforms)
			{
				component.Add(new Vector3[]
				{
					transform2.position
				});
			}
			foreach (object obj in gameObject2.transform)
			{
				Transform transform3 = (Transform)obj;
				if (transform3.GetComponent<CurvySplineSegment>())
				{
					transform3.gameObject.AddComponent<RoadNodeSpline>();
					transform3.gameObject.layer = LayerMask.NameToLayer("NOselfCOL");
				}
			}
			base.transform.root.GetComponent<DisableInDistance>().listOfTransforms.Add(gameObject.transform);
		}
	}

	// Token: 0x04001551 RID: 5457
	public List<Transform> listOfAllTransforms;

	// Token: 0x04001552 RID: 5458
	public List<Transform> listOfTransforms;

	// Token: 0x04001553 RID: 5459
	public bool closedLoop;

	// Token: 0x04001554 RID: 5460
	public Transform[] waypoints;

	// Token: 0x04001555 RID: 5461
	public Transform lastTransform;

	// Token: 0x04001556 RID: 5462
	public float distance;

	// Token: 0x04001557 RID: 5463
	private TerrainTile tile;

	// Token: 0x04001558 RID: 5464
	private bool playerNear;
}
