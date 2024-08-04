using GerenciamentoClientesStreaming.Models;
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
        var cliente = _repository.Get(id);

        if(cliente == null || cliente.ClienteId == 0)
        {
            return NotFound($"Cliente com o id: {id} não foi encontrado.");
        }

        return Ok(_repository.Get(id));
    }

    [HttpPost]
    public IActionResult Post(Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Cliente não foi criado.");
        }

        try
        {
            _repository.Post(cliente);

            if(cliente.ClienteId == 0)
            {
                return BadRequest("Cliente não foi criado.");
            }

            return Ok(cliente);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    public IActionResult Put(Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Cliente não foi alterado.");
        }
        try
        {
            _repository.Put(cliente);

            if (cliente.ClienteId == 0)
            {
                return BadRequest("Cliente não foi alterado.");
            }

            return Ok(cliente);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
