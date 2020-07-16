using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace KantorWCF
{
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę interfejsu „IService1” w kodzie i pliku konfiguracji.
    [ServiceContract]
    public interface IService1
    {
        /*
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        */
        // TODO: dodaj tutaj operacje usługi
        [OperationContract]
        string CheckArchiveRate(string date, string value);

        [OperationContract]
        double MakeExchangeSell(double money, string value);

        [OperationContract]
        double MakeExchangeBuy(double money, string value);

        [OperationContract]
        string GetExchangeRate(string value);

        //OPERACJE NA BAZIE DANYCH
        [OperationContract]
        string RejestracjaUzytkownika(UserDetails userInfo);
        [OperationContract]
        Boolean LogowanieUzytkownika(UserDetails userInfo);
        [OperationContract]
        Dictionary<string, string> PobierzDane(UserDetails userInfo);
        [OperationContract]
        Dictionary<string, decimal> GetStanKonta(UserDetails userInfo);
        [OperationContract]
        string UpdateStanKonta(AccountDetails account);
        [OperationContract]
        string UpgradeStanKonta(AccountDetails account);
        [OperationContract]
        string GetHistoriaPrzelewow(TransferHistory history);
        [OperationContract]
        string SetHistoriaPrzelewow(TransferHistory history);
        [OperationContract]
        string GetTransakcje(Transactions transaction);
        [OperationContract]
        string SetTransakcje(Transactions transaction);
        [OperationContract]
        string DeleteAccount(UserDetails userInfo);
        
    }

    public class UserDetails
    {
        string name;
        string surname;
        string email;
        string password;

        [DataMember]
        public string UserName
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember]
        public string UserSurname
        {
            get { return surname; }
            set { surname = value; }
        }
        [DataMember]
        public string UserEmail
        {
            get { return email; }
            set { email = value; }
        }
        [DataMember]
        public string UserPassword
        {
            get { return password; }
            set { password = value; }
        }
    }

    public class AccountDetails
    {
        int userID;
        string currency;
        decimal amount;
        

        [DataMember]
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        [DataMember]
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        [DataMember]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
    }

    public class TransferHistory
    {
        int userID;
        string type;
        string currency;
        decimal amount;
        SqlDateTime date;


        [DataMember]
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        [DataMember]
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        [DataMember]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        [DataMember]
        public SqlDateTime Date
        {
            get { return date; }
            set { date = value; }
        }
    }

    public class Transactions
    {
        int userID;
        string type;
        SqlDateTime date;
        decimal pln;
        string currency;
        decimal exchange;
        decimal score;
        
        [DataMember]
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        [DataMember]
        public SqlDateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        [DataMember]
        public decimal Pln
        {
            get { return pln; }
            set { pln = value; }
        }
        [DataMember]
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        [DataMember]
        public decimal Exchange
        {
            get { return exchange; }
            set { exchange = value; }
        }
        [DataMember]
        public decimal Score
        {
            get { return score; }
            set { score = value; }
        }
        
        
    }

    // Użyj kontraktu danych, jak pokazano w poniższym przykładzie, aby dodać typy złożone do operacji usługi.
    /*
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }*/
}
