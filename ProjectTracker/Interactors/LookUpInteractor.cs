using ProjectTracker.Entities;
using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectTracker.Interfaces;
using static ProjectTracker.Entities.LookUp;

namespace ProjectTracker.Interactors 
{
    public class LookUpInteractor : ILookUpInteractor
    {
        private readonly PTContext _db;

        public LookUpInteractor(PTContext db)
        {
            this._db = db;
        }

        public async Task<LookUp> GetLookUps()
        {
            var result = new LookUp
            {
                Efforts = _db.Effort.Select(x => new LookUpDetail() { Id = x.Id, Name = x.Description }).ToList(),
                Projects = _db.Project.Select(x => new LookUpDetail() { Id = x.Id, Name = x.Description }).ToList(),
                Resources = _db.Resource.Where(x => x.IsActive).Select(x => new LookUpDetail() { Id = x.Id, Name = x.Name}).ToList(),
                Statuses = _db.Status.Select(x => new LookUpDetail() { Id = x.Id, Name = x.Description }).ToList(),
            };

            return result;
        }

    }
}
