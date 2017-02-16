using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Web;
using System.Data;
using System.IO;
using KeKeSoftPlatform.Common;
using System.Xml.Serialization;
using FluentValidation;
using FluentValidation.Attributes;
using System.Text.RegularExpressions;

namespace KeKeSoftPlatform.Core
{
    [Validator(typeof(ResetPasswordConfigValidator))]
    public class ResetPasswordConfig
    {
        /// <summary>
        /// 县区用户默认密码
        /// </summary>
        public string UserCountyDefaultPwd { get; set; }
        /// <summary>街道办用户默认密码
        /// 街道办用户默认密码
        /// </summary>
        public string UserStreetDefaultPwd { get; set; }
        /// <summary>学校用户默认密码
        /// 学校用户默认密码
        /// </summary>
        public string UserSchooltDefaultPwd { get; set; }
    }
    public class ResetPasswordConfigValidator : AbstractValidator<ResetPasswordConfig>
    {
        public ResetPasswordConfigValidator()
        {
            RuleFor(m => m.UserCountyDefaultPwd)
                .Length(6, 12).WithMessage("6到12位，必须字母和数字")
                .NotEmpty().WithMessage("6到12位，必须字母和数字")
                .Matches(@"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,12}$").WithMessage("6到12位，必须字母和数字");
            RuleFor(m => m.UserStreetDefaultPwd)
                .Length(6, 12).WithMessage("6到12位，必须字母和数字")
                .NotEmpty().WithMessage("6到12位，必须字母和数字")
                .Matches(@"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,12}$").WithMessage("6到12位，必须字母和数字");
            RuleFor(m => m.UserSchooltDefaultPwd)
                 .Length(6, 12).WithMessage("6到12位，必须字母和数字")
                 .NotEmpty().WithMessage("6到12位，必须字母和数字")
                 .Matches(@"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,12}$").WithMessage("6到12位，必须字母和数字");
        }
    }

    public class SMSConfig
    {
        /// <summary>是否发送
        /// 是否发送
        /// </summary>
        public bool IsSendSms { get; set; }
    }

    public class Config
    {
        public static string PATH { get { return FilePath(); } }

        public static string FilePath()
        {
            string logFile = "App_Data/Config.xml";
            if (HttpContext.Current != null)
            {
                logFile = HttpContext.Current.Server.MapPath("~/" + logFile);
            }
            else
            {
                //多线程执行这里
                logFile = logFile.Replace("/", "\\");
                if (logFile.StartsWith("\\"))//确定 String 实例的开头是否与指定的字符串匹配。为下边的合并字符串做准备
                {
                    logFile = logFile.TrimStart('\\');//从此实例的开始位置移除数组中指定的一组字符的所有匹配项。为下边的合并字符串做准备
                }
                //AppDomain表示应用程序域，它是一个应用程序在其中执行的独立环境　　　　　　　
                //AppDomain.CurrentDomain 获取当前 Thread 的当前应用程序域。
                //BaseDirectory 获取基目录，它由程序集冲突解决程序用来探测程序集。
                //AppDomain.CurrentDomain.BaseDirectory综合起来就是返回此代码所在的路径
                //System.IO.Path.Combine合并两个路径字符串
                //Path.Combine(@"C:\11","aa.txt") 返回的字符串路径如后： C:\11\aa.txt
                logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFile);

            }
            return logFile;
        }
        public static Config Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResetPasswordConfig DefaultPwd { get; set; }
        public SMSConfig SMSConfig { get; set; }
        public void Serialize()
        {
            Instance = this;
            XmlHelper.XmlSerializeToFile(this, PATH, Encoding.UTF8);
        }

        static Config()
        {
            Instance = new Config();
            if (File.Exists(PATH))
            {
                Instance = XmlHelper.XmlDeserializeFromFile<Config>(PATH, Encoding.UTF8);
            }
            else
            {
                Instance.DefaultPwd = new ResetPasswordConfig()
                {
                    UserCountyDefaultPwd = "123q456",
                    UserSchooltDefaultPwd = "123456q",
                    UserStreetDefaultPwd = "q123456"
                };
                Instance.SMSConfig = new SMSConfig() { IsSendSms = false };
            }
        }

    }
}
