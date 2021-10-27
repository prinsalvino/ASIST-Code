using System;

namespace ASIST_Web_API.DTO
{
    public class SportAdviceDto
    {
        public long StudentId { get; set; }
        public long SportId { get; set; }
        public DateTime DateOfSportAdvices { get; set; }
        public int Score { get; set; }
    }
}