using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks;
using OnyxDoc.DocumentService.Application.Common.Interfaces;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Commands.DeleteAddressBook
{
    public class DeleteAddressBookCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; } 
    }

    public class DeleteAddressBookCommandHandler : IRequestHandler<DeleteAddressBookCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteAddressBookCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteAddressBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.AddressBooks.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure("Invalid  recipient!");
                }
                 
                _context.AddressBooks.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<AddressBookDto>(entity);
                return Result.Success(" Address Book is now deleted!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"AddressBook  delete failed { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
