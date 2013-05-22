using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionForum.TypeMain
{
    public class Category
    {
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public List<Forum> forums { get; set; }

        public override string ToString()
        {
            return this.categoryName;
        }
    }
}
