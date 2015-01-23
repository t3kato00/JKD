
using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class Shader : IDisposable
	{
		int name;
		uint requireCount = 1;
		public Shader( ShaderType st, string res )
		{
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(res))
            using (StreamReader r = new StreamReader(s, new ASCIIEncoding()))
			{
				name = GL.CreateShader( st );
				JKD.Debug("glCreateShader", name);
				string prog = r.ReadToEnd();
				GL.ShaderSource(name, 1, new string[] { prog }, new int[] { prog.Length });
				JKD.CheckGLError();
				GL.CompileShader(name);
				JKD.CheckGLError();
				int status;
				GL.GetShader(name, ShaderParameter.CompileStatus, out status);
				JKD.CheckGLError();
				if( status != 0 )
					return;
			}

			int length;
			GL.GetShader(name, ShaderParameter.InfoLogLength, out length);
			JKD.CheckGLError();
			int rlength;
			StringBuilder log = new StringBuilder(length);
			GL.GetShaderInfoLog(name, length, out rlength, log);
			JKD.CheckGLError();
			JKD.Debug("Shader compiling failed for", res, "with" , log);
			throw new PlatformNotSupportedException(log.ToString());
		}

		public void Dispose()
		{
			if( name != 0 )
			{
				JKD.Debug("glDeleteShader", name);
				GL.DeleteShader( name );
				JKD.CheckGLError();
				name = 0;
			}
		}

		public static implicit operator int(Shader s)
		{
			return s.name;
		}

		public void Require()
		{
			requireCount += 1;
		}

		public bool Release()
		{
			requireCount -= 1;
			return requireCount == 0;
		}
	}
}
