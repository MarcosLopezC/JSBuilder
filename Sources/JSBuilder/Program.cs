using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace JSBuilder
{
	class Program
	{
		private const int InvalidArgumentsCode = 1;
		private const int BuiltWithErrors      = 2;

		static void Main(string[] args)
		{
			if (args.Length >= 2)
			{
				ProcessFile(args[0], args[1]);
			}
			else
			{
				PrintHelp();
			}
		}

		static void ProcessFile(string inputPath, string outputPath)
		{
			if (!File.Exists(inputPath))
			{
				Console.WriteLine("{0} does not exists.", inputPath);
				Environment.Exit(InvalidArgumentsCode);
			}

			var stopwatch = new Stopwatch();

			stopwatch.Start();
			var result = Builder.BuildFile(inputPath);
			stopwatch.Stop();

			foreach (var error in result.Errors)
			{
				Console.WriteLine("Error: {0}", error.Description);
				Console.WriteLine("File: {0}  Line: {1}", error.File, error.Line);
				Environment.ExitCode = BuiltWithErrors;
			}

			try
			{
				File.WriteAllText(outputPath, result.Output, Encoding.UTF8);

				Console.WriteLine("Build completed successfully. ({0}ms)", stopwatch.ElapsedMilliseconds);
			}
			catch (PathTooLongException)
			{
				PrintArgumentError("The output path is too long.");
			}
			catch (DirectoryNotFoundException)
			{
				PrintArgumentError("The output path is invalid.");
			}
			catch (UnauthorizedAccessException)
			{
				PrintArgumentError("You don't have permission to write to the output path.");
			}
			catch (Exception)
			{
				PrintArgumentError("An error occurred while writting to the output path.");
			}
		}

		static void PrintArgumentError(string message)
		{
			Console.WriteLine("Error: {0}", message);
			Environment.Exit(InvalidArgumentsCode);
		}

		static void PrintHelp()
		{
			Console.WriteLine("JSBuilder by Marcos López C.");
			Console.WriteLine("  Syntax: [inputFile] [outputFile]");
			Console.WriteLine(" Example: jsbulder main.js build.js");
		}
	}
}
