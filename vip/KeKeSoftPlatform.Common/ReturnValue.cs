using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeKeSoftPlatform.Common
{
    public class ReturnValue
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }
        public ReturnValue()
        {
            IsSuccess = false;
        }
    }

    public class ReturnValue<T>
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
        public ReturnValue()
        {
            IsSuccess = false;
        }
    }
}
