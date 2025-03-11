using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Learn_Managment_System_Backend.Models
{
    public class CourseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Titulo { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Descripcion { get; set; } = string.Empty;

        [BsonElement("instructor_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InstructorId { get; set; } = string.Empty;

        [BsonElement("category")]
        public string Categoria { get; set; } = string.Empty;

        [BsonElement("course_front_image")]
        public string CourseFrontImage { get; set; } = string.Empty;

        [BsonElement("language")]
        public string Language { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Precio { get; set; } = 0;

        [BsonElement("lessons")]
        public List<Lessons> Lecciones { get; set; } = new();

        [BsonElement("enrolled_students")]
        public int EstudiantesInscritos { get; set; } = 0;

        [BsonElement("ratings")]
        public List<Ratings> Valoraciones { get; set; } = new();

        [BsonElement("creation_date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Creation_Date { get; set; }
    }

    public class Lessons
    {
        [BsonElement("title")]
        public string Titulo { get; set; } = string.Empty;

        [BsonElement("duration")]
        public int Duracion { get; set; } = 0; // Duración en minutos

        [BsonElement("video_url")]
        public string VideoUrl { get; set; } = string.Empty;
    }

    public class Ratings
    {
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = string.Empty;

        [BsonElement("comment")]
        public string Comentario { get; set; } = string.Empty;

        [BsonElement("rate")]
        public int Calificacion { get; set; } = 0; // 1 - 5 estrellas
    }
}