using System.ComponentModel.DataAnnotations;

namespace Blogic_task.Models
{
    public class Smlouva : IValidatableObject
    {
        [Key]
        public string EvidencniCislo { get; set; } = string.Empty;

        public Instituce Instituce { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatumUzavreni { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatumPlatnosti { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DatumUkonceni { get; set; }

        public int KlientId { get; set; }
        public Klient Klient { get; set; } = null!;

        public int SpravceId { get; set; }
        public Poradce Spravce { get; set; } = null!;

        public ICollection<Poradce> DalsiPoradci { get; set; } = new List<Poradce>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumPlatnosti < DatumUzavreni)
            {
                yield return new ValidationResult(
                    "Smlouva nemůže začít platit dříve, než byla uzavřena.",
                    new[] { nameof(DatumPlatnosti) }
                );
            }

            if (DatumUkonceni.HasValue)
            {
                if (DatumUkonceni.Value < DatumUzavreni)
                {
                    yield return new ValidationResult(
                        "Smlouva nemůže být ukončena dříve, než byla uzavřena.",
                        new[] { nameof(DatumUkonceni) }
                    );
                }

                if (DatumUkonceni.Value < DatumPlatnosti)
                {
                    yield return new ValidationResult(
                        "Smlouva nemůže být ukončena dříve, než vůbec začala platit.",
                        new[] { nameof(DatumUkonceni) }
                    );
                }
            }
        }
    }
}