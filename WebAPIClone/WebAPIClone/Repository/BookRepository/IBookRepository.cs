using WebAPIClone.Commom.Result;
using WebAPIClone.Common;
using WebAPIClone.Model;

namespace WebAPIClone.Repository
{
    public interface IBookRepository
    {
        public ApiResultList GetAllBookAsync(string search, string sort, int page =1, int pageSize = 10);
        public Task<ApiResult<BookModel>> GetBookByIdAsync(int id);
        public Task<ApiResult<BookCreateModel>> AddBookAsync(BookCreateModel model);
        public Task<ApiResult<bool>> UpdateBookAsync(int id, BookModel model);
        //public Task<ApiResult<bool>> PatchUpdateBookAsync(int id, BookModel model);
        public Task<ApiResult<bool>> DeleteBookAsync(int id);
    }
}
