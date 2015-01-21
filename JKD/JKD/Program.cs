using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
    class Program : IDisposable
    {
        int name;
		public Program( IEnumerable<Shader> shaders)
		{
            name = GL.CreateProgram();
            
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
            throw new PlatformNotSupportedException(log.ToString());

        }

		public void Dispose()
		{
			if( name != 0 )
			{
				GL.DeleteProgram( name );
				name = 0;
			}
		}

		public static implicit operator int(Program s)
		{
			return s.name;
		}
    }
}
