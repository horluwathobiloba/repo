
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Enums;
using AutoMapper;

namespace OnyxDoc.AuthService.Application.Clients.Commands.GenerateAPIKey
{
    public class GenerateAPIKeyCommand :  IRequest<Result>
    {
        public string Name { get; set; }
    }

    public class GenerateAPIKeyCommandHandler : IRequestHandler<GenerateAPIKeyCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public GenerateAPIKeyCommandHandler(IApplicationDbContext context, ITokenService tokenService)
        { 
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Result> Handle(GenerateAPIKeyCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var clientDetail = await _context.Clients.FirstOrDefaultAsync(a=>a.Name == request.Name);
                if (clientDetail == null)
                {
                    return Result.Failure(new string[] { "Client does not exist" });
                }

                var token = await _tokenService.GenerateDeveloperToken(clientDetail.Name, clientDetail.Id.ToString());
              
                return Result.Success( "Access Token "+token.AccessToken+"expires in "+token.ExpiresIn);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Generating API key was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
