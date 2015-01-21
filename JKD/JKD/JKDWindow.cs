
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
		double width;
		double height;
        List<Lined> lines;

		public JKDWindow() : base( 800, 600, new GraphicsMode( new ColorFormat(8,8,8,0) ), "Jäykän kappaleen dynamiikka" )
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

            using (Shader vertex = new Shader(ShaderType.VertexShader, "JKD.Resources.Vertex.frag"))
            using (Shader flatColorLine = new Shader(ShaderType.FragmentShader, "JKD.Resources.FlatColorLine.frag"))
            {
                flatColorLineProgram = new Program(new Shader[] {vertex, flatColorLine});
            }

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

			Line[] flatColorLines = new Line[lines.Count];
			for( int index = 0; index < lines.Count; index += 1 )
				flatColorLines[index] = (Line) lines[index];

			using (VertexArray vertexArray = new VertexArray())
			using (Binding vbind = new Binding(vertexArray))
			{
                using (ArrayBuffer<Line> flatColorLinesBuffer = new ArrayBuffer<Line>(flatColorLines))
				using (Binding bbind = new Binding(flatColorLinesBuffer))
				{
					// Create and bind vertex array todo.
					GL.EnableVertexAttribArray(0);
					GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
				}
                using (Binding pbind = new Binding(flatColorLineProgram))
                {
                    flatColorLineProgram.Uniform(0, (Vector2) zoom);
                    flatColorLineProgram.Uniform(1, (Vector2) viewPosition);
                    flatColorLineProgram.Uniform(2, new Vector3(1.0f, 1.0f, 1.0f));
                    GL.DrawArrays(BeginMode.Lines, 0, flatColorLines.Length);    
                }
			}

            if (GL.GetError() != 0)
                throw new GraphicsContextException("Graphics error");
			SwapBuffers();
		}
	}
}
