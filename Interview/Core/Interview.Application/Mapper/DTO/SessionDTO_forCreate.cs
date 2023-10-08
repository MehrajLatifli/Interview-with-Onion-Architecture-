using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class SessionDTO_forCreate
    {

        public decimal EndValue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "VacancyId is required")]
        public int VacancyId { get; set; }

        [Required(ErrorMessage = "CandidateId is required")]
        public int CandidateId { get; set; }

    }
}
