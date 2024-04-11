using Quizify.DataAccess.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Quizify.DataAccess.Entities
{
    public class NetworkInfo : EntityDto
    {
        public string Code { get; set; }
        public string NetworkName { get; set; }
    }
}
