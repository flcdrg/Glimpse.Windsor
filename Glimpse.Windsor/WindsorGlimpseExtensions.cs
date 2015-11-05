extern alias CastleWindsor;
using System;

using CastleWindsor::Castle.MicroKernel;
using CastleWindsor::Castle.Windsor;

namespace Glimpse.Windsor
{
    public static class WindsorGlimpseExtensions
    {
        public static void ActivateGlimpse(this IWindsorContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            WindsorGlimpsePlugin.Container = container;

            container.Kernel.ComponentRegistered += KernelOnComponentRegistered;


        }

        private static void KernelOnComponentRegistered(string key, IHandler handler)
        {
            
        }
    }
}