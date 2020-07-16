using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace KantorWCF
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie, usłudze i pliku konfiguracji.
    // UWAGA: aby uruchomić klienta testowego WCF w celu przetestowania tej usługi, wybierz plik Service1.svc lub Service1.svc.cs w eksploratorze rozwiązań i rozpocznij debugowanie.
    public class Service1 : IService1
    {

        NBPWebApi NBPWebApi = new NBPWebApi();


        public string CheckArchiveRate(string date, string value)
        {
            return string.Format("Kurs waluty {0} z dnia {1} wynosił {2} zł", NBPWebApi.GetCurrencyName(value), date, NBPWebApi.GetCurrencyArchive(value, date));
        }


        public double MakeExchangeSell(double money, string value)
        {
            return money * NBPWebApi.GetCurrencyBid(value);
        }

        public double MakeExchangeBuy(double money, string value)
        {
            return money * NBPWebApi.GetCurrencyAsk(value);
        }
        public string GetExchangeRate(string value)
        {
            
            return string.Format("Aktualny kurs waluty {0} wynosi {1}", NBPWebApi.GetCurrencyName(value), NBPWebApi.GetCurrencyRate(value));

        }

        //Funkcje odpowiadające bazie danych KantorDB
        public string RejestracjaUzytkownika(UserDetails userInfo)
        {
            string Message;
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("insert into Uzytkownicy(imie, nazwisko, email, haslo) values(@imie,@nazwisko,@email,@haslo)", conect);
            cmd.Parameters.AddWithValue("@imie", userInfo.UserName);
            cmd.Parameters.AddWithValue("@nazwisko", userInfo.UserSurname);
            cmd.Parameters.AddWithValue("@email", userInfo.UserEmail);
            cmd.Parameters.AddWithValue("@haslo", userInfo.UserPassword);
            int result = cmd.ExecuteNonQuery();
            if(result == 1)
            {
                Message = string.Format("Użytkownik {0} {1} został utworzony pomyślnie. Możesz się już zalogować.", userInfo.UserName, userInfo.UserSurname);
            }
            else
            {
                Message = string.Format("Wystąpił błąd rejestracji użytkownika {0} {1}", userInfo.UserName, userInfo.UserSurname);
            }
            conect.Close();
            Dictionary<string, string> userData = PobierzDane(userInfo);
            AccountDetails plnAmount = new AccountDetails() { UserID=Int32.Parse(userData["ID"]), Currency="PLN", Amount=0};
            UpgradeStanKonta(plnAmount);
            return Message;
        }
        public Boolean LogowanieUzytkownika(UserDetails userInfo)
        {
            Boolean auth = false;
            string email = userInfo.UserEmail;
            string pass = userInfo.UserPassword;
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("SELECT email, haslo FROM Uzytkownicy WHERE email=@Email", conect);
            cmd.Parameters.AddWithValue("@Email", SqlDbType.NVarChar);
            if(userInfo.UserEmail == null)
            {
                cmd.Parameters["@Email"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@Email"].Value = userInfo.UserEmail;
            }
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                if((email == reader.GetString(0)) && (pass == reader.GetString(1)))
                {
                    auth = true;
                }
                conect.Close();
                return auth;
            }
            else
            {
                conect.Close();
                return auth;
            }
        }
        public Dictionary<string, string> PobierzDane(UserDetails userInfo)
        {
            Dictionary<string, string> userData = new Dictionary<string, string>();
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("SELECT id_user, imie, nazwisko, email FROM Uzytkownicy WHERE email=@Email", conect);
            cmd.Parameters.AddWithValue("@Email", SqlDbType.NVarChar);
            if (userInfo.UserEmail == null)
            {
                cmd.Parameters["@Email"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@Email"].Value = userInfo.UserEmail;
            }
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                userData.Add("ID", reader.GetInt32(0).ToString());
                userData.Add("Imie", reader.GetString(1));
                userData.Add("Nazwisko", reader.GetString(2));
                userData.Add("Email", reader.GetString(3));
                conect.Close();
                return userData;
            }
            else
            {
                conect.Close();
                userData.Add("False", "False");
                return userData;
            }
        }
        public string UpgradeStanKonta(AccountDetails account)
        {
            string Message;
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("insert into Stankonta(id_user, waluta, ilosc) values(@IDuser,@Waluta,@Ilosc)", conect);
            cmd.Parameters.AddWithValue("@IDuser", account.UserID);
            cmd.Parameters.AddWithValue("@Waluta", account.Currency);
            cmd.Parameters.AddWithValue("@Ilosc", account.Amount);
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                Message = string.Format("Stan konta został zaktualizowany: Uzytkownik ID:{0}, Waluta:{1}, Kwota:{2}", account.UserID, account.Currency, account.Amount);
            }
            else
            {
                Message = string.Format("Wystąpił błąd aktualizacji.");
            }
            conect.Close();
            return Message;
        }
        public string UpdateStanKonta(AccountDetails account)
        {
            string Message;
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand select = new SqlCommand("SELECT ilosc FROM Stankonta WHERE id_user=@IDuser AND waluta=@Waluta", conect);
            decimal amountValue = 0;
            select.Parameters.AddWithValue("@IDuser", account.UserID);
            select.Parameters.AddWithValue("@Waluta", account.Currency);
            SqlDataReader reader = select.ExecuteReader();
            if(reader.Read())
            {
                amountValue = reader.GetDecimal(0);
            }
            conect.Close();
            conect.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Stankonta SET ilosc = @Ilosc WHERE id_user = @IDuser AND waluta = @Waluta", conect);
            amountValue += account.Amount;
            cmd.Parameters.AddWithValue("@IDuser", account.UserID);
            cmd.Parameters.AddWithValue("@Waluta", account.Currency);
            cmd.Parameters.AddWithValue("@Ilosc", amountValue);
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                Message = string.Format("Stan konta został zaktualizowany: Uzytkownik ID:{0}, Waluta:{1}, Kwota:{2}", account.UserID, account.Currency, account.Amount);
            }
            else
            {
                Message = string.Format("Wystąpił błąd aktualizacji.");
            }
            conect.Close();
            return Message;

        }
        public Dictionary<string, decimal> GetStanKonta(UserDetails userInfo)
        {
            Dictionary<string, string> userData = PobierzDane(userInfo);
            Dictionary<string, decimal> accountInfo = new Dictionary<string, decimal>();
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("SELECT waluta, ilosc FROM Stankonta WHERE id_user=@ID", conect);
            cmd.Parameters.AddWithValue("@ID", SqlDbType.Int);
            if ( userData["ID"] == null)
            {
                cmd.Parameters["@ID"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@ID"].Value = userData["ID"];
            }
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accountInfo.Add(reader.GetString(0), reader.GetDecimal(1));
            }
            conect.Close();
            return accountInfo;
        }

        public string GetHistoriaPrzelewow(TransferHistory history)
        {
            string Message = string.Empty;
            List<TransferHistory> historyList = new List<TransferHistory>();
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("SELECT typ, waluta, kwota, data FROM Historiaprzelewow WHERE id_user=@ID", conect);
            cmd.Parameters.AddWithValue("@ID", SqlDbType.Int);
            object parameter;
            if (history.UserID == null)
            {
                cmd.Parameters["@ID"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@ID"].Value = history.UserID;
            }
            parameter = cmd.Parameters["@ID"].Value;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                historyList.Add(new TransferHistory() {UserID=history.UserID, Type=reader.GetString(0), Currency=reader.GetString(1), Amount=reader.GetDecimal(2), Date=reader.GetDateTime(3) });
            }
            foreach (TransferHistory line in historyList)
            {
                Message += line.Type + line.Currency + line.Amount + line.Date + ";";
            }
            conect.Close();
            return Message;
        }
        public string SetHistoriaPrzelewow(TransferHistory history)
        {
            string Message;
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("insert into Historiaprzelewow(id_user, typ, waluta, kwota, data) values(@IDuser,@Typ,@Waluta,@Kwota,@Data)", conect);
            cmd.Parameters.AddWithValue("@IDuser", history.UserID);
            cmd.Parameters.AddWithValue("Typ", history.Type);
            cmd.Parameters.AddWithValue("@Waluta", history.Currency);
            cmd.Parameters.AddWithValue("@Kwota", history.Amount);
            cmd.Parameters.AddWithValue("@Data", history.Date);
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                Message = string.Format("Przelew został wykonany: Uzytkownik ID:{0}, Typ przelewu:{1}, Waluta:{2}, Kwota:{3}, Data:{4}", history.UserID, history.Type, history.Currency, history.Amount, history.Date);
            }
            else
            {
                Message = string.Format("Wystąpił błąd przelewu.");
            }
            conect.Close();
            return Message;
        }
        public string GetTransakcje(Transactions transaction)
        {
            string Message = string.Empty;
            List<Transactions> transactionList = new List<Transactions>();
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("SELECT typ, data, pln, waluta, kurs, wynik FROM Transakcje WHERE id_user=@ID", conect);
            cmd.Parameters.AddWithValue("@ID", SqlDbType.Int);
            object parameter;
            if (transaction.UserID == null)
            {
                cmd.Parameters["@ID"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@ID"].Value = transaction.UserID;
            }
            parameter = cmd.Parameters["@ID"].Value;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                transactionList.Add(new Transactions() {UserID=transaction.UserID, Type=reader.GetString(0), Date=reader.GetDateTime(1), Pln=reader.GetDecimal(2), Currency=reader.GetString(3), Exchange=reader.GetDecimal(4), Score=reader.GetDecimal(5) });
            }
            foreach (Transactions line in transactionList)
            {
                Message += line.Type + line.Currency + line.Date + line.Pln + line.Exchange + line.Score + "; ";
            }
            conect.Close();
            return Message;
        }
        public string SetTransakcje(Transactions transaction)
        {
            string Message;
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand cmd = new SqlCommand("insert into Transakcje(id_user, typ, data, pln, waluta, kurs, wynik) values(@IDuser,@Typ,@Data,@Pln,@Waluta,@Kurs,@Wynik)", conect);
            cmd.Parameters.AddWithValue("@IDuser", transaction.UserID);
            cmd.Parameters.AddWithValue("@Typ", transaction.Type);
            cmd.Parameters.AddWithValue("@Data", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.AddWithValue("@Pln", transaction.Pln);
            cmd.Parameters.AddWithValue("@Waluta", transaction.Currency);
            cmd.Parameters.AddWithValue("@Kurs", transaction.Exchange);
            cmd.Parameters.AddWithValue("@Wynik", transaction.Score);            
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                Message = string.Format("Transakcja została wykonana: Waluta:{0} Wynik:{1}", transaction.Currency, transaction.Score);
            }
            else
            {
                Message = string.Format("Wystąpił błąd transakcji.");
            }
            AccountDetails user = new AccountDetails() { UserID=transaction.UserID, Currency=transaction.Currency, Amount=transaction.Score };
            SqlCommand checkexist = new SqlCommand("SELECT 1 FROM Stankonta WHERE waluta=@Waluta", conect);
            checkexist.Parameters.AddWithValue("@Waluta", SqlDbType.VarChar);
            if (transaction.Currency == null)
            {
                checkexist.Parameters["@Waluta"].Value = DBNull.Value;
            }
            else
            {
                checkexist.Parameters["@Waluta"].Value = transaction.Currency;
            }
            SqlDataReader reader = checkexist.ExecuteReader();
            if (reader.Read())
            {
                UpdateStanKonta(user);
            }
            else
            {
                UpgradeStanKonta(user);
            }
            reader.Close();
            conect.Close();
            return Message;
        }
        
        public string DeleteAccount(UserDetails userInfo)
        {
            string Message = string.Empty;
            int userID = 0;
            SqlConnection conect = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=KantorDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conect.Open();
            SqlCommand selectID = new SqlCommand("SELECT id_user FROM Uzytkownicy WHERE email=@Email", conect);
            selectID.Parameters.AddWithValue("@Email", SqlDbType.VarChar);
            if (userInfo.UserEmail == null)
            {
                selectID.Parameters["@Email"].Value = DBNull.Value;
            }
            else
            {
                selectID.Parameters["@Email"].Value = userInfo.UserEmail;
            }
            SqlDataReader reader = selectID.ExecuteReader();
            if (reader.Read()) userID = reader.GetInt32(0);
            reader.Close();
            SqlCommand cmd = new SqlCommand("DELETE FROM Historiaprzelewow WHERE id_user=@ID;" +
                "DELETE FROM Transakcje WHERE id_user=@ID;" +
                "DELETE FROM Stankonta WHERE id_user=@ID;" +
                "DELETE FROM Uzytkownicy WHERE id_user=@ID", conect);
            cmd.Parameters.AddWithValue("@ID", userID);
            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                Message += "Uzytkownik został usunięty";
            }
            else
            {
                Message += "Wsytąpił błąd. Spróbuj ponownie";
            }
            conect.Close();
            return Message;
        }

        /* Niepotrzebne
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        */
    }
}
