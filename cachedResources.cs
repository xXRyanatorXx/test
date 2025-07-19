using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class cachedResources
{
	// Token: 0x06000B1C RID: 2844 RVA: 0x00075488 File Offset: 0x00073688
	public static void InitializeResources()
	{
		cachedResources.resources = new Hashtable();
		UnityEngine.Object[] array = Resources.LoadAll("");
		for (int i = 0; i < array.Length; i++)
		{
			if (!cachedResources.resources.ContainsKey(array[i].name))
			{
				cachedResources.resources.Add(array[i].name, array[i]);
			}
		}
		foreach (object obj in Saver.modParts)
		{
			DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
			if (!cachedResources.resources.ContainsKey(dictionaryEntry.Key))
			{
				cachedResources.resources.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00075554 File Offset: 0x00073754
	public static UnityEngine.Object Load(string path)
	{
		if (cachedResources.resources.ContainsKey(path))
		{
			return (UnityEngine.Object)cachedResources.resources[path];
		}
		UnityEngine.Object @object = Resources.Load(path);
		if (@object == null)
		{
			if (Saver.modParts.ContainsKey(path))
			{
				cachedResources.resources.Add(path, Saver.modParts[path]);
			}
			@object = (UnityEngine.Object)Saver.modParts[path];
		}
		else
		{
			cachedResources.resources.Add(path, @object);
		}
		return @object;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x000755D4 File Offset: 0x000737D4
	public static UnityEngine.Object Load(string path, Type type)
	{
		if (cachedResources.resources.ContainsKey(path))
		{
			return (UnityEngine.Object)cachedResources.resources[path];
		}
		UnityEngine.Object @object = Resources.Load(path, type);
		cachedResources.resources.Add(path, @object);
		return @object;
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x00075614 File Offset: 0x00073814
	public static T Load<T>(string path) where T : UnityEngine.Object
	{
		if (cachedResources.resources.ContainsKey(path))
		{
			return (T)((object)((UnityEngine.Object)cachedResources.resources[path]));
		}
		T t = Resources.Load<T>(path);
		cachedResources.resources.Add(path, t);
		return t;
	}

	// Token: 0x0400138D RID: 5005
	private static Hashtable resources = new Hashtable();
}
