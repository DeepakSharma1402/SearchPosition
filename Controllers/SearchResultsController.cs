using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace SearchPosition.Controllers
{
    [Route("api/[controller]")]
    public class SearchResultsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("[action]")]
        public IEnumerable<int> GetPosition([FromQuery]string url, [FromQuery]string keyword)
        {
            var rng = new Random();

            string strUrl = url;
            Uri newUri = new Uri("http://" + strUrl);
            IEnumerable<int> positions = GetPosition(newUri, keyword);
            
            return positions;
        }

        public IEnumerable<int> GetPosition(Uri url, string searchTerm)
        {
            string raw = "http://www.google.com.au/search?&q={0}&btnG=Search";
            string search = string.Format(raw, HttpUtility.UrlEncode(searchTerm));


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    string html = reader.ReadToEnd();
                    return FindPosition(html, url);
                }
            }
        }
        private IEnumerable<int> FindPosition(string html, Uri url)
        {
            List<int> matchedPositions = new List<int>();
            //string lookup = @"(<a.*?>.*?</a>)";
            string lookup = "<div class=\".....\"><a href+.*?</div>";
            //string lookup = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
            MatchCollection matches = Regex.Matches(html, lookup);
            for (int i = 0; i < matches.Count; i++)
            {
                string match = matches[i].Groups[0].Value;
                if (match.Contains(url.Host))
                    matchedPositions.Add(i + 1);
            }
            return matchedPositions;
        }
    }
}