using System;
using System.Runtime.InteropServices;

// Token: 0x02000157 RID: 343
public class MouseOperations
{
	// Token: 0x06000771 RID: 1905
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetCursorPos(int x, int y);

	// Token: 0x06000772 RID: 1906
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetCursorPos(out MouseOperations.MousePoint lpMousePoint);

	// Token: 0x06000773 RID: 1907
	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	// Token: 0x06000774 RID: 1908 RVA: 0x0003CA1E File Offset: 0x0003AC1E
	public static void SetCursorPosition(int x, int y)
	{
		MouseOperations.SetCursorPos(x, y);
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0003CA28 File Offset: 0x0003AC28
	public static void SetCursorPosition(MouseOperations.MousePoint point)
	{
		MouseOperations.SetCursorPos(point.X, point.Y);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0003CA3C File Offset: 0x0003AC3C
	public static MouseOperations.MousePoint GetCursorPosition()
	{
		MouseOperations.MousePoint result;
		if (!MouseOperations.GetCursorPos(out result))
		{
			result = new MouseOperations.MousePoint(0, 0);
		}
		return result;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0003CA5C File Offset: 0x0003AC5C
	public static void MouseEvent(MouseOperations.MouseEventFlags value)
	{
		MouseOperations.MousePoint cursorPosition = MouseOperations.GetCursorPosition();
		MouseOperations.mouse_event((int)value, cursorPosition.X, cursorPosition.Y, 0, 0);
	}

	// Token: 0x02000158 RID: 344
	[Flags]
	public enum MouseEventFlags
	{
		// Token: 0x04000B58 RID: 2904
		LeftDown = 2,
		// Token: 0x04000B59 RID: 2905
		LeftUp = 4,
		// Token: 0x04000B5A RID: 2906
		MiddleDown = 32,
		// Token: 0x04000B5B RID: 2907
		MiddleUp = 64,
		// Token: 0x04000B5C RID: 2908
		Move = 1,
		// Token: 0x04000B5D RID: 2909
		Absolute = 32768,
		// Token: 0x04000B5E RID: 2910
		RightDown = 8,
		// Token: 0x04000B5F RID: 2911
		RightUp = 16
	}

	// Token: 0x02000159 RID: 345
	public struct MousePoint
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x0003CA83 File Offset: 0x0003AC83
		public MousePoint(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x04000B60 RID: 2912
		public int X;

		// Token: 0x04000B61 RID: 2913
		public int Y;
	}
}
