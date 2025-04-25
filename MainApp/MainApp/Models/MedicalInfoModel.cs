using System;
using System.Collections.Generic;

namespace MainApp.Models
{
    public class MedicalInfoModel
    {
        public string NameProject { get; set; }

        public DateTime MedicalCheckDate { get; set; }

        public string ClientIDStart { get; set; }

        public string ClientIDEnd { get; set; }

        public string Division { get; set; }

        public int Sex { get; set; }

        public DateTime BirthDay { get; set; }
        public string CourseId { get; set; }

        public List<string> TableInsert { get; set; }


    }
}