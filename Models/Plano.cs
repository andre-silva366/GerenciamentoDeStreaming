using System.Text.Json.Serialization;

namespace GerenciamentoClientesStreaming.Models;

public class Plano
{
    [JsonIgnore]
    public int PlanoId { get; set; }
    public string Nome { get; set; } = "";
}
