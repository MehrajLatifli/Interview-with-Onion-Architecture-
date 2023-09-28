using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class JobDegreeDTO_forUpdate
    {

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }



        [Required(ErrorMessage = "Degree is required")]
        public string Degree { get; set; }



    }
}
