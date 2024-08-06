using GerenciamentoClientesStreaming.Models;
using GerenciamentoClientesStreaming.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoClientesStreaming.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AplicativosController : ControllerBase
{
    private IRepository<Aplicativo> _repository;

    public AplicativosController()
    {
        _repository = new AplicativoRepository();
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok(_repository.Get());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
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
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Post([FromBody]Aplicativo aplicativo)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                throw new Exception("Erro ao inserir os dados;");
            }

            _repository.Post(aplicativo);
            return Ok(aplicativo) ;
                
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public IActionResult Put([FromBody]Aplicativo aplicativo)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Erro ao atualizar os dados;");
            }

            _repository.Put(aplicativo);
            return Ok(aplicativo);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            if(id <= 0)
            {
                throw new Exception("Id inválido");
            }

            _repository.Delete(id);
            return Ok("Aplicativo deletado com sucesso");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
