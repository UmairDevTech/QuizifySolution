using Quizify.DataAccess.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Quizify.DataAccess.Entities
{
    public class UserInfo : EntityDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ThreeDigitNumber { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
