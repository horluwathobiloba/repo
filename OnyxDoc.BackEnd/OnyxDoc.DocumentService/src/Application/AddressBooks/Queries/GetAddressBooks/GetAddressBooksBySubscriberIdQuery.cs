using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks;
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
    public class GetAddressBooksBySubscriberIdQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
    }


    public class GetAddressBooksBySubscriberIdQueryHandler : IRequestHandler<GetAddressBooksBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAddressBooksBySubscriberIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetAddressBooksBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.AddressBooks.Where(a => a.SubscriberId == request.SubscriberId)
                    .ToListAsync();
                if (entity == null)
                {
                    throw new NotFoundException(nameof(AddressBook), request.SubscriberId);
                }
                var result = _mapper.Map<List<AddressBookDto>>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error retrieving address book by subscriber id {ex?.Message ?? ex?.InnerException?.Message}");
            }

        }
    }
}
