using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Quizify.DataAccess.Entities.Common
{
    public class EntityDto
    {
        [Key]
        public int Id { get; set; }
    }
}
