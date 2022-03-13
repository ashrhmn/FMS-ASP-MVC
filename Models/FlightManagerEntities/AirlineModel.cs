using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Flight_Management_System.Models.FlightManagerEntities
{
    public class AirlineModel
    {
        public int Id { get; set; }
        [DisplayName("Aircraft Name")]
        [Required(AllowEmptyStrings =false)]
        [DisplayFormat(ConvertEmptyStringToNull =false)]
        public string Name { get; set; }
        public List<TransportScheduleModel> TransportSchedules { get; set; }

        [Required]
        [Range(30,int.MaxValue,ErrorMessage ="Capacity must be at least 30")]
        public int SeatCapacity { get; set; }
        public AirlineModel()
        {
            TransportSchedules = new List<TransportScheduleModel>();
        }
    }
}