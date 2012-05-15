using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Text;
using Microsoft.Phone.Shell;
using System.Diagnostics;

namespace SMSBillet
{
    public partial class MainPage : PhoneApplicationPage
    {
        private int zoneMinimumBillet = 2;
        private int zoneMinimumKlippekort = 2;

        private string tempLastStartingPoint;
        private string tempLastNumberOfZones;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //Billet
            Tombstoning.Save("StartingPoint", this.txtStartingPoint.Text);

            Tombstoning.Save("tbNumberOfZonesSelected", this.tbNumberOfZonesSelected.Text);
            Tombstoning.Save("rbAdult", this.rbAdult.IsChecked.ToString());
            Tombstoning.Save("rbChild", this.rbChild.IsChecked.ToString());
            Tombstoning.Save("rbBike", this.rbBike.IsChecked.ToString());
            Tombstoning.Save("chkExtraBike", this.chkExtraBike.IsChecked.ToString());
            Tombstoning.Save("chkExtraExtra", this.chkExtraExtra.IsChecked.ToString());
            
            //Klippekort panorama item
            Tombstoning.Save("txtStartingPointKlippeKort", this.txtStartingPointKlippeKort.Text);
            Tombstoning.Save("tbNumberOfZonesSelectedKlippeKort", this.tbNumberOfZonesSelectedKlippeKort.Text);
            Tombstoning.Save("rbAdultKlippeKort", this.rbAdultKlippeKort.IsChecked.ToString());
            Tombstoning.Save("rbChildKlippeKort", this.rbChildKlippeKort.IsChecked.ToString());
            Tombstoning.Save("btnCreateMessageKlippeKort", this.btnCreateMessageKlippeKort.IsEnabled.ToString());

            //Pivot item
            Tombstoning.Save("pivotControlSelectedItem", this.pivotControl.SelectedIndex);
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            #region Billet
            string startingPoint = Tombstoning.Load<string>("StartingPoint");

            if (!string.IsNullOrEmpty(startingPoint))
            {
                txtStartingPoint.Text = startingPoint;
            }

            string numberOfZonesSelected = Tombstoning.Load<string>("tbNumberOfZonesSelected");

            if (!string.IsNullOrEmpty(numberOfZonesSelected))
            {
                tbNumberOfZonesSelected.Text = numberOfZonesSelected;
            }

            string adultBillet = Tombstoning.Load<string>("rbAdult");
            if (!string.IsNullOrEmpty(adultBillet))
            {
                if (adultBillet.ToLower() == "true")
                {
                    rbAdult.IsChecked = true;
                }
            }

            string childBillet = Tombstoning.Load<string>("rbChild");
            if (!string.IsNullOrEmpty(childBillet))
            {
                if (childBillet.ToLower() == "true")
                {
                    rbChild.IsChecked = true;
                }
            }

            string bikeBillet = Tombstoning.Load<string>("rbBike");
            if (!string.IsNullOrEmpty(bikeBillet))
            {
                if (bikeBillet.ToLower() == "true")
                {
                    rbBike.IsChecked = true;
                }
            }

            string extraBikeBillet = Tombstoning.Load<string>("chkExtraBike");
            if (!string.IsNullOrEmpty(extraBikeBillet))
            {
                if (extraBikeBillet.ToLower() == "true")
                {
                    chkExtraBike.IsChecked = true;
                }
            }

            string extraExtraBillet = Tombstoning.Load<string>("chkExtraExtra");
            if (!string.IsNullOrEmpty(extraExtraBillet))
            {
                if (extraExtraBillet.ToLower() == "true")
                {
                    chkExtraExtra.IsChecked = true;
                }
            }

            #endregion

            #region Klippekort
            string startingPointKlippeKort = Tombstoning.Load<string>("txtStartingPointKlippeKort");
            if (!string.IsNullOrEmpty(startingPointKlippeKort))
            {
                txtStartingPointKlippeKort.Text = startingPointKlippeKort;
            }

            string numberOfZonesSelectedKlippeKort = Tombstoning.Load<string>("tbNumberOfZonesSelectedKlippeKort");
            if (!string.IsNullOrEmpty(numberOfZonesSelectedKlippeKort))
            {
                tbNumberOfZonesSelectedKlippeKort.Text = numberOfZonesSelectedKlippeKort;
            }

