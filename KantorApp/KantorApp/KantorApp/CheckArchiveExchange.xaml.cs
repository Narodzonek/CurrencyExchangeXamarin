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
    public partial class CheckArchiveExchange : ContentPage
    {
        public CheckArchiveExchange()
        {
            InitializeComponent();
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void CheckCurrency_Clicked(object sender, EventArgs e)
        {
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();
            string info = client.CheckArchiveRate(SelectDate.Date.ToString("yyyy-MM-dd"), CurrencySign.Text);
            CheckLabel.Text = info;
            client.CloseAsync();
        }
    }
}