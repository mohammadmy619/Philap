using CheckLeaderValided;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACL.PersonACL.Implementations
{
    public class PersonACL : IPersonACL
    {
        private readonly CheckLeaderValid.CheckLeaderValidClient _client;

        public PersonACL(CheckLeaderValid.CheckLeaderValidClient client)
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
