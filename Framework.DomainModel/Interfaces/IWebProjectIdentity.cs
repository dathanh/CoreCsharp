using System.Security.Principal;

namespace Framework.DomainModel.Interfaces
{
    public interface IWebProjectIdentity : IIdentity
    {
        int UserIdentityId { get; }
    }
}
