namespace Innovation.Sample.Infrastructure.Settings
{
    using System.ComponentModel.DataAnnotations;

    public class DataBaseSettings
    {
        [Required]
        public string ConnectionString { get; set; }
    }
}
