// Based on https://github.com/castleproject/Windsor/blob/master/src/Castle.Windsor/Windsor/Diagnostics/KernelDebuggerProxy.cs
//
// Copyright 2004-2012 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

extern alias CastleWindsor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CastleWindsor::Castle.MicroKernel;
using CastleWindsor::Castle.Windsor;
using CastleWindsor::Castle.Windsor.Diagnostics;
using CastleWindsor::Castle.Windsor.Diagnostics.DebuggerViews;

namespace Glimpse.Windsor
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