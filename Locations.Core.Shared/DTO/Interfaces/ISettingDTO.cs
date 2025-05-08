using System;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface ISettingDTO : IDTOBase
    {
        int Id { get; set; }
        string Name { get; set; }
        string Value { get; set; }
        DateTime Timestamp { get; set; }
    }
}