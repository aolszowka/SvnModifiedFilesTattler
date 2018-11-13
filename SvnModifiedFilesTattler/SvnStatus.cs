// -----------------------------------------------------------------------
// <copyright file="SvnStatus.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SvnModifiedFilesTattler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal class SvnStatus
    {
        public string Path { get; private set; }

        public string Status { get; private set; }

        public SvnStatus(string path, string status)
        {
            Path = path;
            Status = status;
        }

        public static IEnumerable<SvnStatus> ParseStatusXmlFragment(string svnStatusXmlFragment)
        {
            XDocument statusXml = XDocument.Parse(svnStatusXmlFragment);

            IEnumerable<XElement> entryElements = statusXml.Descendants("entry");

            foreach (XElement entryElement in entryElements)
            {
                string path = entryElement.Attribute("path").Value;
                string status = entryElement.Descendants("wc-status").First().Attribute("item").Value;

                yield return new SvnStatus(path, status);
            }
        }
    }
}
