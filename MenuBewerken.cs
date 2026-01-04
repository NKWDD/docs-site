using System;
using System.Windows.Forms;
using Proeflokaal.Controller;
using Proeflokaal.Models;

namespace Proeflokaal.View
{
    public partial class MenuBewerken : Form
    {
        // Pola klasy
        private MenuItemModel menuItem;
        private Menukaart _view;
        private MenuController controller = new MenuController();
        private bool isLoading = false;

        // Konstruktor
        public MenuBewerken(Menukaart view, MenuItemModel itemToEdit)
        {
            InitializeComponent();
            _view = view;
            menuItem = itemToEdit;
        }

        // Ladowanie formy
        private void MenuBewerken_Load(object sender, EventArgs e)
        {
            isLoading = true;

            LoadCategories();
            LoadMenuItemData();

            isLoading = false;
        }

        // Ladowanie Kategorii
        private void LoadCategories()
        {
            cmbCategory.Items.Clear();

            var categories = controller.GetCategories();
            foreach (var cat in categories)
            {
                cmbCategory.Items.Add(cat);
            }

            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Subcategorie ladowanie
        private void LoadSubCategories(string category)
        {
            cmbSubCategory.Items.Clear();

            var subs = controller.GetSubCategories(category);
            foreach (var sub in subs)
            {
                cmbSubCategory.Items.Add(sub);
            }

            cmbSubCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Wyswietla date zaznaczonego itemu
        private void LoadMenuItemData()
        {
            // Tekstbox
            txtMenuItem.Text = menuItem.MenuItem;

            //Kategoria
            cmbCategory.SelectedItem = menuItem.Category;

            // Subkategoria pierwsze laduje potem wypelnia
            LoadSubCategories(menuItem.Category);
            cmbSubCategory.SelectedItem = menuItem.SubCategory;
        }

        // Jak zmienia się kategoria
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            if (cmbCategory.SelectedItem != null)
            {
                LoadSubCategories(cmbCategory.SelectedItem.ToString());
            }
        }

        // Zapisywanie zmian
        private void btnOpslaan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMenuItem.Text) ||
                cmbCategory.SelectedItem == null ||
                cmbSubCategory.SelectedItem == null)
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.");
                return;
            }

            // Edytowanie
            menuItem.MenuItem = txtMenuItem.Text;
            menuItem.Category = cmbCategory.SelectedItem.ToString();
            menuItem.SubCategory = cmbSubCategory.SelectedItem.ToString();

            int result = controller.Update(
                menuItem.Id,
                menuItem.MenuItem,
                menuItem.Category,
                menuItem.SubCategory
            );

            if (result == 1)
            {
                MessageBox.Show("Menu item succesvol bijgewerkt.");
                _view.RefreshMenu();
                this.Close();
            }
            else
            {
                MessageBox.Show("Fout bij het opslaan.");
            }
        }

        // Anulowanie zmian
        private void btnAnnuleren_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
