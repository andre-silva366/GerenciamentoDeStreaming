using System.Text.Json.Serialization;

namespace GerenciamentoClientesStreaming.Models;

public class Cliente
{
    
    public int ClienteId { get; set; }
    public string Nome { get; set; } 
    public string Telefone { get; set; } 
    public string Email { get; set; }
    
    public int ServidorId { get; set; }
    
    public int PlanoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataUltimoPagamento { get; set; } = DateTime.Now;
    public DateTime DataProximoPagamento { get; set; } = DateTime.Now.AddMonths(1);
    
    public int AplicativoId { get; set; }

    
    public Aplicativo Aplicativo { get; set; }
    
    public Servidor Servidor { get; set; }
   
    public Plano Plano { get; set; } 
}
