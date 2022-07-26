using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractService.Infrastructure.Services
{
    public class ReminderScheduleService : IReminderScheduleService
    {
        public async Task<ReminderConfiguration> ComputeReminderSchedule(ReminderConfiguration reminderConfiguration)
        {
                switch (reminderConfiguration.ReminderScheduleFrequency)
                {
                    case ReminderScheduleFrequency.Daily:
                        reminderConfiguration.NextRecurringPeriod = DateTime.Now.AddDays(1);
                        break;
                    case ReminderScheduleFrequency.Weekly:
                        if (reminderConfiguration.WeeklyReminderSchedule != null)
                        {
                            List<WeeklyReminderSchedule> weeklySchedules = new List<WeeklyReminderSchedule>();
                            foreach (var weeklyScheduleValue in reminderConfiguration.WeeklyReminderSchedule)
                            {
                                weeklySchedules.Add(new WeeklyReminderSchedule
                                {
                                    Day = weeklyScheduleValue.Day,

                                });
                                var weeklyValue = Enum.TryParse(weeklyScheduleValue.Day, true, out WeeklySchedule WeeklySchedule);
                                if (weeklyValue)
                                {
                                    var value = (DayOfWeek)WeeklySchedule;
                                    reminderConfiguration.NextRecurringPeriod = ComputeDateTimeFromDayOfWeek(value);
                                }
                            }
                            reminderConfiguration.WeeklyReminderSchedule = weeklySchedules;
                        }
                        break;
                    case ReminderScheduleFrequency.Monthly:
                        if (reminderConfiguration.MonthlyReminderScheduleValue != null)
                        {
                            var month = DateTime.Now.Month;
                            reminderConfiguration.NextRecurringPeriod = DateTime.Now.AddDays(Convert.ToInt64(reminderConfiguration.MonthlyReminderScheduleValue));
                        }
                        break;
                    case ReminderScheduleFrequency.Yearly:
                        if (reminderConfiguration.YearlyReminderSchedule != null)
                        {
                            reminderConfiguration.YearlyReminderSchedule = new YearlyReminderSchedule
                            {
                                Month = reminderConfiguration.YearlyReminderSchedule.Month,
                                Value = reminderConfiguration.YearlyReminderSchedule.Value
                            };
                        }
                        if (reminderConfiguration.RecurringCountBalance > 0)
                        {
                            reminderConfiguration.NextRecurringPeriod = reminderConfiguration.NextRecurringPeriod.AddYears(reminderConfiguration.RecurringCountBalance);
                        }
                        var monthValue = Enum.TryParse(reminderConfiguration.YearlyReminderSchedule.Month, true, out Year year);
                        if (monthValue)
                        {
                            var month = (int)year;
                            reminderConfiguration.NextRecurringPeriod = DateTime.Now.AddMonths(month).AddDays(reminderConfiguration.YearlyReminderSchedule.Value);
                        }
                        break;
                    default:
                        break;
                }

             return  reminderConfiguration;
            }
        public static DateTime ComputeDateTimeFromDayOfWeek(DayOfWeek dayOfWeek)
        {

            int dayOfWeek2 = (int)DateTime.Today.DayOfWeek;
            DateTime result = DateTime.Today.AddDays((int)dayOfWeek - dayOfWeek2);
            return result;
        }

    }

    }

