using BMDC.Models.Auth;

namespace WebApplication1.Infrastructure.Common.Interface
{
    public interface IFrameRequest
    {
        T AppendDBName<T>(T request) where T : REQBase;

        string GetDBName();
    }
}