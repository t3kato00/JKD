
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

			zoom = new Vector2d(0.2,0.0); // Y is initialized in config.
			viewPosition = new Vector2d(-5.0,0.0); // Y is initialized in config.

			Config();
		}

		public void Config()
		{            
			zoom.Y = 0.2*((double)Width)/(pixelRatio*(double)Height);
			viewPosition.Y = (2.0/((double)Height)-1.0) / zoom.Y;
			GL.Viewport (0, 0, Width, Height);
			JKD.CheckGLError();

			lines = new List<Line>
				{ new Line(new Vector2d(-1.0, -1.0), new Vector2d(1.0, -1.0))
				, new Line(new Vector2d(1.0, -1.0), new Vector2d(0.0, 1.0))
				, new Line(new Vector2d(0.0, 1.0), new Vector2d(-1.0, -1.0))
				};
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

			flatColorLineProgram.Zoom = (Vector2) zoom;
			flatColorLineProgram.ViewPosition = (Vector2) viewPosition;
			flatColorLineProgram.LineColor = new Vector4(255.0f,255.0f,255.0f,1.0f);
			flatColorLineProgram.DrawLines(lines);

			SwapBuffers();
		}

		public void Dispose()
		{
			flatColorLineProgram.Dispose();
		}
	}
}
