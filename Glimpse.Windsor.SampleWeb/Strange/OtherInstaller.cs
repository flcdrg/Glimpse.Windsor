using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Glimpse.Windsor.SampleWeb.Strange
{
    /// <summary>
    /// Add mis-configured things to the container, so we cause extra diagnostics to be displayed
    /// </summary>
    public class OtherInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IThing>().ImplementedBy<Thing>());
        }
    }
}