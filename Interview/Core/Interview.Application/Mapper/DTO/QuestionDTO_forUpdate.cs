using System.ComponentModel.DataAnnotations;

namespace Interview.Application.Mapper.DTO
{
    public class QuestionDTO_forUpdate
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        [Required(ErrorMessage = "LevelId is required")]
        public int LevelId { get; set; }

        [Required(ErrorMessage = "SessionTypeId is required")]
        public int SessionTypeId { get; set; }

        [Required(ErrorMessage = "StructureId is required")]
        public int StructureId { get; set; }

    }
}
