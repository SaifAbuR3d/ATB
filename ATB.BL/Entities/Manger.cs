﻿namespace ATB.Entities
{

    [Obsolete]
    public class Manger
    {
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }

        public Manger(string managerId, string managerName)
        {
            ManagerId = managerId;
            ManagerName = managerName;
        }
    }
}
