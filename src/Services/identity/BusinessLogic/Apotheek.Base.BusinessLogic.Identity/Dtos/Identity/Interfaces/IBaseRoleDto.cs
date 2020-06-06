namespace Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IBaseRoleDto
    {
        object Id { get; }
        bool IsDefaultId();
    }
}
