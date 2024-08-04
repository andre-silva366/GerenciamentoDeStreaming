using GerenciamentoClientesStreaming.Models;
using GerenciamentoClientesStreaming.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoClientesStreaming.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServidoresController : ControllerBase
{
    private IRepository<Servidor> _repository;

    public ServidoresController()
    {
        _repository = new ServidorRepository();
                
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok(_repository.Get());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        try
        {
            return Ok(_repository.Get(id));
        }
        catch(Exception e)
        {            
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public IActionResult Post(Servidor servidor)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erro ao inserir os dados");
            }
            _repository.Post(servidor);

            return Ok(servidor);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public IActionResult Put(Servidor servidor)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erro ao atualizar os dados");
            }
            _repository.Put(servidor);

            return Ok(servidor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        try
        {
            _repository.Delete(id);
            return Ok("Dados deletados com sucesso.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
