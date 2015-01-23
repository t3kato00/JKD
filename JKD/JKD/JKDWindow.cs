
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
	class JKDWindow : GameWindow
	{
		FlatColorLineProgram flatColorLineProgram;
		Vector2d zoom;
		Vector2d viewPosition;
		List<Lined> lines;

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

		public void Initialize()
		{
			Load += (sender, e) => {
				VSync = VSyncMode.On;
			};
			Resize += (sender, e) => { Config (); };
			UpdateFrame += (sender, e) => { Update(); };
			RenderFrame += (sender, e) => { Render(); };
			Closed += (sender, e) => { Exit(); };

			//GL.LoadAll();
			flatColorLineProgram = new FlatColorLineProgram();
			Config();
		}

		public void Config(  )
		{            
			GL.Viewport (0, 0, Width, Height);
			JKD.CheckGLError();
			viewPosition = new Vector2d(0.0,0.0);
			zoom = new Vector2d(0.1,0.1);

			lines = new List<Lined> {new Lined(new Vector2d(0.0, 0.0), new Vector2d(9.0, 1.0)) };
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
			GL.Clear(ClearBufferMask.ColorBufferBit);
			JKD.CheckGLError();

			Vector2[] flatColorLinePoints = new Vector2[2*lines.Count];
			for( int index = 0; index < lines.Count; index += 1 )
			{
				flatColorLinePoints[2*index] = (Vector2) lines[index].a;
				flatColorLinePoints[2*index+1] = (Vector2) lines[index].b;
			}

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
					flatColorLineProgram.LineColor = new Vector3(1.0f, 1.0f, 1.0f);
					JKD.Debug( "glDrawArrays Lines", flatColorLinePoints.Length );
					GL.DrawArrays(PrimitiveType.Lines, 0, flatColorLinePoints.Length);
					JKD.CheckGLError();
				}
			}
			SwapBuffers();
		}
	}
}
