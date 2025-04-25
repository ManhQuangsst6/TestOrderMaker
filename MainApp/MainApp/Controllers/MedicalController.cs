using MainApp.Models;
using MainApp.Tables;
using System;
using System.Arctec.Ar1000k.DataBase;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Http;
using TestOrderMaker;
using TestOrderMaker.Common;
using TestOrderMaker.Models;

namespace MainApp.Controllers
{
    public class MedicalController : ApiController
    {
        protected DbManager db_manager;
        private FirstNode[] nodes;
        private MedicalCheckList[] medical_check_list;
        private MedicalItemListRecord[] medical_item_list_record;
        public MedicalController()
        {
            db_manager = DbManager.getInstance();

        }

        #region Nodeデータ初期化
        private void initNodes()
        {
            nodes = new FirstNode[medical_check_list.Length];
            for (int i = 0; i < medical_check_list.Length; i++)
            {
                List<MedicalItemListRecord> item_list = getMedicalItemListRecord(Int32.Parse(medical_check_list[i].MedicalCheckNo));
                nodes[i] = new FirstNode(medical_check_list[i], item_list);
            }
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Medical/Setup")]
        public void Setup([FromBody] Project project)
        {
            var filePath = Path.Combine(AppSetting.ProjectFolder, AppSetting.ProjectFileName);
            List<string[]> project_data_list = IO.ReadData(filePath, "Shift_JIS");
            var project_dict = initProjectDictionary(project_data_list);
            project.ProjectUrl = project_dict[project.ProjectName].ProjectUrl;
            bool is_success = db_manager.init(project.ProjectUrl);
            medical_check_list = db_manager.getMedicalCheckList();
            medical_item_list_record = db_manager.getMedicalItemListRecordAll();
            initNodes();
        }
        #endregion
        #region MedicalItemListRecord取得
        private List<MedicalItemListRecord> getMedicalItemListRecord(int medical_check_no)
        {
            List<MedicalItemListRecord> ret = new List<MedicalItemListRecord>();
            foreach (MedicalItemListRecord item_record in medical_item_list_record)
            {
                if (item_record.MedicalCheckNo == medical_check_no)
                {
                    ret.Add(item_record);
                }
            }
            return ret;
        }
        #endregion
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Medical/Post")]
        public ApiResponse<MedicalInfoModel> Post([FromBody] MedicalInfoModel medicalInfo)
        {
            // Validate the input
            if (medicalInfo == null)
            {
                return new ApiResponse<MedicalInfoModel>(HttpStatusCode.BadRequest, "Input is null");

            }
            var filePath = Path.Combine(AppSetting.ProjectFolder, AppSetting.ProjectFileName);
            List<string[]> project_data_list = IO.ReadData(filePath, "Shift_JIS");
            var project_dict = initProjectDictionary(project_data_list);
            var projectUrl = project_dict[medicalInfo.NameProject].ProjectUrl;
            bool is_success = db_manager.init(projectUrl);
            medical_check_list = db_manager.getMedicalCheckList();
            medical_item_list_record = db_manager.getMedicalItemListRecordAll();
            initNodes();
            string client_id_start = medicalInfo.ClientIDStart.Trim();
            string client_id_end = medicalInfo.ClientIDStart.Trim();
            if (client_id_end == "")
            {
                client_id_end = client_id_start;    // 後続処理のために開始と終了を同じと見なす
            }
            int client_id_start_int;
            int client_id_end_int;
            is_success = int.TryParse(client_id_start, out client_id_start_int);
            is_success &= int.TryParse(client_id_end, out client_id_end_int);
            int client_id_pad0_num = client_id_end.Length;
            List<TableBase> table_manager_list = new List<TableBase>();
            foreach (var item in medicalInfo.TableInsert)
            {
                string className = item;
                string fullTypeName = $"MainApp.Tables.{className}Table";

                Type type = Type.GetType(fullTypeName);
                if (type != null && typeof(TableBase).IsAssignableFrom(type))
                {
                    var instance = (TableBase)Activator.CreateInstance(type, className, true);
                    table_manager_list.Add(instance);
                }
            }
            #region 既に登録済の場合
            bool is_registed = false;
            foreach (TableBase tm in table_manager_list)
            {
                if (tm.TableName == "MedicalCheckData")
                {
                    // 対象外
                    continue;
                }
                is_registed |= tm.getRegistedClientID(medicalInfo.MedicalCheckDate.ToString("yyyy/MM/dd"), medicalInfo.ClientIDStart.PadLeft(10, ' '),
                    medicalInfo.ClientIDEnd.PadLeft(10, ' '), medicalInfo.Division);
            }
            bool is_registed_old = false;
            //if (oldDataChkBox.Checked)
            //{
            //    foreach (TableBase tm in table_manager_list)
            //    {
            //        is_registed_old |= tm.getRegistedClientID_Old(medical_check_date_old, client_id_start.PadLeft(10, ' '), client_id_end.PadLeft(10, ' '), division);
            //    }
            //}
            #endregion
            #region 登録処理
            // 登録数の初期化
            foreach (TableBase tm in table_manager_list)
            {
                tm.resetInsertNum();
            }

            Dictionary<string, string[]> medical_check_item_str_array = getMedicalCheckItemStrArray();
            Dictionary<string, string> medical_check_state_str_array = getMedicalCheckStateStrArray();
            Dictionary<string, string> medical_check_state_str_array_old = getMedicalCheckStateStrArrayOld();
            for (int client_id = client_id_start_int; client_id <= client_id_end_int; client_id++)
            {
                string regist_client_id = client_id.ToString().PadLeft(client_id_pad0_num, '0').PadLeft(10, ' ');
                foreach (TableBase tm in table_manager_list)
                {
                    try
                    {
                        tm.insert(medicalInfo.MedicalCheckDate.ToString("yyyy/MM/dd"), regist_client_id, medicalInfo.Division, 12, medical_check_item_str_array, medical_check_state_str_array,
                                    medicalInfo.Sex.ToString(), medicalInfo.BirthDay.ToString("yyyy/MM/dd"), medicalInfo.CourseId);
                        //if (oldDataChkBox.Checked)
                        //{
                        //    tm.insertOld(medicalInfo.MedicalCheckDate.ToString("yyyy/MM/dd"), regist_client_id, medicalInfo.Division, 12, medical_check_item_str_array, medical_check_state_str_array,
                        //           medicalInfo.Sex.ToString(), medicalInfo.BirthDay.ToString("yyyy/MM/dd"), medicalInfo.CourseId);
                        //}
                    }
                    catch { }
                }
            }
            #endregion
            return new ApiResponse<MedicalInfoModel>(medicalInfo);

        }
        #region MedicalCheckItem登録データ取得
        private Dictionary<string, string[]> getMedicalCheckItemStrArray()
        {
            // <MedicalCheckNo, Item_value_array>
            Dictionary<string, string[]> ret = new Dictionary<string, string[]>();
            for (int i = 0; i < nodes.Length; i++)
            {
                SecondNode[] second_nodes = nodes[i].SecondNodes;
                string medical_check_no = nodes[i].MedicalCheclNo;
                string[] temp = new string[second_nodes.Length];
                for (int j = 0; j < second_nodes.Length; j++)
                {
                    temp[j] = second_nodes[j].NodeChecked ? "1" : null;
                }
                ret.Add(medical_check_no, temp);
            }
            return ret;
        }
        #endregion
        private Dictionary<string, string> getMedicalCheckStateStrArray()
        {
            // <MedicalCheckNo, State>
            Dictionary<string, string> ret = new Dictionary<string, string>();
            // デフォルト: 0
            for (int i = 1; i <= 100; i++)
            {
                ret.Add(i.ToString(), "0");
            }
            for (int i = 0; i < nodes.Length; i++)
            {
                ret[nodes[i].MedicalCheclNo] = nodes[i].NodeChecked ? "2" : "0";
            }
            return ret;

        }
        private Dictionary<string, string> getMedicalCheckStateStrArrayOld()
        {
            // <MedicalCheckNo, State>
            Dictionary<string, string> ret = new Dictionary<string, string>();
            for (int i = 0; i < nodes.Length; i++)
            {
                ret.Add(nodes[i].MedicalCheclNo, nodes[i].NodeChecked ? "4" : "0");
            }
            return ret;
        }
        private Dictionary<string, Project> initProjectDictionary(List<string[]> project_data_list)
        {
            if (project_data_list == null)
            {
                return null;
            }
            Dictionary<string, Project> project_dict = new Dictionary<string, Project>();
            foreach (string[] array in project_data_list)
            {
                string project_name = array[0];
                string iis_path = array[1];
                string pad_num_str = array.Length < 3 ? "12" : array[2];    // 過去バージョンデータだと登録されていないものもある デフォルト値は12
                int pad_num;
                if (!int.TryParse(pad_num_str, out pad_num))
                {
                    pad_num = 12;
                }
                project_dict.Add(project_name, new Project(project_name, iis_path, pad_num));
            }
            return project_dict;
        }
    }
}
