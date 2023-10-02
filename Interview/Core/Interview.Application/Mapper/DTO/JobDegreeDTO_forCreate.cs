using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class JobDegreeDTO_forCreate
    {

        [Required(ErrorMessage = "Degree is required")]
        public string Degree { get; set; }

    }
}
