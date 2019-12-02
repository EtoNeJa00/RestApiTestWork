using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models
{
    public class PreviewPostRes
    {
        public string Pic { set; get; }

        public string Url { set; get; } 

       public PreviewPostRes(string imgStr, string url)
        {
            Pic = imgStr;
            Url = url;
        }
    }
}
