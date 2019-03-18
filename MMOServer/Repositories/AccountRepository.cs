using MMOServer.MMODatabaseDataSetTableAdapters;
using MMOServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Repositories
{
    class AccountRepository
    {

        private MMODatabaseDataSet ds;
        private AccountTableAdapter ta;

        public AccountRepository()
        {
            this.ds = new MMODatabaseDataSet();
            this.ta = new AccountTableAdapter();
            ta.Fill(ds.Account);
        }

        public int CheckAccountById(int id)
        {
            DataRow[] d = ds.Account.Select("accountID = " + id );
            
            return d.Length;
        }

        public int CheckAccountByEmail(string email)
        {
            DataRow[] d = ds.Account.Select("email = '" + email + "'");

            return d.Length;
        }

        public void InsertAccount(int accountID, string email, string password)
        {
            //Create a new account row in the dataset
            MMODatabaseDataSet.AccountRow accountRow = ds.Account.NewAccountRow();

            //Insert data into this row in the dataset.
            accountRow.accountID = accountID;
            accountRow.email = email;
            accountRow.password = password;
            //Add this row to the dataset
            ds.Account.AddAccountRow(accountRow);

            //Insert the new row in the database.
            ta.Update(ds.Account);

        }

    }
}
