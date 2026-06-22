using Talabat.Core.Entities;

namespace Talabat.Api.Dtos.Pagination
{
    public class PagedResposeDto<T> where T:class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public IEnumerable<T> Data { get; set; }=Array.Empty<T>();
    }
}
