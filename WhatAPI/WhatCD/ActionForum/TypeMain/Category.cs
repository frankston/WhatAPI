using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeMain
{
    public class Category
    {
        public uint categoryID { get; set; }
        public string categoryName { get; set; }
        public List<Forum> forums { get; set; }

        public override string ToString()
        {
            return this.categoryName;
        }
    }
}
