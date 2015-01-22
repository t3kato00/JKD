using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class Program : IDisposable, IBindable
	{
		int name;
		static private int bound = 0;

		public Program( IEnumerable<Shader> shaders)
		{
			name = GL.CreateProgram();
			JKD.Debug("glCreateProgram", name);

			foreach (Shader shader in shaders)
				GL.AttachShader(name, shader);

			GL.LinkProgram(name);

			foreach (Shader shader in shaders)
				GL.DetachShader(name, shader);

			int status;
			GL.GetProgram(name, GetProgramParameterName.LinkStatus, out status);
			if (status != 0)
				return;

			int length;
			GL.GetProgram(name, GetProgramParameterName.InfoLogLength, out length);
			int rlength;
			StringBuilder log = new StringBuilder(length);
			GL.GetProgramInfoLog(name, length, out rlength, log);
			JKD.Debug("Program linking failed with", log.ToString());
			throw new PlatformNotSupportedException(log.ToString());
		}

		public void Dispose()
		{
			if( name != 0 )
			{
				UnBind();
				JKD.Debug("glDeleteProgram", name);
				GL.DeleteProgram( name );
				name = 0;
			}
		}

		public void Bind()
		{
			if( bound != name )
			{
				JKD.Debug("glUseProgam", name);
				GL.UseProgram(name);
				bound = name;
			}
		}
		public void UnBind()
		{
			if( bound == name )
			{
				JKD.Debug("glUseProgam", 0);
				GL.UseProgram(0);
				bound = 0;
			}
		}

		public static implicit operator int(Program s)
		{
			return s.name;
		}


		public void Uniform(int uniform, float value)
		{
			GL.Uniform1(uniform, value);
		}

		public void Uniform(int uniform, Vector2 value)
		{
			GL.Uniform2(uniform, value);
		}

		public void Uniform(int uniform, Vector3 value)
		{
			GL.Uniform3(uniform, value);
		}

		public void Uniform(int uniform, Vector4 value)
		{
			GL.Uniform4(uniform, value);
		}
	}
}