            string adultKlippeKort = Tombstoning.Load<string>("rbAdultKlippeKort");
            if (!string.IsNullOrEmpty(adultKlippeKort))
            {
                if (adultKlippeKort.ToLower() == "true")
                {
                    rbAdultKlippeKort.IsChecked = true;
                }
            }

            string childKlippeKort = Tombstoning.Load<string>("rbChildKlippeKort");
            if (!string.IsNullOrEmpty(childKlippeKort))
            {
                if (childKlippeKort.ToLower() == "true")
                {
                    rbChildKlippeKort.IsChecked = true;
                }
            }

            string createMessageKlippeKort = Tombstoning.Load<string>("btnCreateMessageKlippeKort");
            if (!string.IsNullOrEmpty(createMessageKlippeKort))
            {
                if (createMessageKlippeKort.ToLower() == "true")
                {
                    btnCreateMessageKlippeKort.IsEnabled = true;
                }
            }

            #endregion


            //int panoramaControlSelectedItem = Tombstoning.Load<int>("panoramaControlSelectedItem");
            //panoramaControl.DefaultItem = panoramaControl.Items[panoramaControlSelectedItem];

            //Pivot item
            int pivotControlSelectedItem = Tombstoning.Load<int>("pivotControlSelectedItem");
            pivotControl.SelectedIndex = pivotControlSelectedItem;
        }

        private void btnCreateMessage_Click(object sender, RoutedEventArgs e)
        {           
            SmsComposeTask sms = new SmsComposeTask();
            sms.To = SMSTicketStrings.BilletPhoneNumber;
            sms.Body = CompileSMSTextBillet();
            sms.Show();
        }

