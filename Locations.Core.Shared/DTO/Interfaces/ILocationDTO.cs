using System;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface ILocationDTO : IDTOBase
    {
        int Id { get; set; }
        string City { get; set; }
        string State { get; set; }
        double Lattitude { get; set; }
        double Longitude { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Photo { get; set; }
        DateTime Timestamp { get; set; }
        bool IsDeleted { get; set; }
        string DateFormat { get; set; }
        string TimestampFormatted { get; }
        bool CanDelete { get; }
    }
}