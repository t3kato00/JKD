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
		IEnumerable<Shader> shaders;

		public Program( IEnumerable<Shader> shaders)
		{
			name = GL.CreateProgram();
			JKD.CheckGLError();
			JKD.Debug("glCreateProgram", name);
			this.shaders = shaders;
			foreach (Shader shader in shaders)
				GL.AttachShader(name, shader);
			JKD.CheckGLError();
			GL.LinkProgram(name);
			JKD.CheckGLError();
			int status;
			GL.GetProgram(name, GetProgramParameterName.LinkStatus, out status);
			JKD.CheckGLError();
			if (status != 0)
				return;

			int length;
			GL.GetProgram(name, GetProgramParameterName.InfoLogLength, out length);
			JKD.CheckGLError();
			int rlength;
			StringBuilder log = new StringBuilder(length);
			GL.GetProgramInfoLog(name, length, out rlength, log);
			JKD.CheckGLError();
			JKD.Debug("Program linking failed with", log.ToString());
			throw new PlatformNotSupportedException(log.ToString());
		}

		public void Dispose()
		{
			if( name != 0 )
			{
				foreach (Shader shader in shaders)
				{
					GL.DetachShader(name, shader);
					ShaderManager.Release(shader);
				}
				JKD.CheckGLError();

				UnBind();
				JKD.Debug("glDeleteProgram", name);
				GL.DeleteProgram( name );
				JKD.CheckGLError();
				name = 0;
			}
		}

		public void Bind()
		{
			if( bound != name )
			{
				JKD.Debug("glUseProgam", name);
				GL.UseProgram(name);
				JKD.CheckGLError();
				bound = name;
			}
		}
		public void UnBind()
		{
			if( bound == name )
			{
				JKD.Debug("glUseProgam", 0);
				GL.UseProgram(0);
				JKD.CheckGLError();
				bound = 0;
			}
		}

		public static implicit operator int(Program s)
		{
			return s.name;
		}


		protected void Uniform(int uniform, float value)
		{
			GL.ProgramUniform1(name, uniform, value);
			JKD.CheckGLError();
		}

		protected void Uniform(int uniform, Vector2 value)
		{
			GL.ProgramUniform2(name, uniform, value.X, value.Y);
			JKD.CheckGLError();
		}

		protected void Uniform(int uniform, Vector3 value)
		{
			GL.ProgramUniform3(name, uniform, value.X, value.Y, value.Z);
			JKD.CheckGLError();
		}

		protected void Uniform(int uniform, Vector4 value)
		{
			GL.ProgramUniform4(name, uniform, value.X, value.Y, value.Z, value.W);
			JKD.CheckGLError();
		}

		protected int LoadUniformLocation(string symbol)
		{
			int result = GL.GetUniformLocation(name, symbol);
			JKD.CheckGLError();
			return result;
		}

	}
}
