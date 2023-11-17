using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class StructureTypeDTO_forCreate
    {



        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

    }
}
