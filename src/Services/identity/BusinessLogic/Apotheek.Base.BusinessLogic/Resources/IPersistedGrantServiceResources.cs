using Apotheek.Base.BusinessLogic.Helpers;

namespace Apotheek.Base.BusinessLogic.Resources
{
    public interface IPersistedGrantServiceResources
    {
        ResourceMessage PersistedGrantDoesNotExist();

        ResourceMessage PersistedGrantWithSubjectIdDoesNotExist();
    }
}
