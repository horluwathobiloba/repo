using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Ajos.Command.UpdateAjo
{
    public class UpdateAjoCommand:IRequest<Result>
    {
        public int Id { get; set; }
        public CollectionCycle CollectionCycle { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal AmountPerUser { get; set; }
        public decimal AmountToDisbursePerUser { get; set; }
        public string Name { get; internal set; }
        public string LoggedInUserId { get; set; }
    }
    public class UpdateAjoCommandHandler : IRequestHandler<UpdateAjoCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateAjoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateAjoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Ajos.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (entity is null)
                {
                    return Result.Failure("Name Already Exist");
                }

                entity.StartDate = request.StartDate;
                entity.Status = Status.Active;
                entity.StatusDesc = Status.Active.ToString();
                entity.Name = request.Name;
                entity.EndDate = request.EndDate;
                entity.CreatedDate = DateTime.Now;
                entity.AmountToDisbursePerUser = request.AmountToDisbursePerUser;
                entity.AmountPerUser = request.AmountPerUser;
                entity.CollectionAmount = request.CollectionAmount;
                entity.CollectionCycle = request.CollectionCycle;
                entity.NumberOfUsers = request.NumberOfUsers;
                _context.Ajos.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Ajo update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
           
        }
    }
}
