using OnyxDoc.FormBuilderService.Application.Common.Behaviours;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
       // //private readonly Mock<ILogger<CreateStaffCommand>> _logger;
       // private readonly Mock<ICurrentUserService> _currentUserService;
       ////private readonly Mock<IIdentityService> _identityService;


       // public RequestLoggerTests()
       // {
       //     //_logger = new Mock<ILogger<CreateStaffCommand>>();

       //     _currentUserService = new Mock<ICurrentUserService>();

       //     //_identityService = new Mock<IIdentityService>();
       // }

       // [Test]
       // public async Task ShouldCallGetUserNameAsyncOnceIfContractenticated()
       // {
       //     _currentUserService.Setup(x => x.UserId).Returns("Administrator");

       //    // var requestLogger = new LoggingBehaviour<CreateStaffCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

       //     await requestLogger.Process(new CreateStaffCommand {  UserId = "1", FirstName = "Ahmed" }, new CancellationToken());

       //     _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
       // }

       // [Test]
       // public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
       // {
       //     var requestLogger = new LoggingBehaviour<CreateStaffCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

       //     await requestLogger.Process(new CreateStaffCommand { UserId = "1", FirstName = "Ahmed" }, new CancellationToken());

       //     _identityService.Verify(i => i.GetUserNameAsync(null), Times.Never);
       // }
    }
}
