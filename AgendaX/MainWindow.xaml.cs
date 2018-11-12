using AgendaX.contextos;
using AgendaX.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AgendaX
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //Bloco resposável por preparar os dados para serem salvos no banco de dados
        public void Salvar(Object sender, RoutedEventArgs e)
        {


            if (ValidaCampos())
            {
                Contato contato = new Contato()
                {
                    Nome = nomeTbx.Text,
                    Endereço = enderecoTbx.Text,
                    Cep = cepTbx.Text,
                    Cidade = cidadeTbx.Text,
                    Estado = estadoTbx.Text,
                    Telefone = telTbx.Text,
                    Bairro = bairroTbx.Text
                };

                using (var contexto = new AgendaContexto())
                {
                    contexto.Contatos.Add(contato);
                    contexto.SaveChanges();
                    MessageBox.Show("Contato Incluido com  Sucesso!!");
                    LimpaCampos();
                }
            }
            else
            {
                MessageBox.Show("PREENCHA OS CAMPOS OBRIGATÓRIOS!!!");
            }


        }


        //Função responsável pela Consulta do cep passando uma String como parametro de Busca
        public void ConsultarCep(String cep)
        {
            using (var ws = new AgendaX.WebServiceCep.AtendeClienteClient())
            {
                try
                {
                    var resp = ws.consultaCEP(cep);

                    if (!resp.Equals(""))
                    {
                        enderecoTbx.Text = resp.end;
                        bairroTbx.Text = resp.bairro;
                        cidadeTbx.Text = resp.cidade;
                        estadoTbx.Text = resp.uf;

                        MessageBox.Show(" CEP Encontrado !!!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exceção: " + ex.Message);
                    cepTbx.Text = "";
                }

            }



        }


        //Função responsável por verificar se o Cep esta preenchido e se sim chamar a função que o consulta
        public void VerificarCep(Object sender, RoutedEventArgs e)
        {
            if (cepTbx.Text.Equals(""))
            {
                MessageBox.Show("Campo CEP VAZIO,PREENCHA-O");
            }
            else
            {
                ConsultarCep(cepTbx.Text);
            }

        }


        //Função responsável por validar o preenchimento dos campos do Formulário para serem salvos
        public bool ValidaCampos()
        {
            if (nomeTbx.Text.Equals("") || telTbx.Text.Equals("") ||
                enderecoTbx.Text.Equals("") || cepTbx.Text.Equals("") ||
                bairroTbx.Text.Equals("") || cidadeTbx.Text.Equals(""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        //Função responsável por Limpar os campos do Formulário
        public void LimpaCampos()
        {
            nomeTbx.Text = "";
            enderecoTbx.Text = "";
            estadoTbx.Text = "";
            telTbx.Text = "";
            cidadeTbx.Text = "";
            enderecoTbx.Text = "";
            cepTbx.Text = "";
            bairroTbx.Text = "";

        }

    }
}
