﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;

namespace KeKeSoftPlatform.Common
{
    public class Grid
    {
        private Grid()
        {

        }

        public static Grid<T> Create<T>()
        {
            return new Grid<T>();
        }
    }

    public class Grid<T>
    {
        public const string KEY = "Id";

        public Grid()
        {
            _Columns = new List<GridColumn<T>>();
            this._EnableFilter = true;
        }

        private bool _EnableFilter;
        public Grid<T> EnableFilter(bool enable)
        {
            this._EnableFilter = enable;
            return this;
        }

        private object _HtmlAttributes;
        private bool _ReplaceExisting;
        public Grid<T> Attributes(object htmlAttributes, bool replaceExisting = false)
        {
            this._HtmlAttributes = htmlAttributes;
            this._ReplaceExisting = replaceExisting;
            return this;
        }

        #region 数据源
        private IEnumerable<T> _DataSource;
        public Grid<T> SetDataSource(IEnumerable<T> dataSource)
        {
            this._DataSource = dataSource;
            return this;
        }
        #endregion

        #region 合计数据源
        private T _SumData;
        public Grid<T> Sum(T sumData)
        {
            this._SumData = sumData;
            return this;
        }
        #endregion

        private List<GridColumn<T>> _Columns;


        #region 行主键
        private Func<T, object> _KeySelector;
        public Func<T, object> KeySelector { get { return _KeySelector; } }
        public Grid<T> Key(Func<T, object> keySelector)
        {
            this._KeySelector = keySelector;
            return this;
        }
        #endregion

        /// <summary>
        /// 生成DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ToTable()
        {
            var data = new DataTable();
            foreach (var item in _Columns)
            {
                data.Columns.Add(item.ColumnName);
            }
            if (this._DataSource != null && this._DataSource.Any())
            {
                for (int i = 0; i < _DataSource.Count(); i++)
                {
                    var row = data.NewRow();
                    for (int j = 0; j < _Columns.Count; j++)
                    {
                        var value = _Columns[j].ColumnValueCalculator(_DataSource.ElementAt(i));
                        row[j] = value == null ? "" : value.ToString();
                    }
                    data.Rows.Add(row);
                }
            }
            return data;
        }

