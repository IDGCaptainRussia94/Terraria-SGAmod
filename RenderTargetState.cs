using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod
{
	//Taken from here: https://github.com/pentiumx/HLSLTest/blob/master/HLSLTest/HLSLTest/Planet/RenderTargetState.cs

	/// <summary>
	/// チュートリアル作者の独自クラスだが、現段階だと
	/// ほとんどコードの見通しが悪くなるだけなので正直あまり使いたくない
	/// </summary>
	public class RenderTargetState
	{
		public RenderTarget2D RenderTarget;
		//public DepthStencilBuffer DepthBuffer;
		GraphicsDevice GraphicsDevice;
		RenderTargetState oldState;

		bool CreatedBuffers;

		int renderTargetWidth, renderTargetHeight;
		int depthBufferWidth, depthBufferHeight;

		public RenderTargetState(GraphicsDevice GraphicsDevice,
								 int renderTargetIndex)
		{
			this.GraphicsDevice = GraphicsDevice;
			//RenderTarget = (RenderTarget2D)GraphicsDevice.GetRenderTarget(renderTargetIndex);
			RenderTargetBinding[] rents = GraphicsDevice.GetRenderTargets();
			if (rents.Length > 0) RenderTarget = (RenderTarget2D)rents[0].RenderTarget;

			//DepthBuffer = GraphicsDevice.DepthStencilBuffer;
			CreatedBuffers = false;
		}

		public RenderTargetState(GraphicsDevice GraphicsDevice)
			:
					this(GraphicsDevice, 0)
		{

		}

		public RenderTargetState(GraphicsDevice GraphicsDevice,
								 int renderTargetWidth,
								 int renderTargetHeight)
			: this(GraphicsDevice,
				   renderTargetWidth,
				   renderTargetHeight,
				   renderTargetWidth,
				   renderTargetHeight)
		{
		}

		public RenderTargetState(GraphicsDevice GraphicsDevice,
								 int renderTargetWidth,
								 int renderTargetHeight,
								 int depthBufferWidth,
								 int depthBufferHeight)
		{
			this.GraphicsDevice = GraphicsDevice;

			this.renderTargetWidth = renderTargetWidth;
			this.renderTargetHeight = renderTargetHeight;
			this.depthBufferWidth = depthBufferWidth;
			this.depthBufferHeight = depthBufferHeight;

			CreateBuffers();

			CreatedBuffers = true;

			//GraphicsDevice.DeviceReset += new EventHandler(GraphicsDevice_DeviceReset);
			//GraphicsDevice.DeviceResetting += new EventHandler(GraphicsDevice_DeviceResetting);
			GraphicsDevice.DeviceReset += GraphicsDevice_DeviceReset;
			GraphicsDevice.DeviceResetting += GraphicsDevice_DeviceResetting;
		}

		private void CreateBuffers()
		{
			/*RenderTarget =
				new RenderTarget2D(
					GraphicsDevice,
					renderTargetWidth,
					renderTargetHeight,
					1,
					RenderTargetHelper.SelectRenderTargetMode(false));*/
			RenderTarget = new RenderTarget2D(GraphicsDevice, renderTargetWidth, renderTargetHeight, false, RenderTargetHelper.SelectRenderTargetMode(false), DepthFormat.Depth24);

			/*DepthBuffer =
				new DepthStencilBuffer(
					GraphicsDevice,
					depthBufferWidth,
					depthBufferHeight,
					GraphicsDevice.DepthStencilBuffer.Format);*/
		}

		void GraphicsDevice_DeviceReset(object sender, EventArgs e)
		{
			if (CreatedBuffers)
				CreateBuffers();
		}

		void GraphicsDevice_DeviceResetting(object sender, EventArgs e)
		{
			DestroyBuffers();
		}

		public RenderTargetState SetToDevice()
		{
			return SetToDevice(0);
		}

		public RenderTargetState SetToDevice(int renderTargetIndex)
		{
			oldState = new RenderTargetState(GraphicsDevice,
											 renderTargetIndex);

			//GraphicsDevice.SetRenderTarget(renderTargetIndex,  RenderTarget);
			GraphicsDevice.SetRenderTarget(RenderTarget);
			//GraphicsDevice.DepthStencilBuffer = DepthBuffer;

			return oldState;
		}

		public void DestroyBuffers()
		{
			oldState = null;

			if (CreatedBuffers)
			{
				if (RenderTarget != null)
				{
					RenderTarget.Dispose();
					RenderTarget = null;
				}
				/*if (DepthBuffer != null) {
					DepthBuffer.Dispose();
					DepthBuffer = null;
				}*/
			}
		}

		public RenderTargetState BeginRenderToTexture()
		{
			oldState = SetToDevice();

			return oldState;
		}

		public RenderTargetState BeginRenderToTexture(int renderTargetIndex)
		{
			oldState = SetToDevice(renderTargetIndex);

			return oldState;
		}

		public RenderTargetState EndRenderToTexture()
		{
			return EndRenderToTexture(0);
		}

		public RenderTargetState EndRenderToTexture(int renderTargetIndex)
		{
			RenderTargetState renderBuffer =
					oldState.SetToDevice(renderTargetIndex);

			oldState = null;

			return renderBuffer;
		}

		public Texture2D EndRenderGetTexture()
		{
			return EndRenderGetTexture(0);
		}

		public Texture2D EndRenderGetTexture(int renderTargetIndex)
		{
			RenderTargetState renderBuffer =
					oldState.SetToDevice(renderTargetIndex);

			oldState = null;

			//return renderBuffer.RenderTarget.GetTexture();
			return renderBuffer.RenderTarget;
		}
	}

	public class RenderTargetHelper
	{
		public static DepthFormat SelectStencilMode(
										SurfaceFormat renderTargetFormat)
		{
			// Check stencil formats
			GraphicsAdapter adapter = GraphicsAdapter.DefaultAdapter;
			SurfaceFormat format = adapter.CurrentDisplayMode.Format;

			//DepthFormat depthFormat = DepthFormat.Unknown;
			DepthFormat depthFormat = DepthFormat.Depth24;


			/*if (adapter.CheckDepthStencilMatch(
						DeviceType.Hardware,
						format,
						renderTargetFormat,
						DepthFormat.Depth24Stencil8)) {
				depthFormat = DepthFormat.Depth24Stencil8;
			} else if (adapter.CheckDepthStencilMatch(
						  DeviceType.Hardware,
						  format,
						  renderTargetFormat,
						  DepthFormat.Depth24Stencil8Single)) {
				depthFormat = DepthFormat.Depth24Stencil8Single;
			} else if (adapter.CheckDepthStencilMatch(
						  DeviceType.Hardware,
						  format,
						  renderTargetFormat,
						  DepthFormat.Depth24Stencil4)) {
				depthFormat = DepthFormat.Depth24Stencil4;
			} else if (adapter.CheckDepthStencilMatch(
						  DeviceType.Hardware,
						  format,
						  renderTargetFormat,
						  DepthFormat.Depth15Stencil1)) {
				depthFormat = DepthFormat.Depth15Stencil1;
			} else if (adapter.CheckDepthStencilMatch(
						  DeviceType.Reference,
						  format,
						  renderTargetFormat,
						  DepthFormat.Depth24Stencil8)) {
				depthFormat = DepthFormat.Depth24Stencil8;
			} else if (adapter.CheckDepthStencilMatch(
						  DeviceType.Reference,
						  format,
						  renderTargetFormat,
						  DepthFormat.Depth24Stencil8Single)) {
				depthFormat = DepthFormat.Depth24Stencil8Single;
			} else if (adapter.CheckDepthStencilMatch(
						  DeviceType.Reference,
						  format,
						  renderTargetFormat,
						  DepthFormat.Depth24Stencil4)) {
				depthFormat = DepthFormat.Depth24Stencil4;
			} else if (adapter.CheckDepthStencilMatch(
						  DeviceType.Reference,
						  format,
						  renderTargetFormat,
						  DepthFormat.Depth15Stencil1)) {
				depthFormat = DepthFormat.Depth15Stencil1;
			}*/


			return depthFormat;
		}


		public static SurfaceFormat SelectRenderTargetMode(bool preferFloat)
		{
			// Check stencil formats
			GraphicsAdapter adapter = GraphicsAdapter.DefaultAdapter;
			SurfaceFormat displayFormat = adapter.CurrentDisplayMode.Format;

			DepthFormat depthFormat = SelectStencilMode(displayFormat);

			SurfaceFormat surfaceFormat = SurfaceFormat.Color;

			/*if (preferFloat) {
				if (adapter.CheckDepthStencilMatch(
							DeviceType.Hardware,
							displayFormat,
							SurfaceFormat.Single,
							depthFormat)) {
					surfaceFormat = SurfaceFormat.Single;
				} else if (adapter.CheckDepthStencilMatch(
							  DeviceType.Hardware,
							  displayFormat,
							  SurfaceFormat.HalfSingle,
							  depthFormat)) {
					surfaceFormat = SurfaceFormat.HalfSingle;
				} else if (adapter.CheckDepthStencilMatch(
							  DeviceType.Hardware,
							  displayFormat,
							  SurfaceFormat.Color,
							  depthFormat)) {
					surfaceFormat = SurfaceFormat.Color;
				} else if (adapter.CheckDepthStencilMatch(
							  DeviceType.Reference,
							  displayFormat,
							  SurfaceFormat.Single,
							  depthFormat)) {
					surfaceFormat = SurfaceFormat.Single;
				} else if (adapter.CheckDepthStencilMatch(
							  DeviceType.Reference,
							  displayFormat,
							  SurfaceFormat.HalfSingle,
							  depthFormat)) {
					surfaceFormat = SurfaceFormat.HalfSingle;
				} else if (adapter.CheckDepthStencilMatch(
							  DeviceType.Reference,
							  displayFormat,
							  SurfaceFormat.Color,
							  depthFormat)) {
					surfaceFormat = SurfaceFormat.Color;
				} else {
					surfaceFormat = SurfaceFormat.Unknown;
				}
			}*/

			return surfaceFormat;
		}
	}
}
