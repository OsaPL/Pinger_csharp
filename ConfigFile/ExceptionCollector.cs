using System;
using System.Collections.Generic;

namespace ConfigFile
{
    /// <summary>
    /// Simple class to gather all exceptions into a stack
    /// </summary>
    public class ExceptionCollector
    {
        public List<object> Stack;
        public IEnumerator<object> GetEnumerator()
        {
            foreach (var exception in Stack)
                yield return exception;
        }
        public ExceptionCollector()
        {
            Stack = new List<object>();
        }
        
        public void Add(object Exception)
        {
            Stack.Add(new SingleException(Exception));
        }

        public void AddStack(List<object> ExceptionStack)
        {
            foreach(object exception in ExceptionStack)
            {
                Add(exception);
            }
        }
        public override string ToString()
        {
            string stack = string.Empty;
            foreach(object exception in Stack)
            {
                stack += exception.ToString() + "\r\n";
            }
            return stack;
        }
    }
    public class SingleException
    {
        object Exception;
        DateTime Occurred;
        public SingleException(object Exception)
        {
            this.Exception = Exception;
            Occurred = DateTime.Now;
        }

        public override string ToString()
        {
            return "["+Occurred+"] "+Exception.ToString();
        }
    }
}