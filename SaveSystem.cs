using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class SaveSystem
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x00074F84 File Offset: 0x00073184
	private static BinaryFormatter getBinaryFormatter()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		SurrogateSelector surrogateSelector = new SurrogateSelector();
		surrogateSelector.AddSurrogate(typeof(Color), SaveSystem.sc, new SaveSystem.sColor());
		surrogateSelector.AddSurrogate(typeof(Quaternion), SaveSystem.sc, new SaveSystem.sQuaternion());
		surrogateSelector.AddSurrogate(typeof(Vector4), SaveSystem.sc, new SaveSystem.sVector4());
		surrogateSelector.AddSurrogate(typeof(Vector3), SaveSystem.sc, new SaveSystem.sVector3());
		surrogateSelector.AddSurrogate(typeof(Vector2), SaveSystem.sc, new SaveSystem.sVector2());
		binaryFormatter.SurrogateSelector = surrogateSelector;
		return binaryFormatter;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x00075025 File Offset: 0x00073225
	public void add(string tag, object data)
	{
		if (this.table.ContainsKey(tag))
		{
			this.table[tag] = data;
			return;
		}
		this.table.Add(tag, data);
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00075050 File Offset: 0x00073250
	public object get(string tag, object fallback = null)
	{
		if (this.table.ContainsKey(tag))
		{
			return this.table[tag];
		}
		return fallback;
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00075070 File Offset: 0x00073270
	public bool write()
	{
		bool result;
		try
		{
			FileStream fileStream = new FileStream(this.path, FileMode.Create);
			SaveSystem.bf.Serialize(fileStream, this.table);
			fileStream.Close();
			result = true;
		}
		catch
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x000750BC File Offset: 0x000732BC
	public bool read()
	{
		bool result;
		try
		{
			FileStream fileStream = new FileStream(this.path, FileMode.Open);
			this.table = (Hashtable)SaveSystem.bf.Deserialize(fileStream);
			fileStream.Close();
			result = true;
		}
		catch
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0007510C File Offset: 0x0007330C
	public SaveSystem(string path)
	{
		this.path = path;
	}

	// Token: 0x04001389 RID: 5001
	private static readonly StreamingContext sc = new StreamingContext(StreamingContextStates.All);

	// Token: 0x0400138A RID: 5002
	private static readonly BinaryFormatter bf = SaveSystem.getBinaryFormatter();

	// Token: 0x0400138B RID: 5003
	public Hashtable table = new Hashtable();

	// Token: 0x0400138C RID: 5004
	public string path;

	// Token: 0x020001D6 RID: 470
	private class sColor : ISerializationSurrogate
	{
		// Token: 0x06000B0D RID: 2829 RVA: 0x00075144 File Offset: 0x00073344
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Color color = (Color)obj;
			info.AddValue("r", color.r);
			info.AddValue("g", color.g);
			info.AddValue("b", color.b);
			info.AddValue("a", color.a);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0007519C File Offset: 0x0007339C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Color color = (Color)obj;
			color.r = info.GetSingle("r");
			color.g = info.GetSingle("g");
			color.b = info.GetSingle("b");
			color.a = info.GetSingle("a");
			return color;
		}
	}

	// Token: 0x020001D7 RID: 471
	private class sQuaternion : ISerializationSurrogate
	{
		// Token: 0x06000B10 RID: 2832 RVA: 0x00075200 File Offset: 0x00073400
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Quaternion quaternion = (Quaternion)obj;
			info.AddValue("x", quaternion.x);
			info.AddValue("y", quaternion.y);
			info.AddValue("z", quaternion.z);
			info.AddValue("w", quaternion.w);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00075258 File Offset: 0x00073458
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Quaternion quaternion = (Quaternion)obj;
			quaternion.x = info.GetSingle("x");
			quaternion.y = info.GetSingle("y");
			quaternion.z = info.GetSingle("z");
			quaternion.w = info.GetSingle("w");
			return quaternion;
		}
	}

	// Token: 0x020001D8 RID: 472
	private class sVector4 : ISerializationSurrogate
	{
		// Token: 0x06000B13 RID: 2835 RVA: 0x000752BC File Offset: 0x000734BC
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Vector4 vector = (Vector4)obj;
			info.AddValue("x", vector.x);
			info.AddValue("y", vector.y);
			info.AddValue("z", vector.z);
			info.AddValue("w", vector.w);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00075314 File Offset: 0x00073514
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Vector4 vector = (Vector4)obj;
			vector.x = info.GetSingle("x");
			vector.y = info.GetSingle("y");
			vector.z = info.GetSingle("z");
			vector.w = info.GetSingle("w");
			return vector;
		}
	}

	// Token: 0x020001D9 RID: 473
	private class sVector3 : ISerializationSurrogate
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x00075378 File Offset: 0x00073578
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Vector3 vector = (Vector3)obj;
			info.AddValue("x", vector.x);
			info.AddValue("y", vector.y);
			info.AddValue("z", vector.z);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x000753C0 File Offset: 0x000735C0
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Vector3 vector = (Vector3)obj;
			vector.x = info.GetSingle("x");
			vector.y = info.GetSingle("y");
			vector.z = info.GetSingle("z");
			return vector;
		}
	}

	// Token: 0x020001DA RID: 474
	private class sVector2 : ISerializationSurrogate
	{
		// Token: 0x06000B19 RID: 2841 RVA: 0x00075410 File Offset: 0x00073610
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Vector2 vector = (Vector2)obj;
			info.AddValue("x", vector.x);
			info.AddValue("y", vector.y);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00075448 File Offset: 0x00073648
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Vector2 vector = (Vector2)obj;
			vector.x = info.GetSingle("x");
			vector.y = info.GetSingle("y");
			return vector;
		}
	}
}
