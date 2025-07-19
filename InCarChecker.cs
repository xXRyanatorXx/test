using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class InCarChecker : MonoBehaviour
{
	// Token: 0x060005EF RID: 1519 RVA: 0x0002EF76 File Offset: 0x0002D176
	private void Start()
	{
		this.Player = GameObject.Find("Player");
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0002EF88 File Offset: 0x0002D188
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && other.GetComponent<Crouch>())
		{
			other.GetComponent<Crouch>().inCar = true;
			other.GetComponent<Crouch>().InCARmaxHeight = base.transform.root.GetComponent<MainCarProperties>().InCarHeight;
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0002EFE0 File Offset: 0x0002D1E0
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && other.GetComponent<Crouch>())
		{
			other.GetComponent<Crouch>().WaitStart();
			other.GetComponent<Crouch>().inCar = false;
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0002F018 File Offset: 0x0002D218
	private void OnDestroy()
	{
		if (this.Player && Vector3.Distance(this.Player.transform.position, base.transform.position) < 3f)
		{
			this.Player.GetComponent<Crouch>().WaitStart();
			this.Player.GetComponent<Crouch>().inCar = false;
		}
	}

	// Token: 0x040008E3 RID: 2275
	public GameObject Player;
}
