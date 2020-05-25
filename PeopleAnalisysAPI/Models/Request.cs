using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAnalysis.Models
{
    public enum Status
    {
        Create,
        InProgress,
        Fail,
        Complete,
        Closed
    }

    public class Request
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string User { get; set; }
        public string UserId { get; set; }
        public Uri UserUrl { get; set; }
        public string Social { get; set; }
        public string OwnerId { get; set; }
        public string CreateId { get; set; }
        public Status Status { get; set; }
        public TimeSpan TimeComplete { get; set; }
    }
}
