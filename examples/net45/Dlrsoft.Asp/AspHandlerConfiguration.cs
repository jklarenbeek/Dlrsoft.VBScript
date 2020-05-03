﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Dlrsoft.Asp
{
    public class AspHandlerConfiguration
    {
        static AspHandlerConfiguration()
        {
            Assemblies.Add(typeof(global::Dlrsoft.VBScript.Runtime.BuiltInFunctions).Assembly);

            Trace = true;
        }

        private static IList<Assembly> _assemblies;

        public static IList<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new List<Assembly>();
                }
                return _assemblies;
            }
        }

        public static bool Trace { get; set; }
    }
}
