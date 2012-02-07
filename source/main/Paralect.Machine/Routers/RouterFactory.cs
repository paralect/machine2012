using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Paralect.Machine.Routers
{
    public class RouterFactory
    {
        private readonly ConcurrentDictionary<String, IRouter> _routers;

        public RouterFactory(IDictionary<String, IRouter> routers)
        {
            if (routers == null) throw new ArgumentNullException("routers");
            _routers = new ConcurrentDictionary<string, IRouter>(routers);
        }

        public IRouter GetRouter(String name)
        {
            return _routers[name];
        }
    }
}