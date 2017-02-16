using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public class LocalizationEnumValueAttribute:EnumValueAttribute
    {
        public LocalizationEnumValueAttribute(string value)
            : base(value)
        {

        }
        public override string Value
        {
            get
            {
                return base.Value;
            }
        }
    }
}
