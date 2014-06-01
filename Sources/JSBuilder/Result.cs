using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSBuilder
{
	public class Result
	{
		public string Output { get; private set; }
		public IEnumerable<Error> Errors { get; private set; }

		public Result(string output, IEnumerable<Error> errors)
		{
			this.Output = output;
			this.Errors = errors;
		}
	}
}
