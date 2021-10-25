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
            if (args.Length != 1)
                throw new ArgumentException("Expects a single argument");
            var url = args[0];
            var wr = WebRequest.Create(url);
            using(var wrs = wr.GetResponse())
            {
                var taskList = new List<Task>();
                foreach(var match in ImgScrape.MatchImgUrl(new StreamReader(wrs.GetResponseStream())))
                {
                    var s = match.Value.Substring(1, match.Value.Length - 2);
                    if (s.EndsWith(".jfif",StringComparison.InvariantCultureIgnoreCase)||
                        s.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase)||
                        s.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase)||
                        s.EndsWith(".svg", StringComparison.InvariantCultureIgnoreCase)||
                        s.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (0 > s.IndexOf("://"))
                        {
                            var ub = new UriBuilder(url);
                            if (string.IsNullOrWhiteSpace(ub.Path))
                                ub.Path = s[0] == '/' ? s : "/" + s;
                            else
                                ub.Path += (ub.Path[ub.Path.Length - 1] == '/') ? (s[0] == '/' ? s.Substring(1) : s) : (s[0] == '/' ? s : "/" + s);
                            s = ub.Uri.ToString();
                        }
                        var wr2 = WebRequest.Create(s);
                        using (var wrs2 = wr2.GetResponse())
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
}
