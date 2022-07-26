using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Application.Common;
using Upskillz_invoice_mgt_Application.Common.Models;
using Upskillz_invoice_mgt_Domain.Enums;

namespace Upskillz_invoice_mgt_Application.Invoices.Commands.SaveInvoice
{
    public class SaveInvoiceCommand : AuthToken, IRequest<Response<InvoiceDto>>
    {
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        //public AppUser AppUser { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public ICollection<InvoiceItemDto> InvoiceItems { get; set; }
    }

    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, Response<InvoiceDto>>
    {
        private readonly IMapper _mapper;

        public SaveInvoiceCommandHandler(IMapper mapper)
        {

        }
        public Task<Response<InvoiceDto>> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
