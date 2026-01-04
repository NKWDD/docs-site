using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proeflokaal.Controller;
using Proeflokaal.Models;

namespace Proeflokaal.View
{
    public partial class KlantenBewerken : Form
    {

        private KlantenModel klant;
        private KlantenView _view;
        public KlantenBewerken(KlantenView view, KlantenModel klantToEdit)
        {
            InitializeComponent();
            _view = view;
            klant = klantToEdit;
        }

        List<KlantenModel> klanten = new List<KlantenModel>();
        private void BTNToevoegen_Click(object sender, EventArgs e)
        { 
           if (string.IsNullOrWhiteSpace(txtvoornaam.Text) ||
                string.IsNullOrWhiteSpace(txtachternaam.Text) ||
                string.IsNullOrWhiteSpace(txtemail.Text) ||
                string.IsNullOrWhiteSpace(txttelefoon.Text))
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!txtemail.Text.Contains("@"))
            {
                MessageBox.Show("Het e-mailadres moet een '@' bevatten.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            klant.Voornaam = txtvoornaam.Text.Trim();
            klant.Achternaam = txtachternaam.Text.Trim();
            klant.Email = txtemail.Text.Trim();
            klant.Telefoonnummer = txttelefoon.Text.Trim();

            KlantenController controller = new KlantenController();


            int result = controller.UpdateKlant(
                klant.KlantID,
                klant.Voornaam,
                klant.Achternaam,
                klant.Email,
                klant.Telefoonnummer
            );

            if (result == 1)
            {
                MessageBox.Show($"Klant {klant.Voornaam} succesvol bijgewerkt!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _view.LoadKlanten(); // Vernieuw de lijst
                this.Close(); // Sluit het venster
            }
            else
            {
                MessageBox.Show("Er is iets misgegaan bij het opslaan.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KlantenBewerken_Load(object sender, EventArgs e)
        {
            txtvoornaam.Text = klant.Voornaam;
            txtachternaam.Text = klant.Achternaam;
            txtemail.Text = klant.Email;
            txttelefoon.Text = klant.Telefoonnummer;
        }
    }
}
