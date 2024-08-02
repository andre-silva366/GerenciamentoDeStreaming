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
        _connection = new SqlConnection(@"Server=ANDRE-SILVA366\SQLEXPRESS;Initial Catalog=streaming;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
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
        try
        {
            Cliente cliente = new Cliente();
            SqlCommand command = new SqlCommand();

            command.CommandText = "SELECT c.Nome, c.Telefone, c.Email, c.Valor, c.Data_Ultimo_Pagamento, c.Data_Proximo_Pagamento ," +
                "s.Nome AS Servidor ,a.Nome AS Aplicativo , p.Nome AS Plano FROM Clientes c " +
                "INNER JOIN Aplicativos a ON c.Aplicativo_id = a.Aplicativo_id " +
                "INNER JOIN Planos p ON p.Plano_id = c.Plano_id " +
                "INNER JOIN Servidores s ON s.Servidor_id = c.Servidor_id WHERE c.Cliente_id = @Id;";
            command.Connection =  (SqlConnection) _connection;

            command.Parameters.AddWithValue("@Id", id);

            _connection.Open();

            SqlDataReader dataReader = command.ExecuteReader();

            Dictionary<int, Cliente> clienteDictionary = new Dictionary<int, Cliente>();

            while(dataReader.Read())
            {
                if (!clienteDictionary.ContainsKey(id))
                {
                    cliente.Nome = dataReader.GetString(0);
                    cliente.Telefone = dataReader.GetString(1);
                    cliente.Email = dataReader.GetString(2);
                    cliente.Valor = dataReader.GetDecimal(3);
                    cliente.DataUltimoPagamento = dataReader.GetDateTime(4);
                    cliente.DataProximoPagamento = dataReader.GetDateTime(5);

                    Servidor servidor = new Servidor();
                    List<Servidor> servidores = new List<Servidor>(); 
                    servidor.Nome = dataReader.GetString(6);

                    servidores.Add(servidor);

                    cliente.Servidores = servidores;
                    clienteDictionary.Add(id, cliente);
                }
                else
                {
                    cliente = clienteDictionary[dataReader.GetInt32(0)];
                }

                Aplicativo aplicativo = new Aplicativo();
                List<Aplicativo> aplicativos = new List<Aplicativo>();

                aplicativo.Nome = dataReader.GetString(7);

                aplicativos.Add(aplicativo);
                cliente.Aplicativos = aplicativos;

                Plano plano = new Plano();
                plano.Nome = dataReader.GetString(8);

                cliente.Plano = plano;
            }

            return cliente;

        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            _connection.Close();
        }
        
        
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
