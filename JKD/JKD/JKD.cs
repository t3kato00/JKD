
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
				game.Config();
				game.Run(60.0);
			}
		}
	}
}
