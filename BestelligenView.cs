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
    public partial class BestelligenView : Form
    {

        BestellingenController controller = new BestellingenController();

        public BestelligenView()
        {
            InitializeComponent();
        }

        private void BestelligenView_Load(object sender, EventArgs e)
        {
            lvBestellingen.View = System.Windows.Forms.View.Details;
            lvBestellingen.FullRowSelect = true;


            lvBestellingen.Columns.Add("Reserveringsnummer ID", 200);
            lvBestellingen.Columns.Add("Tafel Nummer", 100);
            lvBestellingen.Columns.Add("Bestel Datum", 200);
            lvBestellingen.Columns.Add("Status", 100);

            LoadBestellingen();
        }

        public void LoadBestellingen()
        {
            lvBestellingen.Items.Clear();

            List<BestellingenModel> bestellingenLijst = controller.Read();

            foreach (var b in bestellingenLijst)
            {
                ListViewItem item = new ListViewItem(b.ReserveringsnummerID.ToString());
                item.SubItems.Add(b.TafelNummer.ToString());
                item.SubItems.Add(b.BestelDatum.ToString("dd-MM-yyyy HH:mm"));
                item.SubItems.Add(b.Status);

                item.Tag = b;
                lvBestellingen.Items.Add(item);
            }
        }

        private void btnToevoegen_Click(object sender, EventArgs e)
        {
            // Controleer of het form al open is
            Form openForm = Application.OpenForms.OfType<BestellingenToevoegen>().FirstOrDefault();

            if (openForm == null)
            {
                // Form is nog niet open, maak een nieuwe
                BestellingenToevoegen toevoegenForm = new BestellingenToevoegen(this);
                toevoegenForm.Show();
            }
            else
            {
                // Form is al open, breng het naar voren en waarschuw de gebruiker
                openForm.BringToFront();
                MessageBox.Show("Dit venster is al open.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBewerken_Click(object sender, EventArgs e)
        {
            // Controleer eerst of er een bestelling geselecteerd is
            if (lvBestellingen.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een bestelling om te bewerken.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Haal de geselecteerde bestelling uit de Tag
            BestellingenModel geselecteerdeBestelling = (BestellingenModel)lvBestellingen.SelectedItems[0].Tag;

            // Controleer of het bewerkvenster al open is
            Form openForm = Application.OpenForms.OfType<BestellingenBewerken>()
                                .FirstOrDefault(f => ((BestellingenBewerken)f).Tag == geselecteerdeBestelling);

            if (openForm == null)
            {
                // Form is nog niet open, maak een nieuwe
                BestellingenBewerken bewerkenForm = new BestellingenBewerken(this, geselecteerdeBestelling);
                bewerkenForm.Tag = geselecteerdeBestelling; // Zorg dat Tag wordt gezet voor check
                bewerkenForm.Show();
            }
            else
            {
                // Form is al open, breng naar voren
                openForm.BringToFront();
                MessageBox.Show("Het bewerkvenster is al open.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnVerwijderen_Click(object sender, EventArgs e)
        {
            if (lvBestellingen.SelectedItems.Count > 0)
            {
                // Tag is nu nooit null
                BestellingenModel taak = (BestellingenModel)lvBestellingen.SelectedItems[0].Tag;

                if (controller.Delete(taak.BestellingID) == 1)
                {
                    MessageBox.Show($"Bestelling {taak.BestellingID} is verwijderd");
                }
                else
                {
                    MessageBox.Show("Er is iets mis gegaan bij het verwijderen");
                }
                LoadBestellingen();
            }
        }
    }
}
