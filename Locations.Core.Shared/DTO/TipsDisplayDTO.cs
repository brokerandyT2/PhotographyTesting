using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModels;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.ViewModels.Interface;

namespace Locations.Core.Shared.DTO
{
    public partial class TipsDisplayDTO : TipDTO, ITipDisplayDTO
    {
        [ObservableProperty]
        private List<ITipType> _tips = new List<ITipType>();

        public TipsDisplayDTO() { }
    }
}