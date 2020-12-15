using System;
using System.IO;
using System.Windows.Forms;

namespace controlit.loja
{
    public partial class frmCadProduto : Form
    {
        string[] aCodProduto = new string[100];
        string[] aNomeProduto = new string[100];
        decimal[] aPrecoProduto = new decimal[100];
        string[] aLocalizacao = new string[100];
        string[] aFornecedor = new string[100];
        DateTime[] aDatadaUltCompra = new DateTime[100];
        decimal[] aVlrUltCompra = new decimal[100];
        bool erro;

        // Desabilita os campos para edição
        private void disable_txtboxes()
        {
            mtbCodProd.Enabled = false;
            txtnomeProduto.Enabled = false;
            txtlocalizacao.Enabled = false;
            txtfornecedor.Enabled = false;
            txtvalorUltimaCompra.Enabled = false;
            txtprecoVenda.Enabled = false;
            DataUltimaCompra.Enabled = false;
        }

        // Habilita os campos para edição
        private void enable_txtboxes()
        {
            mtbCodProd.Enabled = true;
            txtnomeProduto.Enabled = true;
            txtlocalizacao.Enabled = true;
            txtfornecedor.Enabled = true;
            txtvalorUltimaCompra.Enabled = true;
            txtprecoVenda.Enabled = true;
            DataUltimaCompra.Enabled = true;
            mtbCodProd.Focus();
        }

        private void atualizaCampos()
        {
            int linhaDgv = Convert.ToInt32(dgvProdutos.CurrentRow.Index + 1);
            label9.Text = Convert.ToString(linhaDgv);
            label9.Visible = true;


            //Lista todos os dados nos textboxes
            mtbCodProd.Text = aCodProduto[linhaDgv].Trim();
            txtnomeProduto.Text = aNomeProduto[linhaDgv].Trim();
            txtprecoVenda.Text = Convert.ToString(aPrecoProduto[linhaDgv]);
            txtlocalizacao.Text = aLocalizacao[linhaDgv].Trim();
            txtfornecedor.Text = aFornecedor[linhaDgv].Trim();
            DataUltimaCompra.Value = aDatadaUltCompra[linhaDgv];
            txtvalorUltimaCompra.Text = Convert.ToString(aVlrUltCompra[linhaDgv]);
        }

        private void tratarCampos()
        {

            // ------------trata os caracteres usados no campo preco venda produto
            string nova = "";
            string a = "";
            for (int t = 0; t <= txtprecoVenda.TextLength - 1; t++)
            {
                a = txtprecoVenda.Text.Substring(t, 1);
                if (!(a != "0" && a != "1" && a != "2" && a != "3" && a != "4" && a != "5" && a != "6" &&
                    a != "7" && a != "8" && a != "9" && a != ","))
                {
                    nova = nova + a;
                }
                else
                {
                    MessageBox.Show("Valor de venda incorreto");
                    erro = true;
                    return;
                }
            }
            

            //---------Verifica se o valor de venda é menor que o de compra
            if (Convert.ToDecimal(txtprecoVenda.Text) < Convert.ToDecimal(txtvalorUltimaCompra.Text))
            {
                MessageBox.Show("Valor de Venda nao pode ser menos que o valor de compra !");
                erro = true;
            }
            txtlocalizacao.Text = txtlocalizacao.Text.ToUpper();

            //--------------- verifica se localizacao foi digitada corretamente
            if (!txtlocalizacao.Text.Contains("S") || !txtlocalizacao.Text.Contains("A") || !txtlocalizacao.Text.Contains("P"))
            {
                MessageBox.Show("Localização incompleta ! - Redigite");
                erro = true;
            }

           
        }


        public frmCadProduto()
        {
            InitializeComponent();
        }

        private void frmCadProduto_Load(object sender, EventArgs e)
        {


            StreamReader vLeitor = new StreamReader(@"C:\temp\Produtos.txt");


            int i = 0;
            while (!vLeitor.EndOfStream)
            {
                string registro = vLeitor.ReadLine();
                i++;
                aCodProduto[i] = registro.Substring(0, 10);
                aNomeProduto[i] = registro.Substring(10, 20);
                aPrecoProduto[i] = Convert.ToDecimal(registro.Substring(30, 14));
                aLocalizacao[i] = registro.Substring(44, 10);
                aFornecedor[i] = registro.Substring(54, 15);
                aDatadaUltCompra[i] = Convert.ToDateTime(registro.Substring(69, 10));
                aVlrUltCompra[i] = Convert.ToDecimal(registro.Substring(79, 14));

                dgvProdutos.Rows.Add(aCodProduto[i], aNomeProduto[i], aFornecedor[i]);
            }

            disable_txtboxes();


        }


        private void dgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvProdutos_SelectionChanged(object sender, EventArgs e)
        {
            disable_txtboxes();
            atualizaCampos();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            enable_txtboxes();
            atualizaCampos();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            erro = false;
            tratarCampos();
            if (erro)
            {
                return;
            }
        }

        private void txtlocalizacao_TextChanged(object sender, EventArgs e)
        {
           // txtlocalizacao.Text = txtlocalizacao.Text.ToUpper();
        }

        private void txtprecoVenda_TextChanged(object sender, EventArgs e)
        {

           
        }
    }
}
