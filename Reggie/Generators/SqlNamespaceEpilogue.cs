using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlNamespaceEpilogue(TextWriter Response, IDictionary<string, object> Arguments) {
            Response.Flush();
        }
    }
}
