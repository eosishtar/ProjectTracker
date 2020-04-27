using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.Interactors
{
    public class ResourceInteractor : IResourceInteractor
    {
        private readonly PTContext _db;

        public ResourceInteractor(PTContext db)
        {
            this._db = db;
        }


        public List<Resource> GetListResources()
        {
            return _db.Resource
                .OrderBy(x => x.Name)
                .ToList();
        }

        public Resource GetResourceDetails(int? id)
        {
            if (id != null)
            {
                return _db.Resource.Where(x => x.Id == id).FirstOrDefault();
            }

            return null;
        }

        public bool Exists(string resourceName)
        {
            return _db.Task.Any(x => x.Name == resourceName);
        }

        public (bool, string) CreateResource(Resource resource)
        {
            var resourceExist = _db.Resource.Any(x => x.Name == resource.Name);

            if (!resourceExist)
            {
                var now = DateTime.Now;

                resource.CreatedDate = now;
                resource.ModifiedDate = now;

                _db.Resource.Add(resource);
                _db.SaveChanges();

                return (true, null);
            }

            return (false, $"Resource '{resource.Name}' already exists.");
        }

        public (bool, string) UpdateResource(int id, Resource resource)
        {
            var result = _db.Resource.Where(x => x.Id == id).FirstOrDefault();

            if (result != null)
            {
                result.Name = resource.Name?.Trim();
                result.Title= resource.Title?.Trim();
                result.ContactNumber = resource.ContactNumber?.Trim();
                result.EmailAddress = resource.EmailAddress?.Trim();
                result.IsActive = resource.IsActive;
                result.ModifiedDate = DateTime.Now;
                
                _db.Resource.Update(result);
                _db.SaveChanges();

                return (true, null);
            }

            return (false, $"Resource id '{resource.Id}' could not be found.");
        }

        public IQueryable<Resource> GetResourceLookups()
        {
            return _db.Resource.Where(x => x.IsActive);
        }

        public (bool, string) DeleteResource(int id)
        {
            var resource = _db.Resource.Where(x => x.Id == id).FirstOrDefault();

            if (resource != null)
            {
                resource.IsActive = !resource.IsActive;
                resource.ModifiedDate = DateTime.Now;
                _db.Resource.Update(resource);
                _db.SaveChanges();

                return (true, "");
            }

            return (false, $"Resource '{id}' not found.");
        }
    }
}
