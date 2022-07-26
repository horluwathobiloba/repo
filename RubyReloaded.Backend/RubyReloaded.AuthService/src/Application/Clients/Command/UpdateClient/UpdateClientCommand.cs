
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Enums;
using AutoMapper;

namespace RubyReloaded.AuthService.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommand :  IRequest<Result>
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IApplicationDbContext context, IMapper mapper)
        { 
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var clientDetail = await _context.Clients.FindAsync(request.ClientId);
                if (clientDetail == null)
                {
                    return Result.Failure(new string[] { "Client does not exist" });
                }

                var client = new Client
                {
                    LastModifiedBy = clientDetail.Name,
                    LastModifiedDate = DateTime.Now,
                    Name = request.Name.Trim(),
                    Status = Status.Active
                };
                 _context.Clients.Update(client);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success( "Client was updated successfully ", client);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Client update was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
