using System.ComponentModel.DataAnnotations;

namespace Blogic_task.Models
{
    public class Poradce
    {
        public int Id { get; set; }

        [Required]
        public string Jmeno { get; set; } = string.Empty;

        [Required]
        public string Prijmeni { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string RodneCislo { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DatumNarozeni { get; set; }

        public ICollection<Smlouva> SpravovaneSmlouvy { get; set; } = new List<Smlouva>();

        public ICollection<Smlouva> DalsiSmlouvy { get; set; } = new List<Smlouva>();
    }
}