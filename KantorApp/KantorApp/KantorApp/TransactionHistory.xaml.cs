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
    public partial class TransactionHistory : ContentPage
    {
        Dictionary<string, string> getUserData = new Dictionary<string, string>();
        public TransactionHistory()
        {
            InitializeComponent();
            WCFKantor.Transactions transaction = new WCFKantor.Transactions();
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();

            Dictionary<string, decimal> accountData = new Dictionary<string, decimal>();
            foreach (KeyValuePair<string, decimal> currency in accountData)
            {
                TransactionInfo.Children.Add(new Label { Text = currency.Key + ":" });
                TransactionInfo.Children.Add(new Label { Text = currency.Value.ToString() });
            }
            client.CloseAsync();
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}