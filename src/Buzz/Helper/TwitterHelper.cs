using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buzz.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net;

namespace Buzz.Helper
{
    public class TwitterHelper
    {
        public string Key { get; set; }
        public string Secret { get; set; }
        public int MaxResults { get; set; }
        public string Token { get; set; }
        public string Query { get; set; }

        public async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes($"{Key}:{Secret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token");
            request.Headers.Add("Authorization", $"Basic {customerInfo}");
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            dynamic item = JsonConvert.DeserializeObject<object>(json);
            Token = item["access_token"];
            return Token;
        }

        public async Task<IEnumerable<Tweet>> GetTweetsByHashtags(params string[] hashtags)
        {
            if (string.IsNullOrWhiteSpace(Token))
                Token = await GetAccessToken();

            Query = GetHashTagsQueryParams(hashtags);

            var requestUserTimeline = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.twitter.com/1.1/search/tweets.json?q={Query}&result_type=recent&count={MaxResults}&exclude_replies=true&include_rts=false"
            );

            requestUserTimeline.Headers.Add("Authorization", $"Bearer {Token}");

            var httpClient = new HttpClient();
            var responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline);

            dynamic json = JsonConvert.DeserializeObject<dynamic>(await responseUserTimeLine.Content.ReadAsStringAsync());
            var enumerableTweets = (IEnumerable<dynamic>)json["statuses"];

            if (enumerableTweets == null)
                return new List<Tweet>();

            var result = enumerableTweets.Select(t => new Tweet
            {
                Text = t["text"],
                Date = t["created_at"],
                User = t["user"]["screen_name"],
                ProfileImgUrl = t["user"]["profile_image_url"],
                ImgUrl = GetImgUrl(t)
            });

            return result;
        }

        private string GetHashTagsQueryParams(params string[] hashtags)
        {
            if (hashtags == null || hashtags.Count() < 1)
                return string.Empty;

            var result = string.Join(
                string.Empty,
                string.Join(" ", hashtags)
                    .Split(' ')
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(e => WebUtility.UrlEncode(string.Concat("#", e.Replace("#", string.Empty))))
                    .ToArray()
                );

            return result;
        }

        private dynamic GetImgUrl(dynamic t)
        {
            try
            {
                return t["entities"]["media"][0]["media_url"];
            }
            catch
            {
                return null;
            }
        }
    }
}
