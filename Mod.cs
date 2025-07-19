using System;

// Token: 0x02000156 RID: 342
public abstract class Mod
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000753 RID: 1875
	public abstract string Name { get; }

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000754 RID: 1876
	public abstract string ID { get; }

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000755 RID: 1877
	public abstract string Author { get; }

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000756 RID: 1878
	public abstract string Version { get; }

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000757 RID: 1879 RVA: 0x0003CA01 File Offset: 0x0003AC01
	public virtual byte[] Icon { get; }

	// Token: 0x06000758 RID: 1880 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnMenuLoad()
	{
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void SecondPassOnMenuLoad()
	{
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void MenuUpdate()
	{
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void MenuLateUpdate()
	{
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void MenuFixedUpdate()
	{
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void MenuOnGUI()
	{
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnLoad()
	{
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void SecondPassOnLoad()
	{
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void Update()
	{
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void LateUpdate()
	{
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void FixedUpdate()
	{
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnGUI()
	{
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnBarnSave()
	{
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnBarnLoad()
	{
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnBarnSaveFinish()
	{
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnNewMapUnload()
	{
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnNewMapLoad()
	{
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void Continue()
	{
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void SecondPassContinue()
	{
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnSave()
	{
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnSaveFinish()
	{
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnSaveSystemLoad(SaveSystem saver, bool isBarn)
	{
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0000245B File Offset: 0x0000065B
	public virtual void OnSaveSystemSave(SaveSystem saver, bool isBarn)
	{
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0003CA09 File Offset: 0x0003AC09
	public static explicit operator bool(Mod x)
	{
		return x != null;
	}

	// Token: 0x04000B56 RID: 2902
	public bool enabled = true;
}
