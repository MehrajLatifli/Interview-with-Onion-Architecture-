using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Application.Mapper.DTO
{
    public class BranchDTO_forGetandGetAll
    {
        public int Id { get; set; }

      
        public string BranchName { get; set; }

        public int SectorId { get; set; }


    }
}
