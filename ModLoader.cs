using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x02000155 RID: 341
public class ModLoader : MonoBehaviour
{
	// Token: 0x0600073C RID: 1852 RVA: 0x0003BBA0 File Offset: 0x00039DA0
	private void Awake()
	{
		if (ModLoader.instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		ModLoader.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Directory.CreateDirectory("./Mods");
		string[] files = Directory.GetFiles("./Mods", "*.dll");
		Array.Sort<string>(files, StringComparer.InvariantCulture);
		Type typeFromHandle = typeof(Mod);
		ModLoader.i = 0;
		while (ModLoader.i < files.Length)
		{
			try
			{
				Type[] types = Assembly.LoadFrom(files[ModLoader.i]).GetTypes();
				for (int i = 0; i < types.Length; i++)
				{
					if (typeFromHandle.IsAssignableFrom(types[i]))
					{
						ModLoader.mods.Add((Mod)Activator.CreateInstance(types[i]));
					}
				}
			}
			catch (Exception arg)
			{
				Debug.Log(string.Format("Failed to load {0}:\n{1}", files[ModLoader.i], arg));
			}
			ModLoader.i++;
		}
		ModLoader.OnMenuLoad();
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x0003BC9C File Offset: 0x00039E9C
	private void Update()
	{
		if (ModLoader.inGame)
		{
			ModLoader.i = 0;
			while (ModLoader.i < ModLoader.mods.Count)
			{
				if (ModLoader.mods[ModLoader.i].enabled)
				{
					try
					{
						ModLoader.mods[ModLoader.i].Update();
					}
					catch (Exception e)
					{
						ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "Update", e);
					}
				}
				ModLoader.i++;
			}
			return;
		}
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].MenuUpdate();
				}
				catch (Exception e2)
				{
					ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "MenuUpdate", e2);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0003BDAC File Offset: 0x00039FAC
	private void FixedUpdate()
	{
		if (ModLoader.inGame)
		{
			ModLoader.i = 0;
			while (ModLoader.i < ModLoader.mods.Count)
			{
				if (ModLoader.mods[ModLoader.i].enabled)
				{
					try
					{
						ModLoader.mods[ModLoader.i].FixedUpdate();
					}
					catch (Exception e)
					{
						ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "FixedUpdate", e);
					}
				}
				ModLoader.i++;
			}
			return;
		}
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].MenuFixedUpdate();
				}
				catch (Exception e2)
				{
					ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "MenuFixedUpdate", e2);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0003BEBC File Offset: 0x0003A0BC
	private void LateUpdate()
	{
		if (ModLoader.inGame)
		{
			ModLoader.i = 0;
			while (ModLoader.i < ModLoader.mods.Count)
			{
				if (ModLoader.mods[ModLoader.i].enabled)
				{
					try
					{
						ModLoader.mods[ModLoader.i].LateUpdate();
					}
					catch (Exception e)
					{
						ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "LateUpdate", e);
					}
				}
				ModLoader.i++;
			}
			return;
		}
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].MenuLateUpdate();
				}
				catch (Exception e2)
				{
					ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "MenuLateUpdate", e2);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0003BFCC File Offset: 0x0003A1CC
	private void OnGUI()
	{
		if (ModLoader.inGame)
		{
			ModLoader.i = 0;
			while (ModLoader.i < ModLoader.mods.Count)
			{
				if (ModLoader.mods[ModLoader.i].enabled)
				{
					try
					{
						ModLoader.mods[ModLoader.i].OnGUI();
					}
					catch (Exception e)
					{
						ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "OnGUI", e);
					}
				}
				ModLoader.i++;
			}
			return;
		}
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].MenuOnGUI();
				}
				catch (Exception e2)
				{
					ModLoader.modErrorDisable(ModLoader.mods[ModLoader.i], "MenuOnGUI", e2);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0003C0DC File Offset: 0x0003A2DC
	internal static void OnMenuLoad()
	{
		ModLoader.inGame = false;
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnMenuLoad();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnMenuLoad", e);
				}
			}
			ModLoader.i++;
		}
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].SecondPassOnMenuLoad();
				}
				catch (Exception e2)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "SecondPassOnMenuLoad", e2);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0003C1E8 File Offset: 0x0003A3E8
	internal static void OnLoad()
	{
		ModLoader.inGame = true;
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnLoad();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnLoad", e);
				}
			}
			ModLoader.i++;
		}
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].SecondPassOnLoad();
				}
				catch (Exception e2)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "SecondPassOnLoad", e2);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0003C2F4 File Offset: 0x0003A4F4
	internal static void Continue()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].Continue();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "Continue", e);
				}
			}
			ModLoader.i++;
		}
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].SecondPassContinue();
				}
				catch (Exception e2)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "SecondPassContinue", e2);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0003C3FC File Offset: 0x0003A5FC
	internal static void OnNewMapLoad()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnNewMapLoad();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnNewMapLoad", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0003C488 File Offset: 0x0003A688
	internal static void OnNewMapUnload()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnNewMapUnload();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnNewMapUnload", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0003C514 File Offset: 0x0003A714
	internal static void OnSave()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnSave();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnSave", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0003C5A0 File Offset: 0x0003A7A0
	internal static void OnSaveFinish()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnSaveFinish();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnSaveFinish", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0003C62C File Offset: 0x0003A82C
	internal static void OnBarnSaveFinish()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnBarnSaveFinish();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnBarnSaveFinish", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x0003C6B8 File Offset: 0x0003A8B8
	internal static void OnBarnSave()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnBarnSave();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnBarnSave", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0003C744 File Offset: 0x0003A944
	internal static void OnBarnLoad()
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnBarnLoad();
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnBarnLoad", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0003C7D0 File Offset: 0x0003A9D0
	internal static void OnSaveSystemLoad(SaveSystem saver, bool isBarn)
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnSaveSystemLoad(saver, isBarn);
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnSaveSystemLoad", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0003C860 File Offset: 0x0003AA60
	internal static void OnSaveSystemSave(SaveSystem saver, bool isBarn)
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].enabled)
			{
				try
				{
					ModLoader.mods[ModLoader.i].OnSaveSystemSave(saver, isBarn);
				}
				catch (Exception e)
				{
					ModLoader.modError(ModLoader.mods[ModLoader.i], "OnSaveSystemSave", e);
				}
			}
			ModLoader.i++;
		}
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0003C8F0 File Offset: 0x0003AAF0
	private static void modErrorDisable(Mod mod, string method, Exception e)
	{
		mod.enabled = false;
		Debug.Log(string.Format("{0} has thrown an exception in {1} and has been disabled:\n{2}", mod.ID, method, e));
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0003C910 File Offset: 0x0003AB10
	private static void modError(Mod mod, string method, Exception e)
	{
		Debug.Log(string.Format("{0} has thrown an exception in {1}:\n{2}", mod.ID, method, e));
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0003C92C File Offset: 0x0003AB2C
	public static bool IsModInstalled(string id)
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].ID == id)
			{
				return true;
			}
			ModLoader.i++;
		}
		return false;
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0003C980 File Offset: 0x0003AB80
	public static Mod GetModInstance(string id)
	{
		ModLoader.i = 0;
		while (ModLoader.i < ModLoader.mods.Count)
		{
			if (ModLoader.mods[ModLoader.i].ID == id && ModLoader.mods[ModLoader.i].enabled)
			{
				return ModLoader.mods[ModLoader.i];
			}
			ModLoader.i++;
		}
		return null;
	}

	// Token: 0x04000B51 RID: 2897
	public static List<Mod> mods = new List<Mod>();

	// Token: 0x04000B52 RID: 2898
	private static ModLoader instance;

	// Token: 0x04000B53 RID: 2899
	private static bool inGame;

	// Token: 0x04000B54 RID: 2900
	private static int i;
}
