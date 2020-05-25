using PeopleAnalysis.Services.APIs;
using PeopleAnalysis.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeopleAnalysis.Services
{
    public class ApisManager
    {
        private readonly Dictionary<string, ISocialApi> apis = new Dictionary<string, ISocialApi>();

        public ApisManager(VkSocialApi vkSocialApi)
        {
            AddApi(vkSocialApi.Name.ToLower(), vkSocialApi);
        }

        public void AddApi(string key, ISocialApi socialApi) => apis.Add(key, socialApi);
        public ISocialApi this[string name] => apis[name];

        public List<FinderResultViewModel> GetFinded(string search)
        {
            bool isUri = Uri.IsWellFormedUriString(search, UriKind.Absolute);
            var findAction = isUri ? new Func<ISocialApi, FinderResultViewModel>((ISocialApi api) => api.FindPage(new Uri(search))) : (ISocialApi api) => api.Find(search);
            return apis.Select(x => findAction(x.Value)).ToList();
        }
    }
}
