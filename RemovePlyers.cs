using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class RemovePlyers : MonoBehaviour
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x0006401C File Offset: 0x0006221C
	private void Start()
	{
		this.player = GameObject.Find("Player");
		this.playerTransform = this.player.transform;
		base.gameObject.GetComponent<Partinfo>().attachedbolts += 1f;
		base.gameObject.GetComponent<Partinfo>().tightnuts += 1f;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00064084 File Offset: 0x00062284
	private void Update()
	{
		RaycastHit raycastHit;
		if (tools.tool == 0 && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("Windows")) && raycastHit.collider.gameObject == base.gameObject && base.transform.parent != null)
		{
			base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
			base.transform.position = Vector3.Lerp(base.transform.position, this.playerTransform.position, 0.07f);
			base.gameObject.AddComponent<FixedJoint>();
			base.gameObject.transform.SetParent(null);
			base.gameObject.GetComponent<CarProperties>().Remove();
			base.StartCoroutine(this.FallCoroutine());
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0006418D File Offset: 0x0006238D
	private IEnumerator FallCoroutine()
	{
		yield return new WaitForSeconds(3f);
		if (!(base.transform.parent != null))
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
		}
		yield break;
	}

	// Token: 0x040011F5 RID: 4597
	public GameObject player;

	// Token: 0x040011F6 RID: 4598
	private Transform playerTransform;
}
