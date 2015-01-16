
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
				game.Config( 0.0, -10.0, 10.0);
				game.Run(60.0);
			}
		}
	}
}
