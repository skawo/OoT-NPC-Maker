using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class Helpers
    {
        public static UInt16 GetOcarinaTime(string MilitaryTimeString)
        {
            try
            {
                string[] HourMinute = MilitaryTimeString.Split(':');

                if (HourMinute.Length != 2)
                    throw new Exception("Time in wrong format!");

                byte Hour = Convert.ToByte(HourMinute[0]);
                byte Minute = Convert.ToByte(HourMinute[1]);

                if ((Hour > 23) || (Minute > 59))
                    throw new Exception("Time in wrong format!");

                decimal Time = (((Hour * 60) + Minute) * (Int16.MaxValue / 1439));

                return Convert.ToUInt16(Time);
            }
            catch (Exception)
            {
                throw new Exception("Time in wrong format!");
            }
        }

        public static DateTime GetTimeFromOcarinaTime(UInt16 Value)
        {
            int Minutes = Value / (Int16.MaxValue / 1439);

            DateTime Out = new DateTime(2000, 01, 01, 0, 0, 0);
            return Out.AddMinutes(Minutes);
        }
    }
}
