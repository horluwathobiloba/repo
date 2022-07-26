
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using RubyReloaded.AuthService.Application.Clients.Queries.GetClients;

namespace RubyReloaded.AuthService.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommand :  IRequest<Result>
    {
        public string Name { get; set; }

    }

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;


        public CreateClientCommandHandler(IApplicationDbContext context, IPasswordService passwordService, 
            ITokenService tokenService, IConfiguration configuration, IMapper mapper)
        { 
            _context = context;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var clientDetail = await _context.Clients.FirstOrDefaultAsync(a=>a.Name == request.Name );
                if (clientDetail != null)
                {
                    
                    if (clientDetail.Name == request.Name)
                        return Result.Failure(new string[] { "Client already exists with this name" });
                }

                var client = new Client
                {
                    CreatedDate = DateTime.Now,
                    Name = request.Name.Trim()
                };
                byte[] passwordHash = null;
                byte[] passwordSalt = null;
                var setExpiry = _configuration["SetDeveloperTokenExpiry"];
                string password = "AppLogin@onyx.com";
                _passwordService.CreatePasswordHash(password, out passwordHash, out  passwordSalt);
                client.PasswordHash = passwordHash;
                client.PasswordSalt = passwordSalt;
                client.CreatedBy = "SYSTEM";
                 _context.Clients.Add(client);
                var clientVm = _mapper.Map<ClientDto>(client);
                await _context.SaveChangesAsync(cancellationToken);
                var token = await _tokenService.GenerateDeveloperToken(client.Name, client.Id.ToString());
                
                return Result.Success( "Client was created successfully ", clientVm);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Client creation was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
