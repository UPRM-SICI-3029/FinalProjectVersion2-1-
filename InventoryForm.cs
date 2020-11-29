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
    public partial class InventoryForm : Form
    {
        const int MINIMUM_STOCK = 0;
        int selectedRow, stock, oldStock, addOrSubtractStock, newStock;
        double price;
        string productId, productName, sportsCategory;
        string filename = "SportsProducts.txt";
        public InventoryForm()
        {
            InitializeComponent();
        }

        private void btnEditProducts_Click(object sender, EventArgs e)
        {
            productId = txtProductId.Text;
            productName = txtProductName.Text;
            oldStock = Convert.ToInt32(dgvInventory[2, selectedRow].Value);
            addOrSubtractStock = Convert.ToInt32(txtStock.Text);
            price = Convert.ToDouble(txtPrice.Text);
            sportsCategory = txtSportsCategory.Text;
            newStock = oldStock + addOrSubtractStock;
            if (newStock < MINIMUM_STOCK)
            {
                newStock = MINIMUM_STOCK;
                MessageBox.Show("The minimum stock must be 0");
            }
            dgvInventory[0, selectedRow].Value = productId;
            dgvInventory[1, selectedRow].Value = productName;
            dgvInventory[2, selectedRow].Value = newStock;
            dgvInventory[3, selectedRow].Value = price;
            dgvInventory[4, selectedRow].Value = sportsCategory;
            ReturnAndClean();
        }

        private void btnDeleteProducts_Click(object sender, EventArgs e)
        {
            DialogResult deleteProduct;
            deleteProduct = MessageBox.Show("Are you sure you want to delete this product?", "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (deleteProduct == DialogResult.Yes)
            {
                dgvInventory.Rows.RemoveAt(selectedRow);
            }
            SaveDGV();
            CreateDGV();
            ReturnAndClean();
        }

        private void btnBackToAdd_Click(object sender, EventArgs e)
        {
            ReturnAndClean();
        }

        private void btnShowCustomers_Click(object sender, EventArgs e)
        {
            CustomerForm customers = new CustomerForm();
            customers.ShowDialog();
        }

        private void dgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = dgvInventory.CurrentRow.Index;
            txtProductId.Text = Convert.ToString(dgvInventory[0, selectedRow].Value);
            txtProductName.Text = Convert.ToString(dgvInventory[1, selectedRow].Value);
            txtStock.Text = "0";
            txtPrice.Text = Convert.ToString(dgvInventory[3, selectedRow].Value);
            txtSportsCategory.Text = Convert.ToString(dgvInventory[4, selectedRow].Value);
            lblStock.Text = "Add or Subtract Stock:";
            btnAdd.Enabled = false;
            btnEditProducts.Enabled = true;
            btnDeleteProducts.Enabled = true;
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
            CreateDGV();
            ReturnAndClean();
        }

        public void CreateDGV()
        {
          dgvInventory.Rows.Clear();

                using (var streamReader = new StreamReader(filename))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        var values = line.Split(',');
                        var rowIndex = dgvInventory.Rows.Add();
                        for (int i = 0; i < values.Length; i++)
                        {
                            dgvInventory.Rows[rowIndex].Cells[i].Value = values[i];
                        }
                    }
                }
        }

        public void SaveDGV()
        {
            
                const char DELIM = ',';

                StreamWriter productFile;
                productFile = File.CreateText(filename);
                int rowcount = dgvInventory.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    productFile.WriteLine(dgvInventory.Rows[i].Cells[0].Value.ToString() + DELIM
                        + dgvInventory.Rows[i].Cells[1].Value.ToString() + DELIM
                        + dgvInventory.Rows[i].Cells[2].Value.ToString() + DELIM
                        + dgvInventory.Rows[i].Cells[3].Value.ToString() + DELIM
                        + dgvInventory.Rows[i].Cells[4].Value.ToString());
                }
                productFile.Close();
        }

        public void ReturnAndClean()
        {
            btnDeleteProducts.Enabled = false;
            btnEditProducts.Enabled = false;
            btnAdd.Enabled = true;
            txtProductId.Text = "";
            txtProductName.Text = "";
            txtStock.Text = "";
            txtPrice.Text = "";
            txtSportsCategory.Text = "";
            lblStock.Text = "Stock:";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            const double MINIMUM_PRICE = 0.01;
            try
            {
                productName = txtProductName.Text;
                productId = txtProductId.Text;
                sportsCategory = txtSportsCategory.Text;
                stock = Convert.ToInt32(txtStock.Text);
                price = Convert.ToDouble(txtPrice.Text);
                if (stock < MINIMUM_STOCK)
                {
                    stock = MINIMUM_STOCK;
                    MessageBox.Show("The minimum stock must be 0");
                }

                if (price < MINIMUM_PRICE)
                {
                    price = MINIMUM_PRICE;
                    MessageBox.Show("The minimum price must be 0.01");
                }
                int n = dgvInventory.Rows.Add();
                dgvInventory.Rows[n].Cells[0].Value = productId;
                dgvInventory.Rows[n].Cells[1].Value = productName;
                dgvInventory.Rows[n].Cells[2].Value = stock;
                dgvInventory.Rows[n].Cells[3].Value = price;
                dgvInventory.Rows[n].Cells[4].Value = sportsCategory;
                SaveDGV();
                CreateDGV();
                ReturnAndClean();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
