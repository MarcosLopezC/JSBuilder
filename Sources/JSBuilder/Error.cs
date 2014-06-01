using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSBuilder
{
	public class Error
	{
		public string Description { get; private set; }
		public string File { get; private set; }
		public int? Line { get; private set; }
		public bool IsWarning { get; private set; }

		public Error(
			string description,
			string file        = null,
			int?   line        = null,
			bool   isWarning   = true)
		{
			this.Description = description;
			this.File        = file;
			this.Line        = line;
			this.IsWarning   = isWarning;
		}
	}
}
