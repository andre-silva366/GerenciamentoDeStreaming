using System.Text.Json.Serialization;

namespace GerenciamentoClientesStreaming.Models;

public class Cliente
{
    [JsonIgnore]
    public int ClienteId { get; set; }

    [JsonIgnore]
    public int AplicativoId { get; set; }

    [JsonIgnore]
    public int ServidorId { get; set; }

    [JsonIgnore]
    public int PlanoId { get; set; }
    public string Nome { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string Email { get; set; } = "";
    public decimal Valor { get; set; }
    public DateTime DataUltimoPagamento { get; set; }
    public DateTime DataProximoPagamento { get; set; }

    
    public ICollection<Aplicativo> Aplicativos { get; set; } = [];

   
    public ICollection<Servidor> Servidores { get; set; } = [];

    
    public Plano Plano { get; set; } = new();
}
