using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.DTO
{
    public class TipsDisplayDTO : TipDTO, ITipDisplayDTO
    {
        //public event PropertyChangedEventHandler? PropertyChanged;
        public TipsDisplayDTO() { }
        private List<ViewModels.Interface.ITipType> Tips { get; set; }

    }
}
