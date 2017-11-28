using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBManage.Models
{
    public class ReturnResult
    {
        public int code { get; set; }
        public object data { get; set; }
        public object msg { get; set; }
    }
}