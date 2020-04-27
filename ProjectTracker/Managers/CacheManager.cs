using Microsoft.Extensions.Caching.Memory;
using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Managers
{
    public class CacheManager
    {
        private readonly ILookUpInteractor _interactor;
        private readonly Settings _settings;


        public CacheManager(ILookUpInteractor interactor, Settings settings)
        {
            this._interactor = interactor;
            this._settings = settings;
        }

        public async Task<LookUp> CheckLookupCache(IMemoryCache _cache)
        {
            int Timeout = Convert.ToInt32(_settings.CacheTimeInMinutes);

            var myCache = _cache.TryGetValue(CacheKeys.Entry, out LookUp cacheEntry);
            if (myCache == false)
            {
                cacheEntry = await GetLookUps();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(Timeout));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }

        public async Task<LookUp> GetLookUps()
        {
            var LookUpModel = await _interactor.GetLookUps();
            return LookUpModel;
        }
    }
}
