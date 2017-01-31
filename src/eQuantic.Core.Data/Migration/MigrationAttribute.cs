using System;

namespace eQuantic.Core.Data.Migration
{
    public class MigrationAttribute : Attribute
    {
        public DateTime Date { get; private set; }
        public MigrationAttribute(string title, int year, int month, int day, int hour, int mintute, int second)
        {
            Date = new DateTime(year, month, day, hour, mintute, second);
        }
    }
}
