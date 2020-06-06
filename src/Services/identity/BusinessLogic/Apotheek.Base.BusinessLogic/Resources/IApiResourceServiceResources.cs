using Apotheek.Base.BusinessLogic.Helpers;

namespace Apotheek.Base.BusinessLogic.Resources
{
    public interface IApiResourceServiceResources
    {
        ResourceMessage ApiResourceDoesNotExist();
        ResourceMessage ApiResourceExistsValue();
        ResourceMessage ApiResourceExistsKey();
        ResourceMessage ApiScopeDoesNotExist();
        ResourceMessage ApiScopeExistsValue();
        ResourceMessage ApiScopeExistsKey();
        ResourceMessage ApiSecretDoesNotExist();
        ResourceMessage ApiResourcePropertyDoesNotExist();
        ResourceMessage ApiResourcePropertyExistsKey();
        ResourceMessage ApiResourcePropertyExistsValue();
    }
}