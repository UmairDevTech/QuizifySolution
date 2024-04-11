using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Quizify.DataAccess.Entities.Common;

#pragma warning disable CS8618
namespace Quizify.DataAccess.Entities
{
    public class ContentInfo : EntityDto
    {
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
        public int UserId { get; set; }
    }
}
