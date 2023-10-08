using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class StructureTypeDTO_forUpdate
    {

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

    }
}
