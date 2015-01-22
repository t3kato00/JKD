
using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class ArrayBuffer<T> : IDisposable, IBindable where T : struct
	{
		private int name;
		static private int bound = 0;
		
		public ArrayBuffer()
		{
			name = GL.GenBuffer();
			JKD.Debug("glGenBuffer", name);
		}

		public ArrayBuffer( T[] data )
		{
			Type tt = typeof(T);
			name = GL.GenBuffer();
			JKD.Debug("glGenBuffer", name);
			Bind();
			try
			{
				JKD.Debug( "glBufferData ArrayBuffer", data.Length * Marshal.SizeOf(tt), "(" + data.Length.ToString() + ")");
				GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (data.Length * Marshal.SizeOf(tt)), data, BufferUsageHint.StaticDraw);
			}
			finally
			{
			  	UnBind();
			}
		}

		public void Dispose()
		{
			if(name != 0)
			{
				UnBind();
				JKD.Debug("glDeleteBuffer", name);
				GL.DeleteBuffer(name);
				name = 0;
			}
		}

		public void Bind()
		{
			if( bound != name )
			{
				JKD.Debug("glBindBuffer ArrayBuffer", name);
				GL.BindBuffer(BufferTarget.ArrayBuffer, name);
				bound = name;
			}
		}

		public void UnBind()
		{
			if( bound == name )
			{
				JKD.Debug("glBindBuffer ArrayBuffer", 0);
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				bound = 0;
			}
		}
		
		public static implicit operator int(ArrayBuffer<T> b)
		{
			return b.name;
		}
	}
}
