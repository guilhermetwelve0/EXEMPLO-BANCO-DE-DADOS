using System;
using System.Data.SqlClient;
using static System.Console;

namespace CSharpAdoNet
{
    class Program
    {
        static void Main(string[] args)
        {

            WriteLine("========================= CONTROLE DE CLIENTES ==========================\n");
            WriteLine("Selecione uma opção:");
            WriteLine("1- Listar");
            WriteLine("2- Cadastrar");
            WriteLine("3- Editar");
            WriteLine("4- Excluir");
            WriteLine("5- Visualizar");
            Write("Opção: ");
            int opc = Convert.ToInt32(ReadLine());
            Clear();
            switch(opc)
            {
                case 1:
                    Title = "Listagem de Clientes";
                    WriteLine("========================== LISTAGEM DE CLIENTES ===========================\n");
                    ListarClientes();
                    break;
                case 2:
                    Title = "Novo Cliente";
                    WriteLine("========================== CADASTRO DE NOVO CLIENTE ===========================\n");

                    Write("Informe um nome: ");
                    string nome = ReadLine();

                    Write("Informe um e-mail: ");
                    string email = ReadLine();

                    SalvarCliente(nome, email);

                    break;
                case 3:
                    Title = "Alteração de Cliente";
                    WriteLine("========================== ALTERAÇÃO DE CLIENTE ===========================\n");

                    ListarClientes();

                    Write("Selecione um cliente pela ID: ");
                    int idOp = Convert.ToInt32(ReadLine());
                    (int _id, string _nome, string _email) = SelecionarCliente(idOp);
                    Clear();

                    Title = "Alteração de Cliente - " + _nome;
                    WriteLine($"========================== ALTERAÇÃO DE CLIENTE - {_nome} ===========================\n");

                    Write("Informe um nome: ");
                    nome = ReadLine();

                    Write("Informe um e-mail: ");
                    email = ReadLine();

                    nome = nome.Equals("") ? _nome : nome;
                    email = email.Equals("") ? _email : email;

                    SalvarCliente(nome, email, idOp);

                    break;
                case 4:
                    Title = "Exclusão de Cliente";
                    WriteLine("========================== EXCLUSÃO DE CLIENTE ===========================\n");
                    ListarClientes();

                    Write("Selecione um cliente pela ID: ");
                    idOp = Convert.ToInt32(ReadLine());
                    (_id, _nome, _email) = SelecionarCliente(idOp);
                    Clear();

                    Title = "Exclusão de Cliente - " + _nome;
                    WriteLine($"========================== EXCLUSÃO DE CLIENTE - {_nome} ===========================\n");
                    WriteLine("\n**************************** ATENÇÃO ****************************\n");
                    Write("Deseja continuar? (S para SIM, N para NÃO): ");
                    string confirmar = ReadLine().ToUpper();

                    if(confirmar.Equals("S"))
                    {
                        DeletarCliente(idOp);
                    }

                    break;
                case 5:
                    Title = "Visualização de Cliente";
                    WriteLine("========================== VER DETALHES DE CLIENTE ===========================\n");
                    ListarClientes();

                    Write("Selecione um cliente pela ID: ");
                    idOp = Convert.ToInt32(ReadLine());
                    (_id, _nome, _email) = SelecionarCliente(idOp);
                    Clear();

                    Title = "Visualização de Cliente - " + _nome;
                    WriteLine($"========================== VER DETALHES DE CLIENTE - {_nome} ===========================\n");

                    WriteLine("ID: {0}", _id);
                    WriteLine("Nome: {0}", _nome);
                    WriteLine("E-mail: {0}", _email);

                    break;
                default:
                    Title = "Opção Inválida";
                    WriteLine("========================== SELECIONE UMA OPÇÃO VÁLIDA ===========================\n");
                    break;

            }

            ReadLine();
            
        }

        static void ListarClientes()
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString)) 
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes order by id";

                using(SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        WriteLine("ID: {0}", dr["id"]);
                        WriteLine("Nome: {0}", dr["nome"]);
                        WriteLine("---------------------------------------");
                    }

                }

            }
        }

        static void SalvarCliente(string nome, string email)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into clientes (nome, email) values (@nome, @email)";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
            }
        }

        static void SalvarCliente(string nome, string email, int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update clientes set nome = @nome, email = @email where id = @id";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        static void DeletarCliente(int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete from clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        static (int, string, string) SelecionarCliente(int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    return (Convert.ToInt32(dr["id"].ToString()), dr["nome"].ToString(), dr["email"].ToString());
                }
            }
        }
        static string getStringConn()
        {
            string connString = "Server=LAPTOP-65P1PT7M\\SQLEXPRESS;Database=CsharpAdoNET;User Id=sa;Password=112233";
            return connString;
        }
    }
}
