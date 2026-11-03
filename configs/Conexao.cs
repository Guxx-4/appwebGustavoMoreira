
using MySql.Data.MySqlClient;

namespace appwebGustavoMoreira.Configs;

public class Conexao
{
    private readonly IConfiguration _configuration;

    public Conexao(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MySqlConnection GetConnection()
    {
        string conexao = _configuration.GetConnectionString("ConexaoMySQL")!;

        var con = new MySqlConnection(conexao);

        con.Open();

        return con;
    }
}
