using System;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class SaveSlotKey : MonoBehaviour
{
	// Token: 0x06000EAF RID: 3759 RVA: 0x0009BB80 File Offset: 0x00099D80
	public void OnMouseDown()
	{
		this.Barn.SaveSlot = this.SaveSlot;
		base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		for (int i = 0; i < this.OtherKeys.Length; i++)
		{
			this.OtherKeys[i].GetComponent<MeshRenderer>().enabled = true;
		}
	}

	// Token: 0x040017E7 RID: 6119
	public SaveInside Barn;

	// Token: 0x040017E8 RID: 6120
	public GameObject[] OtherKeys;

	// Token: 0x040017E9 RID: 6121
	public int SaveSlot;
}
