﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance_Website.Models
{
    public class Response
    {
        public object Object { get; set; }
        public bool Success { get; set; }
        public bool LogOut { get; set; }
        public string Message { get; set; }
    }
}