using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;
using Onyx.ContractService.Domain.Constants;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Domain.ViewModels;
using Onyx.ContractService.Infrastructure.Services;
using ReventInject;
using ReventInject.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ContractDocuments.Commands.UpdateDimensions
{
    public class UpdateDimensionsCommand : AuthToken, IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public int ContractId { get; set; }
        public string Email { get; set; }
        public Dimension[] DimensionVm { get; set; }
    }

    public class UpdateDimensionsCommandHandler : IRequestHandler<UpdateDimensionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IAuthService _authService;

        public UpdateDimensionsCommandHandler(IApplicationDbContext context, IMapper mapper, IBlobStorageService blobStorageService, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateDimensionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
               // var respo = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganizationId);

                var contract = await _context.Contracts.Where(a => a.Id == request.ContractId).FirstOrDefaultAsync();
                if (contract == null)
                {
                    return Result.Failure($"Invalid contract specified");
                }

                var existingDimension = new List<Dimension>();
                var nonExistingDimension = new List<Dimension>();
                foreach (var dimensionUpdateVm in request.DimensionVm)
                {
                    var dimension = await _context.Dimensions.Where(a => a.ContractId == request.ContractId && a.Id == dimensionUpdateVm.Id).FirstOrDefaultAsync();
                    if (dimension == null)
                    {
                        nonExistingDimension.Add(dimension);
                        continue;
                    }
                    existingDimension.Add(dimension);
                    var fileName = dimension.Name + "_" + DateTime.Now.Ticks + ".png";

                    if (dimensionUpdateVm.Image.Contains("data:"))
                    {
                            dimensionUpdateVm.Image = dimensionUpdateVm.Image.Split(',')[1];
                            var fileBytes = Convert.FromBase64String(dimensionUpdateVm.Image);
                            dimension.Image = await _blobStorageService.UploadFileToBlobAsync("Image_"+DateTime.Now.Ticks, fileBytes, MimeTypes.Png);
                    }
                    else
                    {
                        dimension.Image = dimensionUpdateVm.Image;
                    }
                    dimension.LastModifiedBy = request.Email;
                    dimension.LastModifiedDate = DateTime.Now;
                    dimension.UserId = request.Email;
                   
                    _context.Dimensions.Update(dimension);
                }

                await _context.SaveChangesAsync(cancellationToken);

             
                return Result.Success("Dimensions was updated successfully", new {correctDimension = existingDimension, incorrectDimension = nonExistingDimension});
            }
            catch (Exception ex)
            {
                return Result.Failure($"Dimension update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }

}
