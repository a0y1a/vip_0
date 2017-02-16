using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeKeSoftPlatform.Common
{
    public class DisplayOnlyAttribute:Attribute
    {
        public bool HideValue { get; set; }
        public DisplayOnlyAttribute()
        {
            HideValue = true;
        }
    }
}
