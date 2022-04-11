using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Infrastructure.Common;

namespace WebApplication1
{
    public class NLogConfig
    {
        public static void Initialize()
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("aspnetmvc-controller", typeof(ControllerLayoutRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("aspnetmvc-action", typeof(ActionLayoutRenderer));
        }
    }
}