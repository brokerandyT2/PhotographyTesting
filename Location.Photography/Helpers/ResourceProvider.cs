// Location.Photography/Helpers/ResourceProvider.cs
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Location.Photography.Helpers
{
    /// <summary>
    /// Central resource provider for shared UI resources across the Photography module.
    /// Manages color resources, opacity settings, and library-specific overrides.
    /// </summary>
    public static class ResourceProvider
    {
        // Cache for transparent colors
        private static Dictionary<string, Color> _colorCache = new Dictionary<string, Color>();

        // Flag to track if the provider has been initialized
        private static bool _isInitialized = false;

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
                    errorColorObj is Color)
                {
                    Color errorColor = (Color)errorColorObj;
                    _colorCache["TransparentErrorBrush"] = errorColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentErrorBrush"] = Color.FromArgb("#80FFEBEE");
                }

                // Black overlay color (black with 50% opacity)
                object overlayColorObj = null;
                if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                    Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("Black", out overlayColorObj) &&
                    overlayColorObj is Color)
                {
                    Color overlayColor = (Color)overlayColorObj;
                    _colorCache["TransparentOverlayBrush"] = overlayColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentOverlayBrush"] = Color.FromArgb("#80000000");
                }

                // Warning overlay color (yellow with 50% opacity)
                object warningColorObj = null;
                if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                    Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("WarningYellow", out warningColorObj) &&
                    warningColorObj is Color)
                {
                    Color warningColor = (Color)warningColorObj;
                    _colorCache["TransparentWarningBrush"] = warningColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentWarningBrush"] = Color.FromArgb("#80FFF9C4");
                }

                // Success overlay color (green with 50% opacity)
                object successColorObj = null;
                if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                    Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue("SuccessGreen", out successColorObj) &&
                    successColorObj is Color)
                {
                    Color successColor = (Color)successColorObj;
                    _colorCache["TransparentSuccessBrush"] = successColor.WithAlpha(0.5f);
                }
                else
                {
                    // Fallback if color not found
                    _colorCache["TransparentSuccessBrush"] = Color.FromArgb("#80E8F5E9");
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Error initializing transparent colors: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies standard resources to a page.
        /// This should be called from page constructors or OnAppearing methods.
        /// </summary>
        public static void ApplyStandardResources(ContentPage page)
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
        }

        /// <summary>
        /// Gets a transparent color by name.
        /// </summary>
        /// <param name="name">The name of the cached color resource</param>
        /// <returns>The color, or Colors.Transparent if not found</returns>
        public static Color GetColor(string name)
        {
            // Ensure initialization
            if (!_isInitialized)
                Initialize();

            if (_colorCache.TryGetValue(name, out var color))
                return color;

            return Colors.Transparent;
        }

        /// <summary>
        /// Gets a color resource from the application resources.
        /// </summary>
        /// <param name="resourceKey">The resource key</param>
        /// <returns>The color, or Colors.Transparent if not found</returns>
        public static Color GetResourceColor(string resourceKey)
        {
            object resourceObj = null;
            // Check application resources
            if (Microsoft.Maui.Controls.Application.Current?.Resources != null &&
                Microsoft.Maui.Controls.Application.Current.Resources.TryGetValue(resourceKey, out resourceObj) &&
                resourceObj is Color)
            {
                Color color = (Color)resourceObj;
                return color;
            }

            // Default to transparent
            return Colors.Transparent;
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