using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Datum
    {
        public int DatumID { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return ConvertDagToNederlands(this.Date.DayOfWeek.ToString()) + " (" + this.Date.Day + "/" + this.Date.Month + "/" + this.Date.Year + ")";
        }

        private string ConvertDagToNederlands(string s)
        {
            switch(s)
            {
                case "Monday":
                    return "Maandag";
                case "Tuesday":
                    return "Dinsdag";
                case "Wednesday":
                    return "Woensdag";
                case "Thursday":
                    return "Donderdag";
                case "Friday":
                    return "Vrijdag";
                case "Saturday":
                    return "Zaterdag";
                case "Sunday":
                    return "Zondag";
                default:
                    return null;
            }
        }
    }
}
