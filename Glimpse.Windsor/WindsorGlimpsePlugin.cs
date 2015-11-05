extern alias CastleWindsor;
using System.Collections.Generic;
using System.Linq;

using CastleWindsor::Castle.Windsor;

using Glimpse.Core.Extensibility;

namespace Glimpse.Windsor
{
    public class WindsorGlimpsePlugin : TabBase
    {
        internal static IWindsorContainer Container;

        public override object GetData(ITabContext context)
        {
            if (Container == null)
                return null;

            var proxy = new KernelDebuggerProxy(Container);


            var data = new List<object[]> { new object[] { proxy.Extensions.Select(ex => ex.Name) } };
            data.AddRange(proxy.Extensions.Select(item => new[] {item.Value}));

            // http://glimpse-web-client.azurewebsites.net/test/Layout.html
            return proxy.Extensions.Select(ex => new
            {
                ex.Name,
                ex.Value,
                _metadata = ex.Name == "Potentially misconfigured components" ? (object) new { style = "warn"} : new { }
            }).ToList();
        }

        public override string Name
        {
            get
            {
                return "Windsor";
            }
        }
    }
}