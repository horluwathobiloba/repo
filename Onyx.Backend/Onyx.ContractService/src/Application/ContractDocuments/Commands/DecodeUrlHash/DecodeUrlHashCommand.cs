using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Onyx.ContractService.Application.Contract.Commands.DecodeUrlHash
{
    public class DecodeUrlHashCommand : IRequest<Result>
    {
        public string DocumentLinkHash { get; set; }
    }

    public class DecodeUrlHashCommandHandler : IRequestHandler<DecodeUrlHashCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringHashingService _stringHashingService;

        public DecodeUrlHashCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IStringHashingService stringHashingService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _stringHashingService = stringHashingService;
        }
        public async Task<Result> Handle(DecodeUrlHashCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var documentLinkHash = HttpUtility.UrlEncode(request.DocumentLinkHash);

                //get contractDocument
                List<ContractDocument> contractDocuments = await _context.ContractDocuments.Where(x => x.Hash == request.DocumentLinkHash && x.Status == Status.Active).ToListAsync();
                if (contractDocuments == null || contractDocuments.Count == 0)
                {
                    return Result.Failure($"No Contract Document exists with this Hash");
                }
                //if (contractDocuments.Any(a=>a.IsSigned))
                //{
                //    return Result.Failure(new  { value="Accepted", message = "This document has been signed already" });
                //}
                var contractDocument = contractDocuments.FirstOrDefault();
                //get Contract Recipient Rank
                //get recipient with his signing url to prevent duplicate urls
                var lastRecipientAction = await _context.ContractRecipientActions.Where(a => a.ContractId == contractDocument.ContractId &&
                                                                          a.AppSigningUrl.Contains(request.DocumentLinkHash)).FirstOrDefaultAsync();
                if (lastRecipientAction != null)
                {
                    return Result.Failure( $"This document has been {lastRecipientAction.RecipientAction}(ed) already" );
                }

                var contractRecipient = await _context.ContractRecipients.FirstOrDefaultAsync(a=>a.ContractId == contractDocument.ContractId 
                && a.Email == contractDocument.Email && (a.RecipientCategory == RecipientCategory.InternalSignatory.ToString() ||
                a.RecipientCategory == RecipientCategory.ExternalSignatory.ToString()) );
                var rank = 0;
                if (contractRecipient != null)
                {
                    //getLogrecipientAction
                   // var getRecipientAction = await _context.ContractRecipientActions.Where(a => a.ContractRecipientId == contractRecipient.Id).FirstOrDefaultAsync();
                    //if (getRecipientAction!= null)
                    //{
                       
                   // }
                    rank = contractRecipient.Rank;
                    //get dimensions

                    var dimensions = await _context.Dimensions.Where(a => a.ContractId == contractDocument.ContractId).ToListAsync();
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("Document uploaded successfully!", new { Rank = rank, Email = contractRecipient.Email, Dimensions = dimensions });
                }

                return Result.Failure("No contract recipient exists with this hash !");


            }
            catch (Exception ex)
            {
                return Result.Failure($"Documment upload failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
