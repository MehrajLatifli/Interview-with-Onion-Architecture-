using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class VacancyDTO_forCreate
    {
        [Required(ErrorMessage = "Title name is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description name is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "JobDegreeId name is required")]
        public int JobDegreeId { get; set; }

        [Required(ErrorMessage = "SectorId name is required")]
        public int SectorId { get; set; }



    }
}
