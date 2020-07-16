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
    public partial class UserPanel : ContentPage
    {
        Dictionary<string, string> getUserData = new Dictionary<string, string>();
        public UserPanel()
        {
            InitializeComponent();
            WCFKantor.UserDetails user = new WCFKantor.UserDetails(){UserEmail = Login.emailUser};
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();
            getUserData = client.PobierzDane(user);
            UserName.Text = "Witaj " + getUserData["Imie"] + " " + getUserData["Nazwisko"];
            Dictionary<string, decimal> accountData = client.GetStanKonta(user);
            foreach (KeyValuePair<string, decimal> currency in accountData)
            {
                AccountInfo.Children.Add(new Label { Text=currency.Key +":" });
                AccountInfo.Children.Add(new Label { Text = currency.Value.ToString() });
            }
            client.CloseAsync();
        }

        //Zasil swoje konto
        private async void UpdateStan_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FoundAccount());
        }
        //Sprawdź kurs waluty
        private async void CheckCurrency_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CheckExchange());
        }
        //Sprawdź archiwalny kurs waluty
        private async void CheckArchive_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CheckArchiveExchange());
        }
        //Kup walutę
        private async void BuyCurrency_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new BuyCurrency());
        }
        //Sprzedaj walutę
        private async void SellCurrency_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SellCurrency());
        }
        //Historia transakcji
        private async void TransactionsHistory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TransactionHistory());
        }
        //Historia Przelewów
        private async void TransferHistory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TransferHistory());
        }
        //Wypłać środki
        private async void WithdrawFunds_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Withdraw());
        }
        //Usuń konto
        private async void AccountDelete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DeleteAcount());
        }
        //Wyloguj się
        private async void LogOutBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}