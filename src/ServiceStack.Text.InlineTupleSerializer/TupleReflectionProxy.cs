using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal class TupleReflectionProxy<TTuple> where TTuple
        : IStructuralEquatable, IStructuralComparable, IComparable
    {
        private readonly Type type;

        private int count;

        public int Count
        {
            get
            {
                if (count == 0)
                {
                    count = type.GetGenericArguments().Count();
                }

                return count;
            }
        }

        private Type[] subTypes;

        public Type[] SubTypes
        {
            get
            {
                if (subTypes == null)
                {
                    subTypes = type.GetGenericArguments();
                }

                return subTypes;
            }
        }

        private IList<MethodInfo> methodProxies;

        public IList<MethodInfo> MethodProxies
        {
            get
            {
                if (methodProxies == null)
                {
                    methodProxies = Enumerable.Range(1, Count)
                        .Select(
                            i =>
                                type.GetProperty(string.Concat("Item",
                                    i.ToString(CultureInfo.InvariantCulture))))
                        .Select(pi => pi.GetGetMethod())
                        .ToList();
                }
                return methodProxies;
            }
        }

        public TupleReflectionProxy()
        {
            type = typeof (TTuple).FindTupleDefinition();

            if (type == null)
            {
                throw new InvalidOperationException("Type parameter must be a tuple.");
            }
        }
    }
}