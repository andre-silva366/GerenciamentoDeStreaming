using GerenciamentoClientesStreaming.Models;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace GerenciamentoClientesStreaming.Repositories;

public class AplicativoRepository : IRepository<Aplicativo>
{
    private IDbConnection _connection;

    public AplicativoRepository()
    {
        _connection = new SqlConnection(@"Server=ANDRE-SILVA366\SQLEXPRESS;Initial Catalog=streaming;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
    }

    public List<Aplicativo> Get()
    {
        try
        {
            _connection.Open();
            string query = "SELECT * FROM Aplicativos";
            SqlCommand command = new SqlCommand(query,(SqlConnection) _connection);

            SqlDataReader reader = command.ExecuteReader();

            List<Aplicativo> aplicativos = new List<Aplicativo>();

            while(reader.Read())
            {
                Aplicativo aplicativo = new Aplicativo();

                aplicativo.AplicativoId = reader.GetInt32(0);
                aplicativo.Nome = reader.GetString(1);

                aplicativos.Add(aplicativo);
            }

            return aplicativos;
        }
        catch
        {
            throw new Exception("Ocorreu um erro ao retornar os dados.");
        }
        finally
        {
            _connection.Close();
        }
    }

    public Aplicativo Get(int id)
    {
        try
        {
            _connection.Open();
            string query = "SELECT * FROM Aplicativos WHERE Aplicativo_id = @Id;";
            SqlCommand command = new SqlCommand(query,(SqlConnection) _connection);

            Aplicativo aplicativo = new Aplicativo();

            command.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                throw new Exception($"Não encontrado aplicativo com id: {id}");              
            }

            aplicativo.AplicativoId = reader.GetInt32(0);
            aplicativo.Nome = reader.GetString(1);

            return aplicativo;

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

    public void Post(Aplicativo aplicativo)
    {
        try
        {
            _connection.Open();
            SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();
            
            try
            {
                string query = "INSERT INTO Aplicativos (Nome) VALUES (@Nome); SELECT CAST (scope_identity() AS int);";
                SqlCommand command = new (query, (SqlConnection) _connection, transaction);

                command.Parameters.AddWithValue("@Nome", aplicativo.Nome);

                aplicativo.AplicativoId = (int) command.ExecuteScalar();

                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch(Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void Put(Aplicativo aplicativo)
    {
        try
        {
            _connection.Open();
            SqlTransaction transaction = (SqlTransaction) _connection.BeginTransaction();
            
            try
            {
                string query = "UPDATE Aplicativos SET Nome = @Nome WHERE Aplicativo_id = @Id";
                SqlCommand command = new SqlCommand(query, (SqlConnection)_connection, transaction);

                command.Parameters.AddWithValue("@Id", aplicativo.AplicativoId);
                command.Parameters.AddWithValue("@Nome", aplicativo.Nome);

                if(command.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Erro ao atualizar os dados.");
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
            
        }
        catch (Exception e)
        {
            
            throw new Exception(e.Message);
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
            _connection.Open();
            string query = "DELETE Aplicativos WHERE Aplicativo_id = @Id";
            SqlCommand command = new SqlCommand(query, (SqlConnection)_connection);

            command.Parameters.AddWithValue("@Id", id);
            if(command.ExecuteNonQuery() == 0)
            {
                throw new Exception($"Não foi encontrado o aplicativo com id = {id}");
            }
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
        
}
