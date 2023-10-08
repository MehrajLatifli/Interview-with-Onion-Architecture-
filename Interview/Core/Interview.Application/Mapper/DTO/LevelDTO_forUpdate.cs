using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class LevelDTO_forUpdate
    {
        [Required(ErrorMessage = "Id  is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Coefficient is required")]
        public decimal Coefficient { get; set; }

    }
}
