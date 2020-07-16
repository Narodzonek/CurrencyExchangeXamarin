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
    public partial class DeleteAcount : ContentPage
    {
        public DeleteAcount()
        {
            InitializeComponent();
        }

        private async void BackBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void DeleteBtn_Clicked(object sender, EventArgs e)
        {
            WCFKantor.UserDetails user = new WCFKantor.UserDetails() { UserEmail = Login.emailUser };
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();
            client.DeleteAccount(user);
        }
    }
}