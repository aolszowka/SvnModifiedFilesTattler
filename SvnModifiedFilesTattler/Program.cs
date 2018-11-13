// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SvnModifiedFilesTattler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Utility program to list Subversion Working Copy Modifications
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int errorCode = 0;

            if (args.Any())
            {
                string command = args.First().ToLowerInvariant();

                if (command.Equals("-?") || command.Equals("/?") || command.Equals("-help") || command.Equals("/help"))
                {
                    errorCode = ShowUsage();
                }
                else
                {
                    if (Directory.Exists(command))
                    {
                        string targetPath = command;
                        errorCode = PrintToConsole(targetPath);
                    }
                    else
                    {
                        string error = string.Format("The specified path `{0}` is not valid.", command);
                        Console.WriteLine(error);
                        errorCode = 1;
                    }
                }
            }
            else
            {
                // This was a bad command
                errorCode = ShowUsage();
            }

            Environment.Exit(errorCode);
        }

        private static int ShowUsage()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Given Folder to an SVN Working Copy; Return a listing of modified files.");
            message.AppendLine("Invalid Command/Arguments. Valid commands are:");
            message.AppendLine();
            message.AppendLine("[directory] - [READS] MUST BE AN SVN WORKING COPY. Performs an\n" +
                               "              svn status and then prints the files that were\n" +
                               "              marked as modified.\n" +
                               "              Returns the number of modified files.");
            Console.WriteLine(message);
            return 21;
        }

        static int PrintToConsole(string targetDirectory)
        {
            int modifiedFileCount = 0;

            IEnumerable<string> modifiedFiles = TattleModifiedFiles.Execute(targetDirectory);

            foreach (string modifiedFile in modifiedFiles)
            {
                modifiedFileCount++;
                Console.WriteLine(modifiedFile);
            }

            return modifiedFileCount;
        }
    }
}
