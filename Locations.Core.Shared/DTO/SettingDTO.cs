using System;

namespace Locations.Core.Shared.DTO
{
    /// <summary>
    /// Data Transfer Object for settings
    /// </summary>
    public class SettingDTO
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the setting key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the setting value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the setting description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the SettingDTO class
        /// </summary>
        public SettingDTO()
        {
            Key = string.Empty;
            Value = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the SettingDTO class with values
        /// </summary>
        public SettingDTO(string key, string value, string description = "")
        {
            Key = key;
            Value = value;
            Description = description;
        }
    }
}