using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class VacancyDTO_forUpdate
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "JobDegreeId is required")]
        public int JobDegreeId { get; set; }

        [Required(ErrorMessage = "SectorId is required")]
        public int SectorId { get; set; }


    }
}
