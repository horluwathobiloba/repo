//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using RubyReloaded.AuthService.Application.Common.Interfaces;
//using RubyReloaded.AuthService.Application.Common.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace RubyReloaded.AuthService.Application.Subscriptions.Commands.UpdateSubscription
//{
//    public class UpdateSubscriptionCommand:IRequest<Result>
//    {
//        public int Id { get; set; }
//        public string Email { get; set; }
//        public string PhoneNumber { get; set; }
//        public string CompanyName { get; set; }
//        public string CompanyEmail { get; set; }
//        public string SuscriberCode { get; set; }
//        public string Location { get; set; }
//        public string Name { get; set; }
//    }

//    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, Result>
//    {
//        private readonly IApplicationDbContext _context;
//        public UpdateSubscriptionCommandHandler(IApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<Result> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var entity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.Id == request.Id);
//                entity.CompanyEmail = request.CompanyEmail;
//                entity.CooperativeGoal = request.CooperativeGoal;
//                entity.CooperativeType = request.CooperativeType;
//                entity.CreatedDate = DateTime.Now;
//                entity.TermsAndConditions = request.TermsAndConditions;
//                entity.StatusDesc = Status.Active.ToString();
//                entity.Status = Status.Active;
//                entity.Email = request.Email;
//                entity.License = request.License;
//                entity.Location = request.Location;
//                entity.PhoneNumber = request.PhoneNumber;
//                entity.Name = request.Name;


//                _context.Cooperatives.Update(entity);
//                await _context.SaveChangesAsync(cancellationToken);
//                return Result.Success(entity);
//            }
//            catch (Exception ex)
//            {
//                return Result.Failure(new string[] { "Cooperative update was not successful", ex?.Message ?? ex?.InnerException.Message });
//            }

//        }
//    }
//}
