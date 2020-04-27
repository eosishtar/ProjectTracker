using System.Collections.Generic;

namespace ProjectTracker.Entities
{
    public class LookUp
    {
        public List<LookUpDetail> Projects { get; set; }
        public List<LookUpDetail> Efforts { get; set; }
        public List<LookUpDetail> Resources { get; set; }
        public List<LookUpDetail> Statuses { get; set; }

        public class LookUpDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
