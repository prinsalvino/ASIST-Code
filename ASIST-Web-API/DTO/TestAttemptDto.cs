using System;

namespace ASIST_Web_API.DTO
{
    public class TestAttemptDto
    {
        public long StudentId { get; set; }
        public int FinalScore { get; set; }
        public DateTime DateOfTest { get; set; }
    }
}