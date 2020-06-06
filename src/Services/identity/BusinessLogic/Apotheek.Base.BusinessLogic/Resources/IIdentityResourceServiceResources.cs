using Apotheek.Base.BusinessLogic.Helpers;

namespace Apotheek.Base.BusinessLogic.Resources
{
    public interface IIdentityResourceServiceResources
    {
        ResourceMessage IdentityResourceDoesNotExist();

        ResourceMessage IdentityResourceExistsKey();

        ResourceMessage IdentityResourceExistsValue();

        ResourceMessage IdentityResourcePropertyDoesNotExist();

        ResourceMessage IdentityResourcePropertyExistsValue();

        ResourceMessage IdentityResourcePropertyExistsKey();
    }
}
