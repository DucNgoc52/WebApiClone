using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAPIClone.Commom.MSG;
using WebAPIClone.Commom.Result;
using WebAPIClone.Common;
using WebAPIClone.Data;
using WebAPIClone.Model;

namespace WebAPIClone.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CategoryModel>> GetAllCategoryAsync()
        {
            var cates = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryModel>>(cates);
        }

        public async Task<ApiResult<CategoryModel>> GetCateById(int id)
        {
            var cate = await _context.Categories.FindAsync(id);
            if (cate == null)
            {
                return new ApiErrorResult<CategoryModel>(MsgError.GET_ITEM_byID_FAILED, Code.OK);
            }
            var catemodel = _mapper.Map<CategoryModel>(cate);
            return new ApiSuccesResult<CategoryModel>(catemodel, MsgSuccess.GET_ITEM_SUCCESS, Code.OK);
        }

        public async Task<ApiResult<CategoryModel>> AddCategoryAsync(CategoryModel model)
        {
            var cate = _mapper.Map<Category>(model);
            _context.Add(cate);
            await _context.SaveChangesAsync();
            return new ApiSuccesResult<CategoryModel>(model, MsgSuccess.ITEM_CREATE_SUCCESS, Code.OK);
        }

        public async Task<ApiResult<bool>> UpdateCategoryAsync(int id, CategoryModel model)
        {
            if(id == model.Id)
            {
                var cate = _mapper.Map<Category>(model);
                _context.Categories.Update(cate);
                await _context.SaveChangesAsync();
                return new ApiSuccesResult<bool>(true,MsgSuccess.ITEM_UPDATE_SUCCESS, Code.OK);
            }
            return new ApiErrorResult<bool>(MsgError.ITEM_UPDATE_FAILED, Code.OK);
        }

        public async Task<ApiResult<bool>> DeleteCategoryAsync(int id)
        {
            var cate = await _context.Categories.FindAsync(id);
            if (cate != null)
            {
                _context.Categories.Remove(cate);
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
