using NUnit.Framework;
using System.Threading.Tasks;

namespace Onyx.WorkFlowService.Application.IntegrationTests
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}
