using CommonCoreLibrary.Extensions;
using CommonCoreLibrary.Services;
using Microsoft.AspNetCore.Http;
using PeopleAnalisysAPI.Models.Rabbit;
using PeopleAnalisysAPI.ViewModels;
using PeopleAnalysis.Models;
using PeopleAnalysis.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAnalysis.Services
{
    public class AnaliticService
    {
        private readonly IDatabaseContext databaseContext;
        private readonly ApisManager apisManager;
        private readonly ISender sender;
        private readonly IMapperService mapperService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AnaliticService(IDatabaseContext databaseContext, ApisManager apisManager, ISender sender, IMapperService mapperService, IHttpContextAccessor httpContextAccessor)
        {
            this.databaseContext = databaseContext;
            this.apisManager = apisManager;
            this.sender = sender;
            this.mapperService = mapperService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public AnalitycsViewModel GetAnaliticsAboutUser(string userId, string social, string currentUserId)
        {
            var lastAnalitics = databaseContext.Requests.OrderByDescending(x => x.Id)
                .FirstOrDefault(x => x.UserId == userId && x.Social == social && x.OwnerId == currentUserId);
            if (lastAnalitics == null)
                return new AnalitycsViewModel();
            Result result = null;
            if (lastAnalitics.Status == Status.Complete)
                result = databaseContext.Results.FirstOrDefault(x => x.Request.Id == lastAnalitics.Id);
            return new AnalitycsViewModel
            {
                Status = lastAnalitics.Status,
                ResultObjectsCount = result?.ResultObjects?.Count ?? 0,
                IsResult = result != null,
                Answer = result?.ResultAnswer ?? false,
                Time = lastAnalitics.TimeComplete,
                ResultsNames = result?.ResultObjects.Select(x => x.AnalysObject.Name).ToArray() ?? Array.Empty<string>(),
                ResultsValues = result?.ResultObjects.Select(x => x.Count).ToArray() ?? Array.Empty<float>()
            };
        }

        public bool CreateRequest(AnalitycsRequestModel analitycsRequest, string user, string authorization)
        {
            var isExists = databaseContext.Requests.Any(x => x.OwnerId == user && x.Social == analitycsRequest.Social && x.User == analitycsRequest.Id && x.Status == Status.Complete);
            if (isExists)
                return false;
            var newRequest = new Models.Request
            {
                CreateId = user,
                OwnerId = user,
                Status = Status.Create,
                User = analitycsRequest.UserName,
                Social = analitycsRequest.Social,
                UserId = analitycsRequest.Id,
                UserUrl = apisManager[analitycsRequest.Social].GetUserUri(analitycsRequest.Id)
            };
            databaseContext.Add(newRequest);
            databaseContext.SaveChanges();
            sender.Send(mapperService.Map<RequestRabbit>(newRequest), authorization ?? string.Empty);
            return true;
        }
    }

    public interface IAnaliticAIService
    {
        Task<Request> InProcessAsync(RequestViewModel request, string user);
        Task ReadyResult(ReadyResultViewModel readyResultViewModel);
    }

    public class AnaliticAIService : IAnaliticAIService
    {
        private readonly IDatabaseContext databaseContext;

        public AnaliticAIService(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Request> InProcessAsync(RequestViewModel request, string user)
        {
            var find = databaseContext.Requests.FirstOrDefault(x => x.Id == request.Id);
            if (find == null)
                throw new ApplicationException("Not found request");
            if (find.Status != request.Status)
                throw new ApplicationException("Request is changed");

            var newRequest = new Request
            {
                CreateId = user,
                OwnerId = request.OwnerId,
                Social = request.Social,
                Status = Status.InProgress,
                User = request.User,
                UserId = request.UserId,
                UserUrl = request.UserUrl
            };
            databaseContext.Add(newRequest);

            await databaseContext.SaveChangesAsync();
            return newRequest;
        }

        public async Task ReadyResult(ReadyResultViewModel readyResultViewModel)
        {
            var request = readyResultViewModel.RequestViewModel;

            var completeRequest = new Request
            {
                CreateId = request.CreateId,
                OwnerId = request.OwnerId,
                Social = request.Social,
                Status = Status.Complete,
                User = request.User,
                UserId = request.UserId,
                UserUrl = request.UserUrl,
                TimeComplete = DateTime.Now - request.DateTime
            };

            var readyResult = readyResultViewModel.Result;

            for (int i = 0; i < readyResult.ResultObjects.Count; i++)
            {
                var tmp = databaseContext.AnalysObjects.FirstOrDefault(x => x.Name == readyResult.ResultObjects[i].AnalysObject.Name);
                if (tmp == null)
                    databaseContext.Add(readyResult.ResultObjects[i].AnalysObject);
                else
                    readyResult.ResultObjects[i].AnalysObject = tmp;
            }

            var resultIndex = readyResult.ResultObjects.Sum(x => x.Count * x.AnalysObject.Weight);
            readyResult.ResultAnswer = resultIndex > 1;
            readyResult.Request = completeRequest;

            databaseContext.Add(completeRequest);
            databaseContext.Add(readyResult);
            databaseContext.SaveChanges();
        }
    }
}
