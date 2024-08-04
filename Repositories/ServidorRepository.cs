using GerenciamentoClientesStreaming.Models;
using System.Data.SqlClient;
using System.Data;

namespace GerenciamentoClientesStreaming.Repositories;

public class ServidorRepository : IRepository<Servidor>
{
    // Conexão do ADO
    private IDbConnection _connection;

    public ServidorRepository()
    {
        _connection = new SqlConnection(@"Server=ANDRE-SILVA366\SQLEXPRESS;Initial Catalog=streaming;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
    }

    public List<Servidor> Get()
    {
        try
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT * FROM Servidores;";
            command.Connection = (SqlConnection) _connection;

            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();
                        
            List<Servidor> servidores = new List<Servidor>();

            while(reader.Read())
            {
                Servidor servidor = new Servidor();
                servidor.ServidorId = reader.GetInt32(0);
                servidor.Nome = reader.GetString(1);

                servidores.Add(servidor);
            }            

            return servidores;

        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            _connection.Close();
        }

    }

    public Servidor Get(int id)
    {
        try
        {
            if(id <= 0)
            {
                throw new Exception("Id invalido");
            }
            SqlCommand command = new SqlCommand();
            command.Connection = (SqlConnection) _connection;
            command.CommandText = $"SELECT * FROM Servidores WHERE Servidor_id = {id}";

            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            Servidor servidor = new Servidor();

            while (reader.Read())
            {
                servidor.ServidorId = reader.GetInt32(0);
                servidor.Nome = reader.GetString(1);
            }

            if(servidor.ServidorId  == 0)
            {
                throw new Exception($"Servidor com o id: {id} não encontrado.");
            }

            return servidor;
        }
        
        finally
        {
            _connection.Close();
        }
    }

    public void Post(Servidor servidor)
    {
        SqlCommand command = new SqlCommand();
        _connection.Open();
        SqlTransaction transaction = (SqlTransaction) _connection.BeginTransaction();

        try
        {
            command.Connection = (SqlConnection)_connection;
            command.CommandText = "INSERT INTO Servidores (Nome) VALUES (@Nome); SELECT CAST (scope_identity() AS int);";
            command.Transaction = transaction;
            
            command.Parameters.AddWithValue("@Nome", servidor.Nome);
            
            // Retorna a informação com o ID
            servidor.ServidorId = (int)command.ExecuteScalar();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw new Exception("O Servidor não foi inserido no banco de dados");
        }
        finally
        {
            _connection.Close();
        }
    }

    public void Put(Servidor servidor)
    {
        SqlCommand command = new SqlCommand();
        _connection.Open();
        SqlTransaction transaction = (SqlTransaction) _connection.BeginTransaction();

        try
        {
            command.CommandText = $"UPDATE Servidores SET Nome = @Nome WHERE Servidor_id = @Id;" ;
            command.Transaction = transaction;
            command.Connection = (SqlConnection) _connection;

            command.Parameters.AddWithValue("@Id", servidor.ServidorId);
            command.Parameters.AddWithValue("@Nome", servidor.Nome);

            command.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw new Exception("Erro ao atualizar os dados.");
        }
        finally
        {
            _connection.Close();
            transaction.Dispose();
        }

    }

    public void Delete(int id)
    {
        SqlCommand command = new SqlCommand();
        _connection.Open();

        try
        {
            command.CommandText = $"DELETE Servidores WHERE Servidor_id = {id}";
            command.Connection = (SqlConnection)_connection;
                        
            if (command.ExecuteNonQuery() == 0)
            {
                throw new Exception($"Não existe servidor com o id: {id}");
            }
        }
        finally
        {
            _connection.Close();
        }
    }


}
