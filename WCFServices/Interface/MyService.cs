﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServices.Interface
{
    public class MyService : IMyService
    {
            public string GetMessage(string name)
            {
                return $"Hello, {name}!";
            }
    }
}
