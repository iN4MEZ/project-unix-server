using MongoDB.Bson;

namespace Project_UNIX.Common.Database.Models
{
    public class AccountModel
    {
        public ObjectId _id;

        public string username {  get; set; }
        public string password { get; set; }

        public long uid { get; set; }

        public DateTime createDate { get; set; }
        public string secret { get; set; }
        public string token { get; set; }
    }
}
