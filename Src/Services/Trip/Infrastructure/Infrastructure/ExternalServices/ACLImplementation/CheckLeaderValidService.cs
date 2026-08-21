using CheckLeaderValided;
using Domain.TripAggregate;
using Grpc.Core;

namespace MyGrpcServer.Services
{
    public class CheckLeaderValidService : CheckLeaderValid.CheckLeaderValidBase
    {
        private readonly ITripRepository _TripRepository;

        public CheckLeaderValidService(ITripRepository TripRepository)
        {
            _TripRepository = TripRepository;
        }

        public override async Task<IsLeaderValidResponse> CheckLeaderValidity(
            IsLeaderValidRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.LeaderId, out var leaderId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "leaderId is not a valid Guid"));
            }



            var trip = await _TripRepository.ChecktTripById(
                leaderId,
                context.CancellationToken);


       


            return new IsLeaderValidResponse
            {
                IsValid = trip
            };
        }
    }
}
