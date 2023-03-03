using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAPIClone.Commom.MSG;
using WebAPIClone.Commom.Result;
using WebAPIClone.Common;
using WebAPIClone.Data;
using WebAPIClone.Model;

namespace WebAPIClone.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ApiResultList GetAllBookAsync(string search, string sort, int page = 1, int pageSize = 10)
        {
            var books =  _context.Books.AsQueryable();
            //search
            if (!string.IsNullOrEmpty(search))
            {
                books =  _context.Books.Where(b => b.Title.Contains(search));
            }

            books = books.OrderBy(b => b.Title);

            //sort
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "-title":
                        books = books.OrderByDescending(b => b.Title);
                        break;
                    case "+price":
                        books = books.OrderBy(b => b.Price);
                        break;
                    case "-price":
                        books = books.OrderByDescending(b => b.Price);
                        break;
                }
            }

            //paging
            int totalCount = books.Count();
            books = books.Skip((page - 1) * pageSize).Take(pageSize);

            var result = books.Select(b => new BookModel
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Price = b.Price,
                Quantity = b.Quantity,
                CategoryId = b.CategoryId
            });
            var tempBooks = result.ToList();
            return new ApiResultList()
            {
                code = Code.OK,
                Message = MsgSuccess.GET_ITEM_SUCCESS,
                Data = new ResponsePaging()
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalRecords = totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    content = tempBooks
                }
            };
        }

        public async Task<ApiResult<BookModel>> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return new ApiErrorResult<BookModel>(MsgError.GET_ITEM_byID_FAILED, Code.OK);
            }
            var mapBook = _mapper.Map<BookModel>(book);
            return new ApiSuccesResult<BookModel>(mapBook, MsgSuccess.GET_ITEM_SUCCESS, Code.OK);
        }

        public async Task<ApiResult<BookCreateModel>> AddBookAsync(BookCreateModel model)
        {
            var newbook = _mapper.Map<Book>(model);
            newbook.Id = 0;
            _context.Books.Add(newbook);
            await _context.SaveChangesAsync();
            return new ApiSuccesResult<BookCreateModel>(model, MsgSuccess.ITEM_CREATE_SUCCESS, Code.OK);
        }

        public async Task<ApiResult<bool>> UpdateBookAsync(int id, BookModel model)
        {
            if (id == model.Id)
            {
                var book = _mapper.Map<Book>(model);
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return new ApiSuccesResult<bool>(true, MsgSuccess.ITEM_UPDATE_SUCCESS, Code.OK);
            }
            else
            {
                return new ApiErrorResult<bool>(MsgError.ITEM_UPDATE_FAILED, Code.OK);
            }
        }

        public async Task<ApiResult<bool>> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book != null)
            {
                _context.Books!.Remove(book);
                await _context.SaveChangesAsync();
                return new ApiSuccesResult<bool>(true, MsgSuccess.ITEM_DELETE_SUCCESS, Code.OK);
            }
            else
            {
                return new ApiErrorResult<bool>(MsgError.GET_ITEM_byID_FAILED, Code.OK);
            }
        }
    }
}
