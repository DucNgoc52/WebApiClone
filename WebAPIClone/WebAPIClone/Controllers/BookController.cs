using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIClone.Commom.Result;
using WebAPIClone.Common;
using WebAPIClone.Data;
using WebAPIClone.Model;
using WebAPIClone.Repository;

namespace WebAPIClone.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        public BookController(IBookRepository repo)
        {
            _bookRepo = repo;
        }
        [HttpGet]
        public ApiResultList GetAllBookAsync(string search, string sort, int page =1 , int pageSize = 10)
        {
            var results = _bookRepo.GetAllBookAsync(search, sort, page, pageSize);
            return results;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<BookModel>>> GetBookIdAsync(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<BookCreateModel>>> AddBookAsync(BookCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bookRepo.AddBookAsync(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<bool>>> UpdateBookAsync(int id, BookModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bookRepo.UpdateBookAsync(id, model);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<bool>>> DeleteBookAsync(int id)
        {
            var result = await _bookRepo.DeleteBookAsync(id);
            return Ok(result);
        }
    }
}
