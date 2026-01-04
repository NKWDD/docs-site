using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Proeflokaal.Controller;
using Proeflokaal.Models;

namespace Proeflokaal.View
{
    public partial class ReserveringenBewerken : Form
    {
        private ReserveringenModel reservering;
        private ReserveringenView _view;

        public ReserveringenBewerken(ReserveringenView view, ReserveringenModel reserveringToEdit)
        {
            InitializeComponent();
            _view = view;
            reservering = reserveringToEdit;
        }

        private void ReserveringenBewerken_Load(object sender, EventArgs e)
        {
            InitializeFormControls();
            LoadKlanten();
            LoadReserveringData();
        }

        private void InitializeFormControls()
        {
            // DATUM
            dtpDatum.Format = DateTimePickerFormat.Short;
            dtpDatum.MinDate = new DateTime(1753, 1, 1);

            // TIJDSLOTEN
            cmbTijdslot.Items.Clear();
            cmbTijdslot.Items.Add("16:00 - 18:00");
            cmbTijdslot.Items.Add("18:00 - 20:00");
            cmbTijdslot.Items.Add("20:00 - 22:00");
            cmbTijdslot.DropDownStyle = ComboBoxStyle.DropDownList;

            // STATUS
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Bevestigd");
            cmbStatus.Items.Add("In afwachting");
            cmbStatus.Items.Add("Geannuleerd");
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadKlanten()
        {
            KlantenController klantenController = new KlantenController();
            var klanten = klantenController.Read();

            cmbKlant.Items.Clear();
            foreach (var klant in klanten)
            {
                cmbKlant.Items.Add(new { klant.KlantID, Display = $"{klant.Voornaam} {klant.Achternaam}" });
            }

            cmbKlant.DisplayMember = "Display";
            cmbKlant.ValueMember = "KlantID";
        }

        private void LoadReserveringData()
        {
            // Aantal personen
            txtAantal.Text = reservering.AantalPersonen.ToString();

            // Datum
            dtpDatum.Value = reservering.Datum;

            // Tijdslot instellen in ComboBox zoals "16:00 - 18:00"
            for (int i = 0; i < cmbTijdslot.Items.Count; i++)
            {
                string itemText = cmbTijdslot.Items[i].ToString();
                string startTime = itemText.Split('-')[0].Trim();

                if (startTime == reservering.Tijdslot.ToString(@"hh\:mm"))
                {
                    cmbTijdslot.SelectedIndex = i;
                    break;
                }
            }

            // Status
            cmbStatus.SelectedItem = reservering.Status;

            // Klant
            for (int i = 0; i < cmbKlant.Items.Count; i++)
            {
                dynamic item = cmbKlant.Items[i];
                if (item.KlantID == reservering.KlantID)
                {
                    cmbKlant.SelectedIndex = i;
                    break;
                }
            }
        }

        private void btntoevoegen_Click(object sender, EventArgs e)
        {
            // VALIDATIE
            if (string.IsNullOrWhiteSpace(txtAantal.Text) ||
                cmbTijdslot.SelectedItem == null ||
                cmbStatus.SelectedItem == null ||
                cmbKlant.SelectedItem == null)
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAantal.Text, out int aantalPersonen) || aantalPersonen <= 0)
            {
                MessageBox.Show("Aantal personen moet een geldig getal zijn.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tijdslot ophalen (startuur) uit ComboBox
            string selectedSlot = cmbTijdslot.SelectedItem.ToString();
            TimeSpan tijdslot = TimeSpan.Parse(selectedSlot.Split('-')[0].Trim());

            // KlantID ophalen
            int klantId = (int)((dynamic)cmbKlant.SelectedItem).KlantID;

            // Model updaten
            reservering.AantalPersonen = aantalPersonen;
            reservering.Datum = dtpDatum.Value.Date;
            reservering.Tijdslot = tijdslot;
            reservering.Status = cmbStatus.SelectedItem.ToString();
            reservering.KlantID = klantId;

            // Controller Update
            ReserveringenController controller = new ReserveringenController();
            int result = controller.Update(
                reservering.ReserveringsnummerID,
                reservering.AantalPersonen,
                reservering.Status,
                reservering.Datum,
                reservering.Tijdslot,
                reservering.KlantID
            );

            if (result == 1)
            {
                MessageBox.Show("Reservering succesvol bijgewerkt!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _view.LoadReserveringen();
                this.Close();
            }
            else
            {
                MessageBox.Show("Er is iets misgegaan bij het opslaan.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
