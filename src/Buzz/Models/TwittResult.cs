using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buzz.Models
{
    public class TwittResult
    {
        public TwittResult()
        {
            Tweets = new List<Tweet>();
            Query = string.Empty;
        }

        public string Query { get; set; }
        public IEnumerable<Tweet> Tweets { get; set; }

        public string QueryToShow
        {
            get
            {
                return WebUtility.UrlDecode(Query).Replace("#", " #");
            }
        }
    }
}
