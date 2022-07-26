using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
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
    public class GetAddressBookByIdQuery : IRequest<Result>
    {
        public int AddressBookId { get; set; }
    }


    public class GetAddressBookByIdQueryHandler : IRequestHandler<GetAddressBookByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAddressBookByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAddressBookByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.AddressBooks.Where(a => a.Id == request.AddressBookId)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(AddressBook), request.AddressBookId);
                }
                var result = _mapper.Map<AddressBookDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving address book by id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
