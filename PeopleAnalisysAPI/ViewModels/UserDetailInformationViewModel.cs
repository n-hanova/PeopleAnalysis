using System;

namespace PeopleAnalysis.ViewModels
{
    public class UserDetailInformationViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Birthday { get; set; } = string.Empty;
        public Uri Photo { get; set; } 
        public Uri[] Photos { get; set; }
        public Uri PageUrl { get; set; }
        public bool IsPrivate { get; set; }
        public string Social { get; set; } = string.Empty;
        public AnalitycsViewModel AnalitycsViewModel { get; set; }
    }
}
