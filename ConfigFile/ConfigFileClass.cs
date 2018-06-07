using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace ConfigFile
{
    public class Config
    {
        //our path to the used cfg (if we load a cfg, or in case of invoking method to save without a path)
        public string cfgPath;

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

        //Tries to add a value, if cant it will delete it, it will return false if there was something invalid
        public bool SafeAdd(Field field)
        {
            Values.Add(field);
            if (CheckForValuesToRemove())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Used to make sure there are no unvalid values added, return false when something need to be deleted
        private bool CheckForValuesToRemove()
        {
            List<Field> deletelist = new List<Field>();
            foreach (Field field in Values)
            {
                if (field.ToRemove)
                {
                    deletelist.Add(field);
                }
            }
            if (deletelist.Count > 0)
            {
                foreach (Field todelete in deletelist)
                {
                    int index = Values.IndexOf(todelete);
                    if (index >= 0)
                    {
                        Values.RemoveAt(index);
                    }
                }
                return false;
            }
            return true;
        }
        public void ResetToDefaults()
        {
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
                System.IO.FileInfo file = new System.IO.FileInfo(cfgPath);
                if (!System.IO.File.Exists(cfgPath))  //if cfg file exists
                {
                    file.Directory.Create();
                }

                string output = String.Empty;
                Field last = Values.Last();
                foreach (Field field in Values)
                {
                    if (field.Equals(last))
                    {
                        output += field.ToString();
                    }
                    else
                    {
                        output += field.ToString() + "\r\n";
                    }
                }
                System.IO.File.WriteAllLines(file.FullName, new[] { output }, Encoding.UTF8);
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
            string oldpath = cfgPath;
            cfgPath = path;
            if (SaveCfg())
            {
                return true;
            }
            else
            {
                cfgPath = oldpath;
                return false;
            }
        }
        //Load from a defined path
        public bool LoadCfg(string path)
        {
            string oldpath = cfgPath;
            cfgPath = path;
            if(LoadCfg())
            {
                return true;
            }
            else
            {
                cfgPath = oldpath;
                return false;
            }
        }
        //Here we try to load and validate file
        public bool LoadCfg()
        {
            try
            {
                Values.Clear();
                if (System.IO.File.Exists(cfgPath))  //if cfg file exists
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(cfgPath);
                    string[] loaded = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8);
                    foreach (string line in loaded)
                    {
                        Values.Add(new Field(line));
                    }
                    CheckForValuesToRemove();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //Checks whether we have missing variables from Defaults config, if fix is true, it fixes them.
        public bool CheckForMissingVars(bool fixIt)
        {
            bool missing = false;
            foreach(Field fielddefault in Defaults)
            {
                bool found = false;
                foreach(Field field in Values)
                {
                    if(fielddefault.Name == field.Name && fielddefault.Type == field.Type)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    missing = true;
                    if (fixIt)
                    {
                        Values.Add(fielddefault);
                    }
                    else
                    {
                        return !missing;
                    }
                }
            }
            return !missing;
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
            return "[" + Name.ToString() + "]=\"" + Value.ToString() + "\"{" + Type.AssemblyQualifiedName + "}";
        }
        //TODO: Add more conversion support as needed!
        private void ConvertFrom(string value)
        {
            try
            {
                if (this.Type.ToString().Contains("System.Double"))
                {
                    Value = Double.Parse(value);
                    return;
                }
                else if (this.Type.ToString().Contains("System.Int32"))
                {
                    Value = Int32.Parse(value);
                    return;
                }
                else if (this.Type.ToString().Contains("System.Windows.Media.Color"))
                {
                    Value = (Color)ColorConverter.ConvertFromString(value);
                    return;
                }
                else
                {
                    //If the type is not recognized, we just go and save it as string, let programmer manage the type by himself
                    Value = value;
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
                var regex = Regex.Match(line, "^\\[(\\S+)\\](?:\\s*)=(?:\\s*)\"(.+)\"(?:\\s*){(.+)}$");

                Name = regex.Groups[1].Value;
                Type = Type.GetType(regex.Groups[3].Value);
                //If we cant recognize the type, just make it a string and let developer convert himself or he can just set the type himself
                if(Type == null)
                {
                    Type = Type.GetType("System.String");
                }
                ConvertFrom(regex.Groups[2].Value);
            }
            catch (Exception)
            {
                ToRemove = true;
            }
        }
    }
}