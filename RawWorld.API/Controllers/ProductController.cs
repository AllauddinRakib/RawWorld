namespace RawWorld.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawWorld.API.DTOs;
using RawWorld.API.Services;

[ApiController, Route("api/products")]
public class ProductController(IProductService svc) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search) =>
        Ok(await svc.GetAllAsync(search));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await svc.GetByIdAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateProductDto dto) =>
        Ok(await svc.CreateAsync(dto));

    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, CreateProductDto dto) =>
        await svc.UpdateAsync(id, dto) ? Ok() : NotFound();

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id) =>
        await svc.DeleteAsync(id) ? Ok() : NotFound();
}