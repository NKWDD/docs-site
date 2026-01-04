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
    public partial class BestellingenBewerken : Form
    {
        private BestelligenView parentView;
        private BestellingenModel bestelling;

        public BestellingenBewerken(BestelligenView view, BestellingenModel geselecteerdeBestelling)
        {
            InitializeComponent();
            parentView = view;
            bestelling = geselecteerdeBestelling;

            InitializeFormControls();
            LoadReserveringsnummers();
            VulFormMetData();
        }

        private void InitializeFormControls()
        {
            // Status opties
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Bevestigd");
            cmbStatus.Items.Add("In behandeling");
            cmbStatus.Items.Add("Afgerond");
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            // Datum picker
            dtpBestel.Format = DateTimePickerFormat.Short; // alleen datum
            dtpBestel.Value = DateTime.Today;
        }

        private void LoadReserveringsnummers()
        {
            ReserveringenController controller = new ReserveringenController();
            var reserveringen = controller.Read();

            cmbReservering.Items.Clear();
            foreach (var r in reserveringen)
            {
                cmbReservering.Items.Add(new { r.ReserveringsnummerID, Display = $"Reservering #{r.ReserveringsnummerID}" });
            }

            if (cmbReservering.Items.Count > 0)
            {
                cmbReservering.DisplayMember = "Display";
                cmbReservering.ValueMember = "ReserveringsnummerID";
            }
        }

        private void VulFormMetData()
        {
            // Selecteer de reservering in de combobox
            for (int i = 0; i < cmbReservering.Items.Count; i++)
            {
                dynamic item = cmbReservering.Items[i];
                if (item.ReserveringsnummerID == bestelling.ReserveringsnummerID)
                {
                    cmbReservering.SelectedIndex = i;
                    break;
                }
            }

            txtTafel.Text = bestelling.TafelNummer.ToString();
            dtpBestel.Value = bestelling.BestelDatum.Date;
            cmbStatus.SelectedItem = bestelling.Status;
        }

        private void btntoevoegen_Click(object sender, EventArgs e)
        {
            BestellingenController controller = new BestellingenController();

            // Validatie
            if (cmbReservering.SelectedItem == null || cmbStatus.SelectedItem == null || string.IsNullOrWhiteSpace(txtTafel.Text))
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtTafel.Text, out int tafelNummer) || tafelNummer <= 0)
            {
                MessageBox.Show("Tafelnummer moet een geldig getal zijn.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reserveringsnummerID = (int)((dynamic)cmbReservering.SelectedItem).ReserveringsnummerID;

            bestelling.ReserveringsnummerID = reserveringsnummerID;
            bestelling.TafelNummer = tafelNummer;
            bestelling.BestelDatum = dtpBestel.Value;
            bestelling.Status = cmbStatus.SelectedItem.ToString();

            int result = controller.Update(bestelling);

            if (result == 1)
            {
                MessageBox.Show("Bestelling succesvol bijgewerkt!");
                parentView.LoadBestellingen(); // ververs de lijst
                this.Close();
            }
            else
            {
                MessageBox.Show("Er is iets mis gegaan bij het bijwerken van de bestelling.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
