using Microsoft.AspNetCore.Mvc;
using RssStore.Catalog.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace RssStore.WebApp.MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductAppService _productAppService;

        public CatalogController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet("")]
        [HttpGet("catalog")]
        public async Task<IActionResult> Index()
        {
            return View(await _productAppService.GetAll());
        }

        [HttpGet("product-detail/{id:guid}")]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            return View(await _productAppService.GetById(id));
        }
    }
}
