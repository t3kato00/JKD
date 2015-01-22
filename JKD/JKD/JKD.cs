
using System;
using OpenTK;

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
				game.Run(60.0);
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
				else
					msg += d.ToString();
			}

			Console.WriteLine(msg);
		}
	}
}
