using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Models
{
    class Player
    {
        private int accountID;

        private string name;
        private PlayerClass pclass;
        private int level;
        private double experiencePoints;
        private double lastPosX;
        private double lastPosY;
        private double lastPosZ;

        public Player(int accountID, string name, int pclass, int level, double expPoints, double lastX, double lastY, double lastZ)
        {
            this.accountID = accountID;
            this.name = name;
            this.pclass = (PlayerClass)pclass;
            this.level = level;
            this.experiencePoints = expPoints;
            this.lastPosX = lastX;
            this.lastPosY = lastY;
            this.lastPosZ = lastZ;
        }

    }

    public enum PlayerClass
    {
        Warrior = 1,
        Mage = 2,
        Ranger = 3,
        Assassin = 4,
    }
}
