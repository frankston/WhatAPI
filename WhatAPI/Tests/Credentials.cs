using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatCD;

namespace Tests
{
    [TestClass]
    internal class Credentials
    {

        internal static Api Api;
        internal static WhatCD.Random WhatRandom;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Api = new Api("YourUsername", "YourPassword", true);
            WhatRandom = new WhatCD.Random(Api);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Api.Dispose();
        }

    }
}