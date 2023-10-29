using System.ComponentModel.DataAnnotations;
using RangeAttribute = Interview.Application.Validations.RangeAttribute;

namespace Interview.Application.Mapper.DTO
{
    public class SessionQuestionDTO_forUpdate
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Value is required")]
        [Range(1.0, 5.0)]
        public int Value { get; set; }

        [Required(ErrorMessage = "SessionId is required")]
        public int SessionId { get; set; }

        [Required(ErrorMessage = "QuestionId is required")]
        public int QuestionId { get; set; }

    }
}
