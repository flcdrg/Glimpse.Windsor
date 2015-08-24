extern alias CastleWindsor;
using System;

using CastleWindsor::Castle.Windsor;

namespace Glimpse.Windsor
{
    public static class CastleGlimpseExtensions
    {
        public static void ActivateGlimpse(this IWindsorContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            CastleGlimpsePlugin.Container = container;
        }
    }
}