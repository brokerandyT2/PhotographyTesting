using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Photography.Business.GoogleLoginService.Interface;
using Plugin.Maui.GoogleClient;
namespace Location.Photography.Business.GoogleLoginService
{
    public class GoogleLoginService : IGoogleLoginService
    {
        public GoogleLoginService()
        {
        }
        public async Task<GoogleResponse<GoogleUser>> Login()
        {
            var result = await CrossGoogleClient.Current.LoginAsync();
            return result;
        }
    }
}
