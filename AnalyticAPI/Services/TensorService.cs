using AnalyticAPI.ApplicationAPI;
using AutoMapper;
using CommonCoreLibrary.Services;
using Microsoft.Extensions.Logging;
using PeopleAnalysisML.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PeopleAnalysis.Services
{
    public interface IAIService
    {
        Task ProcessTaskAsync(Request request);
    }

    public class TensorService : IAIService
    {
        private readonly IMLService mLService;
        private readonly ILogger<TensorService> logger;
        private readonly IApplicationAPIClient applicationAPIClient;
        private readonly IMapperService mapper;

        public TensorService(IMLService mLService, ILogger<TensorService> logger, IApplicationAPIClient applicationAPIClient, IMapperService mapper)
        {
            this.mLService = mLService;
            this.logger = logger;
            this.applicationAPIClient = applicationAPIClient;
            this.mapper = mapper;
        }

        public async Task ProcessTaskAsync(Request request)
        {
            logger.LogInformation("Start processing");
            var reqViewModel = mapper.Map<RequestViewModel>(request);
            await applicationAPIClient.ApiAnaliticInprocessAsync(reqViewModel);
            var detail = await applicationAPIClient.ApiPeopleAsync(new OpenPeopleViewModel
            {
                Key = request.UserId,
                Social = request.Social
            });

            List<string> files = new List<string>();
            var path = Path.Combine(Path.GetTempPath(), $"{request.UserId}_{Environment.TickCount64}");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            logger.LogInformation("Download files");
            using (var client = new WebClient())
            {
                foreach (var photo in detail.Photos)
                {
                    var str = Path.Combine(path, photo.Segments.Last());
                    client.DownloadFile(photo, str);
                    files.Add(str);
                }
            }

            logger.LogInformation("Analysis files");
            Dictionary<string, float> resultSet = new Dictionary<string, float>();
            foreach (var photo in files)
            {
                foreach (var val in mLService.Predict(new ModelInput { ImageSource = photo }).Where(x => x.Value > 0.10))
                {
                    if (resultSet.TryGetValue(val.Key, out var tmp))
                        resultSet[val.Key] = (resultSet[val.Key] + tmp) / 2;
                    else
                        resultSet[val.Key] = val.Value;
                }
            }

            logger.LogInformation("Create result");
            var readyResult = new Result
            {
                ResultAnswer = true,
                ResultObjects = new List<ResultObject>(),
                Request = request
            };

            foreach (var obj in resultSet)
            {
                readyResult.ResultObjects.Add(new ResultObject
                {
                    Count = obj.Value,
                    Result = readyResult,
                    AnalysObject = new AnalysObject
                    {
                        Name = obj.Key,
                        Weight = 1
                    }
                });
            }

            await applicationAPIClient.ApiAnaliticReadyresultAsync(new ReadyResultViewModel
            {
                RequestViewModel = mapper.Map<RequestViewModel>(request),
                Result = readyResult
            });
        }
    }
}
