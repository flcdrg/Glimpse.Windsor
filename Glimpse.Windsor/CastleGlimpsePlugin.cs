extern alias CastleWindsor;
using System.Collections.Generic;
using System.Linq;

using CastleWindsor::Castle.Windsor;

using Glimpse.Core.Extensibility;

namespace Glimpse.Windsor
{
    public class CastleGlimpsePlugin : TabBase
    {
        internal static IWindsorContainer Container;

        public override object GetData(ITabContext context)
        {
            if (Container == null)
                return null;

            var proxy = new KernelDebuggerProxy(Container);

            var data = new List<object[]> { new object[] { proxy.Extensions.Select(ex => ex.Name) } };
            data.AddRange(proxy.Extensions.Select(item => new[] {item.Value}));

            return proxy.Extensions.Select(ex => new
            {
                ex.Name,
                ex.Value
            }).ToList();
        }

        public override string Name
        {
            get
            {
                return "Castle";
            }
        }
    }
}