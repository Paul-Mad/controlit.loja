using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace controlit.loja
{
    public partial class frmCadProduto : Form
    {
        string[]  aCodProduto   =   new string[100];
        string[]  aNomeProduto  =   new string[100];
       decimal[]  aPrecoProduto =  new decimal[100];
        string[] aLocalizacao = new string[100];
        string[] aFornecedor = new string[100];
        DateTime[] aDatadaUltCompra = new DateTime[100];
        decimal[] aVlrUltCompra = new decimal[100];




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
                aCodProduto[i] = registro.Substring(0,10);
                aNomeProduto[i] = registro.Substring(10, 20);
                aPrecoProduto[i] = Convert.ToDecimal(registro.Substring(30, 14));
                aLocalizacao[i] = registro.Substring(44, 10);
                aFornecedor[i] = registro.Substring(54, 15);
                aDatadaUltCompra[i] = Convert.ToDateTime(registro.Substring(69, 10));
                aVlrUltCompra[i] = Convert.ToDecimal(registro.Substring(79, 14));

                dgvProdutos.Rows.Add(aCodProduto[i], aNomeProduto[i], aFornecedor[i]);
            }

            
        }

        private void dgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgvProdutos_SelectionChanged(object sender, EventArgs e)
        {
            int linhaDgv = Convert.ToInt32(dgvProdutos.CurrentRow.Index);
            label9.Text = Convert.ToString(linhaDgv);
            label9.Visible = true;
        }
    }
}
