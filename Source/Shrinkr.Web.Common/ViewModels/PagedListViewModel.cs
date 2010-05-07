namespace Shrinkr.Web
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public class PagedListViewModel<TItem> where TItem : class
    {
        public PagedListViewModel(IEnumerable<TItem> items, int currentPage, int itemPerPage, int totalCount)
        {
            Check.Argument.IsNotNull(items, "items");
            Check.Argument.IsNotZeroOrNegative(currentPage, "currentPage");
            Check.Argument.IsNotZeroOrNegative(itemPerPage, "itemPerPage");
            Check.Argument.IsNotNegative(totalCount, "totalCount");

            Items = new List<TItem>(items);
            CurrentPage = currentPage;
            ItemPerPage = itemPerPage;
            TotalCount = totalCount;
        }

        public int CurrentPage
        {
            get;
            private set;
        }

        public int ItemPerPage
        {
            get;
            private set;
        }

        public int TotalCount
        {
            get;
            private set;
        }

        public int PageCount
        {
            [DebuggerStepThrough]
            get
            {
                return PageCalculator.TotalPage(TotalCount, ItemPerPage);
            }
        }

        public IList<TItem> Items
        {
            get;
            private set;
        }
    }
}