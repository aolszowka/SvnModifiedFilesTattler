// -----------------------------------------------------------------------
// <copyright file="SvnUtilities.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SvnModifiedFilesTattler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    static class SvnUtilities
    {
        static string svnPath = @"svn.exe";

        internal static void OverrideSvnPath(string newPath)
        {
            svnPath = newPath;
        }

        internal static string ExecuteSvnCommand(string svnCommand)
        {
            // This will store the svn output
            string svnOutput;

            // Its possible to get an error
            bool wasSuccessful = false;
            int maxRetries = 5;
            int retryCount = 0;
            int lastSvnExitCode = int.MaxValue;

            // Keep trying the command until we're successful or run out of retries
            do
            {
                using (Process svn = new Process())
                {
                    svn.StartInfo.FileName = svnPath;
                    svn.StartInfo.Arguments = svnCommand;
                    svn.StartInfo.UseShellExecute = false;
                    svn.StartInfo.RedirectStandardOutput = true;

                    // Start the process
                    svn.Start();

                    svnOutput = svn.StandardOutput.ReadToEnd();

                    svn.WaitForExit();

                    if (svn.ExitCode != 0)
                    {
                        lastSvnExitCode = svn.ExitCode;
                        retryCount++;

                        // Sleep for a few seconds
                        System.Threading.Thread.Sleep(retryCount * 10000);
                    }
                    else
                    {
                        wasSuccessful = true;
                    }
                }
            } while (!wasSuccessful && retryCount < maxRetries);

            if (!wasSuccessful)
            {
                string message = $"An Error occurred attempting to grab the log (after {retryCount} attempts) using `svn {svnCommand}`; Exit code was {lastSvnExitCode}";
                throw new Exception(message);
            }

            return svnOutput;
        }

        internal static IEnumerable<SvnStatus> GetWorkingCopyStatus(string targetDirectory)
        {
            string statusXmlFragment = ExecuteSvnCommand($"status \"{targetDirectory}\" --xml");

            return SvnStatus.ParseStatusXmlFragment(statusXmlFragment);
        }
    }
}
