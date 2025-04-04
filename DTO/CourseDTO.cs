namespace Learn_Managment_System_Backend.DTO
{
    public class CourseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorId { get; set; }
        public string Category { get; set; }
        public string CourseFrontImage { get; set; }
        public decimal Price { get; set; }
        public string Language { get; set; }
        public List<Lesson> Lessons { get; set; }
        public int EnrolledStudents { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<string> LearningGoals { get; set; }
        public List<ItemVideo> CourseVideos { get; set; }

        public class Lesson
        {
            public string Title { get; set; }
            public int Duration { get; set; }
            public string VideoUrl { get; set; }
        }

        public class Rating
        {
            public string UserId { get; set; }
            public string Comment { get; set; }
            public int Rate { get; set; }
        }

        public class ItemVideo {
            public string description { get; set; }
            public string video { get; set; }
        
        }


    }
}