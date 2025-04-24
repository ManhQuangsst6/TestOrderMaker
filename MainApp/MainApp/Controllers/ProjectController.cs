using MainApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using TestOrderMaker.Common;
using TestOrderMaker.Models;

namespace TestOrderMaker.Controllers
{

    public class ProjectController : ApiController

    {
        protected DbManager db_manager;
        public ProjectController()
        {
            db_manager = DbManager.getInstance();
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Project/Post")]
        public ApiResponse<Project> Post([FromBody] Project project)
        {
            string iis_path = project.ProjectUrl;
            string projectName = project.ProjectName;
            int? pad_num = project.RegistrationNo;

            #region 入力チェック
            //if (iis_path == "")
            //{
            //    MessageBox.Show(message.getMessage("MSG0001-E"));
            //    return;
            //}
            //if (project == "")
            //{
            //    MessageBox.Show(message.getMessage("MSG0002-E"));
            //    return;
            //}
            //if (pad_num == "")
            //{
            //    MessageBox.Show(message.getMessage("MSG0010-E"));
            //    return;
            //}
            //if (!Int32.TryParse(pad_num, out pad_num_int))
            //{
            //    MessageBox.Show(message.getMessage("MSG0011-E"));
            //    return;
            //}
            //if (pad_num_int <= 0 || pad_num_int >= 20)
            //{
            //    MessageBox.Show(message.getMessage("MSG0012-E"));
            //    return;
            //}
            #endregion

            // 既に登録済みの案件の場合、IISパス、RegistrationNo許容値を更新する
            //if (project_dict.ContainsKey(project))
            //{
            //    DialogResult result = MessageBox.Show(message.getMessage("MSG0005-W"), "確認", MessageBoxButtons.YesNo);
            //    if (result != DialogResult.Yes)
            //    {
            //        return;
            //    }

            //    // 辞書を更新する
            //    project_dict[project].IIS_path = iis_path;

            //    // 更新処理
            //    IO.Write(Project.getRegistData(project_dict, padNumTxt.Text, project), project_save_path, project_file_name, "Shift_JIS");

            //    // 更新した案件を選択状態にする
            //    projectNameCmbBox.SelectedItem = project;
            //    return;
            //}

            // 案件名フォルダ作成
            //if (!Directory.Exists(project))
            //{
            //    try
            //    {
            //        Directory.CreateDirectory(project);
            //    }
            //    catch (ArgumentException)
            //    {
            //        MessageBox.Show(message.getMessage("MSG0008-E"));
            //        return;
            //    }
            //    catch (Exception)
            //    {
            //        MessageBox.Show(message.getMessage("MSG0009-E"));
            //        return;
            //    }
            //}

            // 登録処理
            var filePath = Path.Combine(AppSetting.ProjectFolder, AppSetting.ProjectFileName);
            List<string[]> project_data_list = IO.ReadData(filePath, "Shift_JIS");
            var project_dict = initProjectDictionary(project_data_list);

            string regist_text = projectName + "," + iis_path + "," + pad_num;
            IO.Write(regist_text, AppSetting.ProjectFolder, AppSetting.ProjectFileName, "Shift_JIS");

            // コンボボックスに追加
            //projectNameCmbBox.Items.Add(project);

            ////　辞書にも追加
            project_dict.Add(projectName, new Project(projectName, iis_path, pad_num.Value));

            //// 登録した案件を選択状態にする
            //projectNameCmbBox.SelectedIndex = projectNameCmbBox.Items.Count - 1;

            //MessageBox.Show(message.getMessage("MSG0051-I"));
            return new ApiResponse<Project>(project);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Project/Connect")]
        public ApiResponse<Project> SignInProject([FromBody] Project project)
        {
            string project_name = project.ProjectName;
            bool is_success = db_manager.init(project.ProjectUrl);
            // ここにプロジェクトのサインインロジックを追加
            return new ApiResponse<Project>(project);
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
