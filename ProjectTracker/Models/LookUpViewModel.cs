using System.Collections.Generic;

namespace ProjectTracker.Models
{
    public class LookUpViewModel
    {
        public List<LookUp> Projects { get; set; }
        public List<LookUp> Efforts { get; set; }
        public List<LookUp> Resources { get; set; }
        public List<LookUp> Statuses { get; set; }
            

        public class LookUp
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
