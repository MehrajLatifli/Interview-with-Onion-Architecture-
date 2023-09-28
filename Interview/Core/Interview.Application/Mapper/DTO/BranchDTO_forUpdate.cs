using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class BranchDTO_forUpdate
    {

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }


        [Required(ErrorMessage = "BranchName is required")]
        public string BranchName { get; set; }


        [Required(ErrorMessage = "SectorId is required")]
        public int SectorIdForBranch { get; set; }




    }
}
