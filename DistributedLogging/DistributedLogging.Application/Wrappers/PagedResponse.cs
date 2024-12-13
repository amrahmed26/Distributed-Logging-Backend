

namespace DistributedLogging.Application.Wrappers
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
        public bool HasNext
        {
            get
            {
                return Page < (int)Math.Ceiling(Count / (double)PageSize);
            }
        }
        public T FilteredList { get; set; }

        public PagedResponse(T data, int page, int pageSize, long count = 0)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.FilteredList = data;
            this.Count = count;
        }
    }
}
