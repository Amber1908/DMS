using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class IgnoreResponseContentAttribute : Attribute
    {
    }
}