
using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class VertexArray : IDisposable, IBindable
	{
		private int name;
		static private int bound = 0;
		
		public VertexArray()
		{
			name = GL.GenVertexArray();
			JKD.Debug("glGenVertexArray", name);
		}

		public void Dispose()
		{
			if(name != 0)
			{
				UnBind();
				JKD.Debug("glDeleteVertexArray", name);
				GL.DeleteVertexArray(name);
				name = 0;
			}
		}

		public void Bind()
		{
			if( bound != name )
			{
				JKD.Debug("glBindVertexArray", name);
				GL.BindVertexArray(name);
				bound = name;
			}
		}

		public void UnBind()
		{
			if( bound == name )
			{
				JKD.Debug("glBindVertexArray", 0);
				GL.BindVertexArray(0);
				bound = 0;
			}
		}
		
		public static implicit operator int(VertexArray b)
		{
			return b.name;
		}
	}
}
