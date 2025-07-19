using System;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class DeliveryTarget : MonoBehaviour
{
	// Token: 0x06000BF9 RID: 3065 RVA: 0x00084684 File Offset: 0x00082884
	public void Start()
	{
		this.DeliveryBook = GameObject.Find("DelieveriesPizzas").GetComponent<Book>();
		this.AudioParent = GameObject.Find("hand");
		this.Target = UnityEngine.Object.Instantiate<GameObject>(this.MaprkerPrefab, Vector3.zero, Quaternion.identity);
		if (this.Accepted)
		{
			this.Target.transform.SetParent(this.DeliveryBook.AcceptedPizzas.transform);
			this.Target.GetComponent<UpdateMapPosition>().AcceptedButton.SetActive(true);
			this.Target.GetComponent<UpdateMapPosition>().NotAcceptedButton.SetActive(false);
			base.GetComponent<MeshRenderer>().enabled = true;
			base.GetComponent<CapsuleCollider>().enabled = true;
		}
		else
		{
			this.Target.transform.SetParent(this.DeliveryBook.UnacceptedPizzas.transform);
		}
		this.Target.transform.localScale = Vector3.one;
		this.Target.GetComponent<UpdateMapPosition>().DeliveryTarget = base.gameObject;
		this.Reward = 20f + Vector3.Distance(base.transform.position, this.DeliveryBook.transform.position) / 50f;
		this.Target.GetComponent<RectTransform>().anchoredPosition = new Vector2((base.transform.position.x - this.Target.transform.root.transform.position.x) / 3f, (base.transform.position.z - this.Target.transform.root.transform.position.z) / 3f);
		this.Target.GetComponent<UpdateMapPosition>().SCALE();
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x00084850 File Offset: 0x00082A50
	private void OnTriggerEnter(Collider other)
	{
		if (!this.delivered && other.transform.name == "Pizza")
		{
			this.delivered = true;
			Saver component = GameObject.Find("SaveManager").GetComponent<Saver>();
			if (tools.money > 2000f && this.BarnGiver && (component.BarnFind == null || (component.BarnFind && component.BarnFind.GetComponent<SetStart>().spawned)))
			{
				GameObject.Find("Player").GetComponent<tools>().OpenBarnCanvas();
			}
			tools.money += this.Reward;
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			tools.PizzaDeliveriesCount--;
			UnityEngine.Object.Destroy(this.Target);
			UnityEngine.Object.Destroy(other.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040014B0 RID: 5296
	public Book DeliveryBook;

	// Token: 0x040014B1 RID: 5297
	public GameObject MaprkerPrefab;

	// Token: 0x040014B2 RID: 5298
	public float Reward;

	// Token: 0x040014B3 RID: 5299
	public GameObject Target;

	// Token: 0x040014B4 RID: 5300
	public GameObject AudioParent;

	// Token: 0x040014B5 RID: 5301
	public bool Accepted;

	// Token: 0x040014B6 RID: 5302
	private bool delivered;

	// Token: 0x040014B7 RID: 5303
	public bool BarnGiver;
}
