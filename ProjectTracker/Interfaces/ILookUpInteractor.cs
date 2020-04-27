using ProjectTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    public interface ILookUpInteractor
    {
        Task<LookUp> GetLookUps();
    }
}
