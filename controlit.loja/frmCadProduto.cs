using System;
using System.IO;
using System.Windows.Forms;

namespace controlit.loja
{
    public partial class frmCadProduto : Form
    {
        // ------- Declaracao de vetores e variaveis
        string[] aCodProduto = new string[100];
        string[] aNomeProduto = new string[100];
        decimal[] aPrecoProduto = new decimal[100];
        string[] aLocalizacao = new string[100];
        string[] aFornecedor = new string[100];
        DateTime[] aDatadaUltCompra = new DateTime[100];
        decimal[] aVlrUltCompra = new decimal[100];
        bool erro;
        string modo;
        int arrayIndex = 0;

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

        private void inclusao()
        {
            modo = "N";
            toolStripStatusLabel1.Text = "Incluir novo Produto";
            enable_txtboxes();
            limparCampos();



        }

        private void gravacao()
        {
            erro = false;

            //--------------- Inicia gravação

            if (modo == "A")
            {
                //---Pega a linha selecionada no DataGrid
                int linhadgv = dgvProdutos.CurrentRow.Index + 1;


                tratarCampos();
                if (erro)
                {
                    return;
                }
                else
                {
                    gravarArray(linhadgv);
                    disable_txtboxes();
                    atualizaCampos();
                    dgvProdutos.CurrentRow.SetValues(aCodProduto[linhadgv], aNomeProduto[linhadgv], aFornecedor[linhadgv]);
                    toolStripStatusLabel1.Text = "Dados alterados com Sucesso";
                    modo = "";
                }
            }
            else if (modo == "N")
            {
                tratarCampos();
                if (erro)
                {
                    return;
                }
                else
                {
                    int linhaDgv = dgvProdutos.Rows.Count + 1;
                    gravarArray(linhaDgv);


                    dgvProdutos.Rows.Add(aCodProduto[linhaDgv], aNomeProduto[linhaDgv], aFornecedor[linhaDgv]);


                    disable_txtboxes();
                    toolStripStatusLabel1.Text = "Produto incluido com Sucesso";
                    modo = "";
                    //----- seleciona a linha adicionada
                    dgvProdutos[0, linhaDgv - 1].Selected = true;
                }
            }
            else if (modo == "E")
            {
                int linhadgv = dgvProdutos.CurrentRow.Index + 1;

                for (int c = 0; c <= aCodProduto.Length - 2; c++)
                {
                    if (linhadgv <= c)
                    {

                        aCodProduto[c] = aCodProduto[c + 1];
                        aNomeProduto[c] = aNomeProduto[c + 1];
                        aPrecoProduto[c] = aPrecoProduto[c + 1];
                        aLocalizacao[c] = aLocalizacao[c + 1];
                        aFornecedor[c] = aFornecedor[c + 1];
                        aDatadaUltCompra[c] = aDatadaUltCompra[c + 1];
                        aVlrUltCompra[c] = aVlrUltCompra[c + 1];
                    }
                }
                modo = "";
                toolStripStatusLabel1.Text = String.Format("Registro excluido com sucesso!");

                gridupdateRow();




            }

        }
        private void alteracao()
        {
            enable_txtboxes();
          //  atualizaCampos();
            
        }

        private void exclusao()
        {
            modo = "E";
            MessageBox.Show(String.Format("Para confirmar a exclusao de: {0}", txtnomeProduto.Text, "Exclusão de Produto", MessageBoxButtons.OK, MessageBoxIcon.Information));
            toolStripStatusLabel1.Text = "Exclusão de produto";


        }

        private void pesquisar()
        {

            for (int i = 1 ; i < aCodProduto.Length - 1; i++)
            {
                if (aCodProduto[i] == null || txtPesquisar.Text == "")
                {
                    MessageBox.Show("Item nao localizado !", "Não Localizado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (txtPesquisar.Text.ToUpper()  == aCodProduto[i].Trim().ToUpper() 
                    || aNomeProduto[i].Trim().ToUpper().Contains(txtPesquisar.Text.ToUpper()))
                {
                    dgvProdutos[0, i - 1].Selected = true;
                    atualizaCampos();
                    return;
                }
                

            }
            

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

        private void limparCampos()
        {
            mtbCodProd.Clear();
            txtnomeProduto.Clear();
            txtprecoVenda.Clear();
            txtlocalizacao.Clear();
            txtfornecedor.Clear();
            DataUltimaCompra.Value = DateTime.Now;
            txtvalorUltimaCompra.Clear();
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
                    desabilitaMenu();
                    alteracao();

                    return;

                }
            }


            //---------Verifica se o valor de venda é menor que o de compra
            if (Convert.ToDecimal(txtprecoVenda.Text) < Convert.ToDecimal(txtvalorUltimaCompra.Text))
            {
                MessageBox.Show("Valor de Venda nao pode ser menor que o valor de compra !");
                erro = true;
                desabilitaMenu();
                alteracao();
                


            }
            else if (txtprecoVenda.Text == "")
            {
                MessageBox.Show("O valor nao pode ser vazio");
                erro = true;
                desabilitaMenu();
                alteracao();
            }
            txtlocalizacao.Text = txtlocalizacao.Text.ToUpper();

            //--------------- verifica se localizacao foi digitada corretamente
            if (!txtlocalizacao.Text.Contains("S") || !txtlocalizacao.Text.Contains("A") || !txtlocalizacao.Text.Contains("P"))
            {
                MessageBox.Show("Localização incompleta ! - Redigite");
                erro = true;
                desabilitaMenu();
                alteracao();


            }



        }
        //-------Atualiza data grid
        private void gridupdateRow()
        {
            dgvProdutos.Rows.Clear();

            for (int c = 0; c <= aCodProduto.Length - 1; c++)
            {
                if (aCodProduto[c] != null)
                {
                    dgvProdutos.Rows.Add(aCodProduto[c], aNomeProduto[c], aFornecedor[c]);
                }
            }




        }

