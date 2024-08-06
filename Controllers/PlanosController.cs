using GerenciamentoClientesStreaming.Models;
using GerenciamentoClientesStreaming.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoClientesStreaming.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlanosController : ControllerBase
{
    private IRepository<Plano> _repository;

    public PlanosController()
    {
        _repository = new PlanoRepository();
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok(_repository.Get());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        try
        {
            if(id <= 0)
            {
                throw new Exception("Id inválido");
            }

           return Ok(_repository.Get(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public IActionResult Post([FromBody] Plano plano)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erro ao inserir os dados.");
            }

            _repository.Post(plano);

            return Ok(plano);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult Put([FromBody] Plano plano)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erro ao atualizar os dados.");
            }

            _repository.Put(plano);

            return Ok(plano);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                throw new Exception("Id inválido");
            }

            _repository.Delete(id);

            return Ok("Deletado com sucesso.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
