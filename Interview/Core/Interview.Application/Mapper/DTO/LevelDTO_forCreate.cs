using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class LevelDTO_forCreate
    {


        [Required(ErrorMessage = "Name  is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Coefficient  is required")]
        public decimal Coefficient { get; set; }

    }
}