        private string CompileSMSTextBillet()
        {
            var sms = new StringBuilder();

            if (rbBike.IsChecked.Value)
            {
                sms.Append(SMSTicketStrings.Bicycle);

                //Return as nothing else is relevant if cykel is selected
                return sms.ToString();
            }

            sms.Append(txtStartingPoint.Text + " ");

            sms.Append(tbNumberOfZonesSelected.Text + " ");

            if (rbAdult.IsChecked.Value)
                sms.Append(SMSTicketStrings.Adult + " ");

            if (rbChild.IsChecked.Value)
                sms.Append(SMSTicketStrings.Child + " ");

            if (chkExtraBike.IsChecked.Value)
                sms.Append(SMSTicketStrings.Bicycle + " ");

            if (chkExtraExtra.IsChecked.Value)
                sms.Append(SMSTicketStrings.Extra + " ");

            return sms.ToString();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {

        }

        private void rbBike_Checked(object sender, RoutedEventArgs e)
        {
            Tombstoning.Save("tbNumberOfZonesSelected", tbNumberOfZonesSelected.Text);

            tempLastNumberOfZones = tbNumberOfZonesSelected.Text;
            tbNumberOfZonesSelected.Text = "";

            Tombstoning.Save("txtStartingPoint", txtStartingPoint.Text);
            txtStartingPoint.Text = "";

            Tombstoning.Save("chkExtraBikeChecked", chkExtraBike.IsChecked.ToString());
            Tombstoning.Save("chkExtraExtraChecked", chkExtraExtra.IsChecked.ToString());


            chkExtraBike.IsEnabled = false;
            chkExtraExtra.IsEnabled = false;
            chkExtraBike.IsChecked = false;
            chkExtraExtra.IsChecked = false;

            btnMinus.IsEnabled = false;
            btnPlus.IsEnabled = false;

            txtStartingPoint.IsEnabled = false;

            btnCreateMessage.IsEnabled = true;
        }

        private void rbBike_Unchecked(object sender, RoutedEventArgs e)
        {
            chkExtraBike.IsEnabled = true;
            chkExtraExtra.IsEnabled = true;
            btnMinus.IsEnabled = true;
            btnPlus.IsEnabled = true;
            txtStartingPoint.IsEnabled = true;


            txtStartingPoint.Text = Tombstoning.Load<string>("txtStartingPoint");
            tbNumberOfZonesSelected.Text = tempLastNumberOfZones;         

            string extraBikeBillet = Tombstoning.Load<string>("chkExtraBikeChecked");
            if (!string.IsNullOrEmpty(extraBikeBillet))
            {
                if (extraBikeBillet.ToLower() == "true")
                {
                    chkExtraBike.IsChecked = true;
                }
            }

            string extraExtraBillet = Tombstoning.Load<string>("chkExtraExtraChecked");
            if (!string.IsNullOrEmpty(extraExtraBillet))
            {
                if (extraExtraBillet.ToLower() == "true")
                {
                    chkExtraExtra.IsChecked = true;
                }
            }

            EnableCreateMessageButtonBillet();
        }

        private void EnableCreateMessageButtonBillet()
        {  
         

           if (txtStartingPoint.Text == string.Empty)
            {
                btnCreateMessage.IsEnabled = false;
            }
            else
            {
                btnCreateMessage.IsEnabled = true;
            }         
                             
          
        }

        private void slNumberOfZones_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbNumberOfZonesSelected != null)
                tbNumberOfZonesSelected.Text = ((int)e.NewValue).ToString();
        }

        private void chkExtraExtra_Checked(object sender, RoutedEventArgs e)
        {
            zoneMinimumBillet = 1;
        }

        private void chkExtraExtra_Unchecked(object sender, RoutedEventArgs e)
        {
            zoneMinimumBillet = 2;

            if (tbNumberOfZonesSelected.Text == "1")
            {
                tbNumberOfZonesSelected.Text = zoneMinimumBillet.ToString();
            }
        }

        private void txtStartingPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateMessageButtonBillet();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {

            int zones = Int32.Parse(tbNumberOfZonesSelected.Text);

            if (zones < 9)
            {
                zones = (zones + 1);
                tbNumberOfZonesSelected.Text = zones.ToString();
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            int zones = Int32.Parse(tbNumberOfZonesSelected.Text);

            if (zones > zoneMinimumBillet)
            {
                zones = (zones - 1);
                tbNumberOfZonesSelected.Text = zones.ToString();
            }
        }

        private void txtStartingPoint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnPlus.Focus();
            }
        }

        private void txtStartingPoint_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }


        #region KlippeKort
        private void btnKlippekort_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask sms = new SmsComposeTask();
            sms.To = SMSTicketStrings.BilletPhoneNumber;
            sms.Body = SMSTicketStrings.BuyKlippekort;
            sms.Show();
        }

        private void btnKlippekortSaldo_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask sms = new SmsComposeTask();
            sms.To = SMSTicketStrings.BilletPhoneNumber;
            sms.Body = SMSTicketStrings.KlippekortSaldo;
            sms.Show();
        }

        #endregion

        private void txtStartingPointKlippeKort_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateMessageButtonKlippekort();
        }

        private void txtStartingPointKlippeKort_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnPlusKlippeKort.Focus();
            }
        }

        private void EnableCreateMessageButtonKlippekort()
        {
            if (txtStartingPointKlippeKort.Text == string.Empty)
            {
                btnCreateMessageKlippeKort.IsEnabled = false;
            }
            else
            {
                btnCreateMessageKlippeKort.IsEnabled = true;
            }
        }

        private void btnCreateMessageKlippeKort_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask sms = new SmsComposeTask();
            sms.To = SMSTicketStrings.BilletPhoneNumber;
            sms.Body = CompileSMSTextKlippekort();
            sms.Show();
        }

        private string CompileSMSTextKlippekort()
        {
            var sms = new StringBuilder();

            sms.Append(SMSTicketStrings.Klip + " ");

            sms.Append(txtStartingPointKlippeKort.Text + " ");

            sms.Append(tbNumberOfZonesSelectedKlippeKort.Text + " ");

            if (rbAdultKlippeKort.IsChecked.Value)
                sms.Append(SMSTicketStrings.Adult + " ");

            if (rbChildKlippeKort.IsChecked.Value)
                sms.Append(SMSTicketStrings.Child + " ");

            return sms.ToString();
        }

        private void btnMinusKlippeKort_Click(object sender, RoutedEventArgs e)
        {
            int zones = Int32.Parse(tbNumberOfZonesSelectedKlippeKort.Text);

            if (zones > zoneMinimumKlippekort)
            {
                zones = (zones - 1);
                tbNumberOfZonesSelectedKlippeKort.Text = zones.ToString();
            }
        }

        private void btnPlusKlippeKort_Click(object sender, RoutedEventArgs e)
        {
            int zones = Int32.Parse(tbNumberOfZonesSelectedKlippeKort.Text);

            if (zones < 9)
            {
                zones = (zones + 1);
                tbNumberOfZonesSelectedKlippeKort.Text = zones.ToString();
            }
        }
    }
}