using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace JKD
{
	class ShaderManager
	{
		private static Dictionary<string, Shader> shaders = new Dictionary<string, Shader> { };
		
		public static Shader Require(ShaderType st, string res)
		{
			Shader result;

			if (shaders.TryGetValue(res, out result))
			{
				result.Require();
				return result;
			}

			return new Shader(st, res);
		}

		public static void Release(Shader sShader)
		{
			if (sShader.Release())
				sShader.Dispose();

		}

	}
}
