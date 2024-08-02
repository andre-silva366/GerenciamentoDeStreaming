using System.Text.Json.Serialization;

namespace GerenciamentoClientesStreaming.Models;

public class Servidor
{
    [JsonIgnore]
    public int ServidorId { get; set; }
    public string Nome { get; set; } = "";
}
