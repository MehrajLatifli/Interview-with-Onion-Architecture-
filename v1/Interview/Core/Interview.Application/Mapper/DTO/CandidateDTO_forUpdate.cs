using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class CandidateDTO_forUpdate
    {

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }


        [Required(ErrorMessage = "CandidateDocumentId is required")]

        public int CandidateDocumentId { get; set; }
    }
}
