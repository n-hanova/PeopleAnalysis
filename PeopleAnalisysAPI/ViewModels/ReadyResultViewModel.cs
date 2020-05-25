using PeopleAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAnalisysAPI.ViewModels
{
    public class ReadyResultViewModel
    {
        public RequestViewModel RequestViewModel { get; set; }
        public Result Result { get; set; }
    }
}
