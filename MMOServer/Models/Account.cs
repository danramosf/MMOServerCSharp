using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Models
{
    class Account
    {

        private int accountID;
        private string email;
        private string password;
        private List<Player> playerList;
        private AccountStatus status;
        private DateTime dateCreated;
        private DateTime lastLogin;
        private DateTime dateDeleted;
        private DateTime lastBanned;
        private int daysBanned;

        //Constructor
        public Account(int accountID, string email, string password, List<Player> playerList, AccountStatus status, DateTime dateCreated, DateTime lastLogin, DateTime dateDeleted, DateTime lastBanned, int daysBanned)
        {
            this.accountID = accountID;
            this.email = email;
            this.password = password;
            this.playerList = playerList;
            this.status = status;
            this.dateCreated = dateCreated;
            this.lastLogin = lastLogin;
            this.dateDeleted = dateDeleted;
            this.lastBanned = lastBanned;
            this.daysBanned = daysBanned;
        }
    }

    public enum AccountStatus
    {
        Active = 1,
        Premium = 2,
        Inactive = 3,
        Banned = 4
    }
}
