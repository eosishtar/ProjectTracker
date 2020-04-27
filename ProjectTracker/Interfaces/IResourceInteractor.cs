using ProjectTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    public interface IResourceInteractor
    {
        List<Resource> GetListResources();

        Resource GetResourceDetails(int? id);

        (bool, string) CreateResource(Resource resource);

        (bool, string) UpdateResource(int id, Resource resource);

        IQueryable<Resource> GetResourceLookups();

        (bool, string) DeleteResource(int id);

    }
}
