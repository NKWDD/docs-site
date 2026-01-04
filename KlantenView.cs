using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proeflokaal.Controller;
using Proeflokaal.Models;

namespace Proeflokaal.View
{
    public partial class KlantenView : Form
    {
        KlantenController controller = new KlantenController();

        public KlantenView()
        {
            InitializeComponent();
        }

        private void KlantenView_Load(object sender, EventArgs e)
        {
            lvKlanten.View = System.Windows.Forms.View.Details;
            lvKlanten.FullRowSelect = true;

            lvKlanten.Columns.Add("Voornaam", 150);
            lvKlanten.Columns.Add("Achternaam", 150);
            lvKlanten.Columns.Add("Email", 250);
            lvKlanten.Columns.Add("Telefoonnummer", 150);

            LoadKlanten();
        }
        public void LoadKlanten()
        {
            lvKlanten.Items.Clear();

            // 3. Data ophalen via controller
            List<KlantenModel> klantenLijst = controller.Read();

            // 4. Data toevoegen aan ListView
            foreach (var klant in klantenLijst)
            {
                ListViewItem item = new ListViewItem(klant.Voornaam);
                item.SubItems.Add(klant.Achternaam);
                item.SubItems.Add(klant.Email);
                item.SubItems.Add(klant.Telefoonnummer);
                item.Tag = klant;
                lvKlanten.Items.Add(item);
            }
        }

        private void btnToevoegen_Click(object sender, EventArgs e)
        {
            Form openform = Application.OpenForms.OfType<KlantenToevoegen>().FirstOrDefault();

            if (openform == null)
            {
                KlantenToevoegen klant = new KlantenToevoegen(this);
                klant.Show();
            }
            else
            {
                openform.BringToFront();
                MessageBox.Show("Dit venster is al open.");
            }
        }

        private void btnVerwijderen_Click(object sender, EventArgs e)
        {
            if (lvKlanten.SelectedItems.Count > 0)
            {
                KlantenModel taak = (KlantenModel)lvKlanten.SelectedItems[0].Tag;

                if (controller.Delete(taak.KlantID) == 1)
                {
                    MessageBox.Show($"Klant {taak.KlantID} is verwijdred");
                }
                else
                {
                    MessageBox.Show("Er is iets mis gegaan bij het verwijderen");
                }
                LoadKlanten();
            }
        }

        private void btnBewerken_Click(object sender, EventArgs e)
        {
            Form openform = Application.OpenForms.OfType<KlantenBewerken>().FirstOrDefault();

            if (lvKlanten.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een speler om te bewerken.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            KlantenModel geselecteerdeKlant =
            (KlantenModel)lvKlanten.SelectedItems[0].Tag;

            // Open het bewerkvenster als het nog niet open is
            if (openform == null)
            {
                KlantenBewerken bewerkenForm = new KlantenBewerken(this, geselecteerdeKlant);
                bewerkenForm.Show();
            }
            else
            {
                // Breng het bestaande venster naar voren
                openform.BringToFront();
                MessageBox.Show("Het bewerkvenster is al open.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lvKlanten_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
