using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class GetUserByIdResponse
    {
        public bool succeeded { get; set; }
        public Entity entity { get; set; }
        public object permissions { get; set; }
        public string message { get; set; }
        public string[] messages { get; set; }
    }

    public class Entity
    {
        public string firstName { get; set; }
        public string password { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string bvn { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime lastAccessedDate { get; set; }
        public string userId { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string profilePicture { get; set; }
        public string token { get; set; }
        public int userAccessLevel { get; set; }
        public string transactionPin { get; set; }
        public int gender { get; set; }
        public string userName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string createdBy { get; set; }
        public string createdById { get; set; }
        public DateTime createdDate { get; set; }
        public string lastModifiedBy { get; set; }
        public string lastModifiedById { get; set; }
        public string lastModifiedDate { get; set; }
        public int status { get; set; }
        public string statusDesc { get; set; }
    }
}
