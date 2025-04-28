using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

/*
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Haden.Utilities.CF.DirectX {
	
	public class Renderer {
		private static Device device;
		private static VertexBuffer vertices;
		private static Control surface;

		/// <summary>
		/// (Re)initializes the renderer
		/// </summary>
		public static void Init(Control surface) {
			Renderer.surface = surface;
			PresentParameters pres = new PresentParameters();
			pres.Windowed = true;
			pres.SwapEffect = SwapEffect.Discard;


			device = new Device(0, DeviceType.Hardware, surface, CreateFlags.SoftwareVertexProcessing, pres);

			vertices = CreateVertexBuffer(device);
		}

		protected static VertexBuffer CreateVertexBuffer(Device device) {
			device.VertexFormat = CustomVertex.PositionColored.Format;

			CustomVertex.PositionColored[] verts = new CustomVertex.PositionColored[3];


			verts[0] = new CustomVertex.PositionColored(0, 10, 0, Color.Blue.ToArgb());
			verts[1] = new CustomVertex.PositionColored(-10, -10, 0, Color.Green.ToArgb());
			verts[2] = new CustomVertex.PositionColored(10, -10, 0, Color.Red.ToArgb());


			VertexBuffer buf =
			  new VertexBuffer(
			   typeof(CustomVertex.PositionColored),
				verts.Length,
				device,
				0,
				CustomVertex.PositionColored.Format,
				Pool.Default
			);


			GraphicsStream stm = buf.Lock(0, 0, 0);
			stm.Write(verts);
			buf.Unlock();			
			
			return buf;
		}

		/// <summary>
		/// Renders contents to surface
		/// </summary>
		public static void Render() {

			// Clear the back buffer
			device.Clear(ClearFlags.Target, Color.Yellow, 1.0f, 0);


			// Ready Direct3D to begin drawing
			device.BeginScene();

			// Set up projection
			float angle = (float)Environment.TickCount / 500.0F;

			device.RenderState.Lighting = true;
			device.RenderState.CullMode = Cull.None;
			device.Transform.World = Matrix.RotationY(angle);
			device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, -3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
			device.Transform.Projection = Matrix.OrthoLH(20, 20, 1.0f, 10.0f);


			// Draw the scene - 3D Rendering calls go here
			device.VertexFormat = CustomVertex.PositionColored.Format; // It seems we need to set this first or we keep getting InvalidCallException
			device.SetStreamSource(0, vertices, 0);
			device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);

			// Indicate to Direct3D that we’re done drawing
			device.EndScene();


			// Copy the back buffer to the display
			device.Present();
		}


	}

	public class RenderView {
		public static void Init() {
		}
	}
}
*/