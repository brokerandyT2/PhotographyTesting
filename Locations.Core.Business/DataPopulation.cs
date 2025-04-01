using Locations.Core.Business.DataAccess;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Locations.Core.Shared.Enums.SubscriptionType;
using Microsoft.Maui.Platform;
using Newtonsoft.Json.Linq;

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
        public static void PopulateData()
        {

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

            TipTypesQuery<TipTypeViewModel> tipTypesQuery = new TipTypesQuery<TipTypeViewModel>();
            TipQuery<TipViewModel> tq = new TipQuery<TipViewModel>();
            int i = 1;
            foreach (var type in types)
            {
                var id = tipTypesQuery.SaveWithIDReturn(type);


                //var x = ;
                TipViewModel pt = new TipViewModel() { TipTypeID = id.Id, Fstop = "f/1", ISO = "50", Shutterspeed = "1/125", Content = "Text of the tip would appear here.  Zombie ipsum reversus ab viral inferno, nam rick grimes malum cerebro. De carne lumbering animata corpora quaeritis. Summus brains sit​​, morbo vel maleficia? De apocalypsi gorger omero undead survivor dictum mauris. Hi mindless mortuis soulless creaturas, imo evil stalking monstra adventus resi dentevil vultus comedat cerebella viventium. Qui animated corpse, cricket bat max brucks terribilem incessu zomby. The voodoo sacerdos flesh eater, suscitat mortuos comedere carnem virus. Zonbi tattered for solum oculi eorum defunctis go lum cerebro. Nescio brains an Undead zombies. Sicut malus putrid voodoo horror. Nigh tofth eliv ingdead.\r\n\r\nCum horribilem walking dead resurgere de crazed sepulcris creaturis, zombie sicut de grave feeding iride et serpens. Pestilentia, shaun ofthe dead scythe animated corpses ipsa screams. Pestilentia est plague haec decaying ambulabat mortuos. Sicut zeder apathetic malus voodoo. Aenean a dolor plan et terror soulless vulnerum contagium accedunt, mortui iam vivam unlife. Qui tardius moveri, brid eof reanimator sed in magna copia sint terribiles undeath legionis. Alii missing oculis aliorum sicut serpere crabs nostram. Putridi braindead odores kill and infect, aere implent left four dead.\r\n\r\n " + i.ToString(), Title = "How to Kill Zombies " + i.ToString() };
                var yyy = tq.SaveWithIDReturn(pt);// tq.SaveItem(pt);
                i++;
            }
            var loc = new LocationViewModel() { Lattitude = 39.7685, Longitude = 86.1580, Title = "Soldiers and Sailors Monument in Indianapolis In.", Description = "Located in the heart of downtown in Monument Circle, it was originally designed to honor Indiana’s Civil War veterans. It now commemorates the valor of Hoosier veterans who served in all wars prior to WWI, including the Revolutionary War, the War of 1812, the Mexican War, the Civil War, the Frontier Wars and the Spanish-American War. One of the most popular parts of the monument is the observation deck with a 360-degree view of the city skyline from 275 feet up.", Timestamp = DateTime.Now, Photo = "Resources/Images/s_and_sm_new.jpg" };
            var loc2 = new LocationViewModel() { Title = "The Bean", Description = "What is The Bean?\r\nThe Bean is a work of public art in the heart of Chicago. The sculpture, which is officially titled Cloud Gate, is one of the world’s largest permanent outdoor art installations. The monumental work was unveiled in 2004 and quickly became of the Chicago’s most iconic sights.", Lattitude = 41.8827, Longitude = 87.6233, Timestamp = DateTime.Now, Photo = "Resources/Images/chicagobean.jpg" };
            LocationsService ls = new LocationsService();
            var a = ls.SaveSettingWithObjectReturn(loc);
            var b = ls.SaveSettingWithObjectReturn(loc2);

            List<SettingViewModel> list = new List<SettingViewModel>();
            SettingsService ss = new SettingsService();
#if DEBUG

            SettingViewModel y = new() { Name = MagicStrings.SubscriptionType, Value = SubscriptionType.SubscriptionTypeEnum.Premium.Name() };
#elif RELEASE
            SettingViewModel y = new() { Name = MagicStrings.SubscriptionType, Value = SubscriptionType.SubscriptionTypeEnum.Free.Name() };
#endif
            var q = ss.SaveSettingWithObjectReturn(y);
            list.Add(new() { Name = MagicStrings.Hemisphere, Value = Hemisphere.HemisphereChoices.North.Name() });
            list.Add(new() { Name = MagicStrings.FirstName, Value = "" });
            list.Add(new() { Name = MagicStrings.LastName, Value = "" });
            list.Add(new() { Name = MagicStrings.Email, Value = "" });
            list.Add(new() { Name = MagicStrings.SubscriptionExpiration, Value = DateTime.Now.AddDays(-1).ToString() });
            list.Add(new() { Name = MagicStrings.UniqueID, Value = Guid.NewGuid().ToString() });
            list.Add(new() { Name = MagicStrings.HomePageViewed, Value = MagicStrings.False_string });
            list.Add(new() { Name = MagicStrings.LocationListViewed, Value = MagicStrings.False_string });
            list.Add(new() { Name = MagicStrings.TipsViewed, Value = MagicStrings.False_string });
            list.Add(new() { Name = MagicStrings.ExposureCalcViewed, Value = MagicStrings.False_string });
            list.Add(new() { Name = MagicStrings.LightMeterViewed, Value = MagicStrings.False_string });
            list.Add(new() { Name = MagicStrings.SceneEvaluationViewed, Value = MagicStrings.False_string });
            list.Add(new() { Name = MagicStrings.LastBulkWeatherUpdate, Value = DateTime.Now.AddDays(-2).ToString() });
            list.Add(new() { Name = MagicStrings.DefaultLanguage, Value = "en-US" });
            list.Add(new() { Name = MagicStrings.WindDirection, Value = MagicStrings.TowardsWind });
            list.Add(new() { Name = MagicStrings.CameraRefresh, Value = "1000" });
            list.Add(new() { Name = MagicStrings.AppOpenCounter, Value = "1" });
            list.Add(new() { Name = MagicStrings.TimeFormat, Value = MagicStrings.USTimeformat_Pattern });
            list.Add(new() { Name = MagicStrings.DateFormat, Value = MagicStrings.USDateFormat });
            list.Add(new() { Name = MagicStrings.WeatherURL, Value = "https://api.openweathermap.org/data/2.5/weather" });
            list.Add(new() { Name = MagicStrings.Weather_API_Key, Value = "aa24f449cced50c0491032b2f955d610" });

            //list.Add(new() { Name=MagicStrings.})



            foreach (var x in list)
            {
                var z = ss.SaveSettingWithObjectReturn(x);
            }


            // list.Add(new() { Name = MagicStrings.DeviceInfo, Value = Constants.DevInfo_string });
            var zz = ss.SaveSettingWithObjectReturn(new SettingViewModel() { Name = MagicStrings.DeviceInfo, Value = "" });
            //Add after saving as the Constants file is derrived from SettingsDTO
            // SettingViewModel m = new() { Name = MagicStrings.DeviceInfo, Value = Constants.DevInfo_string };
            SettingViewModel n = new() { Name = MagicStrings.TimeFormat, Value = Constants.TimeFormat_string };
            //ss.SaveSettingWithObjectReturn(m);
            ss.SaveSettingWithObjectReturn(n);
        }

    }
}

