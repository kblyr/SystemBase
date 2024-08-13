namespace SystemBase;

public interface IAPIRequest {}

public interface IAPIRequestExecutor
{
    Task<IExecutionResult> Execute<TAPIRequest, TExecutable>(TAPIRequest request, CancellationToken cancellationToken)
        where TAPIRequest : IAPIRequest 
        where TExecutable : IExecutable;
}

sealed class APIRequestExecutor(MapsterMapper.IMapper mapper) : IAPIRequestExecutor
{
    public async Task<IExecutionResult> Execute<TAPIRequest, TExecutable>(TAPIRequest request, CancellationToken cancellationToken)
        where TAPIRequest : IAPIRequest
        where TExecutable : IExecutable
    {
        var executable = mapper.Map<TExecutable>(request);
        return await executable.Execute(cancellationToken);
    }
}
