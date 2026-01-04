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
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Proeflokaal.View
{
    public partial class ReserveringenToevoegen : Form
    {

        private ReserveringenView parentView;
        public ReserveringenToevoegen(ReserveringenView view)
        {
            InitializeComponent();
            parentView = view;
            InitializeFormControls();
            LoadKlanten();
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
            cmbKlant.SelectedIndex = 0;
        }

        private void InitializeFormControls()
        {
            // Datum picker
            dtpDatum.Format = DateTimePickerFormat.Short;
            dtpDatum.MinDate = new DateTime(1753, 1, 1);
            dtpDatum.Value = DateTime.Today;



            cmbTijdslot.Items.Clear(); 
            cmbTijdslot.Items.Add("16:00 - 18:00");
            cmbTijdslot.Items.Add("18:00 - 20:00");
            cmbTijdslot.Items.Add("20:00 - 22:00");
            cmbTijdslot.SelectedIndex = 0;
            cmbTijdslot.DropDownStyle = ComboBoxStyle.DropDownList;

            // Status opties
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Bevestigd");
            cmbStatus.Items.Add("In afwachting");
            cmbStatus.Items.Add("Geannuleerd");
            cmbStatus.SelectedIndex = 0;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadKlanten();
        }

        List<ReserveringenModel> reserveringens = new List<ReserveringenModel>();

        private void btntoevoegen_Click(object sender, EventArgs e)
        {
            ReserveringenModel reservering = new ReserveringenModel();

            ReserveringenController controller = new ReserveringenController();

            // Controleer lege velden
            if (string.IsNullOrWhiteSpace(txtAantal.Text) ||
                cmbTijdslot.SelectedItem == null ||
                cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Alle velden moeten worden ingevuld.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Controleer of aantal een geldig getal is
            if (!int.TryParse(txtAantal.Text, out int aantalPersonen) || aantalPersonen <= 0)
            {
                MessageBox.Show("Aantal personen moet een geldig getal zijn.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            string selectedSlot = cmbTijdslot.SelectedItem.ToString();
            string startTime = selectedSlot.Split('-')[0].Trim();    
            TimeSpan tijdslot = TimeSpan.Parse(startTime);

            // Nieuwe reservering aanmaken
            ReserveringenModel reserveringen = new ReserveringenModel
            {
                AantalPersonen = aantalPersonen,
                Datum = dtpDatum.Value.Date,
                Tijdslot = tijdslot,
                Status = cmbStatus.SelectedItem.ToString(),
                KlantID = (int)((dynamic)cmbKlant.SelectedItem).KlantID
            };

            
            int result = controller.Create(reserveringen);


            if (result == 1)
            {
                // ✅ Gebruik het juiste object 'reserveringen' en niet 'reservering'
                MessageBox.Show($"Reservering toegevoegd voor {reserveringen.AantalPersonen} personen op {reserveringen.Datum.ToShortDateString()}!");
                parentView.LoadReserveringen(); // ververs de lijst
                this.Close(); // sluit het venster
            }
            else
            {
                MessageBox.Show("Er is een fout opgetreden bij het toevoegen van de reservering.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
