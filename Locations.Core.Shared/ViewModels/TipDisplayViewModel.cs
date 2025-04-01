using Locations.Core.Shared.DTO;
using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels
{
    public class TipDisplayViewModel : TipsDisplayDTO, ITipDisplayViewModel
    {
        public TipDisplayViewModel() { }

        public List<TipTypeViewModel> Displays { get; set; }
        
    }
}
