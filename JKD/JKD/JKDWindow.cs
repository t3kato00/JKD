
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
		double time;
		FlatColorLineProgram flatColorLineProgram;
		ArrowProgram arrowProgram;
		CursorProgram cursorProgram;
		Vector2d zoom;
		Vector2d viewPosition;
		List<Line> lines;

		private Vector2d AbsoluteZoom
		{
			get
			{
				return new Vector2d( pixelRatio/(double)Width, 1.0/(double)Height );
			}
		}


		public JKDWindow()
		: base
			( 800, 600
			, new GraphicsMode( new ColorFormat(8,8,8,0),0,0,4 )
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
			UpdateFrame += (sender, e) => { Update(e.Time); };
			RenderFrame += (sender, e) => { Render(); };
			Closed += (sender, e) => { Exit(); };

			//GL.Enable(EnableCap.LineSmooth);
			//GL.LoadAll();
			flatColorLineProgram = new FlatColorLineProgram();
			cursorProgram = new CursorProgram();
			arrowProgram = new ArrowProgram();

			OpenGLFeatures.Initialise();

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
			GL.BlendEquationSeparate(BlendEquationMode.Max, BlendEquationMode.Max);

			lines = new List<Line>{};
			Vector2d[] points = new Vector2d[]
				{ new Vector2d(20.0,0.0)
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
			GL.DrawBuffers(1, new DrawBuffersEnum[] { DrawBuffersEnum.BackLeft });
			JKD.CheckGLError();
		}

		public void Update(double t)
		{
			time += t;
			JKD.Debug("Time update", time);
			if (Keyboard[Key.Escape])
				Exit();
		}

		public void Render()
		{
			GL.Enable(IndexedEnableCap.Blend, 0);
			GL.ClearColor(0.0f, 0.0f, 0.0f, 255.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);
			JKD.CheckGLError();

			flatColorLineProgram.Zoom = (Vector2) zoom;
			flatColorLineProgram.ViewPosition = (Vector2) viewPosition;
			flatColorLineProgram.LineColor = new Vector4(255.0f,255.0f,255.0f,1.0f);
			flatColorLineProgram.DrawLines(lines);

			Cannonball ball = new Cannonball( new Vector2d( 0.0, 0.0 ), MousePosition );
			arrowProgram.ArrowColor = new Vector4(255.0f,0.0f,0.0f,1.0f);
			arrowProgram.Constants = new Vector3(5.0f, 15.0f, 10.0f);
			arrowProgram.AbsoluteZoom = (Vector2)AbsoluteZoom;
			arrowProgram.Zoom = (Vector2)zoom;
			arrowProgram.ViewPosition = (Vector2)viewPosition;
			arrowProgram.DrawArrow(new Line(ball.StartPosition, ball.StartVelocity));
			
			Vector2d pos;
			int index;
			Line line;
			double tc;
			GL.Disable(IndexedEnableCap.Blend, 0);
			if(ball.Collide( lines, out pos, out index, out line, out tc))
			{
				JKD.Debug("Collided");
				flatColorLineProgram.LineColor = new Vector4(255.0f,0.0f,0.0f,1.0f);
				flatColorLineProgram.DrawLine(line);
			}
			GL.Enable(IndexedEnableCap.Blend, 0);
			//Vector2d k = new Vector2d(0.05).DivideBy(zoom);
			//flatColorLineProgram.LineColor = new Vector4(0.0f,255.0f,0.0f,1.0f);
			//flatColorLineProgram.DrawLines( new List<Line>
			//	{ new Line(pos+k*new Vector2d(-1.0,-1.0), pos+k*new Vector2d(1.0,1.0))
			//	, new Line(pos+k*new Vector2d(-1.0,1.0), pos+k*new Vector2d(1.0,-1.0))
			//	} );
			cursorProgram.Zoom = (Vector2) zoom;
			cursorProgram.ViewPosition = (Vector2) viewPosition;
			cursorProgram.AbsoluteZoom = (Vector2) AbsoluteZoom;
			JKD.Debug("Time", time);
			cursorProgram.Time = (float) time;
			cursorProgram.CursorSize = 20.0f;
			cursorProgram.CursorBorder = 5.0f;
			cursorProgram.CursorColor = new Vector4(0.41f, 0.53f, 0.159f, 1.0f);
			cursorProgram.DrawCursor((Vector2) pos);

			flatColorLineProgram.LineColor = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
			flatColorLineProgram.DrawLines(ball.ArcPoints(tc, AbsoluteZoom));
			SwapBuffers();
		}

		public void Dispose()
		{
			flatColorLineProgram.Dispose();
		}
	}
}
