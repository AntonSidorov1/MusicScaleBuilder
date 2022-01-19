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
    /// Класс - структура гаммы
    /// </summary>

    [XmlRoot("GammStrusct", Namespace = "http://www.NoteGamma.com", IsNullable = false)]
    [DataContract]
    public class GammStrusct
    {
        int[] stupens;
        int[] intervals;
        int[,] stupenIntervals;
        string name;


        [XmlArrayAttribute("NotePositions")]
        /// <summary>
        /// Возвращает или задаёт массив позиций нот (структуру гаммы)
        /// </summary>
        public NotePozition[] NotePozitions
        {
            get
            {
                NotePozition[] result = new NotePozition[stupenIntervals.GetLength(1)];
                int[,] result1 = this.StupenIntervals;
                for(int i = 0; i < result.Length; i++)
                {
                    result[i] = new NotePozition(result1[0, i], result1[1, i]);
                }

                return result;
            }
            set
            {
                NotePozition[] result = value;
                int[,] result1 = new int[2, result.Length];
                for (int i = 0; i < result.Length; i++) 
                {
                    result1[0, i] = result[i].Interval;
                    result1[1, i] = result[i].Stupen;
                }
                this.StupenIntervals = result1;
            }
        }


        public GammStrusct():this("")
        {

        }

        /// <summary>
        /// Создаёт структуру гаммы, с названием лада name
        /// </summary>
        public GammStrusct(string name = "")
        {
            stupens = new int[1];
            intervals = new int[1];

            Stupens = new int[1];
            Intervals = new int[1];

            this.name = name;
        }

        /// <summary>
        /// Создаёт структуру гаммы из последавательности ступеней stupens и последовательности интервалов intervals, с названием лада name
        /// </summary>
        /// <param name="stupens"></param>
        /// <param name="intervals"></param>
        public GammStrusct(int[] stupens, int[] intervals, string name = "") : this(name)
        {
            SetGammStruct(stupens, intervals);
        }

        /// <summary>
        /// Создаёт структуру гаммы из intervals: последовательности интервалов (1-ая строчка) и ступеней (2-ая строчка), с названием лада name
        /// </summary>
        /// <param name="intervals"></param>
        public GammStrusct(int[,] intervals, string name = "") : this(name)
        {
            SetGammStruct(intervals);
        }

        /// <summary>
        /// Создаёт структуру гаммы из другой структуры гаммы
        /// </summary>
        /// <param name="Gamma"></param>
        public GammStrusct(GammStrusct Gamma) : this(Gamma.StupenIntervals, Gamma.Name)
        {
        }

        /// <summary>
        /// Создаёт структуру гаммы из другой структуры гаммы Gamma и присваивает ей имя лада name
        /// </summary>
        /// <param name="Gamma"></param>
        /// <param name="name"></param>
        public GammStrusct(GammStrusct Gamma, string name):this(GammStrusct.Copy(Gamma))
        {
            this.Name = name;
        }

        /// <summary>
        /// Создаёт структуру гаммы из Json-файла или Gamm-файла с именем reader.Name
        /// </summary>
        /// <param name="reader"></param>
        public GammStrusct(FileStream reader):this(Load(reader.Name))
        {

        }

        /// <summary>
        /// Возвращает или задаёт название лада
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Name", IsNullable = true)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Возвращает или задаёт последовательность ступеней
        /// </summary>
        [DataMember]
        [XmlIgnore]
        public int[] Stupens
        {
            get
            {
                return stupens;
            }
            set
            {
                try
                {
                    SetGammStruct(value, this.intervals);
                }
                catch
                {
                    SetGammStruct(value, new int[1]);
                }
            }
        }

        /// <summary>
        /// Возвращает или задаёт маследовательность ступеней
        /// </summary>
        [DataMember]
        [XmlIgnore]
        public int[] Intervals
        {
            get
            {
                return intervals;
            }
            set
            {
                try
                {
                    SetGammStruct(this.stupens, value);
                }
                catch
                {
                    SetGammStruct(new int[1], value);
                }
            }
        }

        /// <summary>
        /// Возвращает строку rowIndex из матрицы matrics
        /// </summary>
        /// <param name="matrics"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private int[] RowInMatrics(int[,] matrics, int rowIndex)
        {
            int[] result = new int[matrics.GetLength(1)];
            for (int i = 0; i < matrics.GetLength(1); i++)
            {
                result[i] = matrics[rowIndex, i];
            }
            return result;
        }

        /// <summary>
        /// Задаёт структуру гаммы из последовательности ступеней stupens и интервалов intervals
        /// </summary>
        /// <param name="stupens"></param>
        /// <param name="intervals"></param>
        public void SetGammStruct(int[] stupens, int[] intervals)
        {
            this.stupens = stupens;
            this.intervals = intervals;
            stupenIntervals = new int[2, Math.Min(this.stupens.Length, this.intervals.Length)];
            for (int i = 0; i < stupenIntervals.GetLength(1); i++)
            {
                stupenIntervals[0, i] = this.intervals[i];
                stupenIntervals[1, i] = this.stupens[i];
            }
        }

        /// <summary>
        /// Задаёт структуру гаммы из intervals: последовательности интервалов (первая строчка) и ступеней (вторая строчка)
        /// </summary>
        /// <param name="intervals"></param>
        public void SetGammStruct(int[,] intervals)
        {
            SetGammStruct(RowInMatrics(intervals, 1), RowInMatrics(intervals, 0));
        }

        /// <summary>
        /// Возвращает структуру гаммы
        /// </summary>
        /// <returns></returns>
        public int[,] GetGammStruct()
        {
            SetGammStruct(this.Stupens, this.Intervals);
            return stupenIntervals;
        }

        /// <summary>
        /// Задаёт или возвращает структуру гаммы
        /// </summary>
        [XmlIgnore]
        public int[,] StupenIntervals
        {
            get
            {
                return GetGammStruct();
            }
            set
            {
                SetGammStruct(value);
            }
        }


        /// <summary>
        /// Сохраняет объект obj с типом type в Json-файл namefile
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="namefile"></param>
        private static void JsonWrite(object obj, Type type, string namefile)
        {
            namefile = namefile.Replace('/', '\\');
            DataContractJsonSerializer json = new DataContractJsonSerializer(type);
            FileStream fileStream = new FileStream(namefile, FileMode.Create);
            json.WriteObject(fileStream, obj);
        }

        /// <summary>
        /// Сохраняет объект obj с типом type в XML-файл namefile
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="FileName"></param>
        /// <param name="type"></param>
        private static void XMLWrite(Object obj, string FileName, Type type)
        {
            FileName = FileName.Replace('/', '\\');
            XmlSerializer serializer = new XmlSerializer(type);
            FileStream stream = new FileStream(FileName, FileMode.Create);
            serializer.Serialize(stream, obj);
        }


        /// <summary>
        /// Чтение из Json-файла
        /// </summary>
        /// <param name="namefile"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object JsonRead(string namefile, Type type)
        {
            namefile = namefile.Replace('/', '\\');
            DataContractJsonSerializer json = new DataContractJsonSerializer(type);
            FileStream fileStream = new FileStream(namefile, FileMode.Open);
            object obj = json.ReadObject(fileStream);
            return obj;


        }

        /// <summary>
        /// Чтение из XML-файла
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static object XMLRead(string FileName, Type type)
        {
            FileName = FileName.Replace('/', '\\');
            XmlSerializer serializer = new XmlSerializer(type);
            FileStream stream = new FileStream(FileName, FileMode.Open);
            object obj = serializer.Deserialize(stream);
            return obj;
        }

        /// <summary>
        /// Загружает струкутуру гаммы из Json-файла или Gamm-файла
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static GammStrusct Load(string FileName)
        {
            string[] cut = FileName.Split('.');
            string cut1 = cut[cut.Length - 1].ToLower();
                cut[cut.Length - 1] = cut1;
                FileName = String.Join(".", cut);
                return (GammStrusct)GammFileSave.Load(FileName, typeof(GammStrusct)).Object;
            
        }

        /// <summary>
        /// Загружает струкутуру гаммы из Json-файла или Gamm-файла
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static GammStrusct Load(FileStream stream)
        {
            return Load(stream.Name);
        }

        /// <summary>
        /// Загружает структуру гаммы из другой структуры Gamma
        /// </summary>
        /// <param name="Gamma"></param>
        /// <returns></returns>
        public static GammStrusct Load (GammStrusct Gamma)
        {
            return new GammStrusct(Gamma.stupenIntervals, Gamma.Name);
        }

        /// <summary>
        /// Загружает структуру гаммы из лада StupenIntervals и присваивает название name
        /// </summary>
        /// <param name="StupensIntervals"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GammStrusct Load(int[,] StupensIntervals, string name = "")
        {
            return Load(new GammStrusct(StupensIntervals, name));
        }

        /// <summary>
        /// Загружает структуру гаммы из другой структуры Gamma и присваивает название name
        /// </summary>
        /// <param name="Gamma"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GammStrusct Load(GammStrusct Gamma, string name)
        {
            return Load(new GammStrusct(Gamma, name));
        }

        /// <summary>
        /// Создаёт копию структуры гаммы Gamma
        /// </summary>
        /// <param name="Gamma"></param>
        /// <returns></returns>
        public static GammStrusct Copy(GammStrusct Gamma)
        {
            return GammStrusct.Load(Gamma);
        }

        /// <summary>
        /// Создаёт копию структуры гаммы Gamma, присваивает копии название name
        /// </summary>
        /// <param name="Gamma"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GammStrusct Copy(GammStrusct Gamma, string name)
        {
            return Copy(new GammStrusct(Gamma, name));
        }

        /// <summary>
        /// Создаёт копию структуры гаммы
        /// </summary>
        /// <returns></returns>
        public GammStrusct Copy()
        {
            return GammStrusct.Copy(this);
        }

        /// <summary>
        /// Создаёт копию структуры гаммы, присваивает копие название name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GammStrusct Copy(string name)
        {
            return Copy(new GammStrusct(this, name));
        }



        /// <summary>
        /// Сохраняет структуру гаммы в Json-файл или Gamm-файл с именем FileName
        /// </summary>
        /// <param name="FileName"></param>
        public void Save(string FileName, int FileType = 0)
        {
            string[] cut = FileName.Split('.');
            string cut1 = cut[cut.Length - 1].ToLower();
            if (cut1 == "json" || cut1 == "gamm" || cut1 == "gammj")
            {
                cut[cut.Length - 1] = cut1;
                FileName = String.Join(".", cut);
                JsonWrite(this, typeof(GammStrusct), FileName);
            }
            else if (cut1 == "xml" || cut1 == "ntscl" || cut1 == "gammh")
            {
                cut[cut.Length - 1] = cut1;
                FileName = String.Join(".", cut);
                XMLWrite(this, FileName, typeof(GammStrusct));
            }
            else
            {
                GammFileSave.Save(this, typeof(GammStrusct), FileName, (GammFileSave.FileType)FileType);
            }
        }

        /// <summary>
        /// Сохраняет структуру гаммы в Json-файл или Gamm-файл с именем stream.Name
        /// </summary>
        /// <param name="stream"></param>
        public void Save (FileStream stream, int FileType = 0)
        {
            Save(stream.Name, FileType);
        }

        /// <summary>
        /// Сохраняет структуру гаммы Gamma в Json-файл или Gamm-файл с именем FileName
        /// </summary>
        /// <param name="Gamma"></param>
        /// <param name="FileName"></param>
        public static void Save (GammStrusct Gamma, string FileName, int FileType = 0)
        {
            Gamma.Save(FileName, FileType);
        }


        /// <summary>
        /// Сохраняет структуру гаммы Gamma в Json-файл или Gamm-файл с именем stream.Name
        /// </summary>
        /// <param name="Gamma"></param>
        /// <param name="FileName"></param>
        public static void Save(GammStrusct Gamma, FileStream stream, int FileType = 0)
        {
            Gamma.Save(stream.Name, FileType);
        }
    }
}
