using Ocelot.LoadBalancer.Interfaces;
using Ocelot.Responses;
using Ocelot.Values;

namespace ApiGateway
{
    public class WeightedLoadBalancer : ILoadBalancer
    {
        private readonly List<ServiceHostAndPort> _weighted;
        private int _index = -1;

        public string Type => "Weighted";

        public WeightedLoadBalancer(List<ServiceHostAndPort> services)
        {
            _weighted = new List<ServiceHostAndPort>();

            for (int i = 0; i < services.Count; i++)
            {
                var s = services[i];

                if (i == 0)      
                    _weighted.AddRange(Enumerable.Repeat(s, 5));
                else if (i == 1) 
                    _weighted.AddRange(Enumerable.Repeat(s, 3));
                else if (i == 2) 
                    _weighted.AddRange(Enumerable.Repeat(s, 2));
            }

            if (!_weighted.Any())
                _weighted = services;
        }

        public Task<Response<ServiceHostAndPort>> LeaseAsync(HttpContext context)
        {
            var i = Interlocked.Increment(ref _index);
            var service = _weighted[i % _weighted.Count];

            return Task.FromResult<Response<ServiceHostAndPort>>(
                new OkResponse<ServiceHostAndPort>(service)
            );
        }

        public void Release(ServiceHostAndPort service)
        {

        }
    }
}
