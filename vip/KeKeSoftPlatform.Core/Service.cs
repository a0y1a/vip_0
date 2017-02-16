using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeKeSoftPlatform.Common;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Security.Policy;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using System.Web;
using System.Security.Cryptography;

namespace KeKeSoftPlatform.Core
{
    public class Service
    {
        public const string CAPTCHA = "__CAPTCHA";
        public const string ActionSuccess = "操作成功";
        public const string ActionFailure = "操作失败";
        public static string CreateCaptcha()
        {
            Captcha captcha = new Captcha();
            return captcha.CreateCaptcha(4);
        }

        /// <summary>
        /// 验证二代身份证号正确性
        /// </summary>
        /// <param name="idNum"></param>
        /// <returns></returns>
        public static bool CheckIdNumber(string idNum)
        {
            //判断是否是18位
            if (idNum.ToString().Trim().Replace(" ", "").Length != 18)
            {
                return false;
            }
            //判断是否含有全角字符
            if (Regex.IsMatch(idNum.ToString().Trim(), @"[^\x00-\xff]"))
            {
                return false;
            }
            //判断前17位是否是数字
            if (!Regex.IsMatch(idNum.ToString().Trim().Substring(0, 17), @"^\d{17}$"))
            {
                return false;
            }
            int[] iW = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
            int iSum = 0;
            var v_card = idNum;
            for (var i = 0; i < 17; i++)
            {
                var iC = v_card.Substring(i, 1);
                var iVal = Convert.ToInt32(iC);
                iSum += iVal * iW[i];
            }
            var iJYM = iSum % 11;
            var sJYM = "";
            if (iJYM == 0) sJYM = "1";
            else if (iJYM == 1) sJYM = "0";
            else if (iJYM == 2) sJYM = "x";
            else if (iJYM == 3) sJYM = "9";
            else if (iJYM == 4) sJYM = "8";
            else if (iJYM == 5) sJYM = "7";
            else if (iJYM == 6) sJYM = "6";
            else if (iJYM == 7) sJYM = "5";
            else if (iJYM == 8) sJYM = "4";
            else if (iJYM == 9) sJYM = "3";
            else if (iJYM == 10) sJYM = "2";
            var cCheck = v_card.Substring(17, 1).ToLower();
            if (cCheck != sJYM)
            {
                return false;
            }
            return true;
        }
    }
}
