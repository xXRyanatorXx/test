using System;
using System.IO;

// Token: 0x0200023F RID: 575
public class BinaryReaderMac : BinaryReader
{
	// Token: 0x06000DB6 RID: 3510 RVA: 0x00092F5D File Offset: 0x0009115D
	public BinaryReaderMac(Stream stream) : base(stream)
	{
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x00092F66 File Offset: 0x00091166
	public override ushort ReadUInt16()
	{
		byte[] array = base.ReadBytes(2);
		Array.Reverse(array);
		return BitConverter.ToUInt16(array, 0);
	}
}
