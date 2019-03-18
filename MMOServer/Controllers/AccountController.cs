using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMOServer.Exceptions;
using MMOServer.Repositories;

namespace MMOServer.Controllers
{
    class AccountController
    {

        public void CreateAccount(string email, string password)
        {       
            // ----
            //TODO: Password format validation
            // ----

            AccountRepository repo = new AccountRepository();

            if (repo.CheckAccountByEmail(email) > 0)
            {
                throw new AccountAlreadyExistException("The account could not be created. The account Email is already being used.");
            }

            int accountID = GenerateAccountID();

            while (repo.CheckAccountById(accountID) > 0)
            {
                accountID = GenerateAccountID();
            }            

            try
            {
                repo.InsertAccount(accountID, email, password);

            }catch(Exception ex)
            {
                //Problem accessing database
                Console.WriteLine("Could not save account into the database.");
                Console.WriteLine(ex.Message);
            }
        }

        //Returns a 7 digit account id that doesn't start with 0.
        private int GenerateAccountID()
        {
            Random rnd = new Random();
            int digit = 0;
            string strAccId = "";

            for (int i = 0; i < 7; i++)
            {
                if (i == 0)
                {
                    //generates an integer between 1 and 9
                    digit = rnd.Next(1, 10);
                    strAccId = strAccId + digit;
                } else
                {
                    //generates an integer between 0 and 9
                    digit = rnd.Next(10);
                    strAccId = strAccId + digit;
                }
            }

            return int.Parse(strAccId);
        }

    }
}
