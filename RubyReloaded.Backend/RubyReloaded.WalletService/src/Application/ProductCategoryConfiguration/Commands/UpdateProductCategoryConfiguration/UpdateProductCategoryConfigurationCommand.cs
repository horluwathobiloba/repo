using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enitities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.UpdateProductCategoryConfigurations
{
    public class UpdateProductCategoryConfigurationCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Domain.Enums.ProductCategory ProductCategory { get; set; }
        public ICollection<int> ServiceConfigurationId { get; set; }
    }

    public class UpdateProductCategoryConfigurationCommandHandler : IRequestHandler<UpdateProductCategoryConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateProductCategoryConfigurationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateProductCategoryConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var entity = await _context.ProductCategoryConfigurations.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (entity != null)
                {
                    return Result.Failure(new string[] { "Product Category Configuration id not found" });
                }
                //TODO: get all service config tied to the product category and then update
                var serviceConfigs = new List<Service>();
                foreach (var id in request.ServiceConfigurationId)
                {
                    //TODO: update this service config
                    var serviceConfig = new Service();
                    serviceConfigs.Add(serviceConfig);
                }

                entity.Name = request.Name;
                entity.ProductCategoryProperty = request.ProductCategory;
                entity.ProductCategoryDesc = request.ProductCategory.ToString();
                entity.StatusDesc = Status.Active.ToString();
                entity.Status = Status.Active;
                entity.LastModifiedDate = DateTime.Now;


                _context.ProductCategoryConfigurations.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Product Category Configuration Update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