        /// <summary>
        /// 输出html
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString Render()
        {
            var filter = new TagBuilder("div");
            filter.AddCssClass("filter-columns pull-right");
            var group = new TagBuilder("div");
            group.AddCssClass("btn-group");
            group.InnerHtml += "<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">"
            + "<i class=\"glyphicon glyphicon-th icon-th\"></i> "
             + "<span class=\"caret\"></span>"
        + "</button>";
            var dropdownMenuDiv = new TagBuilder("div");
            dropdownMenuDiv.MergeAttribute("style", "width: 200px; height:260px;max-height: 300px; overflow: auto;overflow-x: hidden; position: absolute; top: 100%; z-index: 1000;left:-288%;");
            dropdownMenuDiv.AddCssClass("dropdown-menu");
            var dropdownMenu = new TagBuilder("ul");
            //dropdownMenu.AddCssClass("dropdown-menu");
            dropdownMenu.MergeAttribute("style", "top: 0;list-style: none;padding: 5px 0;margin: 2px 0 0;");
            var index = 0;
            foreach (var column in this._Columns)
            {
                //if (column.ColumnName.Contains("tableCheck")==false)
                //{
                    if (column.EnableFilter && column.ColumnName.Contains("tableCheck") == false)
                    {
                        if (column.Visible)
                        {
                            dropdownMenu.InnerHtml += "<li><label><input type=\"checkbox\" value=\"filter-" + index + "\" checked=\"checked\"> " + column.ColumnName + "</label></li>";
                        }
                        else
                        {
                            dropdownMenu.InnerHtml += "<li><label><input type=\"checkbox\" value=\"filter-" + index + "\" > " + column.ColumnName + "</label></li>";
                        }
                    }
                    index += 1;
                //}
            }
            dropdownMenuDiv.InnerHtml += dropdownMenu;
            group.InnerHtml += dropdownMenuDiv;
            filter.InnerHtml += group;




            var table = new TagBuilder("table");
            table.MergeAttributes(new RouteValueDictionary(this._HtmlAttributes), this._ReplaceExisting);
            table.AddCssClass("table table-bordered");
            var thead = new TagBuilder("thead");
            var trForTitle = new TagBuilder("tr");
            trForTitle.Attributes.Add("style", "background:rgb(227,239,251)");

            index = 0;
            foreach (var column in this._Columns)
            {
                var th = new TagBuilder("th");
                th.Attributes.Add("style", "text-align:center;");
                th.MergeAttributes(new RouteValueDictionary(column.HtmlAttributes));
                th.AddCssClass("filter-" + index);
                if (column.Visible == false)
                {
                    th.AddCssClass("hidden");
                }
                index += 1;
                //th.SetInnerText(column.ColumnName);
                th.InnerHtml += column.ColumnName;
                trForTitle.InnerHtml += th;
            }

            thead.InnerHtml += trForTitle;
            table.InnerHtml += thead;

            var tbody = new TagBuilder("tbody");
            if (this._DataSource != null && this._DataSource.Any())
            {
                foreach (var item in this._DataSource)
                {
                    var trForData = new TagBuilder("tr");
                    if (_KeySelector != null)
                    {
                        var key = this._KeySelector(item);
                        if (key == null)
                        {
                            throw new Exception("没有找到键");
                        }
                        trForData.Attributes.Add("id", key.ToString());
                    }
                    index = 0;
                    foreach (var column in this._Columns)
                    {
                        var td = new TagBuilder("td");
                        RouteValueDictionary attributes = new RouteValueDictionary();
                        attributes.Add("style", "text-align:center;");
                        RouteValueDictionary add = new RouteValueDictionary(column.HtmlAttributes);
                        if (add.Count > 0)
                        {
                            foreach (var itemDic in add)
                            {
                                if (attributes.ContainsKey(itemDic.Key))
                                {
                                    attributes[itemDic.Key] = attributes[itemDic.Key].ToString() + itemDic.Value.ToString();
                                }
                                else
                                {
                                    attributes.Add(itemDic.Key, itemDic.Value);
                                }
                            }
                        }
                        //td.Attributes.Add("style", "text-align:center;");
                        //td.MergeAttributes(new RouteValueDictionary(column.HtmlAttributes));
                        td.MergeAttributes(attributes);
                        td.AddCssClass("filter-" + index);
                        if (column.Visible == false)
                        {
                            td.AddCssClass("hidden");
                        }
                        td.InnerHtml += column.ColumnValueCalculator(item);
                        trForData.InnerHtml += td;

                        index += 1;
                    }
                    tbody.InnerHtml += trForData;
                }
            }

            #region 合计
            if (this._SumData != null)
            {
                var trSum = new TagBuilder("tr");
                foreach (var column in this._Columns)
                {
                    var td = new TagBuilder("td");
                    td.Attributes.Add("style", "text-align:center;");
                    if (this._Columns.IndexOf(column) == 0)
                    {
                        td.SetInnerText("合计");
                    }
                    else
                    {
                        td.MergeAttributes(new RouteValueDictionary(column.HtmlAttributes));
                        try
                        {
                            td.InnerHtml += column.ColumnValueCalculator(this._SumData);
                        }
                        catch (Exception)
                        { }
                    }
                    trSum.InnerHtml += td;
                }
                tbody.InnerHtml += trSum;
            }
            #endregion
            table.InnerHtml += tbody;

            if (this._EnableFilter)
            {
                return new MvcHtmlString(filter.ToString() + table.ToString());
            }
            else
            {
                return new MvcHtmlString(table.ToString());
            }
        }

        public GridColumn<T> Column(Expression<Func<T, object>> selector, Func<T, object> valueCalculator = null)
        {
            var propertyeName = "";
            if (selector.Body is MemberExpression)
            {
                propertyeName = ((selector.Body as MemberExpression).Member as PropertyInfo).Name;
            }
            else if (selector.Body is UnaryExpression)
            {
                propertyeName = (((selector.Body as UnaryExpression).Operand as MemberExpression).Member as PropertyInfo).Name;
            }
            var column = new GridColumn<T>(propertyeName, selector.Compile(), this);
            _Columns.Add(column);
            return column;
        }

        public GridColumn<T> Column(string columnName, Func<T, object> valueCalculator, object htmlAttributes = null)
        {
            var column = new GridColumn<T>(columnName, valueCalculator, this).Name(columnName).Attributes(htmlAttributes);
            this._Columns.Add(column);
            return column;
        }

        public GridColumn<T> ActionColumn(params Func<T, object>[] valueCalculator)
        {
            var column = new GridColumn<T>("操作", m =>
            {
                var contextBox = new TagBuilder("div");
                contextBox.AddCssClass("grid-action");
                if (valueCalculator != null && valueCalculator.Any())
                    foreach (var calculator in valueCalculator)
                    {
                        contextBox.InnerHtml += calculator(m).ToString();
                    }
                return contextBox;
            }, this).Attributes(new { @class = "grid-action-td" });
            this._Columns.Add(column);
            return column;
        }
    }
}
