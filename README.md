Base application is in Locations.Core.

Additonal "pages" should live in Locations.XXXXXXXX.YYYYYYY.  Where XXXXX is the type of app (Photography, hiking, fishing, etc). Where Y is the type of layer (ex: business, data).  This is to be accomplished through Maui Embedding . . . I don't have time to chase this down in order to be Architecturaly "perfect", so all Views live in Locations.Core.UI for the moment.

Conditional Compilation directives are used to keep builds small, but allow for everything to interrelate. Right now the only **CUSTOM** compiler directive that is fully defined and implemented is *#ifdef PHOTOGRAPHY*.  This in conjuction with *#ifdef RELEASE* for conditional compilation and setting of "values" for debugging / testing.  As an example:

        public static bool IsLoggedIn
        {
            get
            {
                try
                {
                #if PHOTOGRAPHY
                #if RELEASE
                    return ss.GetSettingByName(MagicStrings.Email).Value != string.Empty ? true : false;
                #else
                    return true;
                #endif
                #endif
                }
                ...
            }    
            
