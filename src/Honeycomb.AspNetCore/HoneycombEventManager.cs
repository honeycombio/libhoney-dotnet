using System;
using System.Collections.Generic;
using Honeycomb.Models;
using Microsoft.AspNetCore.Http;

namespace Honeycomb.AspNetCore
{
    public interface IHoneycombEventManager
    {
        /// <summary>
        /// Add eventData to the webEvent
        /// </summary>
        /// <param name="key">The Key</param>
        /// <param name="data">The data</param>
        void AddData(string key, object data);
    }

    public class HoneycombEventManager : IHoneycombEventManager
    {
        public const string ContextItemName = "Honeycomb_event";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HoneycombEventManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddData(string key, object data)
        {
            if (!_httpContextAccessor
                .HttpContext
                .Items
                .TryGetValue(ContextItemName, out var existingData))
                return;

            var ev = existingData as HoneycombEvent;
            ev.Data.Add(key, data);
        }
    }
}
