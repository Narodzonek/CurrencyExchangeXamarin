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
    public partial class CheckExchange : ContentPage
    {
        public CheckExchange()
        {
            InitializeComponent();
        }

        private void CheckCurrency_Clicked(object sender, EventArgs e)
        {
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();
            CheckLabel.Text = client.GetExchangeRate(CurrencySign.Text);
            client.CloseAsync();
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}