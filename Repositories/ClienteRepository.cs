using GerenciamentoClientesStreaming.Models;
using System.Data;
using System.Data.SqlClient;

namespace GerenciamentoClientesStreaming.Repositories;

public class ClienteRepository : IClienteRepository
{
    // Conexão do ADO
    private IDbConnection _connection;

    public ClienteRepository()
    {
        _connection = new SqlConnection(@"Server=ANDRE-SILVA366\SQLEXPRESS;Initial Catalog=iptv;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
    }

    public List<Cliente> Get()
    {
        SqlCommand command;
        List<Cliente> clientes = new List<Cliente>();

        try
        {            
            command = new SqlCommand("SELECT c.Cliente_id, c.Nome, c.Telefone, c.Valor, c.Data_Ultimo_Pagamento, c.Data_Proximo_Pagamento" +
                                     " FROM Clientes c " , (SqlConnection)_connection);

            _connection.Open();

            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                Cliente cliente = new Cliente();
                
                cliente.ClienteId = dataReader.GetInt32(0);
                cliente.Nome = dataReader.GetString("Nome");
                cliente.Telefone = dataReader.GetString("Telefone");
                cliente.Valor = dataReader.GetDecimal("Valor");                
                cliente.DataUltimoPagamento = dataReader.GetDateTime(4);
                cliente.DataProximoPagamento = dataReader.GetDateTime(5);

                clientes.Add(cliente);
            }

            return clientes;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            _connection.Close();
        }
    }

    public Cliente Get(int id)
    {
        //command = new SqlCommand("SELECT c.Nome, c.Telefone, c.Valor, c.Data_Ultimo_Pagamento, " +
        //        "a.Nome AS Aplicativo , p.Nome AS Plano, s.Nome AS Servidor FROM Clientes c " +
        //        "INNER JOIN Aplicativos a ON c.Aplicativo_id = a.Aplicativo_id" +
        //        "INNER JOIN Planos p ON p.Plano_id = c.Plano_id" +
        //        "INNER JOIN Servidores s ON s.Servidor_id = c.Servidor_id;",
                //(SqlConnection)_connection);
        throw new NotImplementedException();
    }

    public void Post(Cliente cliente)
    {
        throw new NotImplementedException();
    }

    public void Put(Cliente cliente)
    {
        throw new NotImplementedException();
    }

    public void Delete(Cliente cliente)
    {
        throw new NotImplementedException();
    }

    
}
