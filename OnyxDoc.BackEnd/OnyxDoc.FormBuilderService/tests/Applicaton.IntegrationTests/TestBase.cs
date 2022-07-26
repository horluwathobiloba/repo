using NUnit.Framework;
using System.Threading.Tasks;

<<<<<<< HEAD:OnyxDoc.FormBuilderService/tests/Applicaton.IntegrationTests/TestBase.cs
namespace OnyxDoc.FormBuilderService.Application.IntegrationTests
=======
namespace OnyxDoc.DocumentService.Application.IntegrationTests
>>>>>>> bc538261377da49ebc942042b220626ca139f0fd:OnyxDoc.DocumentService/tests/Applicaton.IntegrationTests/TestBase.cs
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
