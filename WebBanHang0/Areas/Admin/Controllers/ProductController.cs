using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebBanHang0.Models;
using WebBanHang0.Repository;

namespace WebBanHang0.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin, Seller")]
	public class ProductController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _databaseContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _databaseContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_databaseContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_databaseContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_databaseContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_databaseContext.Brands, "Id", "Name", product.BrandId);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _databaseContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Exsisted");
                    return View(product);
                }

                if (product.ImgUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImgUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImgUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }
                _databaseContext.Add(product);
                await _databaseContext.SaveChangesAsync();
                TempData["success"] = "Added";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["fail"] = "Model Error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _databaseContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_databaseContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_databaseContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_databaseContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_databaseContext.Brands, "Id", "Name", product.BrandId);
            var existed_product = _databaseContext.Products.Find(product.Id);

            if (ModelState.IsValid)
            {
                product.Slug = existed_product.Name.Replace(" ", "-");
                var slug = await _databaseContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "exsisted");
                //    return View(product);
                //}

                if (product.ImgUpload != null)
                {
                    //
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImgUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    //
                    string oldImgFile = Path.Combine(uploadDir, existed_product.Image);

                    try
                    {
                        if (System.IO.File.Exists(oldImgFile))
                        {
                            System.IO.File.Delete(oldImgFile);
                        }

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Failed to delete image");
                    }

                    //
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImgUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;

                }
                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;

                _databaseContext.Update(existed_product);
                await _databaseContext.SaveChangesAsync();
                TempData["success"] = "Updated";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["fail"] = "Model Error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _databaseContext.Products.FindAsync(Id);

            if (product == null)
            {
                return NotFound();
            }

            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            string oldImgFile = Path.Combine(uploadDir, product.Image);

            try
            {
                if (System.IO.File.Exists(oldImgFile))
                {
                    System.IO.File.Delete(oldImgFile);
                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Failed to delete image");   
            }

            _databaseContext.Products.Remove(product);
            await _databaseContext.SaveChangesAsync();
            TempData["fail"] = "Deleted";
            return RedirectToAction("Index");

        }


    }
}
