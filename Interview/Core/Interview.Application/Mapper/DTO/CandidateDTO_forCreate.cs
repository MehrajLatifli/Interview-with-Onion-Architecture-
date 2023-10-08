using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Application.Mapper.DTO
{
    public class CandidateDTO_forCreate
    {

        [Required(ErrorMessage = "CandidateDocumentId is required")]

        public int CandidateDocumentId { get; set; }
    }
}
