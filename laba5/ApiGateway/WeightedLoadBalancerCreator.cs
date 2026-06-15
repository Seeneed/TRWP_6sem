using Ocelot.Configuration;
using Ocelot.LoadBalancer.Interfaces;
using Ocelot.Responses;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;

namespace ApiGateway
{
    public class WeightedLoadBalancerCreator : ILoadBalancerCreator
    {
        public string Type => "Weighted";

        public Response<ILoadBalancer> Create(
            DownstreamRoute route,
            IServiceDiscoveryProvider serviceDiscoveryProvider)
        {
            var services = route.DownstreamAddresses
                .Select(x => new ServiceHostAndPort(x.Host, x.Port))
                .ToList();

            return new OkResponse<ILoadBalancer>(
                new WeightedLoadBalancer(services)
            );
        }
    }
}
