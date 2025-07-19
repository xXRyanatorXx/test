using System;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class NumberPlateManager : MonoBehaviour
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x00054A88 File Offset: 0x00052C88
	public void CreateRandomNumber()
	{
		this.M1 = this.AllMaterials[UnityEngine.Random.Range(0, this.AllMaterials.Length)];
		this.M2 = this.AllMaterials[UnityEngine.Random.Range(0, this.AllMaterials.Length)];
		this.M3 = this.AllMaterials[UnityEngine.Random.Range(0, this.AllMaterials.Length)];
		this.M4 = this.AllMaterials[UnityEngine.Random.Range(0, this.AllMaterials.Length)];
		this.M5 = this.AllMaterials[UnityEngine.Random.Range(0, this.AllMaterials.Length)];
		this.M6 = this.AllMaterials[UnityEngine.Random.Range(0, this.AllMaterials.Length)];
	}

	// Token: 0x04001078 RID: 4216
	public Material Line;

	// Token: 0x04001079 RID: 4217
	public Material Zero;

	// Token: 0x0400107A RID: 4218
	public Material One;

	// Token: 0x0400107B RID: 4219
	public Material Two;

	// Token: 0x0400107C RID: 4220
	public Material Three;

	// Token: 0x0400107D RID: 4221
	public Material Four;

	// Token: 0x0400107E RID: 4222
	public Material Five;

	// Token: 0x0400107F RID: 4223
	public Material Six;

	// Token: 0x04001080 RID: 4224
	public Material Seven;

	// Token: 0x04001081 RID: 4225
	public Material Eight;

	// Token: 0x04001082 RID: 4226
	public Material Nine;

	// Token: 0x04001083 RID: 4227
	public Material A;

	// Token: 0x04001084 RID: 4228
	public Material B;

	// Token: 0x04001085 RID: 4229
	public Material C;

	// Token: 0x04001086 RID: 4230
	public Material D;

	// Token: 0x04001087 RID: 4231
	public Material E;

	// Token: 0x04001088 RID: 4232
	public Material F;

	// Token: 0x04001089 RID: 4233
	public Material G;

	// Token: 0x0400108A RID: 4234
	public Material H;

	// Token: 0x0400108B RID: 4235
	public Material I;

	// Token: 0x0400108C RID: 4236
	public Material J;

	// Token: 0x0400108D RID: 4237
	public Material K;

	// Token: 0x0400108E RID: 4238
	public Material L;

	// Token: 0x0400108F RID: 4239
	public Material M;

	// Token: 0x04001090 RID: 4240
	public Material N;

	// Token: 0x04001091 RID: 4241
	public Material O;

	// Token: 0x04001092 RID: 4242
	public Material P;

	// Token: 0x04001093 RID: 4243
	public Material Q;

	// Token: 0x04001094 RID: 4244
	public Material R;

	// Token: 0x04001095 RID: 4245
	public Material S;

	// Token: 0x04001096 RID: 4246
	public Material T;

	// Token: 0x04001097 RID: 4247
	public Material U;

	// Token: 0x04001098 RID: 4248
	public Material V;

	// Token: 0x04001099 RID: 4249
	public Material W;

	// Token: 0x0400109A RID: 4250
	public Material X;

	// Token: 0x0400109B RID: 4251
	public Material Y;

	// Token: 0x0400109C RID: 4252
	public Material Z;

	// Token: 0x0400109D RID: 4253
	public Material Empty;

	// Token: 0x0400109E RID: 4254
	public Material[] AllMaterials;

	// Token: 0x0400109F RID: 4255
	public Material M1;

	// Token: 0x040010A0 RID: 4256
	public Material M2;

	// Token: 0x040010A1 RID: 4257
	public Material M3;

	// Token: 0x040010A2 RID: 4258
	public Material M4;

	// Token: 0x040010A3 RID: 4259
	public Material M5;

	// Token: 0x040010A4 RID: 4260
	public Material M6;
}
