using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.WebPages;

namespace KeKeSoftPlatform.Common
{
    public class Pager
    {
        public const string FIRST_PAGE_SIZE = "firstPageSize";
        public const string PAGE_NUM_KEY = "pageNum";

        public const int DEFAULT_PAGE_SIZE = 20;

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int ItemCount { get; set; }
        /// <summary>
        /// 首页多少条
        /// </summary>
        public int FirstPageSize { get; set; }
        /// <summary>
        /// 每页多少条
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页页码
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 一共多少页
        /// </summary>
        public int PageCount
        {
            get
            {
                if(FirstPageSize>=0)
                {
                    return (int)Math.Ceiling((double)(ItemCount+PageSize-FirstPageSize) / (double)PageSize);
                }
                return (int)Math.Ceiling((double)ItemCount / (double)PageSize);
            }
        }

        public Pager(int pageSize = Pager.DEFAULT_PAGE_SIZE)
        {
            this.PageSize = pageSize;
            this.FirstPageSize = -1;
        }

        /// <summary>
        /// 生成链接地址
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string GeneratePagerItem(HtmlHelper html, int pageNum)
        {
            return UrlBuilder.CreateInstance(html.ViewContext.RequestContext.HttpContext, html.ViewContext.RequestContext.RouteData)
                .Ignore(Pager.PAGE_NUM_KEY)
                .Attach(Pager.PAGE_NUM_KEY,pageNum)
                .Build();
        }
    }
    public class Pager<T> : Pager
    {
        /// <summary>
        /// 合计数据
        /// </summary>
        public T SumData { get;set;}
        public Pager<T> SetSumData(T sumData)
        {
            this.SumData = sumData;
            return this;
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        public IEnumerable<T> Data { get; set; }

    }
}
