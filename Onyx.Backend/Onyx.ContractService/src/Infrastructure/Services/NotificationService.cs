
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Onyx.ContractService.Application.Common.Interfaces;
using ReventInject.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractService.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendNotification(string deviceID, string message)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var firebaseOptionsServerId = _configuration["Firebase:ServerKey"];
                    var firebaseOptionsSenderId = _configuration["Firebase:SenderId"];

                    client.BaseAddress = new Uri("https://fcm.googleapis.com");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                        $"key={firebaseOptionsServerId}");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={firebaseOptionsSenderId}");

                    var data = new
                    {
                        to = deviceID,
                        notification = new
                        {
                            body = message,
                            title = "Onyx",
                        },
                        priority = "high"
                    };

                    var json = JsonConvert.SerializeObject(data);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var result = await client.PostAsync("/fcm/send", httpContent);
                    var resp = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogInfoAsnyc(ex?.Message ?? ex.InnerException?.Message, "Notification", "Onyx");
            }

        }
    }
}