        private void gravarArray(int linhadgv)
        {

            aCodProduto[linhadgv] = string.Format("{0}          ",mtbCodProd.Text).Substring(0,10);
            aNomeProduto[linhadgv] = string.Format("{0}                    ", txtnomeProduto.Text).Substring(0, 20); 
            aPrecoProduto[linhadgv] = Convert.ToDecimal(txtprecoVenda.Text);
            aLocalizacao[linhadgv] = string.Format("{0}          ", txtlocalizacao.Text).Substring(0, 10); 
            aFornecedor[linhadgv] = string.Format("{0}               ", txtfornecedor.Text).Substring(0, 15); 
            aDatadaUltCompra[linhadgv] = DataUltimaCompra.Value;
            aVlrUltCompra[linhadgv] = Convert.ToDecimal(txtvalorUltimaCompra.Text);



        }

        //-------- Grava Arquivo Sequencial
        private void gravaArqSeq()
        {
            StreamWriter vgravador = new StreamWriter(@"c:\temp\Produtos.txt");

            for (int da = 1; da < aCodProduto.Length; da++)
            {
                if (aCodProduto[da] != null)
                {



                    string linhaDados = "";
                    linhaDados += aCodProduto[da];
                    linhaDados += aNomeProduto[da];
                    linhaDados += String.Format("              {0}", Convert.ToString(aPrecoProduto[da])).Substring(Convert.ToString(aPrecoProduto[da]).Length, 14);
                    linhaDados += aLocalizacao[da];
                    linhaDados += aFornecedor[da];
                    linhaDados += Convert.ToString(aDatadaUltCompra[da]).Substring(0, 10);
                    linhaDados += String.Format("              {0}", Convert.ToString(aVlrUltCompra[da])).Substring(Convert.ToString(aVlrUltCompra[da]).Length, 14);
                    vgravador.WriteLine(linhaDados);
                }
            }
            vgravador.Close();
        }

        private void habilitaMenu()
        {
            btnNovo.Enabled = true;
            btnAlterar.Enabled = true;
            btnExcluir.Enabled = true;
            btnGravar.Enabled = false;
            btnCancelar.Enabled = false;

        }
        private void desabilitaMenu()
        {
            btnNovo.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnGravar.Enabled = true;
            btnCancelar.Enabled = true;
        }


        public frmCadProduto()
        {
            InitializeComponent();
        }




        //----------Inicializa o Form
        private void frmCadProduto_Load(object sender, EventArgs e)
        {

            //-------- Faz leitura do arquivo e preenche o data grid
            StreamReader vLeitor = new StreamReader(@"C:\temp\Produtos.txt");
            arrayIndex = 0;
            while (!vLeitor.EndOfStream)
            {
                string registro = vLeitor.ReadLine();
                arrayIndex++;
                aCodProduto[arrayIndex] = registro.Substring(0, 10);
                aNomeProduto[arrayIndex] = registro.Substring(10, 20);
                aPrecoProduto[arrayIndex] = Convert.ToDecimal(registro.Substring(30, 14));
                aLocalizacao[arrayIndex] = registro.Substring(44, 10);
                aFornecedor[arrayIndex] = registro.Substring(54, 15);
                aDatadaUltCompra[arrayIndex] = Convert.ToDateTime(registro.Substring(69, 10));
                aVlrUltCompra[arrayIndex] = Convert.ToDecimal(registro.Substring(79, 14));

                dgvProdutos.Rows.Add(aCodProduto[arrayIndex], aNomeProduto[arrayIndex], aFornecedor[arrayIndex]);

            }
            vLeitor.Close();


            disable_txtboxes();
            habilitaMenu();

            // Inicializa variavel de modo
            modo = "";
            toolStripStatusLabel1.Text = "Pronto";


        }


        private void dgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvProdutos_SelectionChanged(object sender, EventArgs e)
        {
            disable_txtboxes();
            atualizaCampos();
            modo = "";
            toolStripStatusLabel1.Text = "Pronto";

        }





        private void txtlocalizacao_TextChanged(object sender, EventArgs e)
        {
            // txtlocalizacao.Text = txtlocalizacao.Text.ToUpper();
        }

        private void txtprecoVenda_TextChanged(object sender, EventArgs e)
        {


        }



        //--------------------- Botoes
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            modo = "A";
            toolStripStatusLabel1.Text = "Modo de alteração";
            alteracao();
            //----- desabilita menu de botoes
            desabilitaMenu();
        }



        private void btnGravar_Click(object sender, EventArgs e)
        {
            //------- habilita menu de botoes
            habilitaMenu();
            gravacao();
            

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            disable_txtboxes();
            atualizaCampos();
            modo = "";
            toolStripStatusLabel1.Text = "Pronto";
            habilitaMenu();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {

            inclusao();
            desabilitaMenu();

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            exclusao();
            desabilitaMenu();
        }

        private void frmCadProduto_FormClosing(object sender, FormClosingEventArgs e)
        {
            gravaArqSeq();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            pesquisar();
        }
    }
}
