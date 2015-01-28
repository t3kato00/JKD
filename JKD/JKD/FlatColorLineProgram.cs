using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class FlatColorLineProgram : Program
	{
		public FlatColorLineProgram() 
			: base (new Shader[] { ShaderManager.Require(ShaderType.VertexShader, "JKD.Resources.Vertex.frag")
										, ShaderManager.Require(ShaderType.FragmentShader, "JKD.Resources.FlatColorLine.frag") })
		{
			viewPositionLocation = LoadUniformLocation("viewPosition");
			zoomLocation = LoadUniformLocation("zoom");
			lineColorLocation = LoadUniformLocation("lineColor");
			drawLinesVAO = new VertexArray();
			drawLinesVAO.Bind();

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

		int lineColorLocation;
		public Vector4 LineColor {
			set { Uniform(lineColorLocation, value); }
		}

		private VertexArray drawLinesVAO;
		public void DrawLines( List<Line> lines )
		{
			Vector2[] points = new Vector2[2*lines.Count];
			for( int index = 0; index < lines.Count; index += 1 )
			{
				points[2*index] = (Vector2) lines[index].A;
				points[2*index+1] = (Vector2) lines[index].B;
			}

			this.Bind();
			drawLinesVAO.Bind();
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

				JKD.Debug("glDrawArrays Lines", 0, points.Length);
				GL.DrawArrays(PrimitiveType.Lines, 0, points.Length);
				JKD.CheckGLError();
			}
		}
	}
}
