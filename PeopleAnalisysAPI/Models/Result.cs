using System.Collections.Generic;

namespace PeopleAnalysis.Models
{
    public class Result
    {
        public int Id { get; set; }
        public virtual Request Request { get; set; }
        public bool ResultAnswer { get; set; }
        public virtual List<ResultObject> ResultObjects { get; set; }
    }

    public class ResultObject
    {
        public int Id { get; set; }
        public virtual Result Result { get; set; }
        public virtual AnalysObject AnalysObject { get; set; }
        public float Count { get; set; }
    }

    public class AnalysObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
    }
}
