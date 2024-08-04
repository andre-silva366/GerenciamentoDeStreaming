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

            command.CommandText = "SELECT c.Cliente_id, c.Nome, c.Telefone, c.Email, c.Valor, c.Data_Ultimo_Pagamento, c.Data_Proximo_Pagamento ," +
                "s.Servidor_id, s.Nome AS Servidor ,a.Aplicativo_id, a.Nome AS Aplicativo , p.Plano_id , p.Nome AS Plano FROM Clientes c " +
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
                    cliente.ClienteId = dataReader.GetInt32(0);
                    cliente.Nome = dataReader.GetString(1);
                    cliente.Telefone = dataReader.GetString(2);
                    cliente.Email = dataReader.GetString(3);
                    cliente.Valor = dataReader.GetDecimal(4);
                    cliente.DataUltimoPagamento = dataReader.GetDateTime(5);
                    cliente.DataProximoPagamento = dataReader.GetDateTime(6);

                    Servidor servidor = new Servidor();
                    
                    servidor.ServidorId = dataReader.GetInt32(7);
                    servidor.Nome = dataReader.GetString(8);
                    cliente.Servidor = servidor;
                    clienteDictionary.Add(id, cliente);
                }
                else
                {
                    cliente = clienteDictionary[dataReader.GetInt32(0)];
                }

                Aplicativo aplicativo = new Aplicativo();
                aplicativo.AplicativoId = dataReader.GetInt32(9);
                aplicativo.Nome = dataReader.GetString(10);

                cliente.Aplicativo = aplicativo;

                Plano plano = new Plano();
                plano.PlanoId = dataReader.GetInt32(11);
                plano.Nome = dataReader.GetString(12);

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
        _connection.Open();
        SqlTransaction sqlTransaction = (SqlTransaction)_connection.BeginTransaction();

        try
        {
            DateTime date = DateTime.Now;
            DateTime dateP = DateTime.Now;
                       
            string query = "INSERT INTO Clientes (Nome, Telefone, Email, Servidor_id, Plano_id, Valor, Data_Ultimo_Pagamento,Data_Proximo_Pagamento, Ativo, Aplicativo_id) VALUES (@Nome ,@Telefone ,@Email ,@Servidor_id ,@Plano_id ,@Valor ,@Data_Ultimo_Pagamento ,@Data_Proximo_Pagamento , @Ativo, @Aplicativo_id ); " +
              "SELECT CAST (scope_identity() AS int);";

            SqlCommand command = new SqlCommand(query, (SqlConnection)_connection, sqlTransaction);

            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@Servidor_id", cliente.ServidorId);
            command.Parameters.AddWithValue("@Plano_id", cliente.PlanoId);
            command.Parameters.AddWithValue("@Valor", cliente.Valor);
            command.Parameters.AddWithValue("@Data_Ultimo_Pagamento", cliente.DataUltimoPagamento);
            command.Parameters.AddWithValue("@Data_Proximo_Pagamento", cliente.DataUltimoPagamento.AddMonths(cliente.PlanoId));
            command.Parameters.AddWithValue("@Ativo",1);
            command.Parameters.AddWithValue("@Aplicativo_id", cliente.AplicativoId);

            
            // Retorna a informação
            cliente.ClienteId = (int)command.ExecuteScalar();
            sqlTransaction.Commit();
        }

        catch (Exception ex)
        {
            try
            {
                sqlTransaction.Rollback();
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }
        finally
        {
            _connection.Close();
        }
    }

    public void Put(Cliente cliente)
    {
        _connection.Open();
        SqlTransaction sqlTransaction = (SqlTransaction)_connection.BeginTransaction();

        try
        {
            string query = $"UPDATE Clientes SET Nome = @Nome, Telefone = @Telefone, Email = @Email, Servidor_id = @Servidor_id, Plano_id = @Plano_id, Valor = @Valor, Data_Ultimo_Pagamento = @Data_Ultimo_Pagamento ,Data_Proximo_Pagamento = @Data_Proximo_Pagamento, Ativo = @Ativo, Aplicativo_id = @Aplicativo_id WHERE Cliente_id = {cliente.ClienteId}; " +
              "SELECT CAST (scope_identity() AS int);";

            SqlCommand command = new SqlCommand(query, (SqlConnection)_connection, sqlTransaction);

            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            command.Parameters.AddWithValue("@Email", cliente.Email);
            command.Parameters.AddWithValue("@Servidor_id", cliente.ServidorId);
            command.Parameters.AddWithValue("@Plano_id", cliente.PlanoId);
            command.Parameters.AddWithValue("@Valor", cliente.Valor);
            command.Parameters.AddWithValue("@Data_Ultimo_Pagamento", cliente.DataUltimoPagamento);
            command.Parameters.AddWithValue("@Data_Proximo_Pagamento", cliente.DataUltimoPagamento.AddMonths(cliente.PlanoId));
            command.Parameters.AddWithValue("@Ativo", 1);
            command.Parameters.AddWithValue("@Aplicativo_id", cliente.AplicativoId);

            command.Parameters.AddWithValue("@Cliente_id", cliente.ClienteId);

            command.ExecuteNonQuery();

            // Retorna a informação
            
            sqlTransaction.Commit();
        }

        catch (Exception ex)
        {
            try
            {
                sqlTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        finally
        {
            _connection.Close();
        }
    }

    public void Delete(Cliente cliente)
    {
        throw new NotImplementedException();
    }

    
}
