using System;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class PartsTOphoto : MonoBehaviour
{
	// Token: 0x06000D9E RID: 3486 RVA: 0x00092B10 File Offset: 0x00090D10
	private void Update()
	{
		RuntimePreviewGenerator.BackgroundColor = new Color(0f, 0f, 0f, 0f);
		if (Input.GetKeyDown("e"))
		{
			RuntimePreviewGenerator.PreviewDirection = this.PreviewDirection;
			for (int i = 0; i < this.Parts.Length; i++)
			{
				GameObject gameObject = this.Parts[i];
				this.mySprite = Sprite.Create(RuntimePreviewGenerator.GenerateModelPreview(gameObject.transform, 500, 500, false), new Rect(0f, 0f, 500f, 500f), new Vector2(0.5f, 0.5f), 100f);
				SnapshotCamera.SavePNG(this.mySprite.texture, gameObject.transform.name + ".png", Application.persistentDataPath);
			}
		}
		if (Input.GetKeyDown("r"))
		{
			for (int j = 0; j < this.Parts.Length; j++)
			{
				GameObject gameObject = this.Parts[j];
				for (int k = 0; k < this.Pics.Length; k++)
				{
					Texture2D texture2D = this.Pics[k];
					if (texture2D.name == gameObject.transform.name)
					{
						gameObject.GetComponent<Partinfo>().Thumbnail = texture2D;
					}
				}
			}
		}
	}

	// Token: 0x04001621 RID: 5665
	public GameObject[] Parts;

	// Token: 0x04001622 RID: 5666
	public SnapshotCamera SnapshotCamera;

	// Token: 0x04001623 RID: 5667
	public Sprite mySprite;

	// Token: 0x04001624 RID: 5668
	public Vector3 PreviewDirection;

	// Token: 0x04001625 RID: 5669
	public Texture2D[] Pics;

	// Token: 0x04001626 RID: 5670
	private GameObject Part;
}
