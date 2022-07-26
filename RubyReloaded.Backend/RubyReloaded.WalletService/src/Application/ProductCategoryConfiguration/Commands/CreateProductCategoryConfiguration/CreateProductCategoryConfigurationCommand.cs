using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enitities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.CreateProductCategoryConfiguration
{
    public class CreateProductCategoryConfigurationCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public Domain.Enums.ProductCategory ProductCategory { get; set; }
        public ICollection<int> ServiceConfigurationId { get; set; }
    }

    public class CreateProductCategoryConfigurationCommandHandler : IRequestHandler<CreateProductCategoryConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductCategoryConfigurationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateProductCategoryConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var productConfigExists = await _context.ProductCategoryConfigurations.Where(x => x.ProductCategory == request.ProductCategory &&
                //x.ProductCategoryAndServicesMap.Contains(request.ServiceConfigurationId.));
                //if(productConfigExists != null)
                //{
                //    return Result.Failure(new string[] { "Product Category Configuration already exists" });
                //}

                var productCategoryAndServicesMapping = new List<ProductCategoryAndServicesMap>();
                foreach (var id in request.ServiceConfigurationId)
                {
                    var productCategoryAndServicesMap = new ProductCategoryAndServicesMap()
                    {
                        ServiceConfigurationId = id
                    };
                    productCategoryAndServicesMapping.Add(productCategoryAndServicesMap);
                }

                await _context.BeginTransactionAsync();

                var productCategoryConfiguration = new Domain.Enitities.ProductCategory
                {
                    ProductCategoryProperty = request.ProductCategory,
                    ProductCategoryDesc = request.ProductCategory.ToString(),
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                    ProductCategoryAndServicesMap = productCategoryAndServicesMapping,
                };

                var result = await _context.ProductCategoryConfigurations.AddAsync(productCategoryConfiguration);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(productCategoryConfiguration);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Product Category Configuration creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
