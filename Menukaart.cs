using Proeflokaal.Controller;
using Proeflokaal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proeflokaal.View
{
    public partial class Menukaart : Form
    {
        private MenuController controller = new MenuController();
        private List<MenuItemModel> huidigeItems = new List<MenuItemModel>();


        public Menukaart()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = controller.GetCategories();

            cmbCategory.Items.Clear();

            foreach (var cat in categories)
            {
                cmbCategory.Items.Add(cat);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem != null)
            {
                // Odblokuj SubCategory i wypełnij podkategoriami z bazy
                cmbSubCategory.Enabled = true;
                cmbSubCategory.Items.Clear();

                var subCats = controller.GetSubCategories(cmbCategory.SelectedItem.ToString());
                foreach (var sub in subCats)
                {
                    cmbSubCategory.Items.Add(sub);
                }

                if (cmbSubCategory.Items.Count > 0)
                    cmbSubCategory.SelectedIndex = 0; // zaznacz pierwszą subkategorię
            }
        }

        private void cmbSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem == null || cmbSubCategory.SelectedItem == null)
                return;

            string category = cmbCategory.SelectedItem.ToString();
            string subCategory = cmbSubCategory.SelectedItem.ToString();

            // Itemy sie wypelniaja
            huidigeItems = controller.Read(category, subCategory);

            listBoxMenu.Items.Clear();
            foreach (var item in huidigeItems)
            {
                listBoxMenu.Items.Add(item.MenuItem);
            }
        }

        private void btnbewereken_Click(object sender, EventArgs e)
        {
            if (listBoxMenu.SelectedIndex == -1)
            {
                MessageBox.Show("Selecteer eerst een menu item");
                return;
            }

            MenuItemModel geselecteerd = huidigeItems[listBoxMenu.SelectedIndex];

            MenuBewerken bewerkenForm = new MenuBewerken(this, geselecteerd);
            bewerkenForm.ShowDialog();
        }

        public void RefreshMenu()
        {
            if (cmbCategory.SelectedItem == null || cmbSubCategory.SelectedItem == null)
                return;

            var items = controller.Read(
                cmbCategory.SelectedItem.ToString(),
                cmbSubCategory.SelectedItem.ToString()
            );

            huidigeItems = items;
            listBoxMenu.Items.Clear();

            foreach (var item in items)
                listBoxMenu.Items.Add(item.MenuItem);
        }

        private void btntoevoegen_Click(object sender, EventArgs e)
        {
            MenuToevoegen form = new MenuToevoegen(this);
            form.ShowDialog();
        }

        private void btnverwijderen_Click(object sender, EventArgs e)
        {
            if (listBoxMenu.SelectedIndex == -1)
            {
                MessageBox.Show("Selecteer eerst een menu item.");
                return;
            }

            MenuItemModel geselecteerd = huidigeItems[listBoxMenu.SelectedIndex];

            DialogResult result = MessageBox.Show(
                $"Weet je zeker dat je '{geselecteerd.MenuItem}' wilt verwijderen?",
                "Bevestigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                controller.Delete(geselecteerd.Id);
                RefreshMenu();
                MessageBox.Show("Menu item verwijderd.");
            }
        }

    }
}
