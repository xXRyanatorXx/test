using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000243 RID: 579
[ExecuteInEditMode]
public class PW_Camera_Refraction : MonoBehaviour
{
	// Token: 0x06000DBC RID: 3516 RVA: 0x000931E4 File Offset: 0x000913E4
	private void CommandBufferDestroy(Camera i_cam)
	{
		if (i_cam == null)
		{
			return;
		}
		if (this._cbuf != null)
		{
			i_cam.RemoveCommandBuffer(this._cameraEvent, this._cbuf);
			this._cbuf.Clear();
			this._cbuf.Dispose();
			this._cbuf = null;
		}
		foreach (CommandBuffer commandBuffer in i_cam.GetCommandBuffers(this._cameraEvent))
		{
			if (commandBuffer.name == this._cbufName)
			{
				i_cam.RemoveCommandBuffer(this._cameraEvent, commandBuffer);
				commandBuffer.Clear();
				commandBuffer.Dispose();
			}
		}
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x00093280 File Offset: 0x00091480
	private void CommandBufferCreate()
	{
		this.CommandBufferDestroy(this._camera);
		this._cbuf = new CommandBuffer();
		this._cbuf.name = this._cbufName;
		this._cbuf.Clear();
		this._cbuf.GetTemporaryRT(this._grabID, (int)this.renderSize, (int)this.renderSize, 0, FilterMode.Bilinear);
		this._cbuf.Blit(BuiltinRenderTextureType.CurrentActive, this._grabID);
		this._cbuf.SetGlobalTexture("_CameraOpaqueTexture", this._grabID);
		this._camera.AddCommandBuffer(this._cameraEvent, this._cbuf);
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x00093330 File Offset: 0x00091530
	private void OnPreRender()
	{
		if (this._screenHeight != this._camera.pixelHeight || this._screenWidth != this._camera.pixelWidth)
		{
			this._screenWidth = this._camera.pixelWidth;
			this._screenHeight = this._camera.pixelHeight;
			this.CommandBufferCreate();
		}
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0009338B File Offset: 0x0009158B
	private void OnDisable()
	{
		this.CommandBufferDestroy(this._camera);
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0009339C File Offset: 0x0009159C
	private void OnEnable()
	{
		this._camera = base.GetComponent<Camera>();
		if (this._camera == null)
		{
			this._camera = Camera.main;
		}
		this._screenWidth = this._camera.pixelWidth;
		this._screenHeight = this._camera.pixelHeight;
		this.CommandBufferCreate();
		this._grabID = Shader.PropertyToID("_EchoTemp");
	}

	// Token: 0x0400164C RID: 5708
	private CameraEvent _cameraEvent = CameraEvent.AfterForwardOpaque;

	// Token: 0x0400164D RID: 5709
	private CommandBuffer _cbuf;

	// Token: 0x0400164E RID: 5710
	private string _cbufName = "Echo_Refaction";

	// Token: 0x0400164F RID: 5711
	private int _grabID;

	// Token: 0x04001650 RID: 5712
	private int _screenWidth;

	// Token: 0x04001651 RID: 5713
	private int _screenHeight;

	// Token: 0x04001652 RID: 5714
	private Camera _camera;

	// Token: 0x04001653 RID: 5715
	public PW_Camera_Refraction.PW_RENDER_SIZE renderSize = PW_Camera_Refraction.PW_RENDER_SIZE.HALF;

	// Token: 0x02000244 RID: 580
	[Serializable]
	public enum PW_RENDER_SIZE
	{
		// Token: 0x04001655 RID: 5717
		FULL = -1,
		// Token: 0x04001656 RID: 5718
		HALF = -2,
		// Token: 0x04001657 RID: 5719
		QUARTER = -3
	}
}
