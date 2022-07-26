using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoMembers.Queries.GetAjoMembers
{
    public class GetAjoMemberById:IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class GetAjoMemberByIdHandler : IRequestHandler<GetAjoMemberById, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAjoMemberByIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetAjoMemberById request, CancellationToken cancellationToken)
        {
            try
            {
                var ajoMember = await _context.AjoMembers.FirstOrDefaultAsync(x => x.Id == request.Id);
                return Result.Success(ajoMember);
            }
            catch (Exception ex)
            {
                return Result.Failure("Ajo Member Retrieval was not successful:"+ex?.Message?? ex?.InnerException.Message);
            }
        }
    }

}
