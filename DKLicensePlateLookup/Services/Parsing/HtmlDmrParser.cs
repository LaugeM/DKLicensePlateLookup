using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DKLicensePlateLookup.Services.Parsing
{
    class HtmlDmrParser
    {
        public string GetField(string html, string headingLabel)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Find any heading element (h1–h6) whose normalized text equals the label
            var headings = doc.DocumentNode.SelectNodes("//h1|//h2|//h3|//h4|//h5|//h6");
            if (headings == null || headings.Count == 0) return null;

            var heading = headings.FirstOrDefault(
                h => Normalize(h.InnerText)
                    .Equals(Normalize(headingLabel), StringComparison.OrdinalIgnoreCase));

            if (heading == null) return null;

            // Prefer the first element sibling after the heading (skip text/whitespace nodes)
            var nextElement = heading.XPath != null
                ? heading.SelectSingleNode("following-sibling::*[1]")
                : null;

            // Fallback: walk siblings until we hit a node with non-empty text
            var next = nextElement ?? heading.NextSibling;
            while (next != null && string.IsNullOrWhiteSpace(Normalize(next.InnerText)))
                next = next.NextSibling;

            return next == null ? null : Normalize(next.InnerText);
        }

        /// <summary>
        /// Trim and collapse all whitespace (spaces, tabs, new    /// Trim and collapse all whitespace (spaces, tabs, newlines) to single spaces.
        /// </summary>
        private string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            // Replace any run of whitespace with a single space, then trim.
            return Regex.Replace(s, @"\s+", " ").Trim();
        }
    }
}
