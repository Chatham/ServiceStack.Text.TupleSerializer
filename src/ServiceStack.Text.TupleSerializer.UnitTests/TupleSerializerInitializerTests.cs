﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceStack.Text.InlineTupleSerializer.UnitTests
{
    [TestClass]
    public class TupleSerializerInitializerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NonTupleObject_ThrowsException()
        {
            new TupleSerializerInitializer<FakeTuple>();
        }
    }
}