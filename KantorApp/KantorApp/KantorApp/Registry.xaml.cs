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
    public partial class Registry : ContentPage
    {
        Boolean privacyPolicy;
        public Registry()
        {
            InitializeComponent();
        }

        async void RegistryBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void RegistryContinue_Clicked(object sender, EventArgs e)
        {
            if (privacyPolicy)
            {
                if(UserPass.Text == PassCheck.Text)
                {
                    WCFKantor.UserDetails newUser = new WCFKantor.UserDetails();
                    newUser.UserName = UserName.Text;
                    newUser.UserSurname = UserSurname.Text;
                    newUser.UserEmail = UserEmail.Text;
                    newUser.UserPassword = UserPass.Text;
                    WCFKantor.Service1Client client = new WCFKantor.Service1Client();
                    string result = client.RejestracjaUzytkownika(newUser);
                    CheckLabel.Text = result;
                    client.CloseAsync();
                }
                else
                {
                    CheckLabel.Text = "Sprawdź powtórzenie hasła.";
                }
            }
            else
            {
                CheckLabel.Text = "Musisz zaakceptować naszą Politykę prywatności.";
            }
            
        }

        private void PrivacyPolicyAccept_Toggled(object sender, ToggledEventArgs e)
        {
            privacyPolicy = (e.Value == true) ? true : false;
        }
    }
}