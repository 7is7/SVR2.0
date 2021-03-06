using System.ComponentModel.DataAnnotations.Schema;

namespace VueExample.Entities
{
    [Table("Die_Graphics")]
    public class DieGraphics
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("id_mr")]
        public int? MeasurementRecordingId { get; set; }
        [Column("id_die")]
        public long? DieId { get; set; }
        [Column("id_graphic")]
        public int GraphicId { get; set; }
        [Column("StringGraphic")]
        public string ValueString { get; set; }
    }
}
