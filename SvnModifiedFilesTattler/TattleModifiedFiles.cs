// -----------------------------------------------------------------------
// <copyright file="TattleModifiedFiles.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SvnModifiedFilesTattler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class TattleModifiedFiles
    {
        internal static IEnumerable<string> Execute(string targetDirectory)
        {
            IEnumerable<SvnStatus> workingCopyStatus = SvnUtilities.GetWorkingCopyStatus(targetDirectory);

            IEnumerable<string> modifiedFiles =
                workingCopyStatus
                .Where(svnStatus => svnStatus.Status.Equals("modified", StringComparison.InvariantCultureIgnoreCase))
                .Select(svnStatus => svnStatus.Path);

            return modifiedFiles;
        }
    }
}
