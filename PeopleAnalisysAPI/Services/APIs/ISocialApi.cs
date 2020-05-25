using PeopleAnalysis.Extensions;
using PeopleAnalysis.Models.Configuration;
using PeopleAnalysis.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace PeopleAnalysis.Services.APIs
{
    public interface ISocialApi
    {
        string Name { get; }
        Uri GetUserUri(string id);
        FinderResultViewModel Find(string text);
        FinderResultViewModel FindPage(Uri text);
        UserDetailInformationViewModel GetUserDetailInformationView(string id);
    }

    public class VkSocialApi : VkApi, ISocialApi
    {
        public string Name => "Vk";

        public VkSocialApi(KeysConfiguration keysConfiguration)
        {
            Authorize(new ApiAuthParams { AccessToken = keysConfiguration["vk"].Key });
        }

        private FinderResultViewModel Create(IEnumerable<User> users)
        {
            return new FinderResultViewModel
            {
                Name = Name,
                FindedPeopleViewModels = users.Select(x => new FindedPeopleViewModel
                {
                    FullName = $"{x.FirstName} {x.LastName}",
                    ImagePath = x.Photo200,
                    Age = DateTimeExtensions.Convert(x.BirthDate),
                    Id = x.Id.ToString()
                }).ToList()
            };
        }

        public FinderResultViewModel Find(string text)
        {
            var users = Users.Search(new VkNet.Model.RequestParams.UserSearchParams
            {
                Query = text,
                Count = 100,
                HasPhoto = true,
                Sort = VkNet.Enums.UserSort.ByPopularity,
                Fields = ProfileFields.All
            });
            return Create(users);
        }

        public FinderResultViewModel FindPage(Uri text)
        {
            var users = Users.Search(new VkNet.Model.RequestParams.UserSearchParams
            {
                Query = text?.AbsoluteUri.Split('/').Last(),
                Count = 100,
                HasPhoto = true,
                Sort = VkNet.Enums.UserSort.ByPopularity,
                Fields = ProfileFields.All
            });
            return Create(users);
        }

        public UserDetailInformationViewModel GetUserDetailInformationView(string id)
        {
            var user = Users.Get(new[] { long.Parse(id) }, ProfileFields.All)?.FirstOrDefault();
            VkCollection<Photo> photos = null;

            try
            {
                photos = Photo.GetAll(new VkNet.Model.RequestParams.PhotoGetAllParams { OwnerId = long.Parse(id) });
            }
            catch (Exception) { }

            return new UserDetailInformationViewModel
            {
                Birthday = user?.BirthDate,
                FullName = $"{user?.FirstName} {user?.LastName}",
                Id = id,
                PageUrl = GetUserUri(id),
                Photo = user.PhotoMaxOrig,
                Photos = photos?.Select(x => x.Sizes?.Last()?.Url).Where(x => x != null).ToArray(),
                IsPrivate = user.IsClosed ?? false,
                Social = Name.ToLower()
            };
        }

        public Uri GetUserUri(string id) => new Uri($"https://vk.com/id{id}");
    }
}
