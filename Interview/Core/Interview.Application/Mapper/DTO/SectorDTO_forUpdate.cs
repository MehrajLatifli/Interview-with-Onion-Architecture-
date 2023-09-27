using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class SectorDTO_forUpdate
    {

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }


        [Required(ErrorMessage = "SectorName is required")]
        public string SectorName { get; set; }




    }
}
