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
}
