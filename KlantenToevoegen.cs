using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Proeflokaal.Controller;
using Proeflokaal.Models;

namespace Proeflokaal.View
{
    public partial class KlantenToevoegen : Form
    {

        private KlantenView parentView;
        public KlantenToevoegen(KlantenView view)
        {
            InitializeComponent();
            parentView = view;
        }
        List<KlantenModel> klanten = new List<KlantenModel>();
        private void BTNtoevoegen_Click(object sender, EventArgs e)
        {
            KlantenModel klant = new KlantenModel();

            KlantenController controller = new KlantenController();

            if (string.IsNullOrWhiteSpace(txtVoornaam.Text) ||
                string.IsNullOrWhiteSpace(txtAchternaam.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtTelefoon.Text))
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Controleer of e-mail een '@' bevat
            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Het e-mailadres moet een '@' bevatten.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            KlantenModel Klant = new KlantenModel
            {
                Voornaam = txtVoornaam.Text,
                Achternaam = txtAchternaam.Text,
                Email = txtEmail.Text,
                Telefoonnummer = txtTelefoon.Text,  
            };

            int result = controller.Create(Klant);

            if (result == 1)
            {
                MessageBox.Show($"Klant {Klant.Voornaam} is toegevoegd");
                parentView.LoadKlanten(); // ververs de lijst
                this.Close(); // Sluit het venster
            }
        }
    }
}
