using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MessengerAutomata
{
    public static class Function1
    {
        [FunctionName("ws-automata")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var accountSid = System.Environment.GetEnvironmentVariable("MessengerAutomata__AccountSid");
            var authToken = System.Environment.GetEnvironmentVariable("MessengerAutomata__AuthToken");
            var MessagingServiceSid = System.Environment.GetEnvironmentVariable("MessengerAutomata__MessagingServiceSid");

            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber("+593979214297"));
            messageOptions.MessagingServiceSid = MessagingServiceSid;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string cellPhone = data?.cellPhone;
            string clientName = data?.clientName;
            string urlWS = data?.urlWS;

            messageOptions.Body = $"{cellPhone} - {clientName} - {urlWS}";


            var message = MessageResource.Create(messageOptions);
            string responseMessage = "";

            return new OkObjectResult(responseMessage);
        }
    }
}
