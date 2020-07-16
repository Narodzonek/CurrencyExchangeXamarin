using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KantorApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Withdraw : ContentPage
    {
        public Withdraw()
        {
            InitializeComponent();
        }

        private void AcceptWithdrawBtn_Clicked(object sender, EventArgs e)
        {
            WCFKantor.UserDetails user = new WCFKantor.UserDetails();
            user.UserEmail = Login.emailUser;
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();
            Dictionary<string, string> userData = client.PobierzDane(user);
            WCFKantor.AccountDetails currencyPLN = new WCFKantor.AccountDetails() { UserID = Int32.Parse(userData["ID"]), Currency = "PLN", Amount = -1 * Decimal.Parse(AmountPln.Text) };
            CheckLabel.Text = client.UpdateStanKonta(currencyPLN);
            client.CloseAsync();
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}