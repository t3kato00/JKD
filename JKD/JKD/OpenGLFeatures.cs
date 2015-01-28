using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	static class OpenGLFeatures
	{
		public static int major, minor;
		public static bool separateVertexFormat = false;

		public static void Initialise()
		{
			string version = GL.GetString(StringName.Version);
			//major = int.Parse(version.Split(' ')[0]);
			//minor = int.Parse(version.Split(' ')[1]);

			int count = GL.GetInteger(GetPName.NumExtensions);
			for (int i = 0; i < count; i++)
			{
				string extension = GL.GetString(StringNameIndexed.Extensions, i);
				JKD.Debug("extension", extension);
				if (extension == "GL_ARB_vertex_attrib_binding")
					separateVertexFormat = true;
			}

			JKD.Debug("GL_ARB_vertex_attrib_binding", separateVertexFormat);
		}
	}
}
