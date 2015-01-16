
using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class ArrayBuffer<T> : IDisposable, IBindable where T : struct
	{
		private int name;
		
		public ArrayBuffer()
		{
			name = GL.GenBuffer();
		}

		public ArrayBuffer( T[] data )
		{
			Type tt = typeof(T);
			name = GL.GenBuffer();
			Bind();
			try
			{
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (data.Length * Marshal.SizeOf(tt)), data, BufferUsageHint.StaticDraw);
			}
			finally
			{
			  	UnBind();
			}
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, name);
		}
		public void UnBind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
		
		public void Dispose()
		{
			if(name != 0)
			{
				GL.DeleteBuffer(name);
				name = 0;
			}
		}

		public static implicit operator int(ArrayBuffer<T> b)
		{
			return b.name;
		}
	}
}
