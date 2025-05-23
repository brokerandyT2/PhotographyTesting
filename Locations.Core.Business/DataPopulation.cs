﻿using EncryptedSQLite;
using Locations.Core.Business.DataAccess;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;
using Microsoft.VisualBasic;
using Nominatim.API.Web;
using System.Data.Common;
using System.Net.Mail;
using static Locations.Core.Shared.Enums.SubscriptionType;

namespace Locations.Core.Business
{
    public static class DataPopulation
    {
        private enum TipType
        {
            Landscape,
            Silouette,
            Building,
            Person,
            Baby,
            Animals

        }
        public static void PopulateData(
           string hemisphere, string tempformat, string dateformat, string timeformat, string winddirection, string email)

        {
            var _connection = DataEncrypted.GetAsyncConnection();
     

            var _alertServ = new Location.Core.Helpers.AlertService.EventAlertService();
            var _loggingServ = new Location.Core.Helpers.LoggingService.LoggerService(_connection);

            var _tipRepo = new Locations.Core.Data.Queries.TipRepository(_alertServ, _loggingServ);
            var _locRepo = new LocationRepository(_alertServ, _loggingServ);
            var _tipTypeRepo= new TipTypeRepository(_alertServ, _loggingServ);
            var _settingRepo = new SettingsRepository(_alertServ, _loggingServ);

            var _tipBusiness = new TipService<TipViewModel>(_tipRepo,_alertServ, _loggingServ);
            var _tipTypeBusiness = new TipTypeService<TipTypeViewModel>(_tipTypeRepo,_alertServ, _loggingServ);
            var _locBusiness = new LocationService<LocationViewModel>(_locRepo, _alertServ, _loggingServ);
            var _settingsBusiness = new SettingsService<SettingViewModel>(_settingRepo, _alertServ, _loggingServ);

            var con = new EncryptedSQLite.DataEncrypted();
;
            var aa = _connection.CreateTableAsync<SettingViewModel>().Result;
            var ba = _connection.CreateTableAsync<WeatherViewModel>().Result;
            var ca = _connection.CreateTableAsync<TipViewModel>().Result;
            var da = _connection.CreateTableAsync<TipTypeViewModel>().Result;
            var ea = _connection.CreateTableAsync<LocationViewModel>().Result;
            var fa = _connection.CreateTableAsync<Log>().Result;

            var guid = NativeStorageService.GetSetting(MagicStrings.UniqueID).ToString();


            SettingViewModel vm = new SettingViewModel();
            List<TipTypeViewModel> types = new List<TipTypeViewModel>();
            types.Add(new TipTypeViewModel { Name = "Landscape", I8n = MagicStrings.English_for_i8n });

            types.Add(new TipTypeViewModel { Name = "Silouette", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Building", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Person", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Baby", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Animals", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Blury Water", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Night", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Blue Hour", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Golden Hour", I8n = MagicStrings.English_for_i8n });
            types.Add(new TipTypeViewModel { Name = "Sunset", I8n = MagicStrings.English_for_i8n });
            
            int i = 0;
            foreach (var type in types)
            {
                var id = _tipTypeBusiness.SaveAsync(type);


                //var x = ;
                TipViewModel pt = new TipViewModel() { Id = id.Id, Apeture = "f/1", Iso = "50", Shutterspeed = "1/125", Description = "Text of the tip would appear here.  Zombie ipsum reversus ab viral inferno, nam rick grimes malum cerebro. De carne lumbering animata corpora quaeritis. Summus brains sit​​, morbo vel maleficia? De apocalypsi gorger omero undead survivor dictum mauris. Hi mindless mortuis soulless creaturas, imo evil stalking monstra adventus resi dentevil vultus comedat cerebella viventium. Qui animated corpse, cricket bat max brucks terribilem incessu zomby. The voodoo sacerdos flesh eater, suscitat mortuos comedere carnem virus. Zonbi tattered for solum oculi eorum defunctis go lum cerebro. Nescio brains an Undead zombies. Sicut malus putrid voodoo horror. Nigh tofth eliv ingdead.\r\n\r\nCum horribilem walking dead resurgere de crazed sepulcris creaturis, zombie sicut de grave feeding iride et serpens. Pestilentia, shaun ofthe dead scythe animated corpses ipsa screams. Pestilentia est plague haec decaying ambulabat mortuos. Sicut zeder apathetic malus voodoo. Aenean a dolor plan et terror soulless vulnerum contagium accedunt, mortui iam vivam unlife. Qui tardius moveri, brid eof reanimator sed in magna copia sint terribiles undeath legionis. Alii missing oculis aliorum sicut serpere crabs nostram. Putridi braindead odores kill and infect, aere implent left four dead.\r\n\r\n " + i.ToString(), Title = "How to Kill Zombies " + i.ToString() };
                var yyy = _tipBusiness.SaveAsync(pt);
                i++;
            }
            var loc = new LocationViewModel() { Lattitude = 39.7685, Longitude = -86.1580, Title = "Soldiers and Sailors Monument", Description = "Located in the heart of downtown in Monument Circle, it was originally designed to honor Indiana’s Civil War veterans. It now commemorates the valor of Hoosier veterans who served in all wars prior to WWI, including the Revolutionary War, the War of 1812, the Mexican War, the Civil War, the Frontier Wars and the Spanish-American War. One of the most popular parts of the monument is the observation deck with a 360-degree view of the city skyline from 275 feet up.", Timestamp = DateTime.Now.AddDays(-9), Photo = "Resources/Images/s_and_sm_new.jpg" };
            var loc2 = new LocationViewModel() { Title = "The Bean", Description = "What is The Bean?\r\nThe Bean is a work of public art in the heart of Chicago. The sculpture, which is officially titled Cloud Gate, is one of the world’s largest permanent outdoor art installations. The monumental work was unveiled in 2004 and quickly became of the Chicago’s most iconic sights.", Lattitude = 41.8827, Longitude = -87.6233, Timestamp = DateTime.Now.AddDays(-2), Photo = "Resources/Images/chicagobean.jpg" };
            var loc3 = new LocationViewModel() { Title = "Golden Gate Bridge", Description = "The Golden Gate Bridge is a suspension bridge spanning the Golden Gate strait, the one-mile-wide (1.6 km) channel between San Francisco Bay and the Pacific Ocean. The strait is the entrance to San Francisco Bay from the Pacific Ocean. The bridge connects the city of San Francisco, California, to Marin County, carrying both U.S. Route 101 and California State Route 1 across the strait.", Lattitude = 37.8199, Longitude = -122.4783, Timestamp = DateTime.Now.AddDays(-6), Photo = "Resources/Images/ggbridge.jpg" };

            var loc4 = new LocationViewModel() { Title = "Gateway Arch", Description = "The Gateway Arch is a 630-foot (192 m) monument in St. Louis, Missouri, that commemorates Thomas Jefferson and the westward expansion of the United States. The arch is the centerpiece of the Gateway Arch National Park and is the tallest arch in the world.", Lattitude = 38.6247, Longitude = -90.1848, Timestamp = DateTime.Now.AddDays(-35), Photo = "Resources/Images/stlarch.jpg" };
            
            _locBusiness.SaveAsync(loc);
            _locBusiness.SaveAsync(loc2);
            _locBusiness.SaveAsync(loc3);
            _locBusiness.SaveAsync(loc4);




            List<SettingViewModel> list = new List<SettingViewModel>();








            #region DEFAULT DATA No Matter if Debug or not
            list.Add(new() { Key = MagicStrings.Hemisphere, Value = hemisphere });
            list.Add(new() { Key = MagicStrings.FirstName, Value = "" });
            list.Add(new() { Key = MagicStrings.LastName, Value = "" });
            list.Add(new() { Key = MagicStrings.UniqueID, Value = guid });
            list.Add(new() { Key = MagicStrings.LastBulkWeatherUpdate, Value = DateTime.Now.AddDays(-2).ToString() });
            list.Add(new() { Key = MagicStrings.DefaultLanguage, Value = "en-US" });
            list.Add(new() { Key = MagicStrings.WindDirection, Value = winddirection });
            list.Add(new() { Key = MagicStrings.CameraRefresh, Value = "2000" });
            list.Add(new() { Key = MagicStrings.AppOpenCounter, Value = "1" });
            list.Add(new() { Key = MagicStrings.TimeFormat, Value = timeformat });
            list.Add(new() { Key = MagicStrings.DateFormat, Value = dateformat });
            list.Add(new() { Key = MagicStrings.WeatherURL, Value = "https://api.openweathermap.org/data/3.0/onecall" });
            list.Add(new() { Key = MagicStrings.Weather_API_Key, Value = "aa24f449cced50c0491032b2f955d610" });
            list.Add(new() { Key = MagicStrings.FreePremiumAdSupported, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.TemperatureType, Value = MagicStrings.Fahrenheit });
            list.Add(new() { Key = MagicStrings.DeviceInfo, Value = "" });
            list.Add(new() { Key = MagicStrings.Email, Value = email });
            #endregion




#if DEBUG


            list.Add(new() { Key = MagicStrings.SettingsViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.HomePageViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.LocationListViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.TipsViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.ExposureCalcViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.LightMeterViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.SceneEvaluationViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.AddLocationViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.WeatherDisplayViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.SunCalculatorViewed, Value = MagicStrings.True_string });
            list.Add(new() { Key = MagicStrings.ExposureCalcAdViewed_TimeStamp, Value = DateTime.Now.ToString() });
            list.Add(new() { Key = MagicStrings.LightMeterAdViewed_TimeStamp, Value = DateTime.Now.ToString() });
            list.Add(new() { Key = MagicStrings.SceneEvaluationAdViewed_TimeStamp, Value = DateTime.Now.ToString() });
            list.Add(new() { Key = MagicStrings.SunCalculatorViewed_TimeStamp, Value = DateTime.Now.ToString() });
            list.Add(new() { Key = MagicStrings.SunLocationAdViewed_TimeStamp, Value = DateTime.Now.ToString() });
            list.Add(new() { Key = MagicStrings.WeatherDisplayAdViewed_TimeStamp, Value = DateTime.Now.ToString() });
            list.Add(new() { Key = MagicStrings.SubscriptionType, Value = SubscriptionType.SubscriptionTypeEnum.Premium.Name() });
            list.Add(new() { Key = MagicStrings.SubscriptionExpiration, Value = DateTime.Now.AddDays(100).ToString() });
            list.Add(new SettingViewModel() { Key = MagicStrings.AdGivesHours, Value = "24" });
#else

            list.Add(new() { Key = MagicStrings.SettingsViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.HomePageViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.LocationListViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.TipsViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.ExposureCalcViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.LightMeterViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.SceneEvaluationViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.AddLocationViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.WeatherDisplayViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.SunCalculatorViewed, Value = MagicStrings.False_string });
            list.Add(new() { Key = MagicStrings.Email, Value = "" });    
            list.Add(new() { Key = MagicStrings.ExposureCalcAdViewed_TimeStamp, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add(new() { Key = MagicStrings.LightMeterAdViewed_TimeStamp, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add(new() { Key = MagicStrings.SceneEvaluationAdViewed_TimeStamp, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add(new() { Key = MagicStrings.SunCalculatorViewed_TimeStamp, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add(new() { Key = MagicStrings.SunLocationAdViewed_TimeStamp, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add(new() { Key = MagicStrings.WeatherDisplayAdViewed_TimeStamp, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add( new() { Key = MagicStrings.SubscriptionType, Value = SubscriptionType.SubscriptionTypeEnum.Free.Name() });
            list.Add(new() { Key = MagicStrings.SubscriptionExpiration, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add(new SettingViewModel() { Key = MagicStrings.AdGivesHours, Value = "12" });
#endif




            foreach (var x in list)
            {
                _settingsBusiness.SaveAsync(x);
            }

        }

    }
}

