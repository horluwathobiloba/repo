using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ConractService.Application.ReminderConfigurations.Queries.GetReminderConfigurations;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ConractService.Application.ReminderConfigurations.Commands.UpdateReminderConfigurations
{
    public class UpdateReminderConfigurationsCommand : IRequest<Result>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecurrenceValue { get; set; }
        public ReminderScheduleFrequency ReminderScheduleFrequency { get; set; }
        public List<WeeklyReminderSchedule> WeeklyReminderScheduleValue { get; set; }
        public string MonthlyReminderScheduleValue { get; set; }
        public YearlyReminderSchedule YearlyReminderScheduleValue { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string UserId { get; set; }

    }

    public class UpdateReminderConfigurationsCommandHandler : IRequestHandler<UpdateReminderConfigurationsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateReminderConfigurationsCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateReminderConfigurationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                    var entity = await _context.ReminderConfigurations
                        .Where(x => x.StartDate == request.StartDate
                                      && x.EndDate == request.EndDate && x.RecurrenceValue == request.RecurrenceValue
                                      && x.ReminderScheduleFrequency == request.ReminderScheduleFrequency
                                      && x.OrganisationId == request.OrganisationId)
                        .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return Result.Failure("Reminder Configuration for update does not exist");
                }
                entity.RecurrenceValue = request.RecurrenceValue;
                entity.MonthlyReminderScheduleValue = request.MonthlyReminderScheduleValue;
                entity.YearlyReminderSchedule = request.YearlyReminderScheduleValue;
                entity.ReminderScheduleFrequency = request.ReminderScheduleFrequency;
                entity.StartDate = request.StartDate;
                entity.EndDate = request.EndDate;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                if (request.ReminderScheduleFrequency == ReminderScheduleFrequency.Weekly)
                {
                    List<WeeklyReminderSchedule> weeklySchedules = new List<WeeklyReminderSchedule>();
                    foreach (var weeklyScheduleValue in request.WeeklyReminderScheduleValue)
                    {
                        weeklySchedules.Add(new WeeklyReminderSchedule
                        {
                            Day = weeklyScheduleValue.Day,

                        });
                    }
                    entity.WeeklyReminderSchedule = weeklySchedules;
                }


                _context.ReminderConfigurations.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                var result = _mapper.Map<List<ReminderConfigurationDto>>(entity);
                return Result.Success("Reminder Configuration update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Reminder Configuration update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }


    }


}
