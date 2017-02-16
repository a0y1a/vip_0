using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.WebPages;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace KeKeSoftPlatform.Common
{
    public static class PFExtension
    {
        #region Enum
        public static string EnumMetadataDisplay(this Enum value)
        {
            if (value.ToString() == "0")
            {
                return "";
            }
            var attribute = value.GetType().GetField(Enum.GetName(value.GetType(), value)).GetCustomAttributes(
                 typeof(EnumValueAttribute), false)
                 .Cast<EnumValueAttribute>()
                 .FirstOrDefault();
            if (attribute != null)
            {
                return attribute.Value;
            }

            return Enum.GetName(value.GetType(), value);
        }

        public static IEnumerable<SelectListItem> ToSelectDataSource(this Enum type)
        {
            return Enum.GetValues(type.GetType())
                        .Cast<Enum>()
                        .Select(m =>
                        {
                            string enumVal = Enum.GetName(type.GetType(), m);
                            return new SelectListItem()
                            {
                                Selected = (type.ToString() == enumVal),
                                Text = m.EnumMetadataDisplay(),
                                Value = enumVal
                            };
                        });
        }

        public static IEnumerable<SelectListItem> ToSelectDataSource(this Enum type, List<Enum> outEnum = null)
        {
            return Enum.GetValues(type.GetType())
                        .Cast<Enum>().Where(m => !outEnum.Contains(m))
                        .Select(m =>
                        {
                            string enumVal = Enum.GetName(type.GetType(), m);
                            return new SelectListItem()
                            {
                                Selected = (type.ToString() == enumVal),
                                Text = m.EnumMetadataDisplay(),
                                Value = enumVal
                            };
                        });
        }

        /// <summary>
        /// int转换为enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ToEnum<T>(this string obj) where T : struct
        {
            if (string.IsNullOrEmpty(obj))
            {
                return default(T);
            }
            try
            {
                return (T)Enum.Parse(typeof(T), obj, true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T EnumDisplayToEnum<T>(this string enumValue) where T : struct
        {
            if (string.IsNullOrEmpty(enumValue))
            {
                return default(T);
            }
            try
            {
                var type = typeof(T);
                var enumItem = Enum.GetValues(type)
                        .Cast<Enum>()
                        .First(m => m.EnumMetadataDisplay().Contains(enumValue));


                return (T)Enum.Parse(typeof(T), Enum.GetName(type, enumItem), true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T EnumValueToEnum<T>(this string enumValue) where T : struct
        {
            if (string.IsNullOrEmpty(enumValue))
            {
                return default(T);
            }
            try
            {
                var type = typeof(T);
                var enumItem = Enum.GetValues(type)
                        .Cast<Enum>()
                        .First(m => m.ToString() == enumValue);


                return (T)Enum.Parse(typeof(T), Enum.GetName(type, enumItem), true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }


        #endregion

        #region Enum Flag
        //是否存在权限
        public static bool Has<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        public static bool HasForAny<T>(this Enum type, T defaultValue, params T[] value)
        {
            try
            {
                var result = false;
                result = (((int)(object)type & (int)(object)defaultValue) == (int)(object)defaultValue);
                if (result)
                {
                    return result;
                }
                else
                {
                    if (value != null)
                    {
                        foreach (var item in value)
                        {
                            result = (((int)(object)type & (int)(object)item) == (int)(object)item);
                            if (result)
                            {
                                return result;
                            }
                        }
                    }
                }
                return result;
            }
            catch
            {
                return false;
            }
        }

        //判断权限
        public static bool Is<T>(this System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }
        //添加权限
        public static T Add<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "不能添加类型 '{0}'",
                        typeof(T).Name
                        ), ex);
            }
        }

        //移除权限
        public static T Remove<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "不能移除类型 '{0}'",
                        typeof(T).Name
                        ), ex);
            }
        }
        #endregion

        #region string 
        public static string NullOrValue(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                return value.Trim();
            }
        }

        public static string FormatString(this string value, params object[] p)
        {
            return string.Format(value, p);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 根据长度分割字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static List<string> Split(this string source, int length)
        {
            var result = new List<string>();
            for (int index = 0; index + length <= source.Length; index += length)
            {
                result.Add(source.Substring(index, length));
            }
            return result;
        }

        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool Equals(this string source, string value, bool ignoreCase = true)
        {
            return string.Compare(source, value, ignoreCase) == 0;
        }

        /// <summary>
        /// 转换字符串为bool类型
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBool(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            else
            {
                return bool.Parse(source);
            }
        }

        /// <summary>
        /// 根据附件重命名后的文件名，获取附件原名
        /// </summary>
        /// <param name="fuJianName"></param>
        /// <returns></returns>
        public static string GetOriginalName(this string fuJianName)
        {
            return fuJianName.Substring(0, fuJianName.Length - 19 - System.IO.Path.GetExtension(fuJianName).Length) + System.IO.Path.GetExtension(fuJianName);
        }

        #endregion

        #region IEnumerable
        public static IEnumerable<T> Each<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    action(item);
                }
            }
            return collection;
        }

        #endregion

        #region Image

        /// <summary>
        /// 图片转二进制流
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static byte[] ConvertImgToBase64(this string path)
        {
            string strbaser64 = "";
            Bitmap bmp = new Bitmap(path);
            MemoryStream ms = new MemoryStream();

            try
            {
                switch (System.IO.Path.GetExtension(path).ToLower())
                {
                    case ".gif":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case ".jpg":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".jpeg":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".bmp":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".emf":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Emf);
                        break;
                    case ".exif":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Exif);
                        break;
                    case ".icon":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Icon);
                        break;
                    case ".memorybmp":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.MemoryBmp);
                        break;
                    case ".tiff":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    case ".wmf":
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Wmf);
                        break;
                    default:
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            finally
            {
                bmp.Dispose();
                ms.Dispose();
            }

            return System.Text.Encoding.Default.GetBytes("data:image/jpg;base64," + strbaser64);
        }

        /// <summary>
        /// 根据文件后缀名，获取文件类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileType(this string fileName)
        {
            switch (fileName)
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".bmp":
                case ".dib":
                case ".tif":
                case ".tiff":
                    return "image";
                case ".avi":
                case ".rmvb":
                case ".rm":
                case ".asf":
                case ".divx":
                case ".mpg":
                case ".mpeg":
                case ".mpe":
                case ".mov":
                case ".wmv":
                case ".mp4":
                case ".mkv":
                case ".vob":
                case ".flv":
                    return "video";
                default:
                    return "other";
            }
        }

        #endregion
    }
}
