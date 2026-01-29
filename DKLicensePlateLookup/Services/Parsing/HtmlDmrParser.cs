using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;

namespace DKLicensePlateLookup.Services.Parsing
{
    public class HtmlDmrParser
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

        
        // Extracts a specific field from a key-value section identified by a heading.
        // Useful for sections with multiple key-value pairs (insurance details as an example).
        
        public string GetFieldFromSection(string html, string sectionHeading, string fieldKey)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Find the section heading
            var headings = doc.DocumentNode.SelectNodes("//h1|//h2|//h3|//h4|//h5|//h6");
            if (headings == null || headings.Count == 0) return null;

            var heading = headings.FirstOrDefault(
                h => Normalize(h.InnerText)
                    .Equals(sectionHeading, StringComparison.OrdinalIgnoreCase));

            if (heading == null) return null;

            // Use XPath to find all key spans that follow this heading
            var keySpans = heading.SelectNodes("following-sibling::*//span[@class='key']");
            if (keySpans == null) return null;

            foreach (var keySpan in keySpans)
            {
                var normalizedKey = Normalize(keySpan.InnerText).TrimEnd(':');
                if (normalizedKey.Equals(fieldKey, StringComparison.OrdinalIgnoreCase))
                {
                    // Find the corresponding value span (next sibling of the key span)
                    var valueSpan = keySpan.SelectSingleNode("following-sibling::span[@class='value']");
                    if (valueSpan != null)
                    {
                        // Prefer explicit inner span if present
                        var innerSpan = valueSpan.SelectSingleNode(".//span");
                        var text = innerSpan != null ? innerSpan.InnerText : valueSpan.InnerText;
                        return Normalize(text);
                    }
                }
            }

            return null;
        }

        // Convenience method to get the insurance company name.

        public string GetInsuranceCompany(string html)
        {
            return GetFieldFromSection(html, "Forsikring", "Selskab");
        }

        // Trim and collapse all whitespace (spaces, tabs, newlines) to single spaces.

        private string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            // Replace any run of whitespace with a single space, then trim.
            return Regex.Replace(s, @"\s+", " ").Trim();
        }

        public string[] SplitMakeAndModel(string s)
        {
            string[] MakeAndModel = new string[2];
            string[] SplitString = s.Split(" ");

            MakeAndModel[0] = SplitString[0];
            MakeAndModel[1] = String.Join(" ", SplitString.Skip(1));
            return MakeAndModel;
        }

    }
}
