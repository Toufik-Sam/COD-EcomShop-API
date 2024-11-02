using EcomBusinessLayer.Products.Categories;
using EcomDataAccess;
using EcomDataAccess.ProductsData.CategoriesData;
using EcommerceAppAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory _category;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategory category,ILogger<CategoriesController>logger)
        {
            this._category = category;
            this._logger = logger;
        }
        // GET: api/<CategoriesController>
        [HttpGet("AllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<CategoryDTO>>> GetAllCategories()
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllCategories");
            try
            {
                var CategoriesList = await _category.GetAllCategories();
                return Ok(CategoriesList);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, message: "The GET call to api/AllCategories failed!");
                return BadRequest();
            }
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}/CategoryByID",Name ="GetCategoryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDTO>> GetCategoryByID(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/CategoryByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                CategoryDTO category = await _category.Find(id);
                if (category == null)
                    return NotFound($"Category With ID {id} was not found");
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/CategoryByID failed!", id);
                return BadRequest();
            }
        }

        // POST api/<CategoriesController>
        [HttpPost("AddCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO category)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddCategory");
            if (!ValidateInput(category))
                return BadRequest("Invalid Data!");
            try
            {
                var AddedCategory = await _category.AddNewCategory(category);
                if (AddedCategory != null)
                    return Ok(AddedCategory);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The POST call to api/AddCategory failed!");
                return BadRequest();
            }
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}/UpdateCategory",Name ="UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id,CategoryDTO UpdatedCategory)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();

            _logger.LogInformation("PUT: api/{id}", id);
            if (id < 1 || !ValidateInput(UpdatedCategory))
                return BadRequest("Invalid Data");
            try
            {
                var category = await _category.Find(id);
                if (category == null)
                    return NotFound($"category With ID {id} was not found");
                category.EmployeeID = UpdatedCategory.EmployeeID;
                category.CategoryName = UpdatedCategory.CategoryName;
                category.CategoryDescription = UpdatedCategory.CategoryDescription;

                bool flag = await _category.UpdateCategory(category);
                if (flag)
                    return Ok(category);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateCustomer failed", id);
                return BadRequest();
            }
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}/DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if ((Global.User.Permission & Permissions.Addmin) != Permissions.Addmin &&
              (Global.User.Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();

            _logger.LogInformation("DELETE: api/{id}/DeleteCategory", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _category.DeleteCategory(id);
                if (flag)
                    return Ok($"Category with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteCategory failed", id);
                return BadRequest();
            }
        }
        private bool ValidateInput(CategoryDTO category)
        {
            if (category.EmployeeID<=0 || category.CategoryName=="" || category.CategoryDescription=="")
                return false;
            return true;
        }

    }
}
