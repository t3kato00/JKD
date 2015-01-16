
using System;

namespace JKD
{
	struct Binding : IDisposable
	{
		IBindable bind;

		public Binding( IBindable b )
		{
			bind = b;
			bind.Bind();
		}

		public void Dispose()
		{
			if( bind != null )
			{
				bind.UnBind();
				bind = null;
			}
		}
	}
}

