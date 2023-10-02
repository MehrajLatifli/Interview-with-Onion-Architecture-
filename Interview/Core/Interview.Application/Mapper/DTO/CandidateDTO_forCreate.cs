using Interview.Application.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class CandidateDTO_forCreate
    {

        [Required(ErrorMessage = "Candidate surname is required")]
        public string CandidateSurname { get; set; }

        [Required(ErrorMessage = "Candidate name is required")]
        public string CandidateName { get; set; }

        [Required(ErrorMessage = "Candidate phonenumber name is required")]
        public string CandidatePhonenumber { get; set; }

        [Required(ErrorMessage = "Candidate email is required")]
        [Email(ErrorMessage = "Invalid email format.")]
        public string CandidateEmail { get; set; }

        [Required(ErrorMessage = "Curriculum Vitae is required")]
        [AllowedExtensions(new string[] { ".pdf", ".doc", ".epub", ".docx" })]
        [FileSize(5, 10)]
        public IFormFile CurriculumVitae { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }


    }
}
