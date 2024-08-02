using GerenciamentoClientesStreaming.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoClientesStreaming.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private IClienteRepository _repository;

    public ClientesController()
    {
        _repository = new ClienteRepository();
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_repository.Get());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok(_repository.Get(id));
    }
}
