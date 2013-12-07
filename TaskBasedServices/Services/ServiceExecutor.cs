using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskBasedServices.Services
{
    public class ServiceExecutor
    {
        public void Get<TDto>(Action<TDto> callback)
        {
            var client = new WebClient();
            client.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    throw new Exception(args.Error.Message);
                else if (args.Cancelled)
                    Console.WriteLine("Service call cancelled");
                else
                {
                    var dto = JsonConvert.DeserializeObject<TDto>(args.Result);
                    callback(dto);
                }
            };

            const string baseAddress = "http://localhost:8080/Services/";
            var serviceName = typeof(TDto).Name + ".ashx";
            client.DownloadStringAsync(new Uri(baseAddress + serviceName, UriKind.Absolute));
        }

        public Task<TDto> GetAsTask<TDto>()
        {
            var tcs = new TaskCompletionSource<TDto>();
            var client = new WebClient();

            client.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Error != null)
                    tcs.SetException(args.Error);
                else if (args.Cancelled)
                    tcs.SetCanceled();
                else
                {
                    var dto = JsonConvert.DeserializeObject<TDto>(args.Result);
                    tcs.SetResult(dto);
                }
            };

            const string baseAddress = "http://localhost:8080/Services/";
            var serviceName = typeof(TDto).Name + ".ashx";
            client.DownloadStringAsync(new Uri(baseAddress + serviceName, UriKind.Absolute));
            return tcs.Task;
        }
    }
}