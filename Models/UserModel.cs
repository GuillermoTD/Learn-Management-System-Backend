using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Learn_Managment_System_Backend.Models
{
    public class UserModel
    {

        //Esta propiedad setea este campo como tipo _id en mongodb
        [BsonId]
        //Esta propiedad le dice a mongodb que este campo de en este modelo representa el _id de un documento
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonRequired]
        public required string Name { get; set; }

        [BsonRequired]
        public required string LastName { get; set; }

        [BsonRequired]
        public required string Age { get; set; }

        [BsonRequired]
        public required string Email { get; set; }

        public string PhoneNumber { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Registration_Date { get; set; }

        public string Token { get; set; }

        public string User { get; set; }

        public string Password { get; set; }


    }
}