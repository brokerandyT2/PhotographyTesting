namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface ITipDTO : IDTOBase
    {

        int Id { get; set; }
        string Fstop { get; set; }
        string Shutterspeed { get; set; }
        string ISO { get; set; }  // Note: This is in uppercase
        int TipTypeID { get; set; }
        string Title { get; set; }
        string I8n { get; set; }
        string Content { get; set; }
    }
}