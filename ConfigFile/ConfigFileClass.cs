using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigFile
{
    public class Config
    {
        //our path to the used cfg (if we load a cfg, or in case of invoking method to save without a path)
        string cfgPath;
        //Dictonary class forces us to use one type casts, we dont want that! So we just create lists.
        List<string> Names;
        List<Type> Types;
        List<object> Values;

        
        Config Defaults;
        //Here we can use (if we want) a default config, that will be used for everything
        Config(string path, Config Defaults)
        {
            this.Defaults = Defaults;
            this.cfgPath = path;
            PrepareConfig();
        }

        Config(Config Defaults)
        {
            this.Defaults = Defaults;
            PrepareConfig();
        }

        //We try to load it, if not just create it
        Config(string path)
        {
            this.cfgPath = path;
            PrepareConfig();
        }
        //Get default values if no file provided
        Config()
        {
            PrepareConfig();   
        }

        private void PrepareConfig()
        {
            //do everything to prepare config (this is an extended constructor)

            //try to loadfile, if we cant load it, create it at our path
            //if defaults config is not null, we generate default fields and values
        }
        public void GetValue(string Name, out object value)
        {
            value = null;
            //foreach line search for Name named value and try to take it out
            //then we get type of Value and the convert it to that type
            //object.GetType() == typeof(int)
            //Int32.Convert(object);
            //and so on...
        }
        public string GetValue(string Name)
        {
            string value = string.Empty;
            return value;
            //or we just ask for the string representation of the value *shrugs*
        }

        //Saves to the loaded cfgPath (or just default to anything) Lets return true if everything saves ok
        public bool SaveCfg()
        {
            try
            {
                //save
            }
            catch (Exception)
            {
                //could save cfg
                return false;
            }
            //success!
            return true;
        }

        //Here we can save at a defined path 
        public bool SaveCfg(string path)
        {
            try
            {
                //save
            }
            catch (Exception)
            {
                //could save cfg
                return false;
            }
            //success!
            return true;
        }

    }
}