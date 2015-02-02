
using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JKD
{
	class JKD
	{
		[STAThread]
		public static void Main()
		{
			using (var game = new JKDWindow())
			{
				game.Initialize();
				try {
					game.Run(60.0);
				}
				finally {
					game.Dispose();
				}
			}
		}

		public static void Debug( string caller, params object[] data )
		{
			string msg = caller + ":";
			foreach( object d in data )
			{
				msg += " ";
				if( d is string )
					msg += d;
				if( d is Vector2d )
				{
					Vector2d dd = (Vector2d) d;
					msg += "(" + dd.X + ", " + dd.Y + ")";
				}
				else
					msg += d.ToString();
			}

			Console.WriteLine(msg);
		}

        public static void CheckGLError()
        {
			  bool hasError = false;
			  string result = "Graphics context: ";
			  ErrorCode err;
			  while ((err = GL.GetError()) != ErrorCode.NoError)
			  {
				  hasError = true;
				  switch (err)
				  { 
						case ErrorCode.InvalidEnum:
							result += "Invalid Enum\n";
							break;
						case ErrorCode.InvalidFramebufferOperation:
							result += "Invalid Framebufferopetarion\n";
							break;
					   case ErrorCode.InvalidOperation:
							result += "Invalidoperataion\n";
							break;
						case ErrorCode.InvalidValue:
							result += "Invalidvalue\n";
							break;
						case ErrorCode.OutOfMemory:
							result += "Outofmemory\n";
							break;
						case ErrorCode.StackOverflow:
							result += "StackOverFLow\n";
							break;
						case ErrorCode.StackUnderflow:
							result += "Stackunderflow\n";
							break;
						case ErrorCode.TableTooLarge:
							result += "TableTooLarge\n";
							break;

				  }
				  if(hasError)
						throw new GraphicsContextException(result);
			  }
        }
	}
}
