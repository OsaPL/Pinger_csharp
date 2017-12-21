using System.IO;

namespace pinger_csharp
{
    public class ConfigFile
    {
        string cfgPath;
        ConfigFile(string path)
        {
            //path also includes file name
            if (Directory.Exists(Path.GetFullPath(path)))
            {
                cfgPath = path;
            }
            else
            {
                cfgPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            }
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
    }
}