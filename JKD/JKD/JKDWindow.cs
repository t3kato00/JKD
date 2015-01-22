
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
		Program flatColorLineProgram;
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
			using (Shader vertex = new Shader(ShaderType.VertexShader, "JKD.Resources.Vertex.frag"))
			using (Shader flatColorLine = new Shader(ShaderType.FragmentShader, "JKD.Resources.FlatColorLine.frag"))
			{
				flatColorLineProgram = new Program(new Shader[] {vertex, flatColorLine});
			}

			Config();
		}

		public void Config(  )
		{            
			GL.Viewport (0, 0, Width, Height);
			viewPosition = new Vector2d(0.0,0.0);
			zoom = new Vector2d(0.1,0.1);

			lines = new List<Lined> {new Lined(new Vector2d(0.0, 0.0), new Vector2d(9.0, 1.0)) };
			GL.DrawBuffers(1, new DrawBuffersEnum[] { DrawBuffersEnum.FrontLeft });
		}

		public void Update()
		{
			if (Keyboard[Key.Escape])
				Exit();
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

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
					GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8, 0);

					flatColorLineProgram.Bind();
					flatColorLineProgram.Uniform(0, (Vector2) zoom);
					flatColorLineProgram.Uniform(2, (Vector2) viewPosition);
					flatColorLineProgram.Uniform(4, new Vector3(1.0f, 1.0f, 1.0f));
					JKD.Debug( "glDrawArrays Lines", flatColorLinePoints.Length );
					GL.DrawArrays(PrimitiveType.Lines, 0, flatColorLinePoints.Length);    
				}
			}

			if (GL.GetError() != 0)
				throw new GraphicsContextException("Graphics error");
			SwapBuffers();
		}
	}
}
