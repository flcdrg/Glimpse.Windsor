extern alias CastleWindsor;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using CastleWindsor::Castle.Core;
using CastleWindsor::Castle.MicroKernel;
using CastleWindsor::Castle.Windsor;
using CastleWindsor::Castle.Windsor.Diagnostics;
using CastleWindsor::Castle.Windsor.Diagnostics.Extensions;

using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Windsor
{
    public class WindsorGlimpsePlugin : TabBase, ILayoutControl
    {
        internal static IWindsorContainer Container;

        public override object GetData(ITabContext context)
        {
            if (Container == null)
                return null;

            var proxy = new KernelDebuggerProxy(Container);
           
/*
            var data = new List<object[]> { new object[] { proxy.Extensions.Select(ex => ex.Name) } };
            data.AddRange(proxy.Extensions.Select(item => new[] {item.Value}));
*/

/*
            var model = new WindsorModel();

            model.MisConfiguredModel =
                proxy.Extensions.Select(x => x.Value).OfType<PotentiallyMisconfiguredComponents>().FirstOrDefault();

            return model;
*/

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

        public bool KeysHeadings
        {
            get
            {
                return true;
            }
        }
    }

    public class WindsorModel
    {
        public PotentiallyMisconfiguredComponents MisConfiguredModel { get; set; }

    }

    public class DynamicModel : DynamicObject
    {
        private readonly IDictionary<string, object> _dictionary;

        public object Tada { get; set; }

        public DynamicModel()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param><param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, the <paramref name="value"/> is "Test".</param>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;

            return true;
        }
    }

    public class WindsorInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            MessageBroker = context.MessageBroker;
            var kernel = WindsorGlimpsePlugin.Container.Kernel;
            kernel.ComponentCreated += KernelOnComponentCreated;
            kernel.ComponentRegistered += KernelOnComponentRegistered;
            kernel.ComponentDestroyed += KernelOnComponentDestroyed;
        }

        private void KernelOnComponentDestroyed(ComponentModel model, object instance)
        {
            MessageBroker.Publish(new KernelMessage() { EventName = "Destroyed", EventSubText = model.Name, StartTime = DateTime.Now });
        }

        private void KernelOnComponentRegistered(string key, IHandler handler)
        {
            MessageBroker.Publish(new KernelMessage() { EventName = "Registered", EventSubText = key, StartTime = DateTime.Now });
        }

        public IMessageBroker MessageBroker { get; set; }

        private void KernelOnComponentCreated(ComponentModel model, object instance)
        {
            MessageBroker.Publish(new KernelMessage() { EventName = "Created", EventSubText = model.Name, StartTime = DateTime.Now });
        }
    }

    public class KernelMessage : MessageBase, ITimelineMessage
    {
        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public TimelineCategoryItem EventCategory { get; set; }
        public string EventSubText { get; set; }

        public KernelMessage()
        {
            EventCategory = new TimelineCategoryItem("Kernel", "Black", "Black");
            Duration = TimeSpan.Zero;
            EventSubText = "asdfasdf";

        }
    }
}