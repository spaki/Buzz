using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buzz.Models
{
    public class Tweet
    {
        public string User { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public string ImgUrl { get; set; }
        public string ProfileImgUrl { get; set; }
    }
}
