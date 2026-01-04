using System;
using System.Windows.Forms;
using Proeflokaal.Controller;
using Proeflokaal.Models;

namespace Proeflokaal.View
{
    public partial class MenuToevoegen : Form
    {
        private MenuController controller = new MenuController();
        private Menukaart _view;
        private bool isLoading = false;

        public MenuToevoegen(Menukaart view)
        {
            InitializeComponent();
            _view = view;
        }

        private void MenuToevoegen_Load(object sender, EventArgs e)
        {
            isLoading = true;
            LoadCategories();
            isLoading = false;
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();

            var categories = controller.GetCategories();
            foreach (var cat in categories)
                cmbCategory.Items.Add(cat);

            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadSubCategories(string category)
        {
            cmbSubCategory.Items.Clear();

            var subs = controller.GetSubCategories(category);
            foreach (var sub in subs)
                cmbSubCategory.Items.Add(sub);

            cmbSubCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (cmbCategory.SelectedItem != null)
                LoadSubCategories(cmbCategory.SelectedItem.ToString());
        }

        private void btnOpslaan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMenuItem.Text) ||
                cmbCategory.SelectedItem == null ||
                cmbSubCategory.SelectedItem == null)
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.");
                return;
            }

            MenuItemModel nieuwItem = new MenuItemModel
            {
                MenuItem = txtMenuItem.Text,
                Category = cmbCategory.SelectedItem.ToString(),
                SubCategory = cmbSubCategory.SelectedItem.ToString()
            };

            int result = controller.Create(nieuwItem);

            if (result == 1)
            {
                MessageBox.Show("Menu item toegevoegd!");
                _view.RefreshMenu();
                this.Close();
            }
            else
            {
                MessageBox.Show("Fout bij toevoegen.");
            }
        }

        private void btnAnnuleren_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
