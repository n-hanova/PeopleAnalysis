using System;
using System.Collections.Generic;

namespace PeopleAnalysis.ViewModels
{
    public class FindPeoplePageViewModel
    {
        public FindPeopleViewModel FindPeopleViewModel { get; set; }
        public List<FinderResultViewModel> FinderResultViewModel { get; set; }
    }


    public class FindPeopleViewModel
    {
        public string FindText { get; set; }
    }

    public class FinderResultViewModel
    {
        public string Name { get; set; }
        public List<FindedPeopleViewModel> FindedPeopleViewModels { get; set; }
    }

    public class FindedPeopleViewModel
    {
        public string FullName { get; set; }
        public Uri ImagePath { get; set; }
        public int? Age { get; set; }
        public string Id { get; set; }
    }
}
