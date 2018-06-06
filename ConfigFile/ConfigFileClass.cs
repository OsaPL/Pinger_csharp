using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;

namespace ConfigFile
{
    public class Config
    {
        //our path to the used cfg (if we load a cfg, or in case of invoking method to save without a path)
        string cfgPath;

        //Enumarable list of all values
        public List<Field> Values;
        public IEnumerator<Field> GetEnumerator()
        {
            foreach (var value in Values)
                yield return value;
        }

        public Config Defaults;
        //Here we can use (if we want) a default config, that will be used for everything
        public Config(string path, Config Defaults)
        {
            this.Defaults = Defaults;
            this.cfgPath = path;
            PrepareConfig();
        }

        public Config(Config Defaults)
        {
            this.Defaults = Defaults;
            PrepareConfig();
        }

        //We try to load it, if not just create it
        public Config(string path)
        {
            this.cfgPath = path;
            PrepareConfig();
        }
        //Get default values if no file provided
        public Config()
        {
            PrepareConfig();
        }
        //do everything to prepare config (this is an extended constructor)
        private void PrepareConfig()
        {
            if (String.IsNullOrEmpty(cfgPath))
            {
                cfgPath = AppDomain.CurrentDomain.BaseDirectory + @"/cfg.cfg";
            }

            Values = new List<Field>();

            //try to loadfile, if we cant load it, create it at our path
            //if defaults config is not null, we generate default fields and values
        }

        //Tries to add a value, if cant it will delete it
        public void SafeAdd(Field field)
        {
            Values.Add(field);
            CheckForValuesToRemove();
        }
        //Used to make sure there are no unvalid values added
        private void CheckForValuesToRemove()
        {
            List<Field> deletelist = new List<Field>();
            foreach (Field field in Values)
            {
                if (field.ToRemove)
                {
                    deletelist.Add(field);
                }
            }
            foreach (Field todelete in deletelist)
            {
                int index = Values.IndexOf(todelete);
                if (index >= 0)
                {
                    Values.RemoveAt(index);
                }
            }
        }
        public void ResetToDefaults()
        {
            if (Defaults == null)
                Defaults = new Config();

            this.Values = Defaults.Values;

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
                if (!System.IO.File.Exists(cfgPath))  //if cfg file exists
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(cfgPath);
                    file.Directory.Create();
                }
                //System.IO.File.WriteAllLines(file.FullName, settings.ToArray(), Encoding.UTF8);
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
            cfgPath = path;
            return SaveCfg();
        }

    }

    public class Field
    {
        //Dictonary class forces us to use one type casts, we dont want that! So we just create lists.
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }

        public bool ToRemove { get; set; }

        public Field()
        {
            ToRemove = false;
            Name = "N/A";
            Value = "None";
            Type = Value.GetType();
        }
        public Field(string name, object value)
        {
            ToRemove = false;
            Name = name;
            Value = value;
            Type = Value.GetType();
        }
        public Field(string line)
        {
            ToRemove = false;
            ReadValueFromLine(line);
        }
        public override string ToString()
        {
            return "[" + Name.ToString() + "]=\"" + Value.ToString() + "\"{" + Type.ToString() + "}";
        }

        private void ConvertFrom(string value)
        {
            try
            {
                if (this.Type.ToString() == "System.String")
                {
                    Value = value;
                    return;
                }
                else if (this.Type.ToString() == "System.Double")
                {
                    Value = Double.Parse(value);
                    return;
                }
                else if (this.Type.ToString() == "System.Int32")
                {
                    Value = Int32.Parse(value);
                    return;
                }
            }
            catch (Exception)
            {
                ToRemove = true;
            }
            ToRemove = true;
        }

        public void ReadValueFromLine(string line)
        {
            try
            {
                var regex = Regex.Match(line, "^\\[(\\S+)\\]=\"(\\S+)\"{(\\S+)}$");

                Name = regex.Groups[1].Value;
                Type = Type.GetType(regex.Groups[3].Value);
                ConvertFrom(regex.Groups[2].Value);
            }
            catch (Exception)
            {
                ToRemove = true;
            }
        }
    }
}