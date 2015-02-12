using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class CursorProgram : Program
	{
		public CursorProgram() 
			: base (new Shader[] { ShaderManager.Require(ShaderType.VertexShader, "JKD.Resources.Vertex.frag")
										, ShaderManager.Require(ShaderType.GeometryShader, "JKD.Resources.CursorGeom.frag") 
										, ShaderManager.Require(ShaderType.FragmentShader, "JKD.Resources.Cursor.frag") })
		{
			viewPositionLocation = LoadUniformLocation("viewPosition");
			zoomLocation = LoadUniformLocation("zoom");
			timeLocation = LoadUniformLocation("time");
			absoluteZoomLocation = LoadUniformLocation("absoluteZoom");
			cursorSizeLocation = LoadUniformLocation("cursorSize");
			cursorBorderLocation = LoadUniformLocation("cursorBorder");
			cursorColorLocation = LoadUniformLocation("cursorColor");

			VAO = new VertexArray();
			VAO.Bind();

			JKD.Debug("glEnableVertexAttribArray", 0);
			GL.EnableVertexAttribArray(0);
			JKD.CheckGLError();
			if (OpenGLFeatures.separateVertexFormat)
			{
				JKD.Debug("glVertexAttribFormat", 0, 2, "Float", "false", 0);
				GL.VertexAttribFormat(0, 2, VertexAttribType.Float, false, 0);
				JKD.CheckGLError();

				JKD.Debug("glVertexAttribBinding", 0, 0);
				GL.VertexAttribBinding(0, 0);
				JKD.CheckGLError();
			}
		}

		int viewPositionLocation;
		public Vector2 ViewPosition {
			set { Uniform(viewPositionLocation, value); } 
		}

		int zoomLocation;
		public Vector2 Zoom {
			set { Uniform(zoomLocation, value); }
		}

		int absoluteZoomLocation;
		public Vector2 AbsoluteZoom {
			set { Uniform(absoluteZoomLocation, value); }
		}

		int timeLocation;
		public float Time {
			set { Uniform(timeLocation, value); }
		}

		int cursorSizeLocation;
		public float CursorSize {
			set { Uniform(cursorSizeLocation, value); }
		}

		int cursorBorderLocation;
		public float CursorBorder {
			set { Uniform(cursorBorderLocation, value); }
		}

		int cursorColorLocation;
		public Vector4 CursorColor {
			set { Uniform(cursorColorLocation, value); }
		}

		private VertexArray VAO;
		public void DrawCursor( Vector2 pos )
		{
			Vector2[] points = new Vector2[] { pos };

			this.Bind();
			VAO.Bind();
			using( ArrayBuffer<Vector2> buf = new ArrayBuffer<Vector2>(points) )
			{
				if (OpenGLFeatures.separateVertexFormat)
				{
					JKD.Debug("glBindVertexBuffer", 0, (int)buf, 0, 8);
					GL.BindVertexBuffer(0, buf, (IntPtr)0, 8);
					JKD.CheckGLError();
				}
				else
				{
					GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);	
				}

				JKD.Debug("glDrawArrays Points", 0, points.Length);
				GL.DrawArrays(PrimitiveType.Points, 0, points.Length);
				JKD.CheckGLError();
			}
		}
	}
}
