using EcomBusinessLayer.Products;
using EcomBusinessLayer.Products.Categories;
using EcomDataAccess;
using EcomDataAccess.ProductsData;
using EcomDataAccess.ProductsData.Galleries;
using EcomDataAccess.ProductsData.ProductsCategories;
using EcommerceAppAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _product;
        private readonly ICategory _category;
        private readonly ILogger<ProductsController> _logger;
        private readonly IUserService _global;

        public ProductsController(IProduct product,ICategory category,ILogger<ProductsController>logger,IUserService global)
        {
            this._product = product;
            this._category = category;
            this._logger = logger;
            this._global = global;
        }
        // GET: api/<ProductsController>
        [HttpGet("AllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<ProductDTO>>> GetAllProducts()
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.StockManager)!=Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllProducts");
            try
            {
                var CustomersList = await _product.GetAllProducts();
                return Ok(CustomersList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllProducts failed!");
                return BadRequest();
            }
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}/ProductByID",Name ="GetProductByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> GetProductByID(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                 && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/ProductByID", id);
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                ProductDTO product = await _product.Find(id);
                if (product == null)
                    return NotFound($"Product With ID {id} was not found");
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/ProductByID failed!", id);
                return BadRequest();
            }
        }

        // GET: api/<ProductsController>
        [HttpGet("AllProductCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<ProductCategoryDTO>>> GetAllProductCategories(int ProductID)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
               && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllProductCategories");
            try
            {
                var ProductCategoriesList = await _product.GetProductCategories(ProductID);
                return Ok(ProductCategoriesList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllProductCategories failed!");
                return BadRequest();
            }
        }

        [HttpGet("{id}/ProductGalleryByID", Name = "ProductGalleryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GalleryDTO>> GetProductGalleryByID(int GalleryID)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/{id}/ProductGalleryByID", GalleryID);
            if (GalleryID < 1)
                return BadRequest($"Not Accepted ID {GalleryID}");
            try
            {
                GalleryDTO productGallery = await _product.GetProductGalleryByID(GalleryID);
                if (productGallery == null)
                    return NotFound($"Product With ID {GalleryID} was not found");
                return Ok(productGallery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/{id}/ProductGalleryByID failed!", GalleryID);
                return BadRequest();
            }
        }

        [HttpGet("AllProductGalleries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<ProductDTO>>> GetAllProductGalleries(int ProductID)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "GET: api/AllProductGalleries");
            try
            {
                var ProductGalleries = await _product.GetProductGalleries(ProductID);
                return Ok(ProductGalleries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The GET call to api/AllProductGalleries failed!");
                return BadRequest();
            }
        }

        // POST api/<ProductsController>
        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> Post(ProductDTO product)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddProduct");
            if (!ValidateInput(product))
                return BadRequest("Invalid Data!");
            try
            {
                var AddProduct = await _product.AddNewProduct(product);
                if (AddProduct != null)
                    return Ok(AddProduct);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,message: "The POST call to api/AddProduct failed!");
                return BadRequest();
            }
        }

        // POST api/<ProductsController>
        [HttpPost("AddProductCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> Post(ProductCategoryDTO ProductCategory)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/AddProductCategories");
            bool IsProductExist = await _product.IsProductExist(ProductCategory.ProductID);
            bool IsCategoryExist = await _category.IsCategoryID(ProductCategory.CategoryID);
            if (!IsProductExist || !IsCategoryExist)
                return BadRequest("Invalid Data!");
            try
            {
                var AddedProductCategory = await _product.AddNewProductCategories(ProductCategory);
                if (AddedProductCategory != null)
                    return Ok(AddedProductCategory);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The POST call to api/AddProductCategories failed!");
                return BadRequest();
            }
        }

        [HttpPost("UploadProductImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GalleryDTO>> Post(int ProductID, IFormFile imageFile)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
              && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation(message: "POST: api/UploadProductImage");
            // Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");
            bool IsProductExist = await _product.IsProductExist(ProductID);
            if (!IsProductExist)
                return BadRequest("Invalid ProductID");
            try
            {
                // Directory where files will be uploaded
                var uploadDirectory = @"C:\MyUploads";

                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadDirectory, fileName);

                // Ensure the uploads directory exists, create if it doesn't
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var AddedGallery = await _product.AddNewProductImage(new GalleryDTO(-1, ProductID, filePath));
                if (AddedGallery != null)
                    return Ok(AddedGallery);
                else
                    return BadRequest();
                // Return the file path as a response
                //return Ok(new { filePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The POST call to api/UploadProductImage failed!");
                return BadRequest();
            }

        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}/UpdateProduct",Name ="UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id,ProductDTO UpdatedProduct)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
               && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation("PUT: api/{id}", id);
            if (id < 1 || !ValidateInput(UpdatedProduct))
                return BadRequest("Invalid Data");
            try
            {
                var product = await _product.Find(id);
                if (product == null)
                    return NotFound($"Product With ID {id} was not found");
                product.ProductName= UpdatedProduct.ProductName;
                product.ProductDescription = UpdatedProduct.ProductDescription;
                product.EmployeeID = UpdatedProduct.EmployeeID;
                product.Price = UpdatedProduct.Price;
                product.Quantity = UpdatedProduct.Quantity;
                product.Created_At = UpdatedProduct.Created_At;
            
                bool flag = await _product.UpdateProduct(product);
                if (flag)
                    return Ok(product);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateCustomer failed", id);
                return BadRequest();
            }
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}/UpdateProductCategories", Name = "UpdateProductCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, ProductCategoryDTO UpdatedProductCategory)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
               && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation("PUT: api/{id}/UpdateProductCategories", id);
            bool IsProductExist = await _product.IsProductExist(UpdatedProductCategory.ProductID);
            bool IsCategoryExist = await _category.IsCategoryID(UpdatedProductCategory.CategoryID);
            if (!IsProductExist || !IsCategoryExist)
                return BadRequest("Invalid Data");
            try
            {
                var productcategory = await _product.FindProductCategoryByID(id);
                if (productcategory == null)
                    return NotFound($"ProductCategory With ID {id} was not found");
                productcategory.ProductID = UpdatedProductCategory.ProductID;
                productcategory.CategoryID = UpdatedProductCategory.CategoryID;

                bool flag = await _product.UpdateProductCategory(productcategory);
                if (flag)
                    return Ok(productcategory);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The PUT call to api/{id}/UpdateProductCategories failed", id);
                return BadRequest();
            }
        }

        [HttpPut("{id}/UpdateProductImage", Name = "UpdateProductImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int GalleryID, IFormFile imageFile)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
                && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation("PUT: api/{id}/UpdateProductImage", GalleryID);
            if (GalleryID <= 0)
                return BadRequest("Invalid GalleryID");
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");
            try
            {
                var ProductGallery = await _product.GetProductGalleryByID(GalleryID);
                if (ProductGallery == null)
                    return NotFound($"Gallery with ID {GalleryID} was not found!");
                var uploadDirectory = @"C:\MyUploads";

                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadDirectory, fileName);

                // Ensure the uploads directory exists, create if it doesn't
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                ProductGallery.ImagePath = filePath;
                bool flag = await _product.UpdateProductImage(ProductGallery);
                if (flag)
                    return Ok(ProductGallery);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "The POST call to api/UploadProductImage failed");
                return BadRequest(ex);
            }


        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}/DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
               && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation("DELETE: api/{id}/DeleteProduct", id);
            
            if (id < 1)
                return BadRequest($"Not Accepted ID {id}");
            try
            {
                bool flag = await _product.DeleteProduct(id);
                if (flag)
                    return Ok($"Product with ID {id} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteProduct failed", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id}/DeleteProductGallery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProductGallery(int GalleryID)
        {
            if ((_global.GetUser().Permission & Permissions.Addmin) != Permissions.Addmin
               && (_global.GetUser().Permission & Permissions.StockManager) != Permissions.StockManager)
                return Unauthorized();
            _logger.LogInformation("DELETE: api/{id}/DeleteProductGallery", GalleryID);
            if (GalleryID < 1)
                return BadRequest($"Not Accepted ID {GalleryID}");
            try
            {
                bool flag = await _product.DeleteProductGallery(GalleryID);
                if (flag)
                    return Ok($"Product Gallery with ID {GalleryID} was Deleted Successfully!");
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("the DELETE call to api/{id}/DeleteProductGallery failed", GalleryID);
                return BadRequest();
            }
        }
        private bool ValidateInput(ProductDTO product)
        {
            if (product.ProductName == "" && product.ProductDescription == "" && product.Quantity == 0 && product.Price == 0)
                return false;
            return true;
        }
    }
}
