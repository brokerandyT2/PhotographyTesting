using Locations.Core.Shared.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Location.Photography.Shared.ViewModels.Interfaces
{
    public interface ISceneEvaluation
    {
        // Properties for histograms
        string RedHistogramImage { get; set; }
        string BlueHistogramImage { get; set; }
        string GreenHistogramImage { get; set; }
        string ContrastHistogramImage { get; set; }

        // UI state properties
       

        // Commands
        ICommand EvaluateCommand { get; }

        // Events
        event EventHandler<OperationErrorEventArgs> ErrorOccurred;
    }
}