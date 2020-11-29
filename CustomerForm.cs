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

namespace FinalProjectVersion2
{
    public partial class CustomerForm : Form
    {
        string filename = "CustomerData.txt";
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            CreateDGV();
        }

        public void CreateDGV()
        {
            dgvCustomers.Rows.Clear();

            using (var streamReader = new StreamReader(filename))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var values = line.Split(',');
                    var rowIndex = dgvCustomers.Rows.Add();
                    for (int i = 0; i < values.Length; i++)
                    {
                        dgvCustomers.Rows[rowIndex].Cells[i].Value = values[i];
                    }
                }
            }
        }

    }
}
