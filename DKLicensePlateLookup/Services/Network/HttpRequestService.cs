using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DKLicensePlateLookup.Services.Data;

namespace DKLicensePlateLookup.Services.Network
{
    public class HttpRequestService
    {
        private readonly CookieContainer _cookies;
        private readonly HttpClientHandler _handler;
        private readonly HttpClient _client;

        public HttpRequestService()
        {
            _cookies = new CookieContainer();
            _handler = new HttpClientHandler
            {
                CookieContainer = _cookies,
                UseCookies = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = true
            };
            _client = new HttpClient(_handler);
        }

        public async Task GetInfo(string regNumber)
        {
            //Setting up first request
            string startUrl = "https://motorregister.skat.dk/dmr-kerne/koeretoejdetaljer/visKoeretoej?execution=e1s1";
            var startResponse = await _client.GetAsync(startUrl);
            //Console.WriteLine(startResponse.IsSuccessStatusCode);

            if (!startResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"startResponse.IsSuccessStatusCode: {startResponse.IsSuccessStatusCode}");
                return;
            }
            var startHtml = await startResponse.Content.ReadAsStringAsync();

            //Temp saving page to file
            //LocalDataStore dataHandler = new();
            //dataHandler.save(startHtml, "Test.txt");

            //Getting DmrFormToken
            var token = ExtractDmrFormTokenOrThrow_RegexPlusXElement(startHtml);
            //Console.WriteLine($"dmrFormToken = {token}");

            //Setting up second request
            var postUrl = "https://motorregister.skat.dk/dmr-kerne/koeretoejdetaljer/visKoeretoej?execution=e1s1&_eventId=search";

            var submitFieldName = "/dmr-kerne/koeretoejdetaljer/visKoeretoej?execution=e1s1&_eventId=search";


            var formData = new Dictionary<string, string>
                {
                    { "dmrFormToken", token },
                    { "soegekriterie", "REGISTRERINGSNUMMER" },
                    { "soegeord", regNumber },
                    { submitFieldName, "Søg" }
                };

            using var content = new FormUrlEncodedContent(formData);

            _client.DefaultRequestHeaders.Referrer = new Uri(startUrl);


            var postResp = await _client.PostAsync(postUrl, content);
            Console.WriteLine($"POST status: {(int)postResp.StatusCode} {postResp.ReasonPhrase}");

            var postHtml = await postResp.Content.ReadAsStringAsync();


            var nextUrl = "https://motorregister.skat.dk/dmr-kerne/koeretoejdetaljer/visKoeretoej?execution=e1s2";
            var nextResp = await _client.GetAsync(nextUrl);
            if (nextResp.IsSuccessStatusCode)
            {
                Console.WriteLine("Sucessfully requested vehicle information");
                var nextHtml = await nextResp.Content.ReadAsStringAsync();
                Console.WriteLine("Next state (e1s2):");
                //dataHandler.save(nextHtml, "Info.txt");
                //Console.WriteLine(nextHtml.Length > 1200 ? nextHtml.Substring(0, 1200) : nextHtml);
            }
        }



        private static string ExtractDmrFormTokenOrThrow_RegexPlusXElement(string html)
        {
            var tokenElement = Regex.Matches(html, @"<input[^>]+?dmrFormToken[^>]+?>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline)[0].Value;

            // Ensure self-closing before parsing as XML
            if (!tokenElement.EndsWith("/>", StringComparison.Ordinal))
                tokenElement = tokenElement.TrimEnd('>') + " />";

            var token = XElement.Parse(tokenElement).Attribute("value")?.Value;
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("dmrFormToken <input> had no value attribute.");

            return token;
        }
    }
}
