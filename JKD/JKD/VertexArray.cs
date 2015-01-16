
using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class VertexArray : IDisposable, IBindable
	{
		private int name;
		
		public VertexArray()
		{
			name = GL.GenVertexArray();
		}

		public void Bind()
		{
			GL.BindVertexArray(name);
		}
		public void UnBind()
		{
			GL.BindVertexArray(0);
		}
		
		public void Dispose()
		{
			if(name != 0)
			{
				GL.DeleteVertexArray(name);
				name = 0;
			}
		}

		public static implicit operator int(VertexArray b)
		{
			return b.name;
		}
	}
}
