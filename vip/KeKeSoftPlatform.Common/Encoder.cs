using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KeKeSoftPlatform.Common
{
    public abstract class EncoderProvider
    {
        #region 生成流水号
        public Int64 Next()
        {
            return Interlocked.Increment(ref seed);
        }
        private Int64 seed = Int64.Parse(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0"));
        #endregion

        private string _PrevFix;
        public string PrevFix { get { return _PrevFix; } }
        public EncoderProvider(string prevFix)
        {
            if (string.IsNullOrWhiteSpace(prevFix))
            {
                _PrevFix = "";
            }
        }

        public string NextToString()
        {
            return string.Format("{0}{1}", this._PrevFix, this.Next());
        }
    }
}
