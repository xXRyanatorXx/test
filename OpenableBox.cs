using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class OpenableBox : MonoBehaviour
{
	// Token: 0x06000D77 RID: 3447 RVA: 0x000921FC File Offset: 0x000903FC
	private void Start()
	{
		this.GOList = new List<GameObject>();
		this.m_Collider = base.GetComponent<Collider>();
		this.LidCollider = this.Lid.GetComponent<Collider>();
		base.StartCoroutine(this.waitStart());
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x00092234 File Offset: 0x00090434
	private void OnTriggerEnter(Collider other)
	{
		if (!this.GOList.Contains(other.gameObject) && (!other.transform.parent || (other.transform.parent && other.transform.parent == base.transform)) && (other.gameObject.GetComponent<PickupTool>() || other.gameObject.GetComponent<CarProperties>() || other.gameObject.name == "Jack" || other.gameObject.name == "BatteryCharger" || other.gameObject.name == "DiscBox" || other.gameObject.name == "electrodebox" || other.gameObject.GetComponent<PickupItems>()))
		{
			this.GOList.Add(other.gameObject);
		}
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x0009233F File Offset: 0x0009053F
	private void OnTriggerExit(Collider other)
	{
		this.GOList.Remove(other.gameObject);
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x00092354 File Offset: 0x00090554
	public void Interact()
	{
		if (this.started)
		{
			if (this.Open)
			{
				this.m_Collider.enabled = false;
				this.LidCollider.enabled = true;
				this.GOList.Clear();
				this.m_Collider.enabled = true;
				this.Lid.transform.position = this.LidClosed.transform.position;
				this.Lid.transform.rotation = this.LidClosed.transform.rotation;
				this.Open = false;
				base.StartCoroutine(this.Close());
				return;
			}
			this.Lid.transform.position = this.LidOpen.transform.position;
			this.Lid.transform.rotation = this.LidOpen.transform.rotation;
			this.Open = true;
			this.m_Collider.enabled = false;
			this.LidCollider.enabled = false;
			base.StartCoroutine(this.Opens());
		}
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x00092467 File Offset: 0x00090667
	private IEnumerator waitStart()
	{
		yield return new WaitForSeconds(3f);
		this.started = true;
		using (List<GameObject>.Enumerator enumerator = this.GOList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (!this.m_Collider.bounds.Contains(gameObject.transform.position))
				{
					gameObject.transform.position = this.Box.transform.position;
				}
				UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
				gameObject.transform.SetParent(this.Box.transform);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x00092476 File Offset: 0x00090676
	private IEnumerator Close()
	{
		yield return 0;
		using (List<GameObject>.Enumerator enumerator = this.GOList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (this.m_Collider.bounds.Contains(gameObject.transform.position))
				{
					UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
					gameObject.transform.SetParent(this.Box.transform);
				}
				else
				{
					gameObject.GetComponent<Rigidbody>().isKinematic = false;
					gameObject.transform.SetParent(null);
					if (gameObject.GetComponent<MeshCollider>())
					{
						gameObject.GetComponent<MeshCollider>().isTrigger = false;
					}
					if (gameObject.GetComponent<BoxCollider>())
					{
						gameObject.GetComponent<BoxCollider>().isTrigger = false;
					}
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00092485 File Offset: 0x00090685
	private IEnumerator Opens()
	{
		yield return 0;
		foreach (GameObject gameObject in this.GOList)
		{
			if (!gameObject.GetComponent<Rigidbody>())
			{
				gameObject.AddComponent<Rigidbody>();
			}
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
			gameObject.GetComponent<Rigidbody>().drag = 10f;
			if (gameObject.GetComponent<MeshCollider>())
			{
				gameObject.GetComponent<MeshCollider>().isTrigger = true;
			}
			if (gameObject.GetComponent<BoxCollider>())
			{
				gameObject.GetComponent<BoxCollider>().isTrigger = true;
			}
		}
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		foreach (GameObject gameObject2 in this.GOList)
		{
			gameObject2.GetComponent<Rigidbody>().drag = 0f;
		}
		this.GOList.Clear();
		yield break;
	}

	// Token: 0x04001608 RID: 5640
	public GameObject Box;

	// Token: 0x04001609 RID: 5641
	public GameObject Lid;

	// Token: 0x0400160A RID: 5642
	public GameObject LidOpen;

	// Token: 0x0400160B RID: 5643
	public GameObject LidClosed;

	// Token: 0x0400160C RID: 5644
	public bool Open;

	// Token: 0x0400160D RID: 5645
	private bool started;

	// Token: 0x0400160E RID: 5646
	private Collider m_Collider;

	// Token: 0x0400160F RID: 5647
	private Collider LidCollider;

	// Token: 0x04001610 RID: 5648
	public List<GameObject> GOList;
}
