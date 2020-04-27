using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Interactors
{
    public class EffortInteractor : IEffortInteractor
    {
        private readonly PTContext _db;

        public EffortInteractor(PTContext db)
        {
            this._db = db;
        }

        public IQueryable<Effort> GetListOfEfforts()
        {
            return _db.Effort.OrderBy(x => x.Id).AsQueryable();
        }
    }
}
