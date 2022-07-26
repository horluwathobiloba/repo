using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.JobFunctions.Commands.CreateJobFunctions
{
    public class CreateJobFunctionsCommand:IRequest<Result>
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public List<JobFunctionRequest> JobFunctions { get; set; }
        public string UserId { get; set; }
    }

    public class CreateContractDurationsCommandHandler : IRequestHandler<CreateJobFunctionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateContractDurationsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateJobFunctionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = new List<Domain.Entities.JobFunction>();

                await _context.BeginTransactionAsync();

                foreach (var jobFunction in request.JobFunctions)
                {
                    var exists = await _context.JobFunctions.AnyAsync(x => x.OrganisationId == request.OrganisationId && x.Name == jobFunction.Name);
                    if (exists)
                    {
                        return Result.Failure($"A record with the Jobfunction already exists!");
                    }
                    var entity = new Domain.Entities.JobFunction
                    {
                        OrganisationId = request.OrganisationId,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString(),
                        Name=jobFunction.Name
                    };
                    list.Add(entity);
                }
                await _context.JobFunctions.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                return Result.Success("Jobfunctions created successfully!",list);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Jobfunctions creation failed. Error: { ex?.Message + ex?.InnerException.Message}");
            }
        }
    }
}
