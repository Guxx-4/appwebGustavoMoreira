
using appwebGustavoMoreira.Configs;
using appwebGustavoMoreira.Models;
using MySql.Data.MySqlClient;

namespace appwebGustavoMoreira.DAO;

public class ProcessoDAO
{
    private readonly Conexao _conexao;

    public ProcessoDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    // Cadastra um novo processo
    public void Inserir(Processo processo)
    {
        using var con = _conexao.GetConnection();

        const string sql = @"INSERT INTO processos
            (numero_pro, data_pro, interessado_pro, assunto_pro,
             descricao_pro, situacao_pro)
            VALUES
            (@numero, @data, @interessado, @assunto,
             @descricao, @situacao)";

        using var comando = con.CreateCommand();

        comando.CommandText = sql;

        comando.Parameters.AddWithValue("@numero", processo.Numero);
        comando.Parameters.AddWithValue("@data",
            processo.Data!.Value.ToDateTime(TimeOnly.MinValue));
        comando.Parameters.AddWithValue("@interessado", processo.Interessado);
        comando.Parameters.AddWithValue("@assunto", processo.Assunto);
        comando.Parameters.AddWithValue("@descricao", processo.Descricao);
        comando.Parameters.AddWithValue("@situacao", processo.Situacao);

        comando.ExecuteNonQuery();
    }

    // Lista todos os processos cadastrados
    public List<Processo> Listar()
    {
        using var con = _conexao.GetConnection();

        const string sql = @"SELECT id_pro, numero_pro, data_pro,
            interessado_pro, assunto_pro, descricao_pro, situacao_pro
            FROM processos
            ORDER BY id_pro DESC";

        using var comando = con.CreateCommand();

        comando.CommandText = sql;

        using var leitor = comando.ExecuteReader();

        List<Processo> processos = new();

        while (leitor.Read())
        {
            processos.Add(new Processo
            {
                Id = leitor.GetInt32(leitor.GetOrdinal("id_pro")),
                Numero = leitor.GetString(leitor.GetOrdinal("numero_pro")),
                Data = DateOnly.FromDateTime(
                    leitor.GetDateTime(leitor.GetOrdinal("data_pro"))),
                Interessado = leitor.GetString(leitor.GetOrdinal("interessado_pro")),
                Assunto = leitor.GetString(leitor.GetOrdinal("assunto_pro")),
                Descricao = leitor.GetString(leitor.GetOrdinal("descricao_pro")),
                Situacao = leitor.GetString(leitor.GetOrdinal("situacao_pro"))
            });
        }

        return processos;
    }

    // Busca um processo pelo ID
    public Processo? ObterPorId(int id)
    {
        using var con = _conexao.GetConnection();

        const string sql = @"SELECT id_pro, numero_pro, data_pro,
            interessado_pro, assunto_pro, descricao_pro, situacao_pro
            FROM processos
            WHERE id_pro = @id";

        using var comando = con.CreateCommand();

        comando.CommandText = sql;
        comando.Parameters.AddWithValue("@id", id);

        using var leitor = comando.ExecuteReader();

        if (!leitor.Read())
        {
            return null;
        }

        return new Processo
        {
            Id = leitor.GetInt32(leitor.GetOrdinal("id_pro")),
            Numero = leitor.GetString(leitor.GetOrdinal("numero_pro")),
            Data = DateOnly.FromDateTime(
                leitor.GetDateTime(leitor.GetOrdinal("data_pro"))),
            Interessado = leitor.GetString(leitor.GetOrdinal("interessado_pro")),
            Assunto = leitor.GetString(leitor.GetOrdinal("assunto_pro")),
            Descricao = leitor.GetString(leitor.GetOrdinal("descricao_pro")),
            Situacao = leitor.GetString(leitor.GetOrdinal("situacao_pro"))
        };
    }

    // Atualiza os dados de um processo
    public void Atualizar(Processo processo)
    {
        using var con = _conexao.GetConnection();

        const string sql = @"UPDATE processos
            SET numero_pro = @numero,
                data_pro = @data,
                interessado_pro = @interessado,
                assunto_pro = @assunto,
                descricao_pro = @descricao,
                situacao_pro = @situacao
            WHERE id_pro = @id";

        using var comando = con.CreateCommand();

        comando.CommandText = sql;

        comando.Parameters.AddWithValue("@numero", processo.Numero);
        comando.Parameters.AddWithValue("@data",
            processo.Data!.Value.ToDateTime(TimeOnly.MinValue));
        comando.Parameters.AddWithValue("@interessado", processo.Interessado);
        comando.Parameters.AddWithValue("@assunto", processo.Assunto);
        comando.Parameters.AddWithValue("@descricao", processo.Descricao);
        comando.Parameters.AddWithValue("@situacao", processo.Situacao);
        comando.Parameters.AddWithValue("@id", processo.Id);

        comando.ExecuteNonQuery();
    }

    // Exclui um processo pelo ID
    public void Excluir(int id)
    {
        using var con = _conexao.GetConnection();

        const string sql = "DELETE FROM processos WHERE id_pro = @id";

        using var comando = con.CreateCommand();

        comando.CommandText = sql;
        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }
}
