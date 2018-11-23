﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Data
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
