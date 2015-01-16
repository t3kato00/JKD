
using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace JKD
{
	class JKDWindow : GameWindow
	{
		double zoom;
		Vector2d viewPosition;
		double width;
		double height;
		List<Lined> lines = new List<Lined>();

		public JKDWindow() : base( 800, 600, new GraphicsMode( new ColorFormat(8,8,8,0) ), "Jäykän kappaleen dynamiikka" )
		{
		}

		public void Initialize()
		{
			Load += (sender, e) => {
				VSync = VSyncMode.On;
			};
			Resize += (sender, e) => { Config (-viewPosition.Y, -viewPosition.X, -viewPosition.X+width/zoom); };
			UpdateFrame += (sender, e) => { Update(); };
			RenderFrame += (sender, e) => { Render(); };
			Closed += (sender, e) => { Exit(); };
		}

		public void Config( double y0, double x0, double x1 )
		{
			GL.Viewport (0, 0, Width, Height);
			viewPosition = new Vector2d( -x0, -y0 );
			zoom = Width / (x1 - x0);

			width = Width;
			height = Height;
		}

		public void Update()
		{
			if (Keyboard[Key.Escape])
				Exit();
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			Line[] flatColorLines = new Line[lines.Count];
			for( int index = 0; index < lines.Count; index += 1 )
				flatColorLines[index] = (Line) lines[index];

			using (VertexArray vertexArray = new VertexArray())
			using (Binding vbind = new Binding(vertexArray))
			using (ArrayBuffer<Line> flatColorLinesBuffer = new ArrayBuffer<Line>(flatColorLines))
			{
				using (Binding bbind = new Binding(flatColorLinesBuffer))
				{
					// Create and bind vertex array todo.
					GL.EnableVertexAttribArray(0);
					GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
				}

				GL.DrawArrays( BeginMode.Lines, 0, flatColorLines.Length );
			}

			SwapBuffers();
		}
	}
}
