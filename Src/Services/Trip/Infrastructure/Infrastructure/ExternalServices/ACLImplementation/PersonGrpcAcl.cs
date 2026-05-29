using ACL.PersonACL;
using CheckLeaderValided;



namespace Infrastructure.ExternalServices.ACLImplementation
{
    public class PersonGrpcAcl : IPersonACL
    {

        private readonly CheckLeaderValid.CheckLeaderValidClient _client;

        public PersonGrpcAcl(CheckLeaderValid.CheckLeaderValidClient client)
        {
            _client = client;
        }
        public async Task<bool> IsLeaderValidAsync(Guid leaderId, CancellationToken cancellationToken)
        {
            var request = new IsLeaderValidRequest
            {
                LeaderId = leaderId.ToString()
            };

            var response = await _client.CheckLeaderValidityAsync(
                request,
                cancellationToken: cancellationToken);

            return response.IsValid;
        }
    }
}
