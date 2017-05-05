namespace PhotographyWorkshops.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CamerasMirrorless")]
    public class CameraMirrorless : Camera
    {
        public string MaxVideoResolution { get; set; }

        public int MaxFrameRate { get; set; }
    }
}
