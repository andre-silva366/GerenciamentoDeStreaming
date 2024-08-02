using System.Text.Json.Serialization;

namespace GerenciamentoClientesStreaming.Models;

public class Aplicativo
{
    [JsonIgnore]
    public int AplicativoId { get; set; }
    public string Nome { get; set; } = "";
}
