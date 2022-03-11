using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        [Range(30,int.MaxValue,ErrorMessage ="Capacity must be more than 30")]
        public int SeatCapacity { get; set; }
        public AirlineModel()
        {
            TransportSchedules = new List<TransportScheduleModel>();
        }
    }
}