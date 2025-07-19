using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class SnapshotCamera : MonoBehaviour
{
	// Token: 0x06000C72 RID: 3186 RVA: 0x0008BEB8 File Offset: 0x0008A0B8
	private SnapshotCamera()
	{
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0008BF19 File Offset: 0x0008A119
	public static SnapshotCamera MakeSnapshotCamera(string layer, string name = "Snapshot Camera")
	{
		return SnapshotCamera.MakeSnapshotCamera(LayerMask.NameToLayer(layer), name);
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0008BF28 File Offset: 0x0008A128
	public static SnapshotCamera MakeSnapshotCamera(int layer = 5, string name = "Snapshot Camera")
	{
		if (layer < 0 || layer > 31)
		{
			throw new ArgumentOutOfRangeException("layer", "layer argument must specify a valid layer between 0 and 31");
		}
		GameObject gameObject = new GameObject(name);
		Camera camera = gameObject.AddComponent<Camera>();
		camera.cullingMask = 1 << layer;
		camera.orthographic = true;
		camera.orthographicSize = 1f;
		camera.clearFlags = CameraClearFlags.Color;
		camera.backgroundColor = Color.clear;
		camera.nearClipPlane = 0.1f;
		camera.enabled = false;
		SnapshotCamera snapshotCamera = gameObject.AddComponent<SnapshotCamera>();
		snapshotCamera.cam = camera;
		snapshotCamera.layer = layer;
		return snapshotCamera;
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0008BFB0 File Offset: 0x0008A1B0
	private static string SanitizeFilename(string dirty)
	{
		string arg = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
		string pattern = string.Format("([{0}]*\\.+$)|([{0}]+)", arg);
		return Regex.Replace(dirty, pattern, "_");
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0008BFE8 File Offset: 0x0008A1E8
	public static FileInfo SavePNG(byte[] bytes, string filename = "", string directory = "")
	{
		directory = ((directory != "") ? Directory.CreateDirectory(directory).FullName : Directory.CreateDirectory(Path.Combine(Application.dataPath, "../Snapshots")).FullName);
		filename = ((filename != "") ? (SnapshotCamera.SanitizeFilename(filename) + ".png") : (DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".png"));
		string text = Path.Combine(directory, filename);
		File.WriteAllBytes(text, bytes);
		return new FileInfo(text);
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0008C07A File Offset: 0x0008A27A
	public static FileInfo SavePNG(Texture2D tex, string filename = "", string directory = "")
	{
		return SnapshotCamera.SavePNG(tex.EncodeToPNG(), filename, directory);
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0008C08C File Offset: 0x0008A28C
	private void SetLayersRecursively(GameObject gameObject)
	{
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = this.layer;
		}
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x0008C0C4 File Offset: 0x0008A2C4
	private SnapshotCamera.GameObjectStateSnapshot PrepareObject(GameObject gameObject, Vector3 positionOffset, Quaternion rotation, Vector3 scale)
	{
		SnapshotCamera.GameObjectStateSnapshot result = new SnapshotCamera.GameObjectStateSnapshot(gameObject);
		gameObject.transform.position = base.transform.position + positionOffset;
		gameObject.transform.rotation = rotation;
		gameObject.transform.localScale = scale;
		this.SetLayersRecursively(gameObject);
		return result;
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x0008C114 File Offset: 0x0008A314
	private GameObject PreparePrefab(GameObject prefab, Vector3 positionOffset, Quaternion rotation, Vector3 scale)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, base.transform.position + positionOffset, rotation);
		gameObject.transform.localScale = scale;
		this.SetLayersRecursively(gameObject);
		return gameObject;
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0008C14F File Offset: 0x0008A34F
	public Texture2D TakeObjectSnapshot(GameObject gameObject, int width = 128, int height = 128)
	{
		return this.TakeObjectSnapshot(gameObject, Color.clear, this.defaultPositionOffset, Quaternion.Euler(this.defaultRotation), this.defaultScale, width, height);
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0008C176 File Offset: 0x0008A376
	public Texture2D TakeObjectSnapshot(GameObject gameObject, Color backgroundColor, int width = 128, int height = 128)
	{
		return this.TakeObjectSnapshot(gameObject, backgroundColor, this.defaultPositionOffset, Quaternion.Euler(this.defaultRotation), this.defaultScale, width, height);
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x0008C19A File Offset: 0x0008A39A
	public Texture2D TakeObjectSnapshot(GameObject gameObject, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
	{
		return this.TakeObjectSnapshot(gameObject, Color.clear, positionOffset, rotation, scale, width, height);
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x0008C1B0 File Offset: 0x0008A3B0
	public Texture2D TakeObjectSnapshot(GameObject gameObject, Color backgroundColor, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
	{
		if (gameObject == null)
		{
			throw new ArgumentNullException("gameObject");
		}
		if (gameObject.scene.name == null)
		{
			throw new ArgumentException("gameObject parameter must be an instantiated GameObject! If you want to use a prefab directly, use TakePrefabSnapshot instead.", "gameObject");
		}
		SnapshotCamera.GameObjectStateSnapshot gameObjectStateSnapshot = this.PrepareObject(gameObject, positionOffset, rotation, scale);
		Texture2D result = this.TakeSnapshot(backgroundColor, width, height);
		gameObjectStateSnapshot.Restore();
		return result;
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0008C210 File Offset: 0x0008A410
	public Texture2D TakePrefabSnapshot(GameObject prefab, int width = 128, int height = 128)
	{
		return this.TakePrefabSnapshot(prefab, Color.clear, this.defaultPositionOffset, Quaternion.Euler(this.defaultRotation), this.defaultScale, width, height);
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0008C237 File Offset: 0x0008A437
	public Texture2D TakePrefabSnapshot(GameObject prefab, Color backgroundColor, int width = 128, int height = 128)
	{
		return this.TakePrefabSnapshot(prefab, backgroundColor, this.defaultPositionOffset, Quaternion.Euler(this.defaultRotation), this.defaultScale, width, height);
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0008C25B File Offset: 0x0008A45B
	public Texture2D TakePrefabSnapshot(GameObject prefab, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
	{
		return this.TakePrefabSnapshot(prefab, Color.clear, positionOffset, rotation, scale, width, height);
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0008C274 File Offset: 0x0008A474
	public Texture2D TakePrefabSnapshot(GameObject prefab, Color backgroundColor, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
	{
		if (prefab == null)
		{
			throw new ArgumentNullException("prefab");
		}
		if (prefab.scene.name != null)
		{
			throw new ArgumentException("prefab parameter must be a prefab! If you want to use an instance, use TakeObjectSnapshot instead.", "prefab");
		}
		GameObject obj = this.PreparePrefab(prefab, positionOffset, rotation, scale);
		Texture2D result = this.TakeSnapshot(backgroundColor, width, height);
		UnityEngine.Object.DestroyImmediate(obj);
		return result;
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0008C2D4 File Offset: 0x0008A4D4
	private Texture2D TakeSnapshot(Color backgroundColor, int width, int height)
	{
		this.cam.backgroundColor = backgroundColor;
		this.cam.targetTexture = RenderTexture.GetTemporary(width, height, 24);
		this.cam.Render();
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this.cam.targetTexture;
		Texture2D texture2D = new Texture2D(this.cam.targetTexture.width, this.cam.targetTexture.height, TextureFormat.ARGB32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)this.cam.targetTexture.width, (float)this.cam.targetTexture.height), 0, 0);
		texture2D.Apply(false);
		RenderTexture.active = active;
		this.cam.targetTexture = null;
		RenderTexture.ReleaseTemporary(this.cam.targetTexture);
		return texture2D;
	}

	// Token: 0x04001563 RID: 5475
	public Camera cam;

	// Token: 0x04001564 RID: 5476
	private int layer;

	// Token: 0x04001565 RID: 5477
	public Vector3 defaultPositionOffset = new Vector3(0f, 0f, 1f);

	// Token: 0x04001566 RID: 5478
	public Vector3 defaultRotation = new Vector3(345.8529f, 313.8297f, 14.28433f);

	// Token: 0x04001567 RID: 5479
	public Vector3 defaultScale = new Vector3(1f, 1f, 1f);

	// Token: 0x02000217 RID: 535
	private struct GameObjectStateSnapshot
	{
		// Token: 0x06000C84 RID: 3204 RVA: 0x0008C3AC File Offset: 0x0008A5AC
		public GameObjectStateSnapshot(GameObject gameObject)
		{
			this.gameObject = gameObject;
			this.position = gameObject.transform.position;
			this.rotation = gameObject.transform.rotation;
			this.scale = gameObject.transform.localScale;
			this.layers = new Dictionary<GameObject, int>();
			foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(true))
			{
				this.layers.Add(transform.gameObject, transform.gameObject.layer);
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0008C434 File Offset: 0x0008A634
		public void Restore()
		{
			this.gameObject.transform.position = this.position;
			this.gameObject.transform.rotation = this.rotation;
			this.gameObject.transform.localScale = this.scale;
			foreach (KeyValuePair<GameObject, int> keyValuePair in this.layers)
			{
				keyValuePair.Key.layer = keyValuePair.Value;
			}
		}

		// Token: 0x04001568 RID: 5480
		private GameObject gameObject;

		// Token: 0x04001569 RID: 5481
		private Vector3 position;

		// Token: 0x0400156A RID: 5482
		private Quaternion rotation;

		// Token: 0x0400156B RID: 5483
		private Vector3 scale;

		// Token: 0x0400156C RID: 5484
		private Dictionary<GameObject, int> layers;
	}
}
