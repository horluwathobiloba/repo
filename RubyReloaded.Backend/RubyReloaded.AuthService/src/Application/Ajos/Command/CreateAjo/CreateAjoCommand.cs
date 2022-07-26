using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Ajos.Command
{
    public class CreateAjoCommand : IRequest<Result>
    {
        public CollectionCycle CollectionCycle { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal AmountPerUser { get; set; }
        public decimal AmountToDisbursePerUser { get; set; }
        public string Name { get;  set; }
        public string Code { get;  set; }
        public string LoggedInUserId { get; set; }
    }
    public class CreateAjoCommandHandler : IRequestHandler<CreateAjoCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public CreateAjoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateAjoCommand request, CancellationToken cancellationToken)
        {
            //check if Ajo name exist
            try
            {
                var exists = await _context.Ajos.AnyAsync(x => x.Name == request.Name);
                if (exists)
                {
                    return Result.Failure("Name Already Exist");
                }
                var ajo=new Ajo
                {
                    StartDate = request.StartDate,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    Name = request.Name,
                    EndDate = request.EndDate,
                    CreatedDate = DateTime.Now,
                    AmountToDisbursePerUser = request.AmountToDisbursePerUser,
                    AmountPerUser = request.AmountPerUser,
                    CollectionAmount = request.CollectionAmount,
                    CollectionCycle = request.CollectionCycle,
                    NumberOfUsers = request.NumberOfUsers,
                   
                };
                await _context.Ajos.AddAsync(ajo);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(ajo);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Ajo creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
