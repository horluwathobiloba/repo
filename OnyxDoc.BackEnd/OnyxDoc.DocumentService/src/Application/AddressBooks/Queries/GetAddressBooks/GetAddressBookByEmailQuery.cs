using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Domain.Entities;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks
{
    public class GetAddressBookByEmailQuery : IRequest<Result>
    {
        public string Email { get; set; }
    }


    public class GetAddressBookByEmailQueryHandler : IRequestHandler<GetAddressBookByEmailQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAddressBookByEmailQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAddressBookByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.AddressBooks.Where(a =>  a.EmailAddress == request.Email)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(AddressBook), request.Email);
                }
                var result = _mapper.Map<AddressBookDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving address book by email {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
