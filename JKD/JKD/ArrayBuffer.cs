
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
			JKD.CheckGLError();
			JKD.Debug("glGenBuffer", name);
		}

		public ArrayBuffer( T[] data )
		{
			Type tt = typeof(T);
			name = GL.GenBuffer();
			JKD.Debug("glGenBuffer", name);
			Bind();
			JKD.Debug( "glBufferData ArrayBuffer", data.Length * Marshal.SizeOf(tt), "(" + data.Length.ToString() + ")");
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (data.Length * Marshal.SizeOf(tt)), data, BufferUsageHint.StaticDraw);
			JKD.CheckGLError();
		}

		public void Dispose()
		{
			if(name != 0)
			{
				UnBind();
				JKD.Debug("glDeleteBuffer", name);
				GL.DeleteBuffer(name);
				JKD.CheckGLError();
				name = 0;
			}
		}

		public void Bind()
		{
			if( bound != name )
			{
				JKD.Debug("glBindBuffer ArrayBuffer", name);
				GL.BindBuffer(BufferTarget.ArrayBuffer, name);
				JKD.CheckGLError();
				bound = name;
			}
		}

		public void UnBind()
		{
			if( bound == name )
			{
				JKD.Debug("glBindBuffer ArrayBuffer", 0);
				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                JKD.CheckGLError();
				bound = 0;
			}
		}
		
		public static implicit operator int(ArrayBuffer<T> b)
		{
			return b.name;
		}
	}
}
