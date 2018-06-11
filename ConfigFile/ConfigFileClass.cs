using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace ConfigFile
{
    public class Config
    {
        //our path to the used cfg (if we load a cfg, or in case of invoking method to save without a path)
        public string cfgPath;

        //Secure mode for the cfg
        public bool Secure;
        //DANGEROUS! Allows notsecure cfg file loading. You can load nonsecured cfgs and still keep secure tag.
        public bool IgnoreIfNonSecure;

        public ExceptionCollector ExceptionStack;

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

            Secure = false;
            IgnoreIfNonSecure = false;

            ExceptionStack = new ExceptionCollector();
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
            if(Defaults == null)
            {
                Defaults = new Config();
            }
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
        public void Encrypt()
        {
            string password = @"qZRwSvsTdc=!RESxnNB8BAnmnAwKzWX3km9rACu#?X38VxT7$u6Jyyu+23G2-@rQ=vMCgG3%-XMr%kTbX6!_#qN4Td$d=9EQb$nf&K%z8bT^NyhpE3qTqgenU?$h^KmRGMb^yHZ=BzU%3PB8KA_7kN8?DUSr4pMr8cwys9n=FW6ULWDnx9T4wEb-+_AG?fYQ3aV+gdc4!9GULa+#mD82?w!Sc+rQzX7G#jdMxJzZf$3vLSUe3?FCCSv@_ynZdvzMsPR3j@Kn#^8MqZ%!Lv3*XpJY7Ew@BCbQDD%^tLB5TgTa+!?fUY*$cb&rdTUf$Hhbmw5&5tGMvSk%JyAc_AXSv&+*Dqk_C6EGrYM#x5QrNz*ZBLebZUzch@DcebJ+LW%w3kbrUkCXbzM&UF2D&aHXsev+BJ&-NDFF=dNpT48_5Fr^+8q^dECBXrMm5wtLhn3-y4X=$fnaBra8vy2ANAPTDjFb$?yWQD-rT-ncKgwRH9v$N-HUm%ET@tdJHU9H9@wUJMD?yq+?Rja*!J3XFQjCs=ChVCdSGCaSXFT&gXTVGuc5beVpxVEDvt8QV@STBt4!3hm?Yct#sY_#S2E-r22_7LN7Jj7W@GTCnUZKJTMS&sr3%bCeUmBC^+saxT*+-Ngha4FDMm96e3MMaa?t$^5nRcs9h#WPEMM2wpU3kBtzwR#4wMQfHDH-N7@L9cZK_A*tDCVV4F*KC=L+EsyfaeqAKCtNn&7pd*-EKnex5uZ$mBA!9TvG6pcB+=^tA8jynD9F9y%WQGg%LZ-4Ykjhz8UyTChU32sz8qM?d-L9wmeU57$Z=?yjJ2hk%E^Zg5Ea@Vy=tcm$T-jSttR8^?_nUT@YxQxFsLD@Ft=maFLGGy^Dz-Tz%p9Zdkz8dvYv!EBP!@uSptY8x5AuSdQNeWaf!q@@P4QxGXDbNVdrmATaXDxWM-dnhV&*p*Fb?^T9@2fNw6Ac^Ausj-xmL%bMA6w6_v3!@x%D@H56cx2X$_^^xQhBJSw@U#+n%W6Le8VFvmqkhF!^6+XQX4x%M*_PSagc6Q!Ddj?t&+R6=8Y2777RYc=njwSQh%sDy@%F#fN#Hqf4v!_Hu4QndQQTbhEvRdprdn3&K9DjpNcZpnqs2DfrFp35*7qnrJupnBFbW_%n!N+fT87e9KP_nK!+vD8=ETU-8jvgsJfsRQ+SeqEmzbWcq3Qx!3gEVtSzT^%257Uma%FXExNjpTy#%N@pDxue4NKZsnQ#L&LUg*wPgLfV+H%acrB24-Y4w_+Lka^3vHTh^Vp3c2kh8+TE?3#=_K_WLsMAxh^MFy6gGQw_!mZPW%W$Y=N7Yv5XMs8My$_6CZ&*-LNWfE2sK-8HN?@m@d4uqWVjcWyhL-TYU7Tg^3*#LvGdTFBumcuxjhJhNmMyCejraKSn^$=wKUu%pBnq!$&@c9*X36fD8cBdmcuxe9@YRbK%KtbQQ^vtZP+pjzx?y?#c9%&yr#VMfdmxsfn8KR3-b8aEG^EH44*NtA9WeTx-?nf%6tv*nszm$j?x5!&+Z?uUn^5VYJJKZ!?H9zVKrp9pHx89rGQAWrEG8XUEC=BVA_&8+Y=G5G?VR9JuHQzuNKLZqF33fGcYRHy*FS%ppg%5HDZV42?SzEhEKZk7cQHmZuJg9FGqGp&9mQ%vN7jEg?5st8-QHq9nA2q9Hm6HwvYEF-z5F64X!AjY#6%hduK7_Yd6Y-*62LqJr8s3$J9QwbhCjtwaF3tyytcmHdR$UHfJ?zLNSB?jjTwzY@*Gw+h2=*&czBXPPDa=D$DNS3BS83_BE?+V9d*CJsZV*QLa3#b@TStfXu973u*DYsC_P7?P2x9azffcgzk&#!sMmQ$Dy!n?S2!-vEca_A3GX8D$dHd+ZZ%nLkXZZuWWwbX87jZw2=2PMS@Va_ms3xH*aLMFnamVS6=Z*#NdnG-$g#=xuTJyhZyuK@&8?wNfnr?JYggB729t82SFs*2jABQB@4nhNkLr^vNV_Zq#r%#BBXD@B?nAgkr#p#t7&fD?WWCF&hN=8@sN2jes#nNr45$64#JsPR7ZDe_Ac=mQ";

            // For additional security Pin the password of your files
            GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);

            Encryption Encrypt = new Encryption();
            // Encrypt the file
            if (Encrypt.FileEncrypt(cfgPath, password))
            {
                File.Delete(cfgPath);
                File.Move(cfgPath + ".aes", cfgPath);
            }
            else
            {
                // Security flag gets cleared when its misused
                if (!IgnoreIfNonSecure)
                    Secure = false;
            }

            // To increase the security of the encryption, delete the given password from the memory !
            Encryption.ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
            gch.Free();

            ExceptionStack.AddStack(Encrypt.ExceptionStack.Stack);
        }
        public bool Decrypt()
        {
            bool success = false;
            string password = @"qZRwSvsTdc=!RESxnNB8BAnmnAwKzWX3km9rACu#?X38VxT7$u6Jyyu+23G2-@rQ=vMCgG3%-XMr%kTbX6!_#qN4Td$d=9EQb$nf&K%z8bT^NyhpE3qTqgenU?$h^KmRGMb^yHZ=BzU%3PB8KA_7kN8?DUSr4pMr8cwys9n=FW6ULWDnx9T4wEb-+_AG?fYQ3aV+gdc4!9GULa+#mD82?w!Sc+rQzX7G#jdMxJzZf$3vLSUe3?FCCSv@_ynZdvzMsPR3j@Kn#^8MqZ%!Lv3*XpJY7Ew@BCbQDD%^tLB5TgTa+!?fUY*$cb&rdTUf$Hhbmw5&5tGMvSk%JyAc_AXSv&+*Dqk_C6EGrYM#x5QrNz*ZBLebZUzch@DcebJ+LW%w3kbrUkCXbzM&UF2D&aHXsev+BJ&-NDFF=dNpT48_5Fr^+8q^dECBXrMm5wtLhn3-y4X=$fnaBra8vy2ANAPTDjFb$?yWQD-rT-ncKgwRH9v$N-HUm%ET@tdJHU9H9@wUJMD?yq+?Rja*!J3XFQjCs=ChVCdSGCaSXFT&gXTVGuc5beVpxVEDvt8QV@STBt4!3hm?Yct#sY_#S2E-r22_7LN7Jj7W@GTCnUZKJTMS&sr3%bCeUmBC^+saxT*+-Ngha4FDMm96e3MMaa?t$^5nRcs9h#WPEMM2wpU3kBtzwR#4wMQfHDH-N7@L9cZK_A*tDCVV4F*KC=L+EsyfaeqAKCtNn&7pd*-EKnex5uZ$mBA!9TvG6pcB+=^tA8jynD9F9y%WQGg%LZ-4Ykjhz8UyTChU32sz8qM?d-L9wmeU57$Z=?yjJ2hk%E^Zg5Ea@Vy=tcm$T-jSttR8^?_nUT@YxQxFsLD@Ft=maFLGGy^Dz-Tz%p9Zdkz8dvYv!EBP!@uSptY8x5AuSdQNeWaf!q@@P4QxGXDbNVdrmATaXDxWM-dnhV&*p*Fb?^T9@2fNw6Ac^Ausj-xmL%bMA6w6_v3!@x%D@H56cx2X$_^^xQhBJSw@U#+n%W6Le8VFvmqkhF!^6+XQX4x%M*_PSagc6Q!Ddj?t&+R6=8Y2777RYc=njwSQh%sDy@%F#fN#Hqf4v!_Hu4QndQQTbhEvRdprdn3&K9DjpNcZpnqs2DfrFp35*7qnrJupnBFbW_%n!N+fT87e9KP_nK!+vD8=ETU-8jvgsJfsRQ+SeqEmzbWcq3Qx!3gEVtSzT^%257Uma%FXExNjpTy#%N@pDxue4NKZsnQ#L&LUg*wPgLfV+H%acrB24-Y4w_+Lka^3vHTh^Vp3c2kh8+TE?3#=_K_WLsMAxh^MFy6gGQw_!mZPW%W$Y=N7Yv5XMs8My$_6CZ&*-LNWfE2sK-8HN?@m@d4uqWVjcWyhL-TYU7Tg^3*#LvGdTFBumcuxjhJhNmMyCejraKSn^$=wKUu%pBnq!$&@c9*X36fD8cBdmcuxe9@YRbK%KtbQQ^vtZP+pjzx?y?#c9%&yr#VMfdmxsfn8KR3-b8aEG^EH44*NtA9WeTx-?nf%6tv*nszm$j?x5!&+Z?uUn^5VYJJKZ!?H9zVKrp9pHx89rGQAWrEG8XUEC=BVA_&8+Y=G5G?VR9JuHQzuNKLZqF33fGcYRHy*FS%ppg%5HDZV42?SzEhEKZk7cQHmZuJg9FGqGp&9mQ%vN7jEg?5st8-QHq9nA2q9Hm6HwvYEF-z5F64X!AjY#6%hduK7_Yd6Y-*62LqJr8s3$J9QwbhCjtwaF3tyytcmHdR$UHfJ?zLNSB?jjTwzY@*Gw+h2=*&czBXPPDa=D$DNS3BS83_BE?+V9d*CJsZV*QLa3#b@TStfXu973u*DYsC_P7?P2x9azffcgzk&#!sMmQ$Dy!n?S2!-vEca_A3GX8D$dHd+ZZ%nLkXZZuWWwbX87jZw2=2PMS@Va_ms3xH*aLMFnamVS6=Z*#NdnG-$g#=xuTJyhZyuK@&8?wNfnr?JYggB729t82SFs*2jABQB@4nhNkLr^vNV_Zq#r%#BBXD@B?nAgkr#p#t7&fD?WWCF&hN=8@sN2jes#nNr45$64#JsPR7ZDe_Ac=mQ";

            // For additional security Pin the password of your files
            GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);

            Encryption Decrypt = new Encryption();
            // Decrypt the file
            if (Decrypt.FileDecrypt(cfgPath, cfgPath + ".temp", password))
            {
                success = true;
                File.Delete(cfgPath);
                File.Move(cfgPath + ".temp", cfgPath);
            }
            else
            {
                // Security flag gets cleared when its misused
                if (!IgnoreIfNonSecure)
                    Secure = false;
            }

            // To increase the security of the decryption, delete the used password from the memory !
            Encryption.ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
            gch.Free();


            // We do a cleanup
            try
            {
                File.Delete(cfgPath + ".temp");
            }
            catch (Exception ex)
            {
                ExceptionStack.Add(ex);
            }

            ExceptionStack.AddStack(Decrypt.ExceptionStack.Stack);

            return success;
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
                if (Secure)
                {
                    Encrypt();
                }
            }
            catch (Exception ex)
            {
                ExceptionStack.Add(ex);
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
            if (LoadCfg())
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
                if (Secure)
                {
                    Decrypt();
                }
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

                if (Secure)
                {
                    Encrypt();
                }

            }
            catch (Exception ex)
            {
                ExceptionStack.Add(ex);
                return false;
            }
            return true;
        }

        //Checks whether we have missing variables from Defaults config, if fix is true, it fixes them.
        public bool CheckForMissingVars(bool fixIt)
        {
            bool missing = false;
            foreach (Field fielddefault in Defaults)
            {
                bool found = false;
                foreach (Field field in Values)
                {
                    if (fielddefault.Name == field.Name && fielddefault.Type == field.Type)
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
            if (Type.AssemblyQualifiedName.Contains("mscorlib"))
            {
                return "[" + Name.ToString() + "]=\"" + Value.ToString() + "\"\t{" + Type.ToString() + "}";
            }
            else
            {
                return "[" + Name.ToString() + "]=\"" + Value.ToString() + "\"\t{" + Type.AssemblyQualifiedName + "}";
            }
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
                if (Type == null)
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