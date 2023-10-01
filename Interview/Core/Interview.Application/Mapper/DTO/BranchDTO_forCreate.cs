using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class BranchDTO_forCreate
    {


        [Required(ErrorMessage = "BranchName is required")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "SectorId is required")]
        public int SectorId { get; set; }


    }
}
