﻿using System;
using System.Collections;

namespace ServiceStack.Text.TupleSerializer
{
    internal class TupleSerializerInitializer<TTuple> where TTuple 
        : IStructuralEquatable, IStructuralComparable, IComparable
    {
        public TupleSerializerInitializer()
        {
            if (!typeof(TTuple).IsTuple())
            {
                throw new ArgumentException("Type parameter must be a tuple.");
            }

            var serializationHelper = new TupleSerializer<TTuple>();

            JsConfig<TTuple>.Reset();
            JsConfig<TTuple>.SerializeFn = serializationHelper.GetStringValue;
            JsConfig<TTuple>.DeSerializeFn = serializationHelper.GetTupleFrom;
        }
    }
}
