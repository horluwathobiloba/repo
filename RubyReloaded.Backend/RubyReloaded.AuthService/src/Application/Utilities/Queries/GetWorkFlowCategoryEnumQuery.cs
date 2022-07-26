using MediatR;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Utilities.Queries
{
    public class GetWorkFlowCategoryEnumQuery:IRequest<Result>
    {

    }
    public class GetWorkFlowCategoryEnumQueryHandler : IRequestHandler<GetWorkFlowCategoryEnumQuery, Result>
    {
        public GetWorkFlowCategoryEnumQueryHandler()
        {

        }
        public async Task<Result> Handle(GetWorkFlowCategoryEnumQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetValues(typeof(WorkflowUserCategory));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {

                return Result.Failure("Operation was not successful :" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
