using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace JSBuilder
{
	public static class Builder
	{
		private class Context
		{
			public IList<Error> Errors { get; private set; }
			public IList<string> ProcessedLines { get; private set; }
			public IList<string> IncludedFiles { get; private set; }

			public Context()
			{
				Errors         = new List<Error>(100);
				ProcessedLines = new List<string>(5000);
				IncludedFiles  = new List<string>(100);
			}
		}

		private static readonly Regex requiresPattern =
			new Regex(@"\s*\/\/\s*Requires:\s*(.*)", RegexOptions.Singleline);

		public static Result BuildFile(string path)
		{
			var context = new Context();

			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(path);
			}

			BuildFile(path, context);

			var output = string.Join("\n", context.ProcessedLines);

			return new Result(output, context.Errors);
		}

		private static void BuildFile(string path, Context context)
		{
			var file = File.ReadAllLines(path);

			for (var line = 0; line < file.Length; line += 1)
			{
				var currentLine = file[line];

				var match = requiresPattern.Match(currentLine);

				if (match.Success)
				{
					var requiredFile = match.Groups[1].Value.Trim();

					if (!Path.IsPathRooted(requiredFile))
					{
						var currentDirectory = Path.GetDirectoryName(path);
						requiredFile = Path.Combine(currentDirectory, requiredFile);
					}

					if (context.IncludedFiles.Contains(requiredFile))
					{
						continue;
					}

					if (File.Exists(requiredFile))
					{
						context.IncludedFiles.Add(requiredFile);
						BuildFile(requiredFile, context);
					}
					else
					{
						context.Errors.Add(new Error(
							description: string.Format(
								"The required file ({0}) does not exists.", requiredFile
							),
							file: path,
							line: line + 1,
							isWarning: false
						));
					}
				}
				else
				{
					context.ProcessedLines.Add(currentLine);
				}
			}
		}
	}
}
