using Microsoft.AspNetCore.Mvc;
using RssStore.Catalog.Application.Dtos;
using RssStore.Catalog.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssStore.WebApp.MVC.Controllers.Admin
{
    public class AdminProductsController : Controller
    {
        private readonly IProductAppService _productAppService;
        public AdminProductsController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet("admin-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(await _productAppService.GetAll());
        }

        [HttpGet("novo-produto")]
        public async Task<IActionResult> NewProduct()
        {
            return View(await PopulateCategories(new ProductDto()));
        }

        [HttpPost("novo-produto")]
        public async Task<IActionResult> NewProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid) return View(await PopulateCategories(productDto));

            await _productAppService.AddProduct(productDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("editar-produto")]
        public async Task<IActionResult> UpdateProduct(Guid id)
        {
            return View(await PopulateCategories(await _productAppService.GetById(id)));
        }

        [HttpPost("editar-produto")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductDto productDto)
        {
            var product = await _productAppService.GetById(id);
            productDto.StockAmount = product.StockAmount;

            //remove o stock amount do validate da model state
            ModelState.Remove("StockAmount");
            if (!ModelState.IsValid) return View(await PopulateCategories(productDto));

            await _productAppService.UpdateProduct(productDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("produtos-atualizar-estoque")]
        public async Task<IActionResult> UpdateStock(Guid id)
        {
            return View("Stock", await _productAppService.GetById(id));
        }

        [HttpPost("produtos-atualizar-estoque")]
        public async Task<IActionResult> UpdateStock(Guid id, int amount)
        {
            if(amount > 0)
            {
                await _productAppService.FillStockAmount(id, amount);
            }
            else
            {
                await _productAppService.DebitStockAmount(id, amount);
            }

            return View(nameof(Index), await _productAppService.GetAll());
        }

        private async Task<ProductDto> PopulateCategories(ProductDto productDto)
        {
            productDto.Categories = await _productAppService.GetCategories();
            return productDto;
        }
    }
}
