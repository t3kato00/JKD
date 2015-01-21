
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
		public Shader( ShaderType st, string res )
		{
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(res))
            using (StreamReader r = new StreamReader(s, new ASCIIEncoding()))
			{
				name = GL.CreateShader( st );
				string prog = r.ReadToEnd();
				GL.ShaderSource(name, 1, new string[] { prog }, new int[] { prog.Length });
				GL.CompileShader(name);
				int status;
				GL.GetShader(name, ShaderParameter.CompileStatus, out status);
				if( status != 0 )
					return;
			}

			int length;
			GL.GetShader(name, ShaderParameter.InfoLogLength, out length);
			int rlength;
			StringBuilder log = new StringBuilder(length);
			GL.GetShaderInfoLog(name, length, out rlength, log);
			throw new PlatformNotSupportedException(log.ToString());
		}

		public void Dispose()
		{
			if( name != 0 )
			{
				GL.DeleteShader( name );
				name = 0;
			}
		}

		public static implicit operator int(Shader s)
		{
			return s.name;
		}
	}
}
