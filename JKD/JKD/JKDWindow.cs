
using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Reflection;

namespace JKD
{
	class JKDWindow : GameWindow, IDisposable
	{
		double pixelRatio = 1.4;
		double groundLength = 24.0;
		FlatColorLineProgram flatColorLineProgram;
		Vector2d zoom;
		Vector2d viewPosition;
		List<Line> lines;

		public JKDWindow()
		: base
			( 800, 600
			, new GraphicsMode( new ColorFormat(8,8,8,0) )
			, "Jäykän kappaleen dynamiikka"
			, GameWindowFlags.Default
			, DisplayDevice.Default
			, 3, 3
			, GraphicsContextFlags.ForwardCompatible
			)
		{
		}

		public Vector2d MousePosition
		{
			get
			{
				Vector2d coords = new Vector2d((double) Mouse.X, (double) Height - Mouse.Y);
				coords = coords.DivideBy( new Vector2d((double) Width, (double) Height));
				coords *= 2.0;
				coords -= new Vector2d( 1.0, 1.0 );
				return coords.DivideBy(zoom) - viewPosition;
			}
		}

		public void Initialize()
		{
			Load += (sender, e) => {
				VSync = VSyncMode.On;
			};
			Resize += (sender, e) => { Config (); };
			UpdateFrame += (sender, e) => { Update(); };
			RenderFrame += (sender, e) => { Render(); };
			Closed += (sender, e) => { Exit(); };

			//GL.Enable(EnableCap.LineSmooth);
			//GL.LoadAll();
			flatColorLineProgram = new FlatColorLineProgram();

			zoom = new Vector2d(2.0/groundLength,0.0); // Y is initialized in config.
			viewPosition = new Vector2d(-groundLength/2.0,0.0); // Y is initialized in config.

			Config();
		}

		public void Config()
		{            
			zoom.Y = zoom.X*((double)Width)/(pixelRatio*(double)Height);
			viewPosition.Y = (2.0/((double)Height)-1.0) / zoom.Y;
			GL.Viewport (0, 0, Width, Height);
			JKD.CheckGLError();

			lines = new List<Line>{};
			Vector2d[] points = new Vector2d[] {
				new Vector2d(20.0,0.0)
				, new Vector2d(20.0, 2.0), new Vector2d(21.0, 2.0)
				, new Vector2d(21.0, 4.0), new Vector2d(21.5, 4.5)
				, new Vector2d(22.0, 4.0), new Vector2d(22.0, 2.0)
				, new Vector2d(23.0, 2.0), new Vector2d(23.0, 0.0) };
			Vector2d p0 = points[points.Length - 1];
			foreach (Vector2d p1 in points)
			{
				lines.Add(new Line(p0, p1));
				p0 = p1;
			}
			GL.DrawBuffers(1, new DrawBuffersEnum[] { DrawBuffersEnum.FrontLeft });
			JKD.CheckGLError();
		}

		public void Update()
		{
			if (Keyboard[Key.Escape])
				Exit();
		}

		public void Render()
		{
			GL.ClearColor(0.0f, 0.0f, 0.0f, 255.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			JKD.CheckGLError();

			Vector2[] flatColorLinePoints = new Vector2[2*lines.Count + 2];
			for( int index = 0; index < lines.Count; index += 1 )
			{
				flatColorLinePoints[2*index] = (Vector2) lines[index].A;
				flatColorLinePoints[2*index+1] = (Vector2) lines[index].B;
			}
            flatColorLinePoints[2 * lines.Count] = new Vector2(0.0f, 0.0f);
            flatColorLinePoints[2 * lines.Count + 1] = (Vector2) MousePosition;

			using (VertexArray vertexArray = new VertexArray())
			{
				vertexArray.Bind();
				using (ArrayBuffer<Vector2> flatColorLinesBuffer = new ArrayBuffer<Vector2>(flatColorLinePoints))
				{
					flatColorLinesBuffer.Bind();
					GL.EnableVertexAttribArray(0);
					JKD.CheckGLError();
					GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8, 0);
					JKD.CheckGLError();

					flatColorLineProgram.Bind();
					flatColorLineProgram.Zoom = (Vector2) zoom;
					flatColorLineProgram.ViewPosition = (Vector2) viewPosition;
					flatColorLineProgram.LineColor = new Vector4(255.0f, 255.0f, 255.0f, 1.0f);
					JKD.Debug( "glDrawArrays Lines", flatColorLinePoints.Length );
					GL.DrawArrays(PrimitiveType.Lines, 0, flatColorLinePoints.Length);
					JKD.CheckGLError();
				}
			}
			SwapBuffers();
		}

		public void Dispose()
		{
			flatColorLineProgram.Dispose();
		}
	}
}
