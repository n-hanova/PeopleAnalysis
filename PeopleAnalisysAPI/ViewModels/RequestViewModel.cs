using PeopleAnalysis.Models;
using System;

namespace PeopleAnalisysAPI.ViewModels
{
    public class RequestViewModel
    {
        public Status Status { get; set; }
        public string OwnerId { get; set; }
        public string Social { get; set; }
        public string User { get; set; }
        public string UserId { get; set; }
        public Uri UserUrl { get; set; }
        public int Id { get; set; }
        public string CreateId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
