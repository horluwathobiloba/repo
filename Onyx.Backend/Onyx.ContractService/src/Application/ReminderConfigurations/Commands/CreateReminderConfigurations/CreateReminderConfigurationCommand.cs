using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.ReminderConfiguration.Commands
{
    public class CreateReminderConfigurationCommand : AuthToken, IRequest<Result>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecurrenceValue { get; set; }
        public ReminderScheduleFrequency ReminderScheduleFrequency { get; set; }
        public List<WeeklyReminderScheduleVm> WeeklyReminderScheduleValue { get; set; }
        public string MonthlyReminderScheduleValue { get; set; }
        public YearlyReminderScheduleVm YearlyReminderScheduleValue { get; set; }
        public string UserId { get; set; }
        public int OrganisationId { get; set; }
        public ReminderType ReminderType { get; set; }
    }

    public class CreateReminderConfigurationCommandHandler : IRequestHandler<CreateReminderConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IReminderScheduleService _reminderScheduleService;

        public CreateReminderConfigurationCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IReminderScheduleService reminderScheduleService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _reminderScheduleService = reminderScheduleService;
        }
        public async Task<Result> Handle(CreateReminderConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.ValidateOrganisationData(request.AccessToken, request.OrganisationId);

                var exists = await _context.ReminderConfigurations.AnyAsync(x => x.StartDate == request.StartDate
                                      && x.EndDate == request.EndDate && x.RecurrenceValue == request.RecurrenceValue
                                      && x.ReminderScheduleFrequency == request.ReminderScheduleFrequency
                                      && x.OrganisationId == request.OrganisationId);
                if (exists)
                {
                    return Result.Failure($"Reminder Configuration already exists with this details");
                }
                if (request.StartDate > request.EndDate)
                {
                    return Result.Failure($"Start Date cannot be greater than end Date");
                }
                if (DateTime.Now.Date.Subtract(request.EndDate.Date).TotalDays > 0)
                {
                    
                    return Result.Failure($"End Date cannot be less than Current Date");
                }
                var reminderConfiguration = new Domain.Entities.ReminderConfiguration
                {
                    OrganisationId = request.OrganisationId,
                    OrganisationName = _authService.Organisation.Name,
                    RecurrenceValue = request.RecurrenceValue,
                    RecurringCountBalance = request.RecurrenceValue,
                    MonthlyReminderScheduleValue = request.MonthlyReminderScheduleValue,
                    YearlyReminderSchedule = _mapper.Map<YearlyReminderSchedule>(request.YearlyReminderScheduleValue),
                    ReminderScheduleFrequency = request.ReminderScheduleFrequency,
                    NextRecurringPeriod = request.StartDate,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    ReminderType = request.ReminderType,

                };
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
                    reminderConfiguration.WeeklyReminderSchedule = weeklySchedules;
                }
                reminderConfiguration = await _reminderScheduleService.ComputeReminderSchedule(reminderConfiguration);
               await _context.ReminderConfigurations.AddAsync(reminderConfiguration);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Reminder Configuration was created successfully", reminderConfiguration);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Reminder Configuration creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message} ");
            }
        }

      
    }
}
