namespace PhotographyWorkshops.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CamerasDslr")]
    public class CameraDslr : Camera
    {
        public int MaxShutterSpeed { get; set; }
    }
}