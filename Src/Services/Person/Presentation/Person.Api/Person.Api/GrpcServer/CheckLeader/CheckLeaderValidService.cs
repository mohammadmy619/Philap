using CheckLeaderValided;
using Grpc.Core;
using MediatR;

namespace Person.Api.GrpcServer.CheckLeader
{
    public class CheckLeaderValidService : CheckLeaderValid.CheckLeaderValidBase
    {

        private readonly IMediator _mediator;

        public CheckLeaderValidService(IMediator mediator)
        {
            _mediator = mediator;
        }


        public override async Task<IsLeaderValidResponse> CheckLeaderValidity(
           IsLeaderValidRequest request,
           ServerCallContext context)
        {
            // تبدیل leaderId به Guid
            if (!Guid.TryParse(request.LeaderId, out var leaderId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "leaderId is not a valid Guid"));
            }

            var query = new IsLeaderValidQuery(leaderId);
            var isValid = await _mediator.Send(query, context.CancellationToken);

            return new IsLeaderValidResponse { IsValid = isValid };

           
            //bool isValid = leaderId != Guid.Empty;

            //var response = new IsLeaderValidResponse
            //{
            //    IsValid = isValid
            //};

            //return Task.FromResult(response);
        }
    }
}
