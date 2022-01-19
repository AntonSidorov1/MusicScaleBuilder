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
    /// Класс сохранения и чтения файлов, которые могут быть, как XML, так и Json
    /// </summary>
    class GammFileSave
    {
        /// <summary>
        /// Тип файла - HML или Json
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// Json
            /// </summary>
            Json = 0,

            /// <summary>
            /// HML
            /// </summary>
            HML = 1
        }

        /// <summary>
        /// Создание класса
        /// </summary>
        public GammFileSave():this("text.json")
        {

        }

        string name;
        FileType filetype;
        Object obj;
        Type type;

        /// <summary>
        /// Создание класса
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="fileType"></param>
        public GammFileSave(string FileName, FileType fileType = FileType.Json)
        {
            obj = "";
            name = FileName;
            filetype = fileType;
        }

        /// <summary>
        /// Создание класса
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="fileType"></param>
        public GammFileSave(string FileName, Type type, FileType fileType = FileType.Json) : this(FileName, fileType)
        {
            this.Type = type;
        }

        /// <summary>
        /// Создание класса
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="fileType"></param>
        public GammFileSave(string FileName, object obj, Type type, FileType fileType = FileType.Json):this(FileName, type, fileType)
        {
            this.SetObject(obj, type);
        }

        /// <summary>
        /// Создание класса из объекта
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static GammFileSave FromObject(string FileName, object obj, Type type, FileType fileType = FileType.Json)
        {
            return new GammFileSave(FileName, obj, type, fileType);
        }

        /// <summary>
        /// Создание класса из другого объекта
        /// </summary>
        /// <param name="fileSave"></param>
        public GammFileSave(GammFileSave fileSave) :this(fileSave.FileName, fileSave.Object, fileSave.Type, fileSave.TypeReturn)
        {

        }

        /// <summary>
        /// Текущий объект
        /// </summary>
        public GammFileSave NowFileStruct
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Создание класса из объекта
        /// </summary>
        /// <param name="fileSave"></param>
        /// <returns></returns>
        public static GammFileSave FromObject(GammFileSave fileSave)
        {
            return new GammFileSave(fileSave);
        }

        /// <summary>
        /// Возвращает загруженный объект
        /// </summary>
        public object Object
        {
            get
            {
                return obj;
            }
        }

        /// <summary>
        /// Возвращает тип загруженного объекта
        /// </summary>
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Загружает объект obj с типом type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public void SetObject(object obj, Type type)
        {
            this.obj = obj;
            this.type = type;
        }

        /// <summary>
        /// Возвращает или задаёт тип файла (HML или Json)
        /// </summary>
        public FileType TypeReturn
        {
            get
            {
                return filetype;
            }
            set
            {
                filetype = value;
            }
        }

        /// <summary>
        /// Возвращает или задаёт имя файла
        /// </summary>
        public string FileName
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
            try
            {
                object obj = json.ReadObject(fileStream);
                return obj;
            }
            catch
            {
                fileStream.Close();
                object obj = json.ReadObject(fileStream);
                return obj;
            }


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
        /// Загружает объект из файла
        /// </summary>
        public void Load()
        {
            FileName = FileName.Replace('/', '\\');
            string[] cut = FileName.Split('.');
            string cut1 = cut[cut.Length - 1].ToLower();
            cut[cut.Length - 1] = cut1;
            FileName = String.Join(".", cut);

            try
            {
                object obj = JsonRead(name, type);
                SetObject(obj, type);
                TypeReturn = FileType.Json;

            }
            catch
            {
                object obj = XMLRead(name, type);
                SetObject(obj, type);
                TypeReturn = FileType.Json;
            }
        }

        /// <summary>
        /// Загружает объект из файла
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GammFileSave Load(string FileName, Type type)
        {
            GammFileSave gammFile = new GammFileSave(FileName, type);
            gammFile.Load();
            return gammFile;
        }


        //public static object Load(string FileName, Type type)
        //{
        //    GammFileSave gammFile = GammFileSave.Load(FileName, type);
        //    return gammFile.Object;
        //}

        /// <summary>
        /// Сохраняет объект в файл с именем FileName. fileType - Json или XML
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="type"></param>
        /// <param name="fileType"></param>
        public void Save(string FileName, FileType fileType = FileType.Json)
        {
            FileName = FileName.Replace('/', '\\');
            string[] cut = FileName.Split('.');
            cut[cut.Length - 1] = cut[cut.Length - 1].ToLower();
            FileName = String.Join(".", cut);
            if(fileType == FileType.Json)
            {
                JsonWrite(this.Object, this.Type, FileName);
            }
            else
            {
                XMLWrite(this.Object, FileName, this.Type);
            }
        }

        /// <summary>
        /// Сохраняет объект в файл, в зависимости от конфигурации класса. fileType - Json или XML
        /// </summary>
        /// <param name="fileType"></param>
        public void Save(FileType fileType)
        {
            Save(this.FileName, fileType);
        }


        /// <summary>
        /// Сохраняет объект в файл, в зависимости от конфигурации класса
        /// </summary>
        public void Save()
        {
            Save(this.FileName, this.TypeReturn);
        }

        /// <summary>
        /// Сохраняет объект obj типа type в файл с именем FileName. fileType - Json или XML
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="FileName"></param>
        /// <param name="fileType"></param>
        public static void Save(object obj, Type type, string FileName, FileType fileType = FileType.Json)
        {
            GammFileSave.FromObject(FileName, obj, type, fileType).Save();
        }


        public static void Save(GammFileSave fileSave)
        {
            GammFileSave.FromObject(fileSave).Save();
        }
    }
}
