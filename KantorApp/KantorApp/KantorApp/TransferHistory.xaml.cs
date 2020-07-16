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
    public partial class TransferHistory : ContentPage
    {
        public TransferHistory()
        {
            InitializeComponent();
            WCFKantor.UserDetails user = new WCFKantor.UserDetails() { UserEmail = Login.emailUser };
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();

            Dictionary<string, decimal> accountData = new Dictionary<string, decimal>();
            foreach (KeyValuePair<string, decimal> currency in accountData)
            {
                TransferInfo.Children.Add(new Label { Text = currency.Key + ":" });
                TransferInfo.Children.Add(new Label { Text = currency.Value.ToString() });
            }
            client.CloseAsync();
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}