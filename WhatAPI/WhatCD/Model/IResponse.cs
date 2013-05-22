using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model
{
    public interface IResponse<T>
    {
        string status { get; set; }
        T response { get; set; }
    }
}
