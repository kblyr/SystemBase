namespace SystemBase;

public interface IRequestHandler<TRequest> where TRequest : IRequest
{
    ValueTask<IResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}