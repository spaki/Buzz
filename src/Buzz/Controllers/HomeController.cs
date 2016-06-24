using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Buzz.Helper;
using Microsoft.Extensions.Configuration;
using Buzz.Models;

namespace Buzz.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index(TwittResult model)
        {
            if (string.IsNullOrWhiteSpace(model.Query))
                return View(model);

            var twitter = new TwitterHelper
            {
                Key = configuration.GetValue<string>("AppSettings:TwitterKey"),
                Secret = configuration.GetValue<string>("AppSettings:TwitterSecret"),
                MaxResults = 30
            };

            model.Tweets = await twitter.GetTweetsByHashtags(model.Query);
            model.Query = twitter.Query;

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
