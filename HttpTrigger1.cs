using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs.Extensions.ServiceBus;

namespace Company.Function
{

    public class HttpTrigger1
    {


        [FunctionName("HttpTrigger1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [ServiceBus("queue-asp", Connection = "redondosb")] IAsyncCollector<ServiceBusMessage> collector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            DateTime aDate = DateTime.Now;

            ServiceBusMessage message = new ServiceBusMessage(new BinaryData(aDate.ToString("MM/dd/yyyy HH:mm:ss")))
            {
                SessionId = aDate.ToString("yyyyMMddHHmm")
            };

            //using sdk instead
            //await using var client = new ServiceBusClient("Endpoint=sb://redondosb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=qGWFHR45CJpCJDg1dAcplNSYBw3JM8thChvOo1Y1f1s=");
            //ServiceBusSender sender = client.CreateSender("queue-asp");
            //await sender.SendMessageAsync(message);

            // IAsyncCollector allows sending multiple messages in a single function invocation
            await collector.AddAsync(message);

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}.";

            return new OkObjectResult(responseMessage);
        }
    }
}
