using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace MusicScaleBuilder
{
    /// <summary>
    /// Нота в гамме
    /// </summary>
    public class NotePozition
    {
        /// <summary>
        /// Интервал от предыдущей ноты
        /// </summary>
        [XmlElementAttribute(IsNullable = false, ElementName ="IntervalFromLastNote")]
        public int Interval { get; set; }

        /// <summary>
        /// Ступень
        /// </summary>

        [XmlElementAttribute(IsNullable = false, ElementName = "StupenIsNote")]
        public int Stupen { get; set; }

        /// <summary>
        /// Создание класса
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="stupen"></param>
        public NotePozition(int interval, int stupen)
        {
            Interval = interval;
            Stupen = stupen;
        }

        public NotePozition():this(1,1)
        {

        }

    }
}
