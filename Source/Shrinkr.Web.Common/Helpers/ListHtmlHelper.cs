namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    using Extensions;

    public class ListHtmlHelper
    {
        private readonly HtmlHelper htmlHelper;

        public ListHtmlHelper(HtmlHelper htmlHelper)
        {
            Check.Argument.IsNotNull(htmlHelper, "htmlHelper");

            this.htmlHelper = htmlHelper;
        }

        public string Pager<TItem>() where TItem : class
        {
            PagedListViewModel<TItem> model = (PagedListViewModel<TItem>) htmlHelper.ViewContext.ViewData.Model;

            return Pager(null, null, null, htmlHelper.ViewContext.RouteData.Values, "page", model.PageCount, model.ItemPerPage, 2, model.CurrentPage);
        }

        private string Pager(string routeName, string actionName, string controllerName, IDictionary<string, object> values, string pageParamName, int pageCount, int noOfPageToShow, int noOfPageInEdge, int currentPage)
        {
            Func<string, int, string> getPageLink = (text, page) =>
                                                                    {
                                                                        RouteValueDictionary newValues = new RouteValueDictionary();

                                                                        foreach (KeyValuePair<string, object> pair in values)
                                                                        {
                                                                            if (!pair.Key.Equals("controller", StringComparison.OrdinalIgnoreCase) &&
                                                                                !pair.Key.Equals("action", StringComparison.OrdinalIgnoreCase))
                                                                            {
                                                                                newValues[pair.Key] = pair.Value;
                                                                            }
                                                                        }

                                                                        if (page > 0)
                                                                        {
                                                                            newValues[pageParamName] = page;
                                                                        }

                                                                        string link;

                                                                        if (!string.IsNullOrWhiteSpace(routeName))
                                                                        {
                                                                            link = htmlHelper.RouteLink(text, routeName, newValues).ToHtmlString();
                                                                        }
                                                                        else
                                                                        {
                                                                            actionName = actionName ?? values["action"].ToString();
                                                                            controllerName = controllerName ?? values["controller"].ToString();

                                                                            link = htmlHelper.ActionLink(text, actionName, controllerName, newValues, null).ToHtmlString();
                                                                        }

                                                                        return string.Concat(" ", link);
                                                                    };

            StringBuilder pagerHtml = new StringBuilder();

            if (pageCount > 1)
            {
                pagerHtml.Append("<div class=\"pager\">");

                double half = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(noOfPageToShow) / 2));

                int start = Convert.ToInt32((currentPage > half) ? Math.Max(Math.Min((currentPage - half), (pageCount - noOfPageToShow)), 0) : 0);
                int end = Convert.ToInt32((currentPage > half) ? Math.Min(currentPage + half, pageCount) : Math.Min(noOfPageToShow, pageCount));

                pagerHtml.Append(currentPage > 1 ? getPageLink("Previous", currentPage - 1) : " <span class=\"disabled\">Previous</span>");

                if (start > 0)
                {
                    int startingEnd = Math.Min(noOfPageInEdge, start);

                    for (int i = 0; i < startingEnd; i++)
                    {
                        int pageNo = i + 1;

                        pagerHtml.Append(getPageLink(pageNo.ToString(Culture.Current), pageNo));
                    }

                    if (noOfPageInEdge < start)
                    {
                        pagerHtml.Append(" ...");
                    }
                }

                for (int i = start; i < end; i++)
                {
                    int pageNo = i + 1;

                    pagerHtml.Append(pageNo == currentPage ? " <span class=\"current\">{0}</span>".FormatWith(pageNo) : getPageLink(pageNo.ToString(Culture.Current), pageNo));
                }

                if (end < pageCount)
                {
                    if ((pageCount - noOfPageInEdge) > end)
                    {
                        pagerHtml.Append(" ...");
                    }

                    int endingStart = Math.Max(pageCount - noOfPageInEdge, end);

                    for (int i = endingStart; i < pageCount; i++)
                    {
                        int pageNo = i + 1;
                        pagerHtml.Append(getPageLink(pageNo.ToString(Culture.Current), pageNo));
                    }
                }

                pagerHtml.Append(currentPage < pageCount ? getPageLink("Next", currentPage + 1) : " <span class=\"disabled\">Next</span>");

                pagerHtml.Append("</div>");
            }

            return pagerHtml.ToString();
        }
    }
}