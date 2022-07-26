using OnyxDoc.SubscriptionService.Application.Common.Interfaces; 
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            string userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                //var result  = await _identityService.GetUserNameAsync(userId);
                //userName = result.userName;
            }

            _logger.LogInformation("OnyxDoc.SubscriptionService Request: {Name} {@UserId} {@UserName} {@Request}",

                requestName, userId, userName, request);
        }
    }
}