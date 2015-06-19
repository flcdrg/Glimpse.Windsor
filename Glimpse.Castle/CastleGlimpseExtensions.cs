extern alias CastleWindsor;
using System;
using CastleWindsor::Castle.Windsor;

namespace Glimpse.Castle
{
    public static class CastleGlimpseExtensions
    {
        /// <summary>
        /// Activates the glimpse plugin for autofac.
        /// </summary>
        /// <param name="container">The container to use.</param>
        /// <exception cref="System.ArgumentNullException">If container is <c>null</c>.</exception>
        public static void ActivateGlimpse(this IWindsorContainer container)
        {
            if (container == null) 
                throw new ArgumentNullException("container");

            CastleGlimpsePlugin.Container = container;
        }
    }
}