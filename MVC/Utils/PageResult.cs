using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Utils
{
    public class PageResult<T>
    {
        //private List<T> content = new List<T>(0);

        public int CurrentIndex { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public List<T> Content { get; set; }

        public PageResult(int startIndex,int pageSize,int totalCount,List<T> content)
        {
            this.Content = content;
            this.TotalCount = totalCount;
            if ((totalCount % pageSize)>0)
            {
                this.TotalPage = totalCount / pageSize + 1;
            }
            else
            {
                this.TotalPage = totalCount / pageSize;
            }
            if (((startIndex + 1) % pageSize) > 0)
            {
                this.CurrentPage = (startIndex + 1) / pageSize + 1;
            }
            else
            {
                this.CurrentPage = (startIndex + 1) / pageSize;
            }
            this.PageSize = content.Count;
            this.CurrentIndex = startIndex + content.Count;
        }
    }
}