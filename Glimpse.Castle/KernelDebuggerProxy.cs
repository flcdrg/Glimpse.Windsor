extern alias CastleWindsor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CastleWindsor::Castle.MicroKernel;
using CastleWindsor::Castle.Windsor;
using CastleWindsor::Castle.Windsor.Diagnostics;
using CastleWindsor::Castle.Windsor.Diagnostics.DebuggerViews;

namespace Glimpse.Castle
{
    internal class KernelDebuggerProxy
    {
        private readonly IEnumerable<IContainerDebuggerExtension> _extensions;

        public KernelDebuggerProxy(IWindsorContainer container)
            : this(container.Kernel)
        {
        }

        public KernelDebuggerProxy(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            _extensions =
                (IEnumerable<IContainerDebuggerExtension>)
                    (kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey) as IContainerDebuggerExtensionHost) ??
                new IContainerDebuggerExtension[0];
        }

        [DebuggerDisplay("")]
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public DebuggerViewItem[] Extensions
        {
            get { return _extensions.SelectMany(e => e.Attach()).ToArray(); }
        }
    }
}