using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MySpot.Unit.Tests.Framework
{
    public class ServiceCollectionTests
    {
        [Fact]
        public void Test()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IMessenger, Messenger1>();
            serviceCollection.AddScoped<IMessenger, Messenger2>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //using(var scope = serviceProvider.CreateScope())
            //{
            //var messenger = serviceProvider.GetRequiredService<IMessenger>();
            //messenger.Send();

            //var messenger2 = serviceProvider.GetRequiredService<IMessenger>();
            //messenger2.Send();
            //messenger.ShouldBe(messenger2);
            //}
             
            var messengers = serviceProvider.GetServices<IMessenger>();
            messengers.ShouldNotBeNull();
        }

        private interface IMessenger
        {
            void Send();
        }

        private class Messenger1 : IMessenger
        {
            private readonly Guid _id = Guid.NewGuid();
            public void Send()
            {
                Console.WriteLine($"sending a meessage [{_id}]");
            }
        }

        private class Messenger2 : IMessenger
        {
            private readonly Guid _id = Guid.NewGuid();
            public void Send()
            {
                Console.WriteLine($"sending a meessage [{_id}]");
            }
        }
    }
}
