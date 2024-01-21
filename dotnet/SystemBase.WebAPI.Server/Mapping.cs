using Mapster;

namespace SystemBase;

sealed class Mapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Failed, FailedAPI>();
        config.NewConfig<Success, SuccessAPI>();
        
        config.ForType<PermissionNotFound, PermissionNotFoundAPI>()
            .Map(dest => dest.Id, src => HashIdConverterInstance.Instance.FromInt32(src.Id));

        config.ForType<VerifyPermissionFailed, VerifyPermissionFailedAPI>()
            .Map(dest => dest.PermissionId, src => HashIdConverterInstance.Instance.FromInt32(src.PermissionId));
    }
}

sealed class ResponseTypeMapRegistration : IResponseTypeMapRegistration
{
    public void Register(IResponseTypeMapRegistry registry)
    {
        registry
            .RegisterOK<Success, SuccessAPI>()
            .RegisterBadRequest<Failed, FailedAPI>()
            .RegisterNotFound<PermissionNotFound, PermissionNotFoundAPI>()
            .RegisterBadRequest<VerifyPermissionFailed, VerifyPermissionFailedAPI>();
    }
}