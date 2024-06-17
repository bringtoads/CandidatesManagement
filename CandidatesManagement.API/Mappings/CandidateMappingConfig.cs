using CandidatesManagement.API.Models.Candidate;
using CandidatesManagement.Core.Models;
using Mapster;

namespace CandidatesManagement.API.Mappings
{
    public class CandidateMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CandidateRequest, Candidate>();
        }
    }
}
