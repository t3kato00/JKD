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
		public Vector3 LineColor {
			set { Uniform(lineColorLocation, value); }
		}
	}
}
