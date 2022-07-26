using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Domain.Common
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// This method converts periods to days using the period frequency
        /// </summary>
        /// <typeparam name="frequency">The period frequency type</typeparam>
        /// <param name="period">the period number which will be converted to days using the relevant multiplier</param>
        /// <returns></returns>
        public static int ToPeriodInDays(this FreeTrialPeriodFrequency frequency, int period)
        {
            int days = 0;
            try
            {
                switch (frequency)
                {
                    case FreeTrialPeriodFrequency.Days:
                        days = period;
                        break;
                    case FreeTrialPeriodFrequency.Weeks:
                        days = period * 7;
                        break;
                    case FreeTrialPeriodFrequency.Months:
                        days = period * 30;
                        break;
                    case FreeTrialPeriodFrequency.Years:
                        days = period * 365;
                        break;
                    default:
                        break;
                }
                return days;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
