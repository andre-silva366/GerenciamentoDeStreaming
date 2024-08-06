using GerenciamentoClientesStreaming.Models;
using System.Data;
using System.Data.SqlClient;

namespace GerenciamentoClientesStreaming.Repositories;

public class PlanoRepository : IRepository<Plano>
{
    private IDbConnection _connection;

    public PlanoRepository()
    {
        _connection = new SqlConnection(@"Server=ANDRE-SILVA366\SQLEXPRESS;Initial Catalog=streaming;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
    }

    public List<Plano> Get()
    {
        try
        {
            _connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT * FROM Planos";
            command.Connection = (SqlConnection) _connection;
            SqlDataReader reader = command.ExecuteReader();

            List<Plano> planos = new();

            while (reader.Read())
            {
                Plano plano = new Plano();
                plano.PlanoId = reader.GetInt32(0);
                plano.Nome = reader.GetString(1);

                planos.Add(plano);
            }

            return planos;
        }
        finally
        {
            _connection.Close();
        }

        
    }

    public Plano Get(int id)
    {
        try
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = $"SELECT * FROM Planos WHERE Plano_id = {id}";
            command.Connection = (SqlConnection)_connection;
            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            Plano plano = new Plano();

            while (reader.Read())
            {
                plano.PlanoId = reader.GetInt32(0);
                plano.Nome = reader.GetString(1);
            }

            if(plano.PlanoId == 0)
            {
                throw new Exception($"O Plano de id: {id} não foi encontrado.");
            }

            return plano;
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

    public void Post(Plano plano)
    {
        _connection.Open();
        SqlTransaction transaction = (SqlTransaction) _connection.BeginTransaction();

        try
        {            
            SqlCommand command = new SqlCommand();
            command.CommandText = "INSERT INTO Planos (Nome) VALUES (@Nome) ; SELECT CAST (scope_identity() AS int);";
            command.Connection = (SqlConnection) _connection;
            
            command.Transaction = transaction;

            command.Parameters.AddWithValue("@Nome", plano.Nome);
            plano.PlanoId = (int)command.ExecuteScalar();

            transaction.Commit();
                        
        }
        catch(Exception ex)
        {
            transaction.Rollback();
            throw new Exception(ex.Message);
        }
        finally
        {
            _connection.Close();
        }
    }

    public void Put(Plano plano)
    {
        _connection.Open();
        SqlTransaction transaction = (SqlTransaction) _connection.BeginTransaction();

        try
        {
            string query = "UPDATE Planos SET Nome = @Nome WHERE Plano_id = @Id; SELECT CAST (scope_identity() AS int) ";
            SqlCommand command = new (query, (SqlConnection)_connection, transaction);

            command.Parameters.AddWithValue("@Nome",plano.Nome);
            command.Parameters.AddWithValue("@Id",plano.PlanoId);

            command.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw new Exception("Erro ao atualizar os dados");
        }
        finally
        {
            _connection.Close();
        }
    }

    public void Delete(int id)
    {
        try
        {
            string query = $"DELETE Planos WHERE Plano_id = {id}";
            SqlCommand command = new (query,(SqlConnection)_connection);
            _connection.Open();
            
            if(command.ExecuteNonQuery() == 0)
            {
                throw new Exception($"Plano com id: {id} não encontrado.");
            }
        }
        
        finally
        {
            _connection.Close();
        }
    }


}
