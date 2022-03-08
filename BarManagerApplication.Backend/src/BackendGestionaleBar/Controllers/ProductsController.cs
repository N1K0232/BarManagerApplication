﻿using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Filters;
using BackendGestionaleBar.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("GetMenu")]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Cliente)]
        [ProducesResponseType(200, Type = typeof(DataTable))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMenu()
        {
            DataTable dataTable = await productService.GetMenuAsync();
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return NotFound("no row found");
            }

            return Ok(dataTable);
        }

        [HttpGet("GetProduct")]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Cliente)]
        [ProducesResponseType(204)]
        //[ProducesResponseType(200, Type = typeof(DataTable))]
        //[ProducesResponseType(404)]
        public Task<IActionResult> GetProduct(Guid idProduct)
        {
            return Task.FromResult(NoContent() as IActionResult);
        }

        [HttpPost("RegisterProduct")]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
        [ProducesResponseType(204)]
        public Task<IActionResult> RegisterProduct()
        {
            return Task.FromResult(NoContent() as IActionResult);
        }
    }
}