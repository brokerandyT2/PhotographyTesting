namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface ITipTypeDTO : IDTOBase
    {
        int Id { get; set; }
        string Name { get; set; }
        string I8n { get; set; }
    }
}