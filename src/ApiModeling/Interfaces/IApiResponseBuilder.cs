using System.Collections.Generic;

namespace Phlank.ApiModeling
{
    public interface IApiResponseBuilder
    {
        IApiResponseBuilder WithError(ApiError error);
        IApiResponseBuilder WithErrors(IEnumerable<ApiError> errors);
        IApiResponseBuilder WithWarning(ApiWarning warning);
        public IApiResponseBuilder WithWarnings(IEnumerable<ApiWarning> warnings);
        public ApiResponse Build();
        public ApiResponse<TContent> Build<TContent>(TContent content) where TContent : class;
    }
}
