using System;
using System.Threading.Tasks;

namespace ScienceTest.Api.Clients {
    public interface IUpstreamApiClient {
        Task<string[]> GetValues(Guid correlationId);
    }
}