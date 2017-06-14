using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.Face
{
    public class Token : BaiduFaceBaseResult
    {
        public long expires_in { get; set; }
        public string access_token { get; set; }
    }
}
