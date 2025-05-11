// Location.Core/Helpers/ResourceProvider.cs
using System;
using System.Collections.Generic;

namespace Location.Core
{
    /// <summary>
    /// Central resource provider for shared UI resources across the application.
    /// Manages color resources, opacity settings, and library-specific overrides.
    /// </summary>
    public static class ResourceProvider
    {
        // Cache for transparent colors
        private static Dictionary<string, Microsoft.Maui.Graphics.Color> _colorCache = new Dictionary<string, Microsoft.Maui.Graphics.Color>();

        // Flag to track if the provider has been initialized
        private static bool _isInitialized = false;

        // Library overrides registry - allows libraries to override specific resources
        private static Dictionary<string, Dictionary<string, object>> _libraryOverrides =
            new Dictionary<string, Dictionary<string, object>>();

        /// <summary>
        /// Initializes the resource provider with core resources.
        /// Called at app startup.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized)
                return;

            // Initialize transparent colors for use in error messages and overlays
            InitializeTransparentColors();

            _isInitialized = true;
        }

        /// <summary>
        /// Registers overrides for a specific library.
        /// This allows a library to override specific resources.
        /// </summary>
        /// <param name="libraryName">The name of the library (e.g., "Photography")</param>
        /// <param name="overrides">Dictionary of resource key/value pairs to override</param>
        public static void RegisterLibraryOverrides(string libraryName, Dictionary<string, object> overrides)
        {
            if (string.IsNullOrEmpty(libraryName) || overrides == null)
                return;

            _libraryOverrides[libraryName] = overrides;
        }

        /// <summary>
        /// Creates and caches transparent colors for use throughout the application.
        /// </summary>
        private static void InitializeTransparentColors()
        {
            try
            {
                // Error overlay color (light red with 50% opacity)
                object errorColorObj = null;
                if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                    Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("ErrorRed", out errorColorObj) &&
                    errorColorObj is Microsoft.Maui.Graphics.Color errorColor)
                {
                    _colorCache["TransparentErrorBrush"] = errorColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentErrorBrush"] = Microsoft.Maui.Graphics.Color.FromArgb("#80FFEBEE");
                }

                // Black overlay color (black with 50% opacity)
                object overlayColorObj = null;
                if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                    Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("Black", out overlayColorObj) &&
                    overlayColorObj is Microsoft.Maui.Graphics.Color overlayColor)
                {
                    _colorCache["TransparentOverlayBrush"] = overlayColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentOverlayBrush"] = Microsoft.Maui.Graphics.Color.FromArgb("#80000000");
                }

                // Warning overlay color (yellow with 50% opacity)
                object warningColorObj = null;
                if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                    Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("WarningYellow", out warningColorObj) &&
                    warningColorObj is Microsoft.Maui.Graphics.Color warningColor)
                {
                    _colorCache["TransparentWarningBrush"] = warningColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentWarningBrush"] = Microsoft.Maui.Graphics.Color.FromArgb("#80FFF9C4");
                }

                // Success overlay color (green with 50% opacity)
                object successColorObj = null;
                if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                    Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("SuccessGreen", out successColorObj) &&
                    successColorObj is Microsoft.Maui.Graphics.Color successColor)
                {
                    _colorCache["TransparentSuccessBrush"] = successColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentSuccessBrush"] = Microsoft.Maui.Graphics.Color.FromArgb("#80E8F5E9");
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Error initializing transparent colors: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies standard resources to a page, with library-specific overrides.
        /// This should be called from page constructors or OnAppearing methods.
        /// </summary>
        /// <param name="page">The page to apply resources to</param>
        /// <param name="libraryName">Optional library name for library-specific overrides</param>
        public static void ApplyStandardResources(Microsoft.Maui.Controls.ContentPage page, string libraryName = null)
        {
            if (page == null)
                return;

            // Ensure initialization
            if (!_isInitialized)
                Initialize();

            // Apply the transparent colors
            foreach (var key in _colorCache.Keys)
            {
                page.Resources[key] = _colorCache[key];
            }

            // Apply library overrides if specified
            if (!string.IsNullOrEmpty(libraryName) && _libraryOverrides.ContainsKey(libraryName))
            {
                foreach (var pair in _libraryOverrides[libraryName])
                {
                    page.Resources[pair.Key] = pair.Value;
                }
            }
        }

        /// <summary>
        /// Gets a transparent color by name.
        /// </summary>
        /// <param name="name">The name of the cached color resource</param>
        /// <returns>The color, or Colors.Transparent if not found</returns>
        public static Microsoft.Maui.Graphics.Color GetColor(string name)
        {
            // Ensure initialization
            if (!_isInitialized)
                Initialize();

            if (_colorCache.TryGetValue(name, out var color))
                return color;

            return Microsoft.Maui.Graphics.Colors.Transparent;
        }

        /// <summary>
        /// Gets a color resource from the application resources or library overrides.
        /// </summary>
        /// <param name="resourceKey">The resource key</param>
        /// <param name="libraryName">Optional library name for library-specific overrides</param>
        /// <returns>The color, or Colors.Transparent if not found</returns>
        public static Microsoft.Maui.Graphics.Color GetResourceColor(string resourceKey, string libraryName = null)
        {
            object resource = null;

            // Check library overrides first
            if (!string.IsNullOrEmpty(libraryName) &&
                _libraryOverrides.ContainsKey(libraryName) &&
                _libraryOverrides[libraryName].TryGetValue(resourceKey, out resource) &&
                resource is Microsoft.Maui.Graphics.Color libraryColor)
            {
                return libraryColor;
            }

            // Check application resources
            if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue(resourceKey, out resource) &&
                resource is Microsoft.Maui.Graphics.Color appColor)
            {
                return appColor;
            }

            // Default to transparent
            return Microsoft.Maui.Graphics.Colors.Transparent;
        }

        /// <summary>
        /// Clears the cache and forces reinitialization.
        /// Useful when changing themes or for testing.
        /// </summary>
        public static void ClearCache()
        {
            _colorCache.Clear();
            _isInitialized = false;
        }
    }
}