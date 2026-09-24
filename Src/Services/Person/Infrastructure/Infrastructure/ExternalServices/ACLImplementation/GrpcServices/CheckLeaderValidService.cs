using CheckLeaderValided;
using Domain.Persons.Leader;
using Grpc.Core;

namespace   Infrastructure.ExternalServices.ACLImplementation.GrpcServices;
public class CheckLeaderValidService : CheckLeaderValid.CheckLeaderValidBase
{
    private readonly ILeaderRepository _LeaderRepository;

    public CheckLeaderValidService(ILeaderRepository LeaderRepository)
    {
        _LeaderRepository = LeaderRepository;
    }

    public override async Task<IsLeaderValidResponse> CheckLeaderValidity(
        IsLeaderValidRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.LeaderId, out var leaderId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "leaderId is not a valid Guid"));
        }



        var Leader = await _LeaderRepository.ChecktLeaderById(
            leaderId,
            context.CancellationToken);





        return new IsLeaderValidResponse
        {
            IsValid = Leader
        };
    }
}