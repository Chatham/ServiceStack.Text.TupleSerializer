using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text.InlineTupleSerializer
{
    internal class TupleReflectionProxy<TTuple> where TTuple
        : IStructuralEquatable, IStructuralComparable, IComparable
    {
        private readonly Type _type;

        private int _count;

        public int Count
        {
            get
            {
                if (_count == 0)
                {
                    _count = _type.GetGenericArguments().Count();
                }

                return _count;
            }
        }

        private Type[] _subTypes;

        public Type[] SubTypes
        {
            get
            {
                if (_subTypes == null)
                {
                    _subTypes = _type.GetGenericArguments();
                }

                return _subTypes;
            }
        }

        private IList<MethodInfo> _methodProxies;

        public IList<MethodInfo> MethodProxies
        {
            get
            {
                if (_methodProxies == null)
                {
                    _methodProxies = _type.GetProperties()
                        .Select(pi => pi.GetGetMethod())
                        .ToList();
                }
                return _methodProxies;
            }
        }

        public TupleReflectionProxy()
        {
            _type = typeof (TTuple).FindTupleDefinition();

            if (_type == null)
            {
                throw new InvalidOperationException("Type parameter must be a tuple.");
            }
        }
    }
}