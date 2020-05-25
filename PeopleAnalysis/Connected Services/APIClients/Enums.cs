using PeopleAnalysis.ApplicationAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAnalysis.Connected_Services.APIClients
{
    public enum StatusEnum
    {
        Create,
        InProgress,
        Fail,
        Complete,
        Closed
    }

    public static class EnumStatic
    {
        public static Status Raw(this StatusEnum statusEnum) => (Status)statusEnum;
    }
}
