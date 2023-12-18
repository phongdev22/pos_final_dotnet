using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos.Config;
using pos.Entities;
using pos.Models.Product;
using pos.Models;


namespace pos.Controllers
{
	[Authorize]
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext _context;
		public int PageSize { get; set; } = 10;
		public ProductsController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index(int page = 1, [FromQuery] int store = 1)
		{
			ViewData["Title"] = "POS | List Products";
			var retailStores = _context.RetailStores.ToList();

			var products = _context.Products
					.Where(p => p.Inventories.Any(i => i.RetailStoreId == store))
					.Skip((page - 1) * PageSize)
					.Take(PageSize)
					.ToList();

			ViewBag.Store = store;
			ViewBag.Stores = retailStores;

			var Page = new PageViewModel<Product>() { Items = products, PageNumber = page, PageSize = PageSize, TotalItems = products.Count };

			return View(Page);
		}

		// SEARCH
		public async Task<IActionResult> Search([FromQuery] string keyword)
		{
			var products = _context.Products.AsNoTracking()
				.Where(p => p.Barcode.Equals(keyword) || p.Name.Contains(keyword))
				.ToList();

			var data = new List<ProductSearchResponse>();

			foreach (var product in products)
			{
				if(product.Inventories.Any(inv => inv.Quantity > 0))
				{
					data.Add(new ProductSearchResponse()
					{
						Id = product.Id.ToString(),
						Name = product.Name,
						Barcode = product.Barcode,
						Price = product.Price,
						ImagePath = product.ImagePath,
						Quantity = product.Quantity,
					});
				}
			}
			
			return Ok(data);
		}

		// CREATE
		public IActionResult Create()
		{
			ViewData["Title"] = "POS | Create New Product";

			var RetailStores = _context.RetailStores.ToList();
			var Categories = _context.Categories.ToList();

			return View(new ProductModel() { Categories = Categories, RetailStores = RetailStores });
		}

		[HttpPost]
		public async Task<IActionResult> Create(Product product, [FromForm] int categoryId, [FromForm] int retailId)
		{
			ViewData["Title"] = "POS | Create New Product";

			product.CategoryId = categoryId;

			var result = _context.Products.Add(product);

			_context.Inventory.Add(new Inventory() { RetailStoreId = retailId, Quantity = product.Quantity, Product = product });
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		// EDIT
		public IActionResult Edit(int? id, [FromQuery] int store)
		{
			ViewData["Title"] = "POS | Edit Product";

			var retailStore = _context.RetailStores.FirstOrDefault(rs => rs.Id == store);
			var Categories = _context.Categories.ToList();
			var product = _context.Products.FirstOrDefault(x => x.Id == id);

			ViewBag.Store = retailStore;

			return View(new ProductModel() { Product = product, Categories = Categories });
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Product product, int categoryId, [FromQuery] int store)
		{
			var _product = await _context.Products.FindAsync(id);

			if (_product != null)
			{
				_product.Name = product.Name;
				_product.Price = product.Price;
				_product.Cost = product.Cost;
				_product.CategoryId = categoryId;
				_product.Barcode = product.Barcode;
				_product.Quantity = product.Quantity;

				var inventory = await _context.Inventory.FirstOrDefaultAsync(inv => inv.ProductId == id && inv.RetailStoreId == store);
				if (inventory != null)
				{
					inventory.Quantity = product.Quantity;
				}

				// Process image file

			}
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		
		[HttpDelete]		
		public async Task<IActionResult> Delete(int id)
		{
            var product = await _context.Products.FirstOrDefaultAsync(od => od.Id == id);

            if (product == null) return Ok(new { code = 1, Message = "Not found!" });
            
			_context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(new { code = 0, Message = "Success" });
        }
	}
}
