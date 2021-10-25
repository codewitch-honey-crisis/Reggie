using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImgScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = args[0];
            var wr = WebRequest.Create(url);
            using(var wrs = wr.GetResponse())
            {
                var taskList = new List<Task>();
                foreach(var match in ImgScrape.MatchImgUrl(new StreamReader(wrs.GetResponseStream())))
                {
                    var s = match.Value.Substring(1, match.Value.Length - 2);
                    if (s[0] == '/') s = url + s;
                    var wr2= WebRequest.Create(s);
                    using(var wrs2= wr2.GetResponse())
                    {
                        var fn = s.Substring(s.LastIndexOf('/') + 1);
                        Console.WriteLine(s);
                        using (var f = File.Open(fn, FileMode.Create))
                        {
                            f.SetLength(0);
                            wrs2.GetResponseStream().CopyTo(f);
                        }
                    }
                }
            }
        }
    }
}
