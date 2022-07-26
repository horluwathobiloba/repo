using NUnit.Framework;
using System.Threading.Tasks;

<<<<<<< HEAD:RubyReloaded.SubscriptionService/tests/Applicaton.IntegrationTests/TestBase.cs
namespace RubyReloaded.SubscriptionService.Application.IntegrationTests
=======
namespace RubyReloaded.DocumentService.Application.IntegrationTests
>>>>>>> bc538261377da49ebc942042b220626ca139f0fd:RubyReloaded.DocumentService/tests/Applicaton.IntegrationTests/TestBase.cs
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            //await ResetState();
        }
    }
}
