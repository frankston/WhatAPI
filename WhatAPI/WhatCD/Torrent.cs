using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD
{
    public class Torrent
    {
        public byte[] Bytes { get; private set; }
        public string Name { get; private set; }

        public Torrent(byte[] bytes, string name)
        {
            this.Bytes = bytes;
            this.Name = name;
        }
    }
}
