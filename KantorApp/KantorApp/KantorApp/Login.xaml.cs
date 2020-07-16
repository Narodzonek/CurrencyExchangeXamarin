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
    public partial class Login : ContentPage
    {
        public static string emailUser;
        public Login()
        {
            InitializeComponent();
        }

        private async void LoginBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void LoginContinue_Clicked(object sender, EventArgs e)
        {
            WCFKantor.UserDetails user = new WCFKantor.UserDetails() { UserEmail = UserEmail.Text, UserPassword = UserPass.Text };
            WCFKantor.Service1Client client = new WCFKantor.Service1Client();
            Boolean result = client.LogowanieUzytkownika(user);
            await client.CloseAsync();
            if (result)
            {
                emailUser = UserEmail.Text;
                await Navigation.PushModalAsync(new UserPanel());
            }
            else
            {
                CheckLabel.Text = "Sprawdź poprawność danych logowania.";
            }
            
        }
    }
}