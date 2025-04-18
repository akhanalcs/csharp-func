using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AshK.Functions
{
    public class EventGridBlobTrigger
    {
        private readonly ILogger<EventGridBlobTrigger> _logger;

        public EventGridBlobTrigger(ILogger<EventGridBlobTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(EventGridBlobTrigger))]
        public async Task Run([BlobTrigger("samples-workitems/{name}", Source = BlobTriggerSource.EventGrid, Connection = "5f2f77_STORAGE")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob Trigger (using Event Grid) processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
