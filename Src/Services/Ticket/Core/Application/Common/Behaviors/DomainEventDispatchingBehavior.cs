using BuildingBlocks.Domain;
using MediatR;

namespace Application.Common.Behaviors;

public sealed class DomainEventDispatchingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IDomainEventDispatcher _dispatcher;

    public DomainEventDispatchingBehavior(IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        await _dispatcher.DispatchEventsAsync(cancellationToken);

        return response;
    }
}